using System;
using System.Collections.Generic;

namespace XamarinLogic.Models.Analytics
{
	// needs to have contructor for newtonsoft serialization JsonFlatFileDataStore
	public class BaseAnalyticsEvent
	{
		public double TimeMilliseconds { get; } = DateTime.Now.Millisecond;
	}
}