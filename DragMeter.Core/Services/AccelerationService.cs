using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cirrious.MvvmCross.Plugins.Location;
using DragMeter.Core.ServiceContracts;

namespace DragMeter.Core.Services
{
	public class AccelerationService : IAccelerationService
	{
		private readonly IGpsService _gpsService;
		private readonly IStopWatchService _stopWatchService;
		private readonly IMotionManagementService _motionManagementService;

		public AccelerationService(IGpsService gpsService, IStopWatchService stopWatchService, IMotionManagementService motionManagementService)
		{
			_gpsService = gpsService;
			_stopWatchService = stopWatchService;
			_motionManagementService = motionManagementService;
			
			_gpsService.GotLocation += OnLocation;
		}

		AccelerationObject _currentData;

		private void OnLocation(object sender, EventArgs<MvxGeoLocation> e)
		{
			var location = e.Parameter;
			if (_started && location.Coordinates.Speed * 3600.0 / 1000.0 > 1.0)
			{
				if (_currentData.StartLocation == null)
				{
					_currentData.StartLocation = e.Parameter;
				}

				_currentData.AddCoordinate(e.Parameter.Coordinates);
			}
		}

		public AccelerationObject StartMeasure()
		{
			_motionManagementService.GotAcceleration += (sender, args) =>
				{
					_motionManagementService.StopWaitingForAcceleration();
					_started = true;
					_stopWatchService.Start();
				};

			_motionManagementService.WaitForAcceleration();

			_currentData = new AccelerationObject();
			return _currentData;
		}

		public void StopMeasure()
		{
			_started = false;
			_stopWatchService.Stop();
			_currentData = null;
		}

		private volatile bool _started = false;
	}
}
