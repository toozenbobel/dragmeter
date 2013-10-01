using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using DragMeter.Core.ServiceContracts;
using DragMeter.Core.ServiceContracts.Motion;

namespace DragMeter.Core.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
	    private readonly IMotionService _motionService;
	    private readonly IGpsService _gpsService;

	    public StartPageViewModel(IMotionService motionService, IGpsService gpsService)
	    {
		    _motionService = motionService;
		    _gpsService = gpsService;
	    }

	    public void Init()
		{
			InitializeGps();
		    InitializeMotionSensor();
		}

	    private void InitializeMotionSensor()
	    {
		    _motionService.MotionServiceStateChanged += (sender, args) => { IsMotionServiceReady = _motionService.IsMotionServiceReady; };
		    _motionService.StartMotionService();
	    }

	    private void InitializeGps()
	    {
			if (IsGpsEnabled)
			{
				IsGpsReady = _gpsService.IsGpsReady;
				
				_gpsService.GpsStateChanged += (sender, args) => { IsGpsReady = _gpsService.IsGpsReady; };
				_gpsService.StartGps();
			}
			else
			{
				IsGpsReady = false;
			}
		}

	    public ICommand StartMeasureCommand
	    {
		    get
		    {
			    return new MvxCommand(() => ShowViewModel<MeasurePageViewModel>());
		    }
	    }

	    #region Strings

	    public string GpsStatus
	    {
		    get
		    {
			    if (!IsGpsEnabled)
			    {
				    return "Disabled";
			    }
				else
				{
					if (IsGpsReady)
					{
						return "Ready";
					}
					else
					{
						return "Connecting...";
					}
				}
		    }
	    }

		public string MotionStatus
		{
			get
			{
				if (IsMotionServiceReady)
				{
					return "Ready";
				}

				return "Connecting...";
			}
		}

		#endregion

		#region Settings

		private bool _isGpsEnabled = true;
	    public bool IsGpsEnabled
	    {
		    get
		    {
			    return _isGpsEnabled;
		    }
		    set
		    {
			    _isGpsEnabled = value;
			    OnPropertyChanged(() => IsGpsEnabled);
				OnPropertyChanged(() => GpsStatus);
				OnPropertyChanged(() => IsReadyToGo);
				InitializeGps();
		    }
	    }

	    private bool _isGpsReady;
	    public bool IsGpsReady
	    {
		    get
		    {
			    return _isGpsReady;
		    }
		    set
		    {
			    _isGpsReady = value;
			    OnPropertyChanged(() => IsGpsReady);
				OnPropertyChanged(() => GpsStatus);
				OnPropertyChanged(() => IsReadyToGo);
		    }
	    }

	    private bool _isMotionServiceReady;
	    public bool IsMotionServiceReady
	    {
		    get
		    {
			    return _isMotionServiceReady;
		    }
			set
			{
				_isMotionServiceReady = value;
				OnPropertyChanged(() => IsMotionServiceReady);
				OnPropertyChanged(() => MotionStatus);
				OnPropertyChanged(() => IsReadyToGo);
			}
	    }

	    public bool IsReadyToGo
	    {
			get
			{
				return IsGpsEnabled && IsGpsReady && IsMotionServiceReady;
			}
	    }

	    #endregion
    }
}
