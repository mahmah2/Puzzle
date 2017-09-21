using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfHelper.Common
{
	public class BaseViewModel : DependencyObject, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(memberName));

				var callerMethodInfo = new StackTrace().GetFrame(1).GetMethod();

				var callerContainerType = callerMethodInfo.ReflectedType;

				//var Listeners = Reflection.GetListeners(callerContainerType, memberName);
				var Listeners = WPFHelper.DependencyManager.GetListeners(callerContainerType, memberName);

				foreach (var listener in Listeners)
					PropertyChanged(this, new PropertyChangedEventArgs(listener));
			}
		}
	}
}
