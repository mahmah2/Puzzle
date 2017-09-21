using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPFHelper
{
	public class Run
	{
		public static void DelayRun(SendOrPostCallback callback, object param, double seconds)
		{
			var waitHandle = new AutoResetEvent(false);
			ThreadPool.RegisterWaitForSingleObject(
				waitHandle,
				(state, timeout) =>
				{
					var sc = (SynchronizationContext)state;
					sc.Post(callback, param);
				},
				SynchronizationContext.Current,
				TimeSpan.FromSeconds(seconds),
				true);
		}

	}
}
