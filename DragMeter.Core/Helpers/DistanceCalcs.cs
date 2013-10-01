using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragMeter.Core.Helpers
{
	public class DistanceCalcs
	{
		public static Double DistanceInMetres(double lat1, double lon1, double lat2, double lon2)
		{

			if (Math.Abs(lat1 - lat2) < 0.00001 && Math.Abs(lon1 - lon2) < 0.00001)
				return 0.0;

			var theta = lon1 - lon2;

			var distance = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) +
						   Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
						   Math.Cos(Deg2Rad(theta));

			distance = Math.Acos(distance);
			if (double.IsNaN(distance))
				return 0.0;

			distance = Rad2Deg(distance);
			distance = distance * 60.0 * 1.1515 * 1609.344;

			return (distance);
		}

		private static double Deg2Rad(double deg)
		{
			return (deg * Math.PI / 180.0);
		}

		private static double Rad2Deg(double rad)
		{
			return (rad / Math.PI * 180.0);
		}
	}
}
