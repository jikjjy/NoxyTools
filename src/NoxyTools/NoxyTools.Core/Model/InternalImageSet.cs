using System.Threading;
using System.Windows.Media.Imaging;

namespace NoxyTools.Core.Model
{
    internal class InternalImageSet
    {
        public string URL { get; set; } = string.Empty;
        public BitmapSource? Image { get; set; } = null;

        /// <summary>0 = 미요청, 1 = 요청됨(또는 완료). Interlocked CAS 용.</summary>
        public int IsRequested = 0;

        /// <summary>true = 이미지 로드 완료</summary>
        public bool IsLoaded = false;
    }
}
