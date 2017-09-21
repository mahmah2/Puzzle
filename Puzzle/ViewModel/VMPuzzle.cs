using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfHelper.Common;
using System.Drawing;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Windows;
using System.Diagnostics;
using WPFHelper;

namespace Puzzle.ViewModel
{
	public class VMPuzzle : BaseViewModel
	{
		public VMPuzzle(int columnCount, int rowCount)
		{
			NumberOfScrambles = 10;
			WaitBetweenEachMoveMilliseconds = 200;
			ScrambleButtonTitle = "Scramble";
			SolveButtonTitle = "Solve";

			ColumnCount = columnCount;
			RowCount = rowCount;

			Numbers = new NumbersState(columnCount, rowCount);
			Moves = new BoardActionCollection();

			GenerateNumbers();
		}

		private SynchronizationContext ViewContext = SynchronizationContext.Current;

		private void GenerateNumbers()
		{
			int currentNumber = 1;

			for (int row = 0; row < RowCount; row++)
				for (int column = 0; column < RowCount; column++)
				{
					if ((row == RowCount - 1) && (column == ColumnCount - 1))
						continue;

					var ni = new VMNumberInfo(currentNumber, column, row);

					ni.OnClick += NumberInfo_OnClick;

					Numbers.Add(ni);

					currentNumber++;
				}
		}

		public enum MoveDirection
		{
			Left,
			Right,
			Up,
			Down
		}

		public bool MoveEmptyCell(MoveDirection direction)
		{
			var ep = Numbers.GetEmptyCellPosition();
			VMNumberInfo c = null;

			switch (direction)
			{
				case MoveDirection.Left:
					c = Numbers.GetRightCell(ep.X, ep.Y);
					break;
				case MoveDirection.Right:
					c = Numbers.GetLeftCell(ep.X, ep.Y);
					break;
				case MoveDirection.Up:
					c = Numbers.GetDownCell(ep.X, ep.Y);
					break;
				case MoveDirection.Down:
					c = Numbers.GetUpCell(ep.X, ep.Y);
					break;
				default:
					break;
			}

			if (c == null)
				return false;

			MoveToEmptyCell(c);

			return true;
		}

		private void ResetPositions()
		{
			foreach (var numberInfo in Numbers)
			{
				numberInfo.ResetCoordinates();
			}

			Moves.Clear();
		}

		private void NumberInfo_OnClick(VMNumberInfo numberInfo)
		{
			MoveToEmptyCell(numberInfo);
		}

		public BoardActionCollection Moves;

		private void MoveToEmptyCell(VMNumberInfo numberInfo, bool saveAction = true)
		{
			if (numberInfo == null)
				return;

			for (int row = 0; row < RowCount; row++)
				for (int column = 0; column < RowCount; column++)
				{
					var cni = Numbers.AtPosition(column, row);
					if (cni == null && Numbers.AreCrossAdjacent(numberInfo, column, row))
					{
						if (saveAction)
							Moves.Push(new BoardAction(numberInfo.X, numberInfo.Y, column, row));

						//move this NumberInfo to the empty position
						numberInfo.UpdateCoordinates(column, row);

						OnPropertyChanged(nameof(TotalDisplacement));

						if (TotalDisplacement == 0)
						{
							Moves.Clear();
							OnSolved?.Invoke();
						}

						return;
					}
				}

		}


		public int ColumnCount { get; set; }
		public int RowCount { get; set; }
		public NumbersState Numbers { get; set; }
		public int NumberOfScrambles { get; set; } 
		public int WaitBetweenEachMoveMilliseconds { get; set; }

		public delegate void OnSolvedHandler();
		public event OnSolvedHandler OnSolved;

		public string ScrambleButtonTitle
		{
			get { return (string)GetValue(ScrambleButtonTitleProperty); }
			set { SetValue(ScrambleButtonTitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ScrambleButtonTitle.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ScrambleButtonTitleProperty =
			DependencyProperty.Register("ScrambleButtonTitle", typeof(string), typeof(VMPuzzle), new PropertyMetadata(""));

		public string SolveButtonTitle
		{
			get { return (string)GetValue(SolveButtonTitleProperty); }
			set { SetValue(SolveButtonTitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SolveButtonTitle.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SolveButtonTitleProperty =
			DependencyProperty.Register("SolveButtonTitle", typeof(string), typeof(VMPuzzle), new PropertyMetadata(""));

		private ICommand _reset;
		public ICommand Reset
		{
			get
			{
				return _reset ?? (_reset = new CommandViewModel((p) => DoReset()));
			}
		}
		public void DoReset()
		{
			ResetPositions();
		}

		private ICommand _scramble;
		public ICommand Scramble
		{
			get
			{
				return _scramble ?? (_scramble = new CommandViewModel((p) => DoScramble()));
			}
		}
		public async void DoScramble()
		{
			if (_scramblingCancellationTokenSource!=null)
			{
				_scramblingCancellationTokenSource.Cancel();
				ClearScramblingModeTitle();
				return;
			}
			
			//Prevent thread access error
			var ns = NumberOfScrambles;
			var wt = WaitBetweenEachMoveMilliseconds;

			_scramblingCancellationTokenSource = new CancellationTokenSource();
			SetScramblingModeTitle();

			await Task.Run(() => ScramblePositions(ns, wt), _scramblingCancellationTokenSource.Token);

			//scrambling finished
			_scramblingCancellationTokenSource = null;
			ClearScramblingModeTitle();	
		}

		private CancellationTokenSource _scramblingCancellationTokenSource;

		private void ScramblePositions(int numberOfScrambles, int waitTime)
		{
			var rnd = new Random();

			int excludedNumber = 0; //Prevent reversing last action

			for (int i = 0; i < numberOfScrambles; i++)
			{
				ViewContext.Post(s => { //Apply our changes in the UI thread
					var ec = Numbers.GetEmptyCellPosition();
					List<VMNumberInfo> adjs = Numbers.GetCrossAdjacents(ec).Where(a => a.Number != excludedNumber).ToList();
					var index = rnd.Next(adjs.Count);
					excludedNumber = adjs[index].Number;
					MoveToEmptyCell(adjs[index]);
				}, null);

				if (_scramblingCancellationTokenSource.IsCancellationRequested)
				{
					_scramblingCancellationTokenSource = null;
					return;
				}

				Thread.Sleep(waitTime);
			}
		}

		private void SetScramblingModeTitle()
		{
			ScrambleButtonTitle = "Cancel Scramble";
		}
		private void ClearScramblingModeTitle()
		{
			ScrambleButtonTitle = "Scramble";
		}

		private ICommand _solve;
		public ICommand Solve
		{
			get
			{
				return _solve ?? (_solve = new CommandViewModel((p) => DoSolve()));
			}
		}

		private void DoSolve()
		{
			


			Task.Run(() => {
				while (Moves.Count > 0)
				{
					var ba = Moves.Pop();

					ViewContext.Post(s=>{
							MoveToEmptyCell(Numbers.AtPosition(ba.To.X, ba.To.Y), false);
					},null);

					Thread.Sleep(WaitBetweenEachMoveMilliseconds);
				}
			});
		}



		public float TotalDisplacement {
			get {
				return Numbers.GetTotalDisplacement();
			}
		}




	}

}
