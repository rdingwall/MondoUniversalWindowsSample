using Windows.UI.Xaml.Input;
using Mondo;

namespace MondoUniversalWindowsSample.ViewModels
{
    public sealed class LoginPageViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isBusy;
        private bool _isEnabled = true;
        private string _statusText;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        public ObservableCommand LoginCommand { get; } = new ObservableCommand();

        public ObservableCommand<KeyRoutedEventArgs> KeyPressCommand { get; } = new ObservableCommand<KeyRoutedEventArgs>();

        public AccessToken AccessToken { get; set; }
    }
}