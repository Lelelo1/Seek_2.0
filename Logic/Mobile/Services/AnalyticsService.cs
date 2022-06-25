using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Models;
using Logic.Models.Analytics;
using Logic.Native;


namespace Logic.Services
{

    public class AnalyticsService : IBase
    {

        static TaskCompletionSource<IBase> Initializing { get; } = new TaskCompletionSource<IBase>();

        public static async Task<IBase> Init()
        {
            var nativeMixpanel = N.Get<INativeMixpanel>();
            var utilitiesService = N.Get<IUtilitiesService>();
            var user = utilitiesService.GetUser();
            var runtime = utilitiesService.Runtime;
            Initializing.SetResult(new AnalyticsService(nativeMixpanel, runtime, user));
            return await Initializing.Task;
        }

        INativeMixpanel NativeMixpanel { get; }


        protected AnalyticsService(INativeMixpanel nativeMixpanel, Runtime runtime, User user)
        {   
            NativeMixpanel = nativeMixpanel;
            NativeMixpanel.Identify(user.Id);
            NativeMixpanel.Set(new Dictionary<string, object>()
            {
                { "Runtime", runtime.ToString()},
                { "LoggedIn", user.IsLoggedIn }
            });
 
        }



        public void Track<T>(T analyticsEvent) where T : BaseAnalyticsEvent
        {
            NativeMixpanel.Track(analyticsEvent);




            // send events on every 'Track' so that all events reaches mixpanel
            // previous default flush interval was used before, meaning sending each 60 seconds
            NativeMixpanel.Flush();

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
