using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragMeter.Core.ServiceContracts;
using DragMeter.Core.Services;

namespace DragMeter.Core.ViewModels
{
	public class ChartPageViewModel : ViewModelBase
	{
		private readonly IAccelerationObjectContainerService _containerService;

		public ChartPageViewModel(IAccelerationObjectContainerService containerService)
		{
			_containerService = containerService;
		}

		public void Init()
		{
			//Data = new List<TimeValuePair>()
			//	{
			//		new TimeValuePair() { SpeedValue = 3, Time = 0.2 },
			//		new TimeValuePair() { SpeedValue = 10, Time = 1.3 },
			//		new TimeValuePair() { SpeedValue = 15, Time = 4.4 },
			//		new TimeValuePair() { SpeedValue = 30, Time = 7.6 },
			//		new TimeValuePair() { SpeedValue = 56, Time = 8.5 },
			//		new TimeValuePair() { SpeedValue = 86, Time = 9.2 },
			//		new TimeValuePair() { SpeedValue = 105, Time = 10.4 },
			//		new TimeValuePair() { SpeedValue = 133, Time = 11.2 }
			//	};
			var data = _containerService.Get();
			if (data != null)
				Data = Filter(data);

		}

		private List<TimeValuePair> Filter(IEnumerable<TimeValuePair> data)
		{
			List<TimeValuePair> ret = new List<TimeValuePair>();

			var timeValuePairs = data as TimeValuePair[] ?? data.ToArray();
			if (timeValuePairs.Any())
			{
				int maxTime = (int) Math.Floor(timeValuePairs.Max(tv => tv.Time));
				for (int i = 0; i <= maxTime; i++)
				{
					TimeValuePair toAdd = new TimeValuePair();
					var target = timeValuePairs.First(t => t.Time >= i);
					toAdd.Time = Math.Round(target.Time, 1);
					toAdd.SpeedValue = Math.Round(target.SpeedValue);
					ret.Add(toAdd);
				}

				
				if (Math.Abs(ret.Last().Time - timeValuePairs.Last().Time) > 0.000001)
				{
					TimeValuePair toAdd = new TimeValuePair();
					var target = timeValuePairs.Last();
					toAdd.Time = Math.Round(target.Time, 1);
					toAdd.SpeedValue = Math.Round(target.SpeedValue);
					ret.Add(toAdd);
				}
			}

			return ret;
		}

		private List<TimeValuePair> _data;
		public List<TimeValuePair> Data
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
				OnPropertyChanged(() => Data);
			}
		}
	}

	public class TimeValuePair
	{
		public double Time { get; set; }
		public double SpeedValue { get; set; }
	}
}
