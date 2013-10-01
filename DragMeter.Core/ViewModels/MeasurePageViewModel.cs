using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using DragMeter.Core.ServiceContracts;
using DragMeter.Core.Services;

namespace DragMeter.Core.ViewModels
{
	public class MeasurePageViewModel : ViewModelBase
	{
		private readonly IAccelerationService _accelerationService;
		private readonly IStopWatchService _stopWatchService;
		private readonly IAccelerationObjectContainerService _objectContainer;

		public MeasurePageViewModel(IAccelerationService accelerationService, IStopWatchService stopWatchService, IAccelerationObjectContainerService objectContainer)
		{
			_accelerationService = accelerationService;
			_stopWatchService = stopWatchService;
			_objectContainer = objectContainer;
		}

		public void Init()
		{
			StartMeasure();
		}

		public ICommand MeasureCommand
		{
			get {return new MvxCommand(StartMeasure);}
		}

		public ICommand StopMeasureCommand
		{
			get {return new MvxCommand(StopMeasure);}
		}

		public ICommand GoToChartCommand
		{
			get {return new MvxCommand(() => ShowViewModel<ChartPageViewModel>());}
		}

		private void StopMeasure()
		{
			lock (_locker)
			{
				_isMeasuring = false;
				_stopWatchService.Stop();
				_accelerationService.StopMeasure();
			}

			CanStop = false;

			_objectContainer.Store(_traceDump);
		}

		private AccelerationObject _measureData;

		private bool _isMeasuring;

		private bool _canStop;
		public bool CanStop
		{
			get
			{
				return _canStop;
			}
			set
			{
				_canStop = value;
				OnPropertyChanged(() => CanStop);
				OnPropertyChanged(() => CanStart);
			}
		}

		public bool CanStart
		{
			get
			{
				return !CanStop;
			}
		}

		/// <summary>
		/// Запускаем измерения для заезда
		/// </summary>
		private void StartMeasure()
		{
			_traceDump.Clear();

			CanStop = true;

			StartStopwatchMonitoring(); // вьюмодель начинает следить за показаниями таймера
			
			_measureData = _accelerationService.StartMeasure();
			_measureData.Go1To100Passed += (sender, args) =>
				{
					Time1To100 = _stopWatchService.GetElapsed() * 1000;
				};

			_measureData.Go402MetresPassed += (sender, args) =>
				{
					Speed402Metres = _measureData.Speed402;
					Time402Metres = _stopWatchService.GetElapsed() * 1000;
				};

			_measureData.Go1KmPassed += (sender, args) =>
				{
					Speed1Km = _measureData.Speed1Km;
					Time1Km = _stopWatchService.GetElapsed() * 1000;
				};
		}

		private double _speed402Metres;
		public double Speed402Metres
		{
			get
			{
				return _speed402Metres;
			}
			set
			{
				_speed402Metres = value;
				OnPropertyChanged(() => Speed402Metres);
			}
		}

		private double _speed1Km;
		public double Speed1Km
		{
			get
			{
				return _speed1Km;
			}
			set
			{
				_speed1Km = value;
				OnPropertyChanged(() => Speed1Km);
			}
		}

		private double _time1To100;
		public double Time1To100
		{
			get
			{
				return _time1To100;
			}
			set
			{
				_time1To100 = value;
				OnPropertyChanged(() => Time1To100);
			}
		}

		private double _time402Metres;
		public double Time402Metres
		{
			get
			{
				return _time402Metres;
			}
			set
			{
				_time402Metres = value;
				OnPropertyChanged(() => Time402Metres);
			}
		}

		private double _time1Km;
		public double Time1Km
		{
			get
			{
				return _time1Km;
			}
			set
			{
				_time1Km = value;
				OnPropertyChanged(() => Time1Km);
			}
		}

		private double _currentSpeed;
		public double CurrentSpeed
		{
			get
			{
				return _currentSpeed;
			}
			set
			{
				_currentSpeed = value;
				OnPropertyChanged(() => CurrentSpeed);
			}
		}

		private double _currentDistance;
		public double CurrentDistance
		{
			get
			{
				return _currentDistance;
			}
			set
			{
				_currentDistance = value;
				OnPropertyChanged(() => CurrentDistance);
			}
		}

		Timer _stopWatchUpdateTimer;
		private readonly object _locker = new object();

		private void StartStopwatchMonitoring()
		{
			lock (_locker)
			{
				_isMeasuring = true;
			}

			ScheduleUpdate();
		}

		private void ScheduleUpdate()
		{
			lock (_locker)
			{
				if (_isMeasuring)
				{
					if (_stopWatchUpdateTimer == null)
					{
						_stopWatchUpdateTimer = new Timer(OnTimerTick, null, TimeSpan.FromMilliseconds(100), TimeSpan.Zero);
					}
					else
					{
						_stopWatchUpdateTimer.Change(TimeSpan.FromMilliseconds(100), TimeSpan.Zero);
					}
				}
			}
		}

		private void OnTimerTick(object state)
		{
			lock (_locker)
			{
				_stopWatchUpdateTimer.Dispose();
				_stopWatchUpdateTimer = null;
			}
			
			Dispatcher.RequestMainThreadAction(() =>
				{
					var elapsed = _stopWatchService.GetElapsed();
					CurrentPassedTime = TimeSpan.FromMilliseconds(elapsed).ToString("hh\\:mm\\:ss\\.fff");
					CurrentDistance = _measureData.Distance;
					CurrentSpeed = _measureData.GetLastSpeed();

					_traceDump.Add(new TimeValuePair()
						{
							Time = elapsed / 1000.0,
							SpeedValue = CurrentSpeed
						});
				});

			ScheduleUpdate();
		}

		private readonly List<TimeValuePair> _traceDump = new List<TimeValuePair>(); 

		private string _currentPassedTime;
		public string CurrentPassedTime
		{
			get
			{
				return _currentPassedTime;
			}
			set
			{
				_currentPassedTime = value;
				OnPropertyChanged(() => CurrentPassedTime);
			}
		}
	}
}
