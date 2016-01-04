using Windows.UI.Xaml.Controls;

namespace MondoUniversalWindowsSample
{
    public interface INavigationService
    {
        void NavigateTo<TPage>(object viewModel) where TPage : Page;
    }

    public sealed class NavigationService : INavigationService
    {
        private readonly Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo<TPage>(object viewModel) where TPage : Page
        {
            _frame.Navigate(typeof(TPage));
            _frame.DataContext = viewModel;
        }
    }
}