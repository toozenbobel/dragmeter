using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragMeter.Core.Services;
using DragMeter.Core.ViewModels;

namespace DragMeter.Core.ServiceContracts
{
	public interface IAccelerationObjectContainerService
	{
		void Store(IEnumerable<TimeValuePair> obj);
		IEnumerable<TimeValuePair> Get();
	}
}
