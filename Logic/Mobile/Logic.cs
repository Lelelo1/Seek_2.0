using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinLogic.Native;
using XamarinLogic.Services;
using XamarinLogic.Utils;
using XamarinLogic.ViewModels;
using System.Linq;
using Microsoft.AppCenter.Crashes;
using XamarinLogic.Models;
using XamarinLogic.Services.PermissionRequired;
using XamarinLogic.Game;

namespace XamarinLogic
{
    public class Logic
    {
        // logging

        public delegate void LogDelegate(string message);
        public static LogDelegate Log { get; set; }

        public static void SetLogger(LogDelegate logger)
        {
            Log = logger;
        }

        public static void iOSLogger(string msg) => Console.WriteLine(msg);
        public static void AndroidLogger(string msg) => System.Diagnostics.Debug.WriteLine(msg);


        // threading

        public delegate void MainThreadDelegate(Action action);

        public static MainThreadDelegate MainThread { get; private set; }

        public delegate Task MainThreadAsyncDelegate(Action action);

        public static MainThreadAsyncDelegate MainThreadInvokeAsync { get; private set; }

        public static void SetThreading(MainThreadDelegate main, MainThreadAsyncDelegate mainAsync)
        {
            MainThread = main;
            MainThreadInvokeAsync = mainAsync;
        }

        public static void SetNativeDependencies(List<INative> nativeDependencies)
        {
            N.AddRange(nativeDependencies);
        }

        public static TaskCompletionSource<bool> SafeInitialization = new TaskCompletionSource<bool>();

        public static void Init(ProjectorConfig projectorConfig)
        {
            var initHistoryService = HistoryService.Init();
            var initAnalyticsService = AnalyticsService.Init(CovariantCast<HistoryService>(initHistoryService));
            var initLogic = new List<Task<IBase>>()
            {
                initHistoryService,
                initAnalyticsService,
                LocationService.Init(), // permission required - should not accessed until started in 'MainViewModel'
                MainViewModel.Init(),
                SearchViewModel.Init(CovariantCast<AnalyticsService>(initAnalyticsService), CovariantCast<SearchService>(SearchService.Init())),
                Projector.Init(projectorConfig)
            };

            var initialization = Task.WhenAll(initLogic);
            CatchInitializationErrors(initialization);
        }

        static async void CatchInitializationErrors(Task<IBase[]> initialization)
        {
            try
            {
                var bases = await initialization; // wait for logic services to become available
                DependencyBox.AddRange(bases.ToList());
                // returning bool so that there is better indication that bases should be accessed from 'L.Get<AnalyticsService>' eg
                SafeInitialization.SetResult(true);
                return;
            }
            catch (Exception exc)
            {
                Log(exc.Message);
                Crashes.TrackError(exc, Error.Properties("When initializing Logic"), null);

                if (N.Get<IUtilitiesService>().Runtime == Runtime.Debug)
                {   // so that I can see and get the full exception
                    throw exc;
                };
            }

            SafeInitialization.SetResult(false);
        }

        // https://stackoverflow.com/questions/37818642/cannot-convert-type-taskderived-to-taskinterface
        static async Task<T> CovariantCast<T>(Task<IBase> initialization)
        {
            return (T)await initialization;
        }

        public static DependencyBox<IBase> DependencyBox { get; } = new DependencyBox<IBase>();

    }
}