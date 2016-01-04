namespace MondoUniversalWindowsSample.ViewModels
{
    public sealed class TransactionViewModel : ViewModelBase
    {
        private string _imageUrl;
        private string _description;
        private decimal _amount;

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { SetProperty(ref _imageUrl, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public decimal Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }
    }
}