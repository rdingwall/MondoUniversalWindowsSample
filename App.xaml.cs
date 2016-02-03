using System;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Mondo;
using MondoUniversalWindowsSample.ViewModels;
using MondoUniversalWindowsSample.Views;

namespace MondoUniversalWindowsSample
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size { Height = 900, Width = 600 };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // smooth merchant thumbnail image resizing
            new BitmapTransform().InterpolationMode = BitmapInterpolationMode.Cubic;

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                var navigationService = new NavigationService(rootFrame);
                var loginPageViewModel = new LoginPageViewModel();
                var accountPageViewModel = new AccountPageViewModel();
                var schedulerService = new SchedulerService();

                var mondoAuthorizationClient = new MondoAuthorizationClient("YOUR_CLIENT_ID_HERE", "YOUR_CLIENT_SECRET_HERE", "https://production-api.gmon.io");

                var appController = new AppController(navigationService, mondoAuthorizationClient, loginPageViewModel, accountPageViewModel, schedulerService);

                appController.Start();
                rootFrame.Navigate(typeof (LoginPage), e.Arguments);
                rootFrame.DataContext = loginPageViewModel;
            }

            Window.Current.Activate();
        }

        private static void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
