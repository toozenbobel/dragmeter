using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragMeter.Core.ServiceContracts
{
	public interface IStopWatchService
	{
		void Start();
		void Stop();
		long GetElapsed();
	}
}
