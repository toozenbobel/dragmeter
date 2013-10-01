using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Plugins.Location;
using DragMeter.Core.Services;

namespace DragMeter.Core.ServiceContracts
{
	public interface IGpsService
	{
		void StartGps();
		void StopGps();
		event EventHandler GpsStateChanged;
		bool IsGpsReady { get; }
		event EventHandler<EventArgs<MvxGeoLocation>> GotLocation;
	}
}
