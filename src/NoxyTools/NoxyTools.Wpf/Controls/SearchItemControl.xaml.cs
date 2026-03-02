using NoxyTools.Wpf.ViewModels;
using System.Windows.Controls;

namespace NoxyTools.Wpf.Controls
{
    /// <summary>
    /// SearchItemControl.xaml의 코드비하인드.
    /// 모든 로직은 SearchItemViewModel에 위임합니다.
    /// </summary>
    public partial class SearchItemControl : UserControl
    {
        public SearchItemControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// DataGrid 선택 변경 시 선택된 행이 화면에 보이도록 스크롤합니다.
        /// </summary>
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb && DataContext is SearchItemViewModel vm)
                vm.FilterText = tb.Text;
        }

        private void ItemDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dg && dg.SelectedItem != null)
            {
                // 가상화 환경에서는 Dispatcher로 지연 실행해야 합니다
                dg.Dispatcher.InvokeAsync(() =>
                    { if (dg.SelectedItem is not null) dg.ScrollIntoView(dg.SelectedItem); },
                    System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }
}
