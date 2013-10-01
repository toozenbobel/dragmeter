using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using DragMeter.Core.ServiceContracts;
using DragMeter.Core.ServiceContracts.Motion;
using DragMeter.Phone.Services;
using Microsoft.Phone.Controls;

namespace DragMeter.Phone
{
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
			Mvx.LazyConstructAndRegisterSingleton<IStopWatchService, StopwatchService>();
			Mvx.LazyConstructAndRegisterSingleton<IMotionService, MotionService>();

            return new Core.App();
        }
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}