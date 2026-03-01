using System.Threading;
using System.Windows.Forms;

namespace NoxypediaEditor
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            Mutex mutex = new Mutex(true, "NoxypediaEditor", out createdNew);
            if (createdNew == false)
            {
                MessageBox.Show($"Noxypedia Editor가 이미 실행중입니다.");
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Main());
        }
    }
}
