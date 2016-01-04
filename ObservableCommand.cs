using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace MondoUniversalWindowsSample
{
    public sealed class ObservableCommand : ObservableCommand<Unit>
    {
        public ObservableCommand(IObservable<bool> canExecute) : base(canExecute)
        {
        }

        public ObservableCommand()
        {
        }
    }

    public class ObservableCommand<TParam> : ICommand, IDisposable, IObservable<TParam>
    {
        private readonly BehaviorSubject<bool> _canExecuteSubject = new BehaviorSubject<bool>(false);
        private readonly Subject<TParam> _values = new Subject<TParam>();
        private readonly IDisposable _subscription;
        private readonly Func<TParam, bool> _canExecuteFunc;

        private bool _enabled = true;

        public ObservableCommand(IObservable<bool> canExecute)
        {
            _subscription = canExecute
                .Subscribe(value =>
                {
                    _canExecuteSubject.OnNext(value);
                    OnCanExecuteChanged();
                });
        }

        public ObservableCommand(Func<TParam, bool> canExecuteFunc)
        {
            _canExecuteFunc = canExecuteFunc;
        }

        public ObservableCommand()
            : this(Observable.Return(true))
        {
        }

        public bool CanExecute(object parameter)
        {
            if (!_enabled)
            {
                return false;
            }

            if (_canExecuteFunc != null)
            {
                return _canExecuteFunc((TParam)parameter);
            }

            return _canExecuteSubject.First();
        }

        public void Execute(object parameter)
        {
            TParam paramValue = (parameter == null)
                ? default(TParam)
                : (TParam)parameter;

            _values.OnNext(paramValue);
        }

        protected void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        public void Dispose()
        {
            _subscription.Dispose();
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnCanExecuteChanged();
            }
        }

        public IDisposable Subscribe(IObserver<TParam> observer)
        {
            return _values.Subscribe(observer);
        }
    }
}