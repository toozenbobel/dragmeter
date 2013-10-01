using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Plugins.Location;
using DragMeter.Core.Helpers;

namespace DragMeter.Core.Services
{
	public class AccelerationObject
	{
		public MvxGeoLocation StartLocation { get; set; }

		private readonly List<MvxCoordinates> _trace = new List<MvxCoordinates>();
		public List<MvxCoordinates> Trace
		{
			get
			{
				return _trace;
			} 
		}

		public double Distance { get; private set; }

		public bool Is402MetresPassed { get; private set; }
		public bool Is1KmPassed { get; private set; }
		public bool Is1to100Passed { get; private set; }

		public double MaxSpeed { get; private set; }

		public double Speed402 { get; private set; }
		public double Speed1Km { get; private set; }

		public event EventHandler Go1To100Passed;
		protected virtual void OnGo1To100Passed()
		{
			EventHandler handler = Go1To100Passed;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		public event EventHandler Go402MetresPassed;

		protected virtual void OnGo402MetresPassed()
		{
			EventHandler handler = Go402MetresPassed;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		public event EventHandler Go1KmPassed;

		protected virtual void OnGo1KmPassed()
		{
			EventHandler handler = Go1KmPassed;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		public void AddCoordinate(MvxCoordinates coordinates)
		{
			var currentSpeed = coordinates.Speed * 3600.0 / 1000.0;

			var lastCoordinate = Trace.LastOrDefault();
			if (lastCoordinate == null)
			{
				Trace.Add(coordinates);
				Distance = 0.0;

				Is1KmPassed = false;
				Is402MetresPassed = false;
				Is1to100Passed = false;
			}
			else
			{
				Trace.Add(coordinates);

				Distance += DistanceCalcs.DistanceInMetres(coordinates.Latitude, coordinates.Longitude, lastCoordinate.Latitude, lastCoordinate.Longitude);
				
				if (!Is1to100Passed && currentSpeed.HasValue && currentSpeed.Value > 100)
				{
					Is1to100Passed = true;
					OnGo1To100Passed();
				}

				if (!Is402MetresPassed && Distance > 402)
				{
					Is402MetresPassed = true;
					if (currentSpeed.HasValue)
						Speed402 = currentSpeed.Value;
					OnGo402MetresPassed();
				}
				
				if (!Is1KmPassed && Distance > 1000)
				{
					if (currentSpeed.HasValue)
						Speed1Km = currentSpeed.Value;

					Is1KmPassed = true;
					OnGo1KmPassed();
				}
			}

			
			if (currentSpeed.HasValue && currentSpeed > MaxSpeed)
				MaxSpeed = currentSpeed.Value;
		}

		public double GetLastSpeed()
		{
			var coord = Trace.LastOrDefault();
			if (coord != null && coord.Speed.HasValue)
				return coord.Speed.Value * 3600.0 / 1000.0;

			return 0.0;
		}
	}
}