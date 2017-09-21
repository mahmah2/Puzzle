using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfHelper.Common;

namespace Puzzle.ViewModel
{
	public class VMNumberInfo : BaseViewModel
	{
		public VMNumberInfo(int number, int x, int y)
		{
			X = _originalX = x;
			Y = _originalY = y;
			Number = number;
		}

		public int Number { get; set; }

		public void UpdateCoordinates(int x, int y)
		{
			X = x;
			Y = y;
		}

		private int _originalX;
		private int _originalY;

		public int X
		{
			get { return (int)GetValue(XProperty); }
			set {
				SetValue(XProperty, value);
				OnPropertyChanged();
			}
		}

		public static readonly DependencyProperty XProperty =
			DependencyProperty.Register("X", typeof(int), typeof(VMNumberInfo), new PropertyMetadata(0));

		public int Y
		{
			get { return (int)GetValue(YProperty); }
			set {
				SetValue(YProperty, value);
				OnPropertyChanged();
			}
		}

		public static readonly DependencyProperty YProperty =
			DependencyProperty.Register("Y", typeof(int), typeof(VMNumberInfo), new PropertyMetadata(0));

		public delegate void NumberClickHandler(VMNumberInfo number);
		public event NumberClickHandler OnClick;

		private ICommand _clicked;
		public ICommand Clicked
		{
			get
			{
				return _clicked ?? (_clicked = new CommandViewModel(p => DoClick(p)));
			}
		}
		public void DoClick(object p)
		{
			OnClick?.Invoke(p as VMNumberInfo);
		}

		public void ResetCoordinates()
		{
			X = _originalX;
			Y = _originalY;
		}

		public float Displacement()
		{
			return Math.Abs(X - _originalX) + Math.Abs(Y - _originalY);
		}
	}
}
