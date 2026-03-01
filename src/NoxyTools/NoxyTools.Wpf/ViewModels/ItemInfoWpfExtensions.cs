using Noxypedia.Model;
using NoxyTools.Core.Model;
using NoxyTools.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// ItemSet → FlowDocument 변환 (정보 탭, 테크 탭)
/// </summary>
internal static class ItemInfoWpfExtensions
{
    // 직업 이름 → EClassFlags 매핑 (SearchItemViewModel.ClassMap 복제)
    private static readonly IReadOnlyDictionary<string, EClassFlags> _classNames =
        new Dictionary<string, EClassFlags>
        {
            ["공용"]     = EClassFlags.Common,
            ["기사"]     = EClassFlags.Knight,
            ["마법사"]   = EClassFlags.Wizard,
            ["힐러"]     = EClassFlags.Priest,
            ["궁수"]     = EClassFlags.Archer,
            ["드루이드"] = EClassFlags.Druid,
            ["용술사"]   = EClassFlags.Summoner
        };

    // ── 공통 브러시 ─────────────────────────────────────────────────────────

    private static readonly SolidColorBrush FgDefault =
        Freeze(new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)));
    private static readonly SolidColorBrush FgPositive =
        Freeze(new SolidColorBrush(Color.FromRgb(0x32, 0xCD, 0x32)));  // LimeGreen
    private static readonly SolidColorBrush FgNegative =
        Freeze(new SolidColorBrush(Color.FromRgb(0xDC, 0x14, 0x3C)));  // Crimson
    private static readonly SolidColorBrush FgOrange =
        Freeze(new SolidColorBrush(Color.FromRgb(0xFF, 0x69, 0x00)));  // OrangeRed
    private static readonly SolidColorBrush FgSecondary =
        Freeze(new SolidColorBrush(Color.FromRgb(0x99, 0x99, 0x99)));
    private static readonly SolidColorBrush BgDoc =
        Freeze(new SolidColorBrush(Color.FromRgb(0x1E, 0x1E, 0x1E)));

    // ── 기본 정보 탭 ────────────────────────────────────────────────────────

    /// <summary>아이템 기본 정보(직업/스탯/고유옵션) FlowDocument 생성</summary>
    public static FlowDocument BuildInfoDocument(
        ItemSet item,
        StatisticsService statistics)
    {
        var doc = MakeDoc();

        // 착용 가능 직업
        {
            var p = Bold("　[착용 가능 직업]", doc);
            doc.Blocks.Add(p);
            if (!item.WearableClass.HasFlag(EClassFlags.Common))
            {
                var classes = _classNames
                    .Where(kv => item.WearableClass.HasFlag(kv.Value))
                    .Select(kv => kv.Key)
                    .ToList();
                doc.Blocks.Add(Para($"「{string.Join(", ", classes)}」"));
            }
            else
            {
                doc.Blocks.Add(Para("「공용」"));
            }
            doc.Blocks.Add(EmptyPara());
        }

        // 스탯
        if (!item.IsUnidentified)
        {
            bool hasStats = item.Strength.HasValue || item.Agility.HasValue
                || item.Inteligence.HasValue || item.HP.HasValue
                || item.MP.HasValue || item.Attack.HasValue || item.Armor.HasValue;

            if (hasStats)
            {
                doc.Blocks.Add(Bold("　[추가 스텟]", doc));
                const int width = 5;
                const StatisticsService.Align align = StatisticsService.Align.Left;

                void AppendStat(string label, int? val, string? suffix = null)
                {
                    if (!val.HasValue) return;
                    int v = val.Value;
                    string padL = statistics.GetLeftSpacing(label, width, align);
                    string padR = statistics.GetRightSpacing(label, width, align);
                    var para = new Paragraph { Margin = new Thickness(0) };
                    AppendDefault(para, padL + label + padR + ": ");
                    para.Inlines.Add(new Run($"{v:+#,0;-#,0;}")
                        { Foreground = v >= 0 ? FgPositive : FgNegative });
                    if (suffix != null && v > 0)
                        AppendDefault(para, "\t" + suffix);
                    doc.Blocks.Add(para);
                }

                AppendStat("힘",    item.Strength,
                    $"(체력　: {item.Strength.GetValueOrDefault() * ItemSimulatorService.HP_PER_STATE:+#,0;-#,0;})");
                AppendStat("민첩",  item.Agility,
                    $"(방어력: {item.Agility.GetValueOrDefault()   * ItemSimulatorService.ARMOR_PER_STATE:+#,0;-#,0;})");
                AppendStat("지능",  item.Inteligence,
                    $"(마나　: {item.Inteligence.GetValueOrDefault() * ItemSimulatorService.MP_PER_STATE:+#,0;-#,0;})");
                AppendStat("체력",  item.HP);
                AppendStat("마나",  item.MP);
                AppendStat("공격력", item.Attack);
                AppendStat("방어력", item.Armor);
                doc.Blocks.Add(EmptyPara());
            }

            // 고유 옵션
            if (item.UniqueOptions.Count > 0)
            {
                doc.Blocks.Add(Bold("　[고유 옵션]", doc));
                foreach (var opt in item.UniqueOptions)
                {
                    doc.Blocks.Add(Para($"「{opt.Name}」"));
                    if (!string.IsNullOrWhiteSpace(opt.EffectDescription))
                        doc.Blocks.Add(Para($"　- {opt.EffectDescription}", FgSecondary));
                }
            }
        }
        else
        {
            // 미확인 아이템
            var p = Bold("　[옵션 미확인 아이템]", doc);
            p.Foreground = FgOrange;
            doc.Blocks.Add(p);
        }

        return doc;
    }

    // ── 획득 정보 탭 ────────────────────────────────────────────────────────

    /// <summary>레시피 한 줄 텍스트 생성 (기존 txbCraftRecipe 내용)</summary>
    public static string BuildCraftRecipeText(ItemSet item)
    {
        if (item.BeforeItems.Count == 0) return string.Empty;

        var before = item.BeforeItems[0];
        var sb = new System.Text.StringBuilder();
        sb.Append($"「{before.CraftRecipe.Location.Name}」\t[{item.Grade.Name}]{item.Name} = ");
        sb.Append($"([{before.Grade.Name}]{before.Name}");
        for (int i = 1; i < item.BeforeItems.Count; i++)
            sb.Append($" or [{item.BeforeItems[i].Grade.Name}]{item.BeforeItems[i].Name}");
        sb.Append(")");
        foreach (var mat in before.CraftRecipe.Materials)
            sb.Append($" + {mat.Name}");
        if (before.CraftRecipe.SubstituteMaterials.Count > 0)
        {
            sb.Append(" + (");
            sb.Append(string.Join(" or ", before.CraftRecipe.SubstituteMaterials.First().Select(s => s.Name)));
            sb.Append(")");
        }
        if (before.CraftRecipe.SuccessProbability.HasValue)
            sb.Append($" [성공률: {before.CraftRecipe.SuccessProbability.Value:0}%]");
        return sb.ToString();
    }

    /// <summary>드롭 크리프 정보 FlowDocument</summary>
    public static FlowDocument BuildDropCreepsDocument(
        IEnumerable<CreepSet> creeps,
        string regionName)
    {
        var doc = MakeDoc();
        var filtered = creeps.Where(c => c.Regions.Any(r => r.Name == regionName));
        foreach (var creep in filtered)
        {
            doc.Blocks.Add(Para($"〔 {creep.Name} 〕"));
            doc.Blocks.Add(Separator());
        }
        return doc;
    }

    // ── 테크 탭 ─────────────────────────────────────────────────────────────

    /// <summary>테크 트리 텍스트 FlowDocument 생성</summary>
    public static FlowDocument BuildTechDocument(
        ItemSet currentItem,
        ItemSet beginItem,
        ItemSet finalItem,
        bool detailOption)
    {
        var doc = MakeDoc();
        if (beginItem == null || finalItem == null) return doc;

        // 헤더
        var header = new Paragraph { Margin = new Thickness(0) };
        AppendDefault(header, $"[{beginItem.Grade.Name}]{beginItem.Name}");
        if (beginItem.DropCreeps.Count > 0)
        {
            string drops = string.Join(", ", beginItem.DropCreeps.Select(d => d.Name));
            AppendDefault(header, $" (드랍: {drops})");
        }
        doc.Blocks.Add(header);
        doc.Blocks.Add(Separator());

        if (beginItem.Name == finalItem.Name) return doc;

        // 테크 경로 계산 (WinForms와 동일 로직)
        var techItems = BuildTechPath(currentItem, beginItem, finalItem);
        var allNames  = new HashSet<string>(techItems.Select(i => i.Name));
        string nl     = detailOption ? "\n\t" : string.Empty;

        foreach (var item in techItems)
        {
            if (item.CraftDestinations.Count == 0) continue;

            ItemSet dest = item.CraftDestinations.Find(d => allNames.Contains(d.Name)) ?? finalItem;

            var para = new Paragraph { Margin = new Thickness(0) };
            AppendDefault(para, $"「{item.CraftRecipe.Location.Name}」\t[{dest.Grade.Name}]{dest.Name} = ");
            AppendDefault(para, "{" + string.Join(" or ", dest.BeforeItems.Select(b => $"[{b.Grade.Name}]{b.Name}")) + "}");

            if (item.CraftRecipe.Materials.Count > 0)
                AppendDefault(para, " + " + nl);

            string MatText(ItemSet m)
            {
                string drop = detailOption && m.DropCreeps.Count > 0
                    ? $" (드랍: {string.Join(", ", m.DropCreeps.Select(d => d.Name))})" : "";
                return m.Name + drop;
            }

            AppendDefault(para, string.Join(" + " + nl, item.CraftRecipe.Materials.Select(MatText)));

            if (item.CraftRecipe.SubstituteMaterials.Count > 0)
            {
                AppendDefault(para, " + {");
                AppendDefault(para, string.Join(" or ", item.CraftRecipe.SubstituteMaterials.First().Select(MatText)));
                AppendDefault(para, "}" + nl);
            }

            if (item.CraftRecipe.SuccessProbability.HasValue)
                AppendDefault(para, $" [성공률: {item.CraftRecipe.SuccessProbability.Value:0}%]");

            doc.Blocks.Add(para);
            doc.Blocks.Add(Separator());
        }

        return doc;
    }

    /// <summary>등급 목록 FlowDocument (Tech 탭 우측 등급 색상 범례)</summary>
    public static FlowDocument BuildTechGradeDocument(IEnumerable<ItemGradeSet> grades)
    {
        var doc = MakeDoc();
        foreach (var grade in grades.OrderBy(g => g.GradeOrder))
        {
            var para = new Paragraph { Margin = new Thickness(0) };
            var brush = GradeBrush(grade.Color);
            para.Inlines.Add(new Run($"● {grade.Name}") { Foreground = brush });
            doc.Blocks.Add(para);
        }
        return doc;
    }

    /// <summary>
    /// 장비 시뮬레이터 유니크 옵션 합산 표시용 FlowDocument 생성.
    /// </summary>
    public static FlowDocument BuildUniqueOptionsDocument(IEnumerable<UniqueOptionSet> options)
    {
        var doc = MakeDoc();
        const string OPTION_DODGE        = "회피";
        const string OPTION_MAGIC_REGIST = "마방";

        var optList = options.ToList();
        bool dupDodge       = optList.Count(o => o.Name.Contains(OPTION_DODGE))        > 1;
        bool dupMagicRegist = optList.Count(o => o.Name.Contains(OPTION_MAGIC_REGIST)) > 1;

        foreach (var group in optList.GroupBy(o => o.Name).OrderBy(g => g.Key))
        {
            var para = new Paragraph { Margin = new Thickness(0) };
            para.Inlines.Add(new Run($"「{group.Key}」")
                { FontWeight = System.Windows.FontWeights.Bold, Foreground = FgDefault });
            var desc = group.First().EffectDescription ?? "";
            if (!string.IsNullOrWhiteSpace(desc))
                para.Inlines.Add(new Run($" {desc}")
                    { FontSize = 10, Foreground = FgSecondary });

            if (group.Count() > 1)
                para.Inlines.Add(new Run($" (중복: {group.Count()}개)")
                    { Foreground = FgOrange });
            if (dupDodge && group.Key.Contains(OPTION_DODGE))
                para.Inlines.Add(new Run(" (회피 중복)")
                    { Foreground = FgOrange });
            if (dupMagicRegist && group.Key.Contains(OPTION_MAGIC_REGIST))
                para.Inlines.Add(new Run(" (마방 중복)")
                    { Foreground = FgOrange });

            doc.Blocks.Add(para);
        }
        return doc;
    }

    // ── 내부 헬퍼 ───────────────────────────────────────────────────────────

    private static List<ItemSet> BuildTechPath(ItemSet currentItem, ItemSet beginItem, ItemSet finalItem)
    {
        var techItems = new List<ItemSet>();

        // 시작 → 현재
        ItemSet tech = beginItem;
        while (tech.CraftDestinations.Count > 0)
        {
            techItems.Add(tech);
            if (tech.CraftDestinations.Any(d => d.Name == currentItem.Name))
            {
                techItems.Add(currentItem);
                break;
            }
            if (tech.CraftDestinations.Count > 1) break;
            tech = tech.CraftDestinations.First();
        }

        // 끝 → 현재 역방향 채우기
        string lastName = techItems.Count > 0 ? techItems.Last().Name : string.Empty;
        if (finalItem.BeforeItems.Any(b => b.Name == lastName) == false && lastName != finalItem.Name)
        {
            var rev = new List<ItemSet>();
            ItemSet t = finalItem;
            while (t.BeforeItems.Count > 0)
            {
                if (!techItems.Contains(t)) rev.Add(t);
                if (t.BeforeItems.Any(b => b.Name == currentItem.Name)) break;
                t = t.BeforeItems.First();
                if (t.Name == lastName) break;
            }
            rev.Reverse();
            techItems.AddRange(rev);
        }

        return techItems;
    }

    private static FlowDocument MakeDoc() => new()
    {
        FontFamily = new System.Windows.Media.FontFamily("맑은 고딕, Malgun Gothic"),
        FontSize   = 12,
        Background = BgDoc,
        Foreground = FgDefault,
        PagePadding = new Thickness(8),
        LineHeight  = 18
    };

    private static Paragraph Para(string text, SolidColorBrush? fg = null)
    {
        var p = new Paragraph(new Run(text) { Foreground = fg ?? FgDefault })
            { Margin = new Thickness(0) };
        return p;
    }

    private static Paragraph Bold(string text, FlowDocument doc)
    {
        var p = new Paragraph(new Run(text)
            { Foreground = FgDefault, FontWeight = System.Windows.FontWeights.Bold })
            { Margin = new Thickness(0) };
        return p;
    }

    private static Paragraph EmptyPara() =>
        new Paragraph(new Run("")) { Margin = new Thickness(0) };

    private static Paragraph Separator() =>
        Para("_______________________________________________", FgSecondary);

    private static void AppendDefault(Paragraph para, string text) =>
        para.Inlines.Add(new Run(text) { Foreground = FgDefault });

    private static SolidColorBrush GradeBrush(System.Drawing.Color c)
    {
        byte a = c.A == 0 ? (byte)255 : c.A;
        return Freeze(new SolidColorBrush(
            Color.FromArgb(a, c.R, c.G, c.B)));
    }

    private static T Freeze<T>(T obj) where T : System.Windows.Freezable
    { obj.Freeze(); return obj; }
}
