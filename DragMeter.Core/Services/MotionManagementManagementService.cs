using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.Accelerometer;
using DragMeter.Core.ServiceContracts;
using DragMeter.Core.ServiceContracts.Motion;

namespace DragMeter.Core.Services
{
	public class MotionManagementManagementService : IMotionManagementService
	{
		private readonly IMotionService _motionService;
		private readonly object _accelerationAwaitLocker = new object();

		public MotionManagementManagementService(IMotionService motionService)
		{
			_motionService = motionService;
		}

		public event EventHandler<EventArgs<double>>  GotAcceleration;

		protected virtual void OnGotAcceleration(double acceleration)
		{
			EventHandler<EventArgs<double>> handler = GotAcceleration;
			if (handler != null)
				handler(this, new EventArgs<double>(acceleration));
		}

		public void StopWaitingForAcceleration()
		{
			lock (_accelerationAwaitLocker)
			{
				_isWaitingForAcceleration = false;
			}
		}

		private bool _isWaitingForAcceleration;

		public void WaitForAcceleration()
		{
			lock (_accelerationAwaitLocker)
			{
				_isWaitingForAcceleration = true;

				_motionService.GotLinearAcceleration += OnAcceleration;
				_motionService.StartMotionService();
			}
		}

		private void OnAcceleration(object sender, EventArgs<double> eventArgs)
		{
			lock (_accelerationAwaitLocker)
			{
				if (_isWaitingForAcceleration)
				{
					if (eventArgs.Parameter > 0.2)
						OnGotAcceleration(eventArgs.Parameter);
				}
			}
		}
	}
}
