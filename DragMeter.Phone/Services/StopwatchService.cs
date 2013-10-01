using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DragMeter.Core.ServiceContracts;

namespace DragMeter.Phone.Services
{
	public class StopwatchService : IStopWatchService
	{
		readonly Stopwatch _stopwatch = new Stopwatch();

		public void Start()
		{
			_stopwatch.Reset();
			_stopwatch.Start();
		}

		public void Stop()
		{
			_stopwatch.Stop();
		}

		public long GetElapsed()
		{
			return _stopwatch.ElapsedMilliseconds;
		}
	}
}
