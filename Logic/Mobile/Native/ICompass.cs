using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinLogic.Native
{
	public delegate void HeadingChanged();
	public interface ICompass
	{
		double Heading { get; set; }

		// Orientation Orientation { get; set; }

		void Start(double degreeSensitivity);
		void Stop();

		event HeadingChanged OnHeadingChanged;


	}
	/*
	public enum Orientation
	{
		Portrait,
		LandscapeLeft,
		LandscapeRight,
		UpsideDown,
		Unknown
	}
	*/
}