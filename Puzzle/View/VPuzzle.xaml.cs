using Puzzle.Controls;
using Puzzle.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFHelper.Helper;
using static Puzzle.ViewModel.VMPuzzle;

namespace Puzzle.View
{
	/// <summary>
	/// Interaction logic for Puzzle.xaml
	/// </summary>
	public partial class VPuzzle : UserControl
	{
		private VMPuzzle _vm;

		public VPuzzle()
		{
			InitializeComponent();

			_vm = new VMPuzzle(3,3);

			_vm.OnSolved += _vm_OnSolved;

			DataContext = _vm;
		}

		public event OnSolvedHandler OnPuzzleSolved;

		private void _vm_OnSolved()
		{
			var buttons = SearchVisualTree.FindVisualChildren<PuzzleButton>(this);

			Dispatcher.BeginInvoke(new Action(() => {
					buttons.ToList().ForEach(b => b.Glow());
				}), DispatcherPriority.ApplicationIdle, null);

			OnPuzzleSolved?.Invoke();
		}

		private void Puzzle_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Left:
					_vm.MoveEmptyCell(VMPuzzle.MoveDirection.Left);
					break;
				case Key.Up:
					_vm.MoveEmptyCell(VMPuzzle.MoveDirection.Up);
					break;
				case Key.Right:
					_vm.MoveEmptyCell(VMPuzzle.MoveDirection.Right);
					break;
				case Key.Down:
					_vm.MoveEmptyCell(VMPuzzle.MoveDirection.Down);
					break;
				default:
					break;
			}


		}
	}
}
