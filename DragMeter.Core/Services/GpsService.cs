using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Plugins.Location;
using DragMeter.Core.ServiceContracts;

namespace DragMeter.Core.Services
{
	public class GpsService : IGpsService
	{
		private readonly IMvxGeoLocationWatcher _watcher;
		
		public GpsService(IMvxGeoLocationWatcher watcher)
		{
			_watcher = watcher;
		}

		public void StartGps()
		{
			_watcher.Start(new MvxGeoLocationOptions() { EnableHighAccuracy = true }, OnLocation, OnStartError);
		}
		
		private void OnStartError(MvxLocationError err)
		{
			IsGpsReady = false;

			OnGpsStateChanged();
		}

		private void OnLocation(MvxGeoLocation loc)
		{
			if (!IsGpsReady)
			{
				IsGpsReady = _watcher.Started;
				OnGpsStateChanged();
			}
			
			OnGotLocation(new EventArgs<MvxGeoLocation>(loc));
		}

		public void StopGps()
		{
			IsGpsReady = false;

			_watcher.Stop();
		}

		public event EventHandler GpsStateChanged;

		protected virtual void OnGpsStateChanged()
		{
			EventHandler handler = GpsStateChanged;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		public bool IsGpsReady { get; private set; }
		

		public event EventHandler<EventArgs<MvxGeoLocation>> GotLocation;

		protected virtual void OnGotLocation(EventArgs<MvxGeoLocation> e)
		{
			EventHandler<EventArgs<MvxGeoLocation>> handler = GotLocation;
			if (handler != null) handler(this, e);
		}
	}
}
