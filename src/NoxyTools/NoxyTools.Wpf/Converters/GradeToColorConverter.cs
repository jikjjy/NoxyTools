using Noxypedia.Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace NoxyTools.Wpf.Converters
{
    /// <summary>
    /// ItemGradeSet.Color (System.Drawing.Color) → WPF SolidColorBrush
    /// DataGrid 행의 전경색에 사용
    /// </summary>
    [ValueConversion(typeof(ItemGradeSet), typeof(Brush))]
    public class GradeToColorConverter : IValueConverter
    {
        // 다크 테마에서 텍스트 가시성을 위한 최소 HSL 밝기 (0~1)
        private const double MinLightness = 0.62;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ItemGradeSet grade && !grade.Color.IsEmpty)
            {
                var dc = grade.Color;

                // A == 0 이면 완전 투명 → 불투명 처리
                byte a = dc.A == 0 ? (byte)255 : dc.A;

                // R=G=B=0 이면 사실상 색상 미지정 → 기본 텍스트 색 반환
                if (dc.R == 0 && dc.G == 0 && dc.B == 0)
                    return SystemColors.ControlTextBrush;

                // 다크 테마 가시성: HSL 밝기가 MinLightness 미만이면 보정
                var (h, s, l) = RgbToHsl(dc.R, dc.G, dc.B);
                if (l < MinLightness)
                    l = MinLightness;
                var (r2, g2, b2) = HslToRgb(h, s, l);

                var brush = new SolidColorBrush(Color.FromArgb(a, r2, g2, b2));
                brush.Freeze();
                return brush;
            }
            return SystemColors.ControlTextBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        // ── HSL 변환 헬퍼 ─────────────────────────────────────────────────────

        private static (double h, double s, double l) RgbToHsl(byte r, byte g, byte b)
        {
            double rf = r / 255.0, gf = g / 255.0, bf = b / 255.0;
            double max = Math.Max(rf, Math.Max(gf, bf));
            double min = Math.Min(rf, Math.Min(gf, bf));
            double l = (max + min) / 2.0;
            double h = 0, s = 0;
            double delta = max - min;
            if (delta > 1e-9)
            {
                s = delta / (1.0 - Math.Abs(2.0 * l - 1.0));
                if (max == rf)      h = 60.0 * (((gf - bf) / delta) % 6.0);
                else if (max == gf) h = 60.0 * ((bf - rf) / delta + 2.0);
                else                h = 60.0 * ((rf - gf) / delta + 4.0);
                if (h < 0) h += 360.0;
            }
            return (h, s, l);
        }

        private static (byte r, byte g, byte b) HslToRgb(double h, double s, double l)
        {
            double c = (1.0 - Math.Abs(2.0 * l - 1.0)) * s;
            double x = c * (1.0 - Math.Abs((h / 60.0) % 2.0 - 1.0));
            double m = l - c / 2.0;
            double rf, gf, bf;
            if      (h < 60)  { rf = c; gf = x; bf = 0; }
            else if (h < 120) { rf = x; gf = c; bf = 0; }
            else if (h < 180) { rf = 0; gf = c; bf = x; }
            else if (h < 240) { rf = 0; gf = x; bf = c; }
            else if (h < 300) { rf = x; gf = 0; bf = c; }
            else              { rf = c; gf = 0; bf = x; }
            return (
                (byte)Math.Round((rf + m) * 255),
                (byte)Math.Round((gf + m) * 255),
                (byte)Math.Round((bf + m) * 255)
            );
        }
    }
}
