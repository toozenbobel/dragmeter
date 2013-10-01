using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragMeter.Core.ServiceContracts
{
	public interface IMotionManagementService
	{
		event EventHandler<EventArgs<double>> GotAcceleration;
		void StopWaitingForAcceleration();
		void WaitForAcceleration();
	}
}
