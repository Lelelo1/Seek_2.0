using System;
using XamarinLogic.Models;

namespace XamarinLogic.Testing
{
	public class Simulate
	{
		static Simulate instance;
		public static Simulate Get()
		{
			if(instance == null)
			{
				instance = new Simulate();
			}
			return instance;
		}
		// does not work when passes 359 and 0 deg into it. Actually probably works!

		static double __toRad__ = Math.PI / 180;
		static double __toDeg__ = 180 / Math.PI;
		public Location __GetRandomDestinationFor__(Location userLocation, double distanceMeters, double initialBearingDegrees = 0) // 0 is north
		{
			var initialBearingRadians = initialBearingDegrees * __toRad__;
			var distanceKilometers = distanceMeters / 1000;
			var radiusEarthKilometres = 6371.01;
			var distRatio = distanceKilometers / radiusEarthKilometres;
			var distRatioSine = Math.Sin(distRatio);
			var distRatioCosine = Math.Cos(distRatio);

			var startLatRad = userLocation.Latitude * __toRad__;
			var startLonRad = userLocation.Longitude * __toRad__;

			var startLatCos = Math.Cos(startLatRad);
			var startLatSin = Math.Sin(startLatRad);

			var endLatRads = Math.Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Math.Cos(initialBearingRadians)));
			var endLonRads = startLonRad + Math.Atan2(Math.Sin(initialBearingRadians) * distRatioSine * startLatCos,
				distRatioCosine - startLatSin * Math.Sin(endLatRads));
			return new Location(endLatRads * __toDeg__, endLonRads * __toDeg__);
		}

	}
}