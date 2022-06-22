using System;
using System.Collections.Generic;
using XamarinLogic.Services;
using System.Linq;
using XamarinLogic.Utils;
using XamarinLogic.Models.Analytics;
using System.Threading.Tasks;
using XamarinLogic.Models;
using Xamarin.Essentials;

namespace XamarinLogic.Services
{
	public class HistoryService : IBase
	{

		static TaskCompletionSource<IBase> Initializing { get; } = new TaskCompletionSource<IBase>();

		public static async Task<IBase> Init()
		{

			Initializing.SetResult(new HistoryService());

			return await Initializing.Task;
		}
		/*
		static Storage<BaseAnalyticsEvent> Storage { get; } = new Storage<BaseAnalyticsEvent>("AnalyticsHistory.json");

		public List<BaseAnalyticsEvent> Read() => Storage.Read();

		public void Set(BaseAnalyticsEvent analyticsEvent)
		{
			Storage.WriteOrAppend(analyticsEvent);
		}
		*/
	}
}