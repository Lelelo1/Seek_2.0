using System;
using System.Threading;
using System.Threading.Tasks;
using LogicLibrary.Utils;

namespace LogicLibrary.Utils
{
    /* Works like a listener which updates for every milliseconds specified.
     * OnReadingChanged has no otherwise typical Xamarin sender and event args.
     * The data is taken directly from the instance that the EventRegistration was given to */
    public class EventRegistration
    {
        public delegate void ReadingChanged();
        public ReadingChanged OnReadingChanged;

        double _notifiedMilliseconds;
        public double NotifiedMilliseconds
        {
            set
            {
                _notifiedMilliseconds = value;
                PeriodicTask.Period = TimeSpan.FromMilliseconds(_notifiedMilliseconds);
            }
            get => _notifiedMilliseconds;
        }
        // public CancellationTokenSource CancellationTokenSource;
        public PeriodicTask PeriodicTask { get; }
        public EventRegistration(double notifiedMilliseconds)
        {
            PeriodicTask = new PeriodicTask(TimeSpan.FromMilliseconds(_notifiedMilliseconds));
            NotifiedMilliseconds = notifiedMilliseconds;

        }

        public void Stop() // use this instead of calling ptask directly
        {
            PeriodicTask.CancellationTokenSource.Cancel();
        }
 
    }

    // https://stackoverflow.com/questions/4890915/is-there-a-task-based-replacement-for-system-threading-timer
    public class PeriodicTask
    {
        public TimeSpan Period { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; }
        public PeriodicTask(TimeSpan period)
        {
            Period = period;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public async Task Run(Action action)
        {
            var token = CancellationTokenSource.Token;
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(Period, token);
                if (!token.IsCancellationRequested)
                {
                    action.Invoke();
                }
            }
            Console.WriteLine("Stopped, due to cancellation requested");
            OnStopped?.Invoke();
        }
        public delegate void Stopped();
        public event Stopped OnStopped;

    }
}
