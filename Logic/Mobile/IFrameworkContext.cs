﻿using System;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic
{
	public interface IFrameworkContext
	{
		void ReportCrash(Exception exc, string message);
		Task<Location> GetLocationAsync();
	}
}