using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragMeter.Core.ServiceContracts;
using DragMeter.Core.ViewModels;

namespace DragMeter.Core.Services
{
	public class AccelerationObjectContainerService : IAccelerationObjectContainerService
	{
		private IEnumerable<TimeValuePair> _storedObject;

		public void Store(IEnumerable<TimeValuePair> obj)
		{
			_storedObject = obj;
		}

		public IEnumerable<TimeValuePair> Get()
		{
			return _storedObject;
		}
	}
}
