using System.Windows.Forms;

namespace NoxypediaEditor
{
    public static partial class Utils
    {
        public static void CheckKeyPressFilteringForbiddenCharacters(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '[':
                case ']':
                case '\'':
                case '&':
                case '*':
                case '%':
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }
    }
}
