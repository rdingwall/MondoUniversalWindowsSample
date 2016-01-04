using System.Reactive.Concurrency;

namespace MondoUniversalWindowsSample
{
    public interface ISchedulerService
    {
        IScheduler Dispatcher { get; }

        IScheduler TaskPool { get; }
    }

    public sealed class SchedulerService : ISchedulerService
    {
        public IScheduler Dispatcher { get; } = CoreDispatcherScheduler.Current;

        public IScheduler TaskPool { get; } = TaskPoolScheduler.Default;
    }
}