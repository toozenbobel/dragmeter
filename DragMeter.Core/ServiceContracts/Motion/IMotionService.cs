using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragMeter.Core.ServiceContracts.Motion
{
	public interface IMotionService
	{
		void StartMotionService();
		void StopMotionService();
		event EventHandler MotionServiceStateChanged;
		bool IsMotionServiceReady { get; }
		event EventHandler<EventArgs<double>> GotLinearAcceleration;
	}
}
