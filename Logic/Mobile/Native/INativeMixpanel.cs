using System;
using System.Collections.Generic;
using XamarinLogic.Models.Analytics;

namespace XamarinLogic.Native
{
	public interface INativeMixpanel : INative
	{
		void Track(BaseAnalyticsEvent analyticsEvent);
		void Identify(string distinctId);
		void Set(Dictionary<string, object> properties);
		void Flush();
	}
}