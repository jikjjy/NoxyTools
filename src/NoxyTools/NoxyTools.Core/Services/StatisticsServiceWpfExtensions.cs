using NoxyTools.Core.Model;
using SaveParser;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace NoxyTools.Core.Services
{
    /// <summary>
    /// StatisticsService의 WPF 전용 확장.
    /// FlowDocument 생성 및 RTF 직렬화를 담당합니다.
    /// </summary>
    public static class StatisticsServiceWpfExtensions
    {
        private static readonly SolidColorBrush DefaultFg =
            Freeze(new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)));
        private static readonly SolidColorBrush SeparatorFg =
            Freeze(new SolidColorBrush(Color.FromRgb(0x60, 0x60, 0x60)));
        private static readonly SolidColorBrush GradeNameBg =
            Freeze(new SolidColorBrush(Color.FromRgb(0x1E, 0x1E, 0x1E)));
        private static readonly SolidColorBrush DocBg =
            Freeze(new SolidColorBrush(Color.FromRgb(0x1E, 0x1E, 0x1E)));

        // ── Public API ────────────────────────────────────────────

        /// <summary>
        /// StatisticsService 데이터를 WPF FlowDocument 형태로 내보냅니다.
        /// </summary>
        public static FlowDocument ExportToFlowDocument(
            this StatisticsService statistics,
            ConfigService config)
        {
            var doc = new FlowDocument
            {
                FontFamily = new FontFamily("맑은 고딕, Malgun Gothic"),
                FontSize = 12,
                Background = DocBg,
                Foreground = DefaultFg,
                PagePadding = new Thickness(8),
                LineHeight = 18
            };

            if (!statistics.IsLoaded)
            {
                doc.Blocks.Add(MakePara("세이브 파일을 읽는 중...", DefaultFg));
                return doc;
            }

            var getItems = statistics.NewItems.ToList();
            var statisticsData = config.MakeValidReport.StatisticsData;

            // ── 헤더 ─────────────────────────────────────────────
            doc.Blocks.Add(MakePara($"아　　이　　디 : {statisticsData.ID}", DefaultFg));
            doc.Blocks.Add(MakePara($"플 레 이　버 젼 : {statisticsData.PlayVersion}", DefaultFg));
            doc.Blocks.Add(MakePara($"클　　래　　스 : {statisticsData.ClassName}", DefaultFg));
            doc.Blocks.Add(MakePara($"서　　　　　버 : {statisticsData.ServerName}", DefaultFg));
            doc.Blocks.Add(MakeSeparator());

            // ── 스탯 ─────────────────────────────────────────────
            doc.Blocks.Add(MakePara("[획득 시만 기입]", DefaultFg));
            CharacterStateSet deltaState = new CharacterStateSet();
            CharacterStateSet totalState = new CharacterStateSet();
            var delta = statistics.StateDelta;
            if (config.MakeValidReport.UseStateStatisticsExport
                && (delta.Strength > 0 || delta.Agility > 0 || delta.Intelligence > 0))
            {
                deltaState = delta;
                totalState = statistics.NewData;
            }
            doc.Blocks.Add(MakePara(
                $"획득한 힘/민첩/지능 : {deltaState.Strength}/{deltaState.Agility}/{deltaState.Intelligence}",
                DefaultFg));
            doc.Blocks.Add(MakePara(
                $"힘/민첩/지능 총량 : {totalState.Strength}/{totalState.Agility}/{totalState.Intelligence}",
                DefaultFg));
            doc.Blocks.Add(MakeSeparator());

            // ── 획득 아이템 목록 (최소 10줄) ─────────────────────
            doc.Blocks.Add(MakePara(
                "[본 리플레이 인증 아이템 명 기입란 - 10개 이상 시 자유 편집]", DefaultFg));

            var rootItems = statistics.OrderItems(config, getItems);
            int listCount = Math.Max(rootItems.Count, 10);
            for (int i = 0; i < listCount; i++)
            {
                var para = new Paragraph { Margin = new Thickness(0) };
                para.Inlines.Add(new Run($"{i + 1:00}. ") { Foreground = DefaultFg });
                if (i < rootItems.Count)
                    AppendItemInlines(statistics, para, rootItems[i]);
                doc.Blocks.Add(para);
            }
            doc.Blocks.Add(MakeSeparator());

            // ── 등급별 카운트 ─────────────────────────────────────
            doc.Blocks.Add(MakePara("[인증 아이템 총 갯수 카운트]", DefaultFg));

            var oldByGrade = statistics.OldItems
                .GroupBy(k => k.GradeIndex, v => v)
                .OrderBy(g => g.Key)
                .ToArray();
            var newByGrade = getItems
                .GroupBy(k => k.GradeIndex, v => v)
                .OrderBy(g => g.Key)
                .ToArray();

            int oi = 0, ni = 0;
            for (EItemGrade g = EItemGrade.오메가; g < EItemGrade.Last; g++)
            {
                string gradeName = g.ToString().Replace("_", "－");
                const int space = 7;
                string title = statistics.GetLeftSpacing(gradeName, space, StatisticsService.Align.Left)
                    + gradeName
                    + statistics.GetRightSpacing(gradeName, space, StatisticsService.Align.Left);

                int oldCount = 0;
                if (oi < oldByGrade.Length && oldByGrade[oi].Key == g)
                { oldCount = oldByGrade[oi].Count(); oi++; }

                int newCount = 0;
                if (ni < newByGrade.Length && newByGrade[ni].Key == g)
                { newCount = newByGrade[ni].Count(); ni++; }

                string suffix = newCount > 0 ? $"(+{newCount})" : string.Empty;
                doc.Blocks.Add(MakePara($"{title}: {oldCount,-2} {suffix}", DefaultFg));
            }
            doc.Blocks.Add(MakeSeparator());

            // ── 자유 사용란 ──────────────────────────────────────
            doc.Blocks.Add(MakePara("[이하 기타 자유 사용란]", DefaultFg));

            if (config.MakeValidReport.UseAddInfoSaveCode)
            {
                var saveFile = statistics.LastSaveFile;
                if (saveFile?.FileInfo.Exists == true)
                {
                    doc.Blocks.Add(MakePara(
                        File.ReadAllText(saveFile.FileInfo.FullName), SeparatorFg));
                }
            }

            if (config.MakeValidReport.UseAddInfoAllItems)
            {
                var allItems = statistics.OrderItems(
                    config,
                    statistics.OldItems.Concat(getItems).ToList());
                foreach (var item in allItems)
                {
                    var para = new Paragraph { Margin = new Thickness(0) };
                    AppendItemInlines(statistics, para, item);
                    doc.Blocks.Add(para);
                }
            }

            return doc;
        }

        /// <summary>
        /// FlowDocument를 RTF 문자열로 직렬화합니다 (클립보드 복사용).
        /// </summary>
        public static string FlowDocumentToRtf(FlowDocument doc)
        {
            var range = new TextRange(doc.ContentStart, doc.ContentEnd);
            using var ms = new MemoryStream();
            range.Save(ms, DataFormats.Rtf);
            // RTF는 ANSI 기반 포맷. ASCII보다 Encoding.Default(현재 시스템 코드페이지)를
            // 사용해야 한글 등 비ASCII 바이트 손상을 방지합니다.
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 검증 보고서를 HTML 프래그먼트로 내보냅니다.
        /// 등급색 인라인 스타일을 포함하므로 네이버 카페 등 웹 에디터에서 색상이 보존됩니다.
        /// </summary>
        public static string ExportToHtml(this StatisticsService statistics, ConfigService config)
        {
            var sb = new StringBuilder();

            static string E(string s) => WebUtility.HtmlEncode(s);
            static string Hex(System.Drawing.Color c)
            {
                byte a = c.A == 0 ? (byte)255 : c.A;
                return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
            }

            sb.Append("<div style=\"font-family:'맑은 고딕','Malgun Gothic',sans-serif;font-size:11pt;line-height:1.6;\">");

            if (!statistics.IsLoaded)
            {
                sb.Append("<div>세이브 파일을 읽는 중...</div></div>");
                return sb.ToString();
            }

            var sd       = config.MakeValidReport.StatisticsData;
            var getItems = statistics.NewItems.ToList();

            void Line(string text) => sb.Append($"<div>{E(text)}</div>");
            void Sep()             => sb.Append("<div>_______________________________________________</div>");
            void ItemLine(ItemSet item)
            {
                string gc        = Hex(item.GradeColor);
                string gradeName = $"［{item.GradeName.Replace("_", "－")}］";
                string lead      = statistics.GetLeftSpacing(gradeName,  9, StatisticsService.Align.Right);
                string trail     = statistics.GetRightSpacing(gradeName, 9, StatisticsService.Align.Right);
                sb.Append("<div>");
                if (lead.Length  > 0) sb.Append(E(lead));
                sb.Append($"<span style=\"color:{gc};font-weight:bold;background-color:#1e1e1e\">{E(gradeName)}</span>");
                if (trail.Length > 0) sb.Append(E(trail));
                sb.Append($"　{E(item.Name)}</div>");
            }

            // ── 헤더 ─────────────────────────────────────────────
            Line($"아　　이　　디 : {sd.ID}");
            Line($"플 레 이　버 젼 : {sd.PlayVersion}");
            Line($"클　　래　　스 : {sd.ClassName}");
            Line($"서　　　　　버 : {sd.ServerName}");
            Sep();

            // ── 스탯 ─────────────────────────────────────────────
            Line("[획득 시만 기입]");
            CharacterStateSet deltaState = new(), totalState = new();
            var delta = statistics.StateDelta;
            if (config.MakeValidReport.UseStateStatisticsExport
                && (delta.Strength > 0 || delta.Agility > 0 || delta.Intelligence > 0))
            { deltaState = delta; totalState = statistics.NewData; }
            Line($"획득한 힘/민첩/지능 : {deltaState.Strength}/{deltaState.Agility}/{deltaState.Intelligence}");
            Line($"힘/민첩/지능 총량 : {totalState.Strength}/{totalState.Agility}/{totalState.Intelligence}");
            Sep();

            // ── 획득 아이템 목록 ──────────────────────────────────
            Line("[본 리플레이 인증 아이템 명 기입란 - 10개 이상 시 자유 편집]");
            var rootItems = statistics.OrderItems(config, getItems);
            int listCount = Math.Max(rootItems.Count, 10);
            for (int i = 0; i < listCount; i++)
            {
                sb.Append($"<div>{i + 1:00}. ");
                if (i < rootItems.Count)
                {
                    var item     = rootItems[i];
                    string gc    = Hex(item.GradeColor);
                    string gn    = $"［{item.GradeName.Replace("_", "－")}］";
                    string lead  = statistics.GetLeftSpacing(gn,  9, StatisticsService.Align.Right);
                    string trail = statistics.GetRightSpacing(gn, 9, StatisticsService.Align.Right);
                    if (lead.Length  > 0) sb.Append(E(lead));
                    sb.Append($"<span style=\"color:{gc};font-weight:bold;background-color:#1e1e1e\">{E(gn)}</span>");
                    if (trail.Length > 0) sb.Append(E(trail));
                    sb.Append($"　{E(item.Name)}");
                }
                sb.Append("</div>");
            }
            Sep();

            // ── 등급별 카운트 ─────────────────────────────────────
            Line("[인증 아이템 총 갯수 카운트]");
            var oldByGrade = statistics.OldItems.GroupBy(k => k.GradeIndex).OrderBy(g => g.Key).ToArray();
            var newByGrade = getItems.GroupBy(k => k.GradeIndex).OrderBy(g => g.Key).ToArray();
            int oi = 0, ni = 0;
            for (EItemGrade g = EItemGrade.오메가; g < EItemGrade.Last; g++)
            {
                string gn = g.ToString().Replace("_", "－");
                const int space = 7;
                string title = statistics.GetLeftSpacing(gn,  space, StatisticsService.Align.Left)
                    + gn
                    + statistics.GetRightSpacing(gn, space, StatisticsService.Align.Left);
                int oldCount = 0;
                if (oi < oldByGrade.Length && oldByGrade[oi].Key == g) { oldCount = oldByGrade[oi].Count(); oi++; }
                int newCount = 0;
                if (ni < newByGrade.Length && newByGrade[ni].Key == g) { newCount = newByGrade[ni].Count(); ni++; }
                string suffix = newCount > 0 ? $"(+{newCount})" : string.Empty;
                Line($"{title}: {oldCount,-2} {suffix}");
            }
            Sep();

            // ── 자유 사용란 ──────────────────────────────────────
            Line("[이하 기타 자유 사용란]");
            if (config.MakeValidReport.UseAddInfoSaveCode)
            {
                var saveFile = statistics.LastSaveFile;
                if (saveFile?.FileInfo.Exists == true)
                {
                    var saveText = File.ReadAllText(saveFile.FileInfo.FullName);
                    foreach (var saveLine in saveText.Split('\n'))
                        Line(saveLine.TrimEnd('\r'));
                }
            }
            if (config.MakeValidReport.UseAddInfoAllItems)
            {
                var allItems = statistics.OrderItems(config, statistics.OldItems.Concat(getItems).ToList());
                foreach (var item in allItems) ItemLine(item);
            }

            sb.Append("</div>");
            return sb.ToString();
        }

        /// <summary>
        /// HTML 프래그먼트를 Windows CF_HTML 클립보드 형식으로 래핑합니다.
        /// </summary>
        public static string WrapAsCfHtml(string htmlFragment)
        {
            const string tpl  = "Version:0.9\r\nStartHTML:{0:D10}\r\nEndHTML:{1:D10}\r\nStartFragment:{2:D10}\r\nEndFragment:{3:D10}\r\n";
            const string pre  = "<html><body><!--StartFragment-->";
            const string post = "<!--EndFragment--></body></html>";

            // 오프셋은 UTF-8 바이트 기준
            int hLen          = Encoding.UTF8.GetByteCount(string.Format(tpl, 0, 0, 0, 0));
            int startHtml     = hLen;
            int startFragment = hLen + Encoding.UTF8.GetByteCount(pre);
            int endFragment   = startFragment + Encoding.UTF8.GetByteCount(htmlFragment);
            int endHtml       = endFragment + Encoding.UTF8.GetByteCount(post);

            return string.Format(tpl, startHtml, endHtml, startFragment, endFragment)
                + pre + htmlFragment + post;
        }

        // ── 내부 헬퍼 ─────────────────────────────────────────────

        private static void AppendItemInlines(
            StatisticsService statistics, Paragraph para, ItemSet item)
        {
            const int space = 9;
            string gradeName = $"［{item.GradeName.Replace("_", "－")}］";
            var gradeBrush = DrawingColorToWpfBrush(item.GradeColor);

            // 앞 공백 (Right align)
            string lead = statistics.GetLeftSpacing(gradeName, space, StatisticsService.Align.Right);
            if (lead.Length > 0)
                para.Inlines.Add(new Run(lead) { Foreground = DefaultFg });

            // 등급명 (색상 + 볼드 + 등급색 기반 반투명 배경)
            var gradeBgBrush = DrawingColorToWpfBgBrush(item.GradeColor);
            para.Inlines.Add(new Run(gradeName)
            {
                Foreground = gradeBrush,
                FontWeight = FontWeights.Bold,
                Background = gradeBgBrush
            });

            // 뒤 공백 (Right align)
            string trail = statistics.GetRightSpacing(gradeName, space, StatisticsService.Align.Right);
            if (trail.Length > 0)
                para.Inlines.Add(new Run(trail) { Foreground = DefaultFg });

            // 아이템 이름
            para.Inlines.Add(new Run($"　{item.Name}") { Foreground = DefaultFg });
        }

        private static SolidColorBrush DrawingColorToWpfBrush(System.Drawing.Color c)
        {
            // Drawing.Color의 A=0은 완전 투명이므로 불투명으로 보정
            byte a = c.A == 0 ? (byte)255 : c.A;
            return Freeze(new SolidColorBrush(Color.FromArgb(a, c.R, c.G, c.B)));
        }

        /// <summary>등급색을 ~22% 불투명의 배경 브러시로 변환합니다.</summary>
        private static SolidColorBrush DrawingColorToWpfBgBrush(System.Drawing.Color c)
            => Freeze(new SolidColorBrush(Color.FromArgb(56, c.R, c.G, c.B)));

        private static Paragraph MakePara(string text, SolidColorBrush fg)
            => new Paragraph(new Run(text) { Foreground = fg }) { Margin = new Thickness(0) };

        private static Paragraph MakeSeparator()
            => MakePara("_______________________________________________", SeparatorFg);

        private static SolidColorBrush Freeze(SolidColorBrush brush)
        { brush.Freeze(); return brush; }
    }
}
