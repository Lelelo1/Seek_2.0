using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinLogic.Models;
using XamarinLogic.Models.Analytics;
using XamarinLogic.Native;
using Xamarin.Essentials;

namespace XamarinLogic.Services
{

    public class AnalyticsService : IBase
    {

        static TaskCompletionSource<IBase> Initializing { get; } = new TaskCompletionSource<IBase>();

        public static async Task<IBase> Init(Task<HistoryService> initHistoryService)
        {
            var nativeMixpanel = N.Get<INativeMixpanel>();
            var utilitiesService = N.Get<IUtilitiesService>();
            var user = utilitiesService.GetUser();
            var runtime = utilitiesService.Runtime;
            var historyService = await initHistoryService;
            Initializing.SetResult(new AnalyticsService(nativeMixpanel, runtime, user, historyService));
            return await Initializing.Task;
        }

        INativeMixpanel NativeMixpanel { get; }
        HistoryService HistoryService { get; }

        protected AnalyticsService(INativeMixpanel nativeMixpanel, Runtime runtime, User user, HistoryService historyService)
        {   
            NativeMixpanel = nativeMixpanel;
            NativeMixpanel.Identify(user.Id);
            NativeMixpanel.Set(new Dictionary<string, object>()
            {
                { "Runtime", runtime.ToString()},
                { "LoggedIn", user.IsLoggedIn }
            });

            HistoryService = historyService;
 
        }



        public void Track<T>(T analyticsEvent) where T : BaseAnalyticsEvent
        {
            NativeMixpanel.Track(analyticsEvent);




            // send events on every 'Track' so that all events reaches mixpanel
            // previous default flush interval was used before, meaning sending each 60 seconds
            NativeMixpanel.Flush();

            /*
            Logic.MainThread.Invoke(() =>
            {
                HistoryService.Set(analyticsEvent);
            });
            */

            SetUserHistoryFlags(analyticsEvent);
        }

        public string BubbleShownFlag { get; } = "BubbleShownFlag";

        void SetUserHistoryFlags(BaseAnalyticsEvent analyticsEvent)
        {
            var bubbleShown = analyticsEvent as BubbleShown;
            if (bubbleShown == null)
            {
                return;
            }
            Logic.Log("Set bubble shown: " + bubbleShown.Name);
            if(!Preferences.ContainsKey(BubbleShownFlag))
            {
                Preferences.Set(BubbleShownFlag, true);
            }

        }


        // user properties in mixpanel
        // potentially this call overwrites all
        public void Set(Dictionary<string,object> userProperties)
        {

            NativeMixpanel.Set(userProperties);

            NativeMixpanel.Flush();
        }

        

        
    }
}
