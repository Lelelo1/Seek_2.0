﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicLibrary.Native;
using LogicLibrary.Services;
using LogicLibrary.Utils;
using LogicLibrary.ViewModels;
using System.Linq;

using LogicLibrary.Models;
using LogicLibrary.Services.PermissionRequired;
using LogicLibrary.Game;

namespace LogicLibrary
{
	public class Logic
	{
		public static IFrameworkContext FrameworkContext { get; private set; }

		public static Action<string> Log { get; private set; }

		public static void SetLogger(Action<string> logger)
		{
			Log = logger;
		}

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

		public static void Init(IFrameworkContext frameworkContext)
		{
			FrameworkContext = frameworkContext;

			var initAnalyticsService = AnalyticsService.Init();
			var initLogicLibrary = new List<Task<IBase>>()
			{

				initAnalyticsService,
				LocationService.Init(), // permission required - should not accessed until started in 'MainViewModel'
				MainViewModel.Init(),
				SearchViewModel.Init(CovariantCast<SearchService>(SearchService.Init())),
				Projector.Init()
			};

			var initialization = Task.WhenAll(initLogicLibrary);
			CatchInitializationErrors(initialization);
		}

		static async void CatchInitializationErrors(Task<IBase[]> initialization)
		{
			try
			{
				var bases = await initialization; // wait for LogicLibrary services to become available
				DependencyBox.AddRange(bases.ToList());
				// returning bool so that there is better indication that bases should be accessed from 'L.Get<AnalyticsService>' eg
				SafeInitialization.SetResult(true);
				return;
			}
			catch (Exception exc)
			{
				Log(exc.Message);
				//Crashes.TrackError(exc, Error.Properties("When initializing LogicLibrary"), null);

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