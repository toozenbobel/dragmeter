using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragMeter.Core.Services;

namespace DragMeter.Core.ServiceContracts
{
	public interface IAccelerationService
	{
		AccelerationObject StartMeasure();
		void StopMeasure();
	}
}
