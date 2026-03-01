using ReplayParser;
using SaveParser;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NoxyTools.Core.Services
{
    public partial class ItemStatisticsService
    {
        public int SelectReplayIndex
        {
            get
            {
                return mSelectReplayIndex;
            }
            set
            {
                setReplayIndex(value);
            }
        }
        public IReadOnlyList<GameSaveSet> SelectSaveGroup => mSelectSaveGroup;
        public IReadOnlyList<GameSaveSet> BeforeSaveGroup => mBeforeSaveGroup;
        public IReadOnlyList<GameReplaySet> ReplayFileInfos => mReplayFileInfos;
        public IReadOnlyList<GameSaveSet> SaveFileInfos => mSaveFileInfos;
        public IReadOnlyList<ItemSet> BeforeCollectItems => mBeforeCollectItems;
        public IReadOnlyList<ItemSet> CollectItems => mCollectItems;
        public int StatBeginStr { get; private set; } = 0;
        public int StatBeginAgi { get; private set; } = 0;
        public int StatBeginInt { get; private set; } = 0;
        public int StatDeltaStr { get; private set; } = 0;
        public int StatDeltaAgi { get; private set; } = 0;
        public int StatDeltaInt { get; private set; } = 0;
        private int mSelectReplayIndex = -1;
        private List<GameReplaySet> mReplayFileInfos = new List<GameReplaySet>();
        private List<GameSaveSet> mSaveFileInfos = new List<GameSaveSet>();
        private List<GameSaveSet> mSelectSaveGroup = new List<GameSaveSet>();
        private List<GameSaveSet> mBeforeSaveGroup = new List<GameSaveSet>();
        private List<ItemSet> mBeforeCollectItems = new List<ItemSet>();
        private List<ItemSet> mCollectItems = new List<ItemSet>();
        private readonly ComparerItemSet mComparerItemSet = new ComparerItemSet();

        public void Load(ConfigService config)
        {
            string w3ReplayPath = config.W3ReplayPath;
            directoryCheck(w3ReplayPath);
            DirectoryInfo di = new DirectoryInfo(w3ReplayPath);
            mReplayFileInfos = di.GetFiles("*.w3g")
                .OrderBy(item => item.CreationTime)
                .Select(item => new GameReplaySet() { FileInfo = item, Replay = item.GetReplayInfoSetFromFileInfo() })
                .Where(item => item.Replay.IsReplay && item.Replay.IsSupport)
                .ToList();

            string w3SavePath = config.W3SavePath;
            directoryCheck(w3SavePath);
            di = new DirectoryInfo(w3SavePath);
            mSaveFileInfos = di.GetFiles("*.txt")
                .OrderBy(item => item.CreationTime)
                .Select(item => new GameSaveSet() { FileInfo = item, Character = item.GetChracterSetFromFileInfo() })
                .Where(item => item.Character.IsNoxy)
                .ToList();
        }

        private void directoryCheck(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }

        private void setReplayIndex(int setValue)
        {
            List<GameReplaySet> replayFileInfos = mReplayFileInfos;
            List<GameSaveSet> saveFileInfos = mSaveFileInfos;
            if (replayFileInfos.Count == 0)
            {
                mSelectReplayIndex = -1;
                return;
            }
            if (setValue < 0)
            {
                setValue = 0;
            }
            if (setValue >= replayFileInfos.Count)
            {
                setValue = replayFileInfos.Count - 1;
            }
            mSelectReplayIndex = setValue;

            FileInfo selectReplayFileInfo = replayFileInfos[mSelectReplayIndex].FileInfo;
            if (mSelectReplayIndex > 0)
            {
                var beforeReplayFileInfo = replayFileInfos[mSelectReplayIndex - 1];
                mSelectSaveGroup = saveFileInfos
                    .Where(item => item.FileInfo.LastWriteTime > beforeReplayFileInfo.FileInfo.LastWriteTime && item.FileInfo.LastWriteTime < selectReplayFileInfo.LastWriteTime)
                    .ToList();
                mBeforeSaveGroup = saveFileInfos
                    .Where(item => item.FileInfo.LastWriteTime < beforeReplayFileInfo.FileInfo.LastWriteTime)
                    .ToList();

                if (mSelectSaveGroup.Count == 0)
                {
                    mSelectSaveGroup = mBeforeSaveGroup;
                }
            }
            else
            {
                mSelectSaveGroup = saveFileInfos
                    .Where(item => item.FileInfo.LastWriteTime < selectReplayFileInfo.LastWriteTime)
                    .ToList();
                mBeforeSaveGroup = mSelectSaveGroup;
            }

            mBeforeCollectItems = mBeforeSaveGroup
                .SelectMany(item => item.Character.Items)
                .GroupBy(item => item.Name)
                .Select(item => item.First())
                .ToList();
            List<ItemSet> collectItems = mSelectSaveGroup
                .SelectMany(item => item.Character.Items)
                .GroupBy(item => item.Name)
                .Select(item => item.First())
                .ToList();
            mCollectItems = collectItems
                .Except(mBeforeCollectItems, mComparerItemSet)
                .ToList();

            GameSaveSet firstGameSave = mSelectSaveGroup.First();
            GameSaveSet lastGameSave = mSelectSaveGroup.Last();
            StatBeginStr = firstGameSave.Character.Strength;
            StatBeginAgi = firstGameSave.Character.Agility;
            StatBeginInt = firstGameSave.Character.Intelligence;
            StatDeltaStr = lastGameSave.Character.Strength - StatBeginStr;
            StatDeltaAgi = lastGameSave.Character.Agility - StatBeginAgi;
            StatDeltaInt = lastGameSave.Character.Intelligence - StatBeginInt;
        }
    }
}
