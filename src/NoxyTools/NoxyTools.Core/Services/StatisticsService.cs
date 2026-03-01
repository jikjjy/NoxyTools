using SaveParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NoxyTools.Core.Services
{
    public partial class StatisticsService
    {
        public enum Align
        {
            Left,
            Center,
            Right
        }
        public bool IsLoaded { get; private set; } = false;
        public IReadOnlyList<ItemSet> NewItems => mNewItems;
        public CharacterStateSet StateDelta => mStateDelta;
        public IReadOnlyList<ItemSet> OldItems => mOldData?.Items ?? Array.Empty<ItemSet>().ToList();
        public CharacterStateSet NewData => mNewData ?? new CharacterStateSet();
        public GameSaveSet? LastSaveFile => mLastSaveFile;
        public readonly List<ItemSet> UserInputItems = new List<ItemSet>();
        public event EventHandler<string> LogMessage;
        private CharacterStateSet mOldData;
        private CharacterStateSet mNewData;
        private CharacterStateSet mStateDelta;
        private GameSaveSet mLastSaveFile;
        private List<ItemSet> mNewItems = new List<ItemSet>();

        /// <summary>config.MakeValidReport.ItemOrderMethod 에 따라 아이템을 정렬합니다.</summary>
        public List<ItemSet> OrderItems(ConfigService config, List<ItemSet> src)
            => getOrderByItems(config, src);

        public void Refresh(ConfigService config)
        {
            var statisticsData = config.MakeValidReport.StatisticsData;
            mOldData = statisticsData.Data;
            if (mOldData == null)
            {
                mOldData = new CharacterStateSet();
            }

            string w3SavePath = config.MakeValidReport.W3SavePath;
            directoryCheck(w3SavePath);
            var di = new DirectoryInfo(w3SavePath);
            List<GameSaveSet> saveFileInfos = di.GetFiles("*.txt")
                .OrderBy(item => item.LastWriteTime)
                .Select(item => new GameSaveSet() { FileInfo = item, Character = item.GetChracterSetFromFileInfo() })
                .Where(item => item.Character.IsNoxy)
                .ToList();

            CharacterStateSet data = new CharacterStateSet();
            if (saveFileInfos.Count > 0)
            {
                GameSaveSet lastItem = saveFileInfos.Last();
                data.Strength = lastItem.Character.Strength;
                data.Agility = lastItem.Character.Agility;
                data.Intelligence = lastItem.Character.Intelligence;

                data.Items = saveFileInfos
                    .SelectMany(item => item.Character.Items)
                    .GroupBy(item => item.Name)
                    .Select(item => item.First())
                    .Where(item => item.GradeIndex >= EItemGrade.오메가)
                    .Concat(UserInputItems)
                    .ToList();

                mLastSaveFile = lastItem;
            }
            mNewData = data;

            data = new CharacterStateSet();
            {
                data.Strength = mNewData.Strength - mOldData.Strength;
                data.Agility = mNewData.Agility - mOldData.Agility;
                data.Intelligence = mNewData.Intelligence - mOldData.Intelligence;
            }
            mStateDelta = data;

            mNewItems = mNewData.Items
                .Except(mOldData.Items)
                .ToList();

            IsLoaded = true;
        }

        public void Save(ConfigService config)
        {
            if (IsLoaded == false)
            {
                return;
            }

            // 저장
            var statisticsData = config.MakeValidReport.StatisticsData;
            statisticsData.Data.Strength = mNewData.Strength;
            statisticsData.Data.Agility = mNewData.Agility;
            statisticsData.Data.Intelligence = mNewData.Intelligence;
            statisticsData.Data.Items.AddRange(mNewItems);
            statisticsData.Data.Items.AddRange(UserInputItems.Where(item => !statisticsData.Data.Items.Contains(item)));
            config.MakeValidReport.StatisticsData = statisticsData;

            // 갱신
            mOldData = mNewData = config.MakeValidReport.StatisticsData.Data;
            mStateDelta = new CharacterStateSet();
            mNewItems.Clear();
            UserInputItems.Clear();
        }

        // TODO Phase 2: ExportCafeUploadFormat → WPF FlowDocument 또는 HTML 생성으로 재구현
        // TODO Phase 2: SetClipboard → IClipboardService.SetHtml() 으로 재구현

        public string GetLeftSpacing(string src, int width, Align align)
        {
            char blink = '　';
            if (src.Length < width)
            {
                StringBuilder sb = new StringBuilder();
                switch (align)
                {
                    case Align.Center:
                        {
                            int frontSpace = (width - src.Length) / 2;
                            for (int i = 0; i < frontSpace; i++)
                            {
                                sb.Append(blink);
                            }
                        }
                        break;
                    case Align.Right:
                        {
                            int frontSpace = (width - src.Length);
                            for (int i = 0; i < frontSpace; i++)
                            {
                                sb.Append(blink);
                            }
                        }
                        break;
                    case Align.Left:
                    default:
                        {
                        }
                        break;
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        public string GetRightSpacing(string src, int width, Align align)
        {
            char blink = '　';
            if (src.Length < width)
            {
                StringBuilder sb = new StringBuilder();
                switch (align)
                {
                    case Align.Center:
                        {
                            int rearSpace = width - (src.Length + (width - src.Length) / 2);
                            for (int i = 0; i < rearSpace; i++)
                            {
                                sb.Append(blink);
                            }
                        }
                        break;
                    case Align.Right:
                        {
                        }
                        break;
                    case Align.Left:
                    default:
                        {
                            int rearSpace = (width - src.Length);
                            for (int i = 0; i < rearSpace; i++)
                            {
                                sb.Append(blink);
                            }
                        }
                        break;
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        private List<ItemSet> getOrderByItems(ConfigService config, List<ItemSet> src)
        {
            switch (config.MakeValidReport.ItemOrderMethod)
            {
                case 0:
                    {
                        return src;
                    }
                case 1:
                default:
                    {
                        return src
                            .OrderBy(item => item.Name)
                            .OrderBy(item => item.GradeIndex)
                            .ToList();
                    }
            }
        }

        private void directoryCheck(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }

        private void raiseLogMessageEvent(string message)
        {
            LogMessage?.Invoke(this, message);
        }
    }
}
