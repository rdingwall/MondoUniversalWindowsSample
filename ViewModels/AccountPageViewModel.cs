using System.Collections.ObjectModel;

namespace MondoUniversalWindowsSample.ViewModels
{
    public sealed class AccountPageViewModel : ViewModelBase
    {
        private decimal _balance;
        private decimal _spentToday;
        private ObservableCollection<TransactionViewModel> _transactions = new ObservableCollection<TransactionViewModel>();
        private string _accountName;

        public string AccountName
        {
            get { return _accountName; }
            set { SetProperty(ref _accountName, value); }
        }

        public decimal Balance
        {
            get { return _balance; }
            set { SetProperty(ref _balance, value); }
        }

        public decimal SpentToday
        {
            get { return _spentToday; }
            set { SetProperty(ref _spentToday, value); }
        }

        public ObservableCollection<TransactionViewModel> Transactions
        {
            get { return _transactions; }
            set { SetProperty(ref _transactions, value); }
        }

        public ObservableCommand LogoutCommand { get; } = new ObservableCommand();
    }
}