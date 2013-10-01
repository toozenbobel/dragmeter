using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using Cirrious.MvvmCross.ViewModels;

namespace DragMeter.Core.ViewModels
{
	public class ViewModelBase : MvxViewModel
	{
		public void StartAsyncTask<T>(Func<T> task, Action<T> callback, Action<Exception> error = null)
		{
			ThreadPool.QueueUserWorkItem(_ =>
			{
				try
				{
					var result = task();
					if (callback != null)
						InvokeOnMainThread(() => callback(result));
				}
				catch (Exception e)
				{
					if (error != null)
						InvokeOnMainThread(() => error(e));
				}

			});
		}

		#region MVVM

		public void OnPropertyChanged(string propertyName)
		{
			RaisePropertyChanged(propertyName);
		}

		public void OnPropertyChanged<T>(Expression<Func<T>> property)
		{
			RaisePropertyChanged(property);
		}

		#endregion
	}
}
