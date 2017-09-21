using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfHelper.Common
{
	public class CommandViewModel : ICommand
	{
		private readonly Action<object> _action;

		public CommandViewModel(Action<object> action)
		{
			_action = action;
		}

		public void Execute(object o)
		{
			_action(o);
		}

		public bool CanExecute(object o)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged
		{
			add { }
			remove { }
		}
	}
}
