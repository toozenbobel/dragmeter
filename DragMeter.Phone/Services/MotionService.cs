using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragMeter.Core.ServiceContracts;
using DragMeter.Core.ServiceContracts.Motion;
using Microsoft.Devices.Sensors;

namespace DragMeter.Phone.Services
{
	public class MotionService : IMotionService
	{
		readonly Motion _motion = new Motion();

		public void StartMotionService()
		{
			_motion.CurrentValueChanged += OnMotion;
			_motion.Start();
		}

		private void OnMotion(object sender, SensorReadingEventArgs<MotionReading> e)
		{
			if (!IsMotionServiceReady)
			{
				IsMotionServiceReady = _motion.IsDataValid;
				OnMotionServiceStateChanged();
			}

			OnGotLinearAcceleration(new EventArgs<double>(e.SensorReading.DeviceAcceleration.Length()));
		}

		public void StopMotionService()
		{
			_motion.Stop();
		}

		public event EventHandler MotionServiceStateChanged;
		protected virtual void OnMotionServiceStateChanged()
		{
			EventHandler handler = MotionServiceStateChanged;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		public bool IsMotionServiceReady { get; private set; }
		
		public event EventHandler<EventArgs<double>> GotLinearAcceleration;
		protected virtual void OnGotLinearAcceleration(EventArgs<double> e)
		{
			EventHandler<EventArgs<double>> handler = GotLinearAcceleration;
			if (handler != null)
				handler(this, e);
		}
	}
}
