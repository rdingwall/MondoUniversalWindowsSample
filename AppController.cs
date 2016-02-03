using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.System;
using Mondo;
using MondoUniversalWindowsSample.ViewModels;
using MondoUniversalWindowsSample.Views;

namespace MondoUniversalWindowsSample
{
    public sealed class AppController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly SerialDisposable _accessTokenRefreshDisposable = new SerialDisposable();
        private readonly LoginPageViewModel _loginPageViewModel;
        private readonly AccountPageViewModel _accountPageViewModel;
        private readonly IMondoAuthorizationClient _mondoAuthorizationClient;
        private readonly INavigationService _navigationService;
        private readonly ISchedulerService _schedulerService;

        public AppController(
            INavigationService navigationService,
            IMondoAuthorizationClient mondoAuthorizationClient,
            LoginPageViewModel loginPageViewModel,
            AccountPageViewModel accountPageViewModel,
            ISchedulerService schedulerService)
        {
            _navigationService = navigationService;
            _mondoAuthorizationClient = mondoAuthorizationClient;
            _loginPageViewModel = loginPageViewModel;
            _accountPageViewModel = accountPageViewModel;
            _schedulerService = schedulerService;
        }

        public void Start()
        {
            ObserveLoginClicked();
            ObserveLogoutKeyPress();
            ObserveLogoutClicked();
        }

        private void ObserveLogoutKeyPress()
        {
            _disposables.Add(_loginPageViewModel.KeyPressCommand.Subscribe(e =>
             {
                 if (e.Key == VirtualKey.Enter)
                 {
                     Login();
                 }
             }));
        }

        private void ObserveLoginClicked()
        {
            _disposables.Add(_loginPageViewModel.LoginCommand.Subscribe(_ => Login()));
        }

        private void ObserveLogoutClicked()
        {
            _disposables.Add(_accountPageViewModel.LogoutCommand.Subscribe(_ =>
            {
                _accessTokenRefreshDisposable.Disposable = null;
                _navigationService.NavigateTo<LoginPage>(_loginPageViewModel);
            }));
        }

        private void ScheduleAccessTokenRefresh()
        {
            DateTimeOffset refreshTime = DateTimeOffset.UtcNow.AddSeconds(_loginPageViewModel.AccessToken.ExpiresIn);

            _accessTokenRefreshDisposable.Disposable = Observable.Timer(refreshTime)
                .SubscribeOn(_schedulerService.TaskPool)
                .ObserveOn(_schedulerService.Dispatcher)
                .Subscribe(async _ =>
                {
                    _loginPageViewModel.AccessToken = await _mondoAuthorizationClient.RefreshAccessTokenAsync(_loginPageViewModel.AccessToken.RefreshToken);
                    ScheduleAccessTokenRefresh();
                });
        }

        private async void Login()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_loginPageViewModel.Username) ||
                    string.IsNullOrWhiteSpace(_loginPageViewModel.Password))
                {
                    _loginPageViewModel.ErrorMessage = "Please enter your username and password.";
                    return;
                }

                _loginPageViewModel.ErrorMessage = null;
                _loginPageViewModel.IsEnabled = false;
                _loginPageViewModel.IsBusy = true;

                _loginPageViewModel.StatusText = "Authenticating...";

                _loginPageViewModel.AccessToken = await _mondoAuthorizationClient.AuthenticateAsync(_loginPageViewModel.Username, _loginPageViewModel.Password);

                using (var mondoClient = new MondoClient(_loginPageViewModel.AccessToken.Value, "https://production-api.gmon.io"))
                {
                    ScheduleAccessTokenRefresh();

                    _loginPageViewModel.StatusText = "Fetching accounts...";
                    IList<Account> accounts = await mondoClient.GetAccountsAsync();

                    _loginPageViewModel.StatusText = "Fetching balance...";
                    Balance balance = await mondoClient.GetBalanceAsync(accounts[0].Id);

                    _loginPageViewModel.StatusText = "Fetching transactions...";
                    IList<Transaction> transactions =
                        await mondoClient.GetTransactionsAsync(accounts[0].Id, expand: "merchant");

                    _accountPageViewModel.AccountName = accounts[0].Description;
                    _accountPageViewModel.Balance = balance.Value/100m;
                    _accountPageViewModel.SpentToday = Math.Abs(balance.SpendToday/100m);

                    foreach (Transaction transaction in transactions.OrderByDescending(t => t.Created))
                    {
                        var transactionViewModel = new TransactionViewModel();

                        transactionViewModel.Amount = transaction.Amount/100m;
                        transactionViewModel.ImageUrl = transaction.Merchant?.Logo;
                        transactionViewModel.Description = transaction.Merchant?.Name ?? transaction.Description;

                        _accountPageViewModel.Transactions.Add(transactionViewModel);
                    }

                    _navigationService.NavigateTo<AccountSummaryPage>(_accountPageViewModel);

                    _loginPageViewModel.Password = null;
                    _loginPageViewModel.Username = null;
                }
            }
            catch (Exception ex)
            {
                _loginPageViewModel.ErrorMessage = ex.Message;
            }
            finally
            {
                _loginPageViewModel.StatusText = null;
                _loginPageViewModel.IsBusy = false;
                _loginPageViewModel.IsEnabled = true;
            }
        }

        public void Dispose()
        {
            _accessTokenRefreshDisposable.Dispose();
            _disposables.Dispose();
        }
    }
}