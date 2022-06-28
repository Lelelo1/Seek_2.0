using System;
using System.Collections.Generic;
using LogicLibrary.Models.Analytics;

namespace LogicLibrary.Native
{
    public interface INativeMixpanel : INative
    {
        void Track(BaseAnalyticsEvent analyticsEvent);
        void Identify(string distinctId);
        void Set(Dictionary<string, object> properties);
        void Flush();
    }
}
