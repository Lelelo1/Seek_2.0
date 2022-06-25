using System;
using Foundation;
using Xam.Plugin.Mixpanel.iOS;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using Logic.Native;
using Logic.Models.Analytics;
using Seek.iOS.Services;
using Logic.Utils;
[assembly: Dependency(typeof(Seek.iOS.iOS_Mixpanel))]
namespace Seek.iOS
{
    public class iOS_Mixpanel : INativeMixpanel
    {
        // https://eu.mixpanel.com/report/2280633/view/2827321/setup/#code
        Mixpanel Mixpanel { get; set; }

        public iOS_Mixpanel()
        {
            var runtime = N.Get<iOS_UtilitiesService>().Runtime;
            // flush default is 60 seconds https://help.mixpanel.com/hc/en-us/articles/115004601543-Mobile-Customize-Flush-Interval
            var token = runtime == Runtime.Production ? Secret.Secrets.Seek.MIXPANEL_TOKEN_PRODUCTION : Secret.Secrets.Seek.MIXPANEL_TOKEN_DEVELOPMENT;
            Mixpanel = new Mixpanel(token, 60);
        }

        public void Track(BaseAnalyticsEvent analyticsEvent)
        {
            var properties = analyticsEvent.ToDictionary();
            Mixpanel.Track(analyticsEvent.GetClassName(), properties.ToNSDictionary());
        }

        public void Identify(string distinctId)
        {
            Mixpanel.Identify(distinctId, true);
        }

        // Mixpanel updates/appends the 'properties' to existing ones
        public void Set(Dictionary<string, object> properties)
        {
            Mixpanel.People.Set(properties.ToNSDictionary());
        }

        
        public void Flush()
        {
            Mixpanel.Flush();
        }
        
    }
    public static class Extension
    {
        // does not support NSArray
        public static NSDictionary ToNSDictionary(this IDictionary<string, object> dict)
        {
            // try to do a conevrtionof the value of the object that is an List<strin>
            // to nsarray: https://stackoverflow.com/questions/47850490/put-list-of-strings-into-nsuserdefaults-object
            // could not handle array object, aka array event property 
            var nsDict = NSDictionary.FromObjectsAndKeys(dict.Values?.ToArray()
                                               ,dict.Keys.ToArray());
            return nsDict;
            
        }

    }
}
