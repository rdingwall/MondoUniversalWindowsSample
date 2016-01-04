using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MondoUniversalWindowsSample.Controls
{
    public sealed class BalanceBox : Control
    {
        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(
            "Amount", typeof(string), typeof(BalanceBox), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            "Label", typeof(string), typeof(BalanceBox), new PropertyMetadata(default(string)));

        public BalanceBox()
        {
            DefaultStyleKey = typeof(BalanceBox);
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public string Amount
        {
            get { return (string)GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
        }
    }
}
