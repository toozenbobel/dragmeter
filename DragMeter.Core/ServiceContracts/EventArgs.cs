using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragMeter.Core.ServiceContracts
{
	public class EventArgs<T> : EventArgs
	{
		public EventArgs(T parameter)
		{
			Parameter = parameter;
		}

		public T Parameter { get; private set; }
	}
}
