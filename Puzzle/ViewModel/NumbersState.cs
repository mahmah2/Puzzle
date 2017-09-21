using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle.ViewModel
{
	public class NumbersState : ObservableCollection<VMNumberInfo> 
	{
		public NumbersState(int columnCount, int rowCount)
		{
			_columnCount = columnCount;
			_rowCount = rowCount;
		}

		private int _columnCount;
		private int _rowCount;

		public NumbersState Clone()
		{
			var result = new ObservableCollection<VMNumberInfo>(this);

			return (NumbersState)result;
		}


		public bool AreAdjacent(VMNumberInfo a, int X, int Y)
		{
			return (Math.Abs(a.X - X) <= 1) && (Math.Abs(a.Y - Y) <= 1);
		}
		public bool AreCrossAdjacent(VMNumberInfo a, int X, int Y)
		{
			return ((a.X == X) && (Math.Abs(a.Y - Y) <= 1)) ||
				   ((a.Y == Y) && (Math.Abs(a.X - X) <= 1));
		}

		public bool AreCrossAdjacent(VMNumberInfo a, VMNumberInfo b)
		{
			return AreCrossAdjacent(a, b.X, b.Y);
		}

		public VMNumberInfo AtPosition(int X, int Y)
		{
			return this.FirstOrDefault(ni => ni.X == X && ni.Y == Y);
		}

		public List<VMNumberInfo> GetAllAdjacents(System.Drawing.Point ec)
		{
			var result = new List<VMNumberInfo>();

			for (int column = ec.X - 1; column < ec.X + 2; column++)
				for (int row = ec.Y - 1; row < ec.Y + 2; row++)
				{
					var eni = AtPosition(column, row);

					if (eni != null)
						result.Add(eni);
				}

			return result;
		}

		public List<VMNumberInfo> GetCrossAdjacents(System.Drawing.Point ec)
		{
			var result = new List<VMNumberInfo>();

			if (ec.X > 0)
				result.Add(AtPosition(ec.X - 1, ec.Y));

			if (ec.X < _columnCount - 1)
				result.Add(AtPosition(ec.X + 1, ec.Y));

			if (ec.Y > 0)
				result.Add(AtPosition(ec.X, ec.Y - 1));

			if (ec.Y < _rowCount - 1)
				result.Add(AtPosition(ec.X, ec.Y + 1));

			return result;
		}

		public VMNumberInfo GetLeftCell(int x, int y)
		{
			if (x >0 )
				return AtPosition(x - 1, y);
			return null;
		}

		public VMNumberInfo GetRightCell(int x, int y)
		{
			if (x < _columnCount - 1)
				return AtPosition(x + 1, y);
			return null;
		}

		public VMNumberInfo GetUpCell(int x, int y)
		{
			if ( y > 0 )
				return AtPosition(x, y - 1);
			return null;
		}

		public VMNumberInfo GetDownCell(int x, int y)
		{
			if (y < _rowCount - 1)
				return AtPosition(x, y + 1);
			return null;
		}

		public System.Drawing.Point GetEmptyCellPosition()
		{
			for (int row = 0; row < _rowCount; row++)
				for (int column = 0; column < _columnCount; column++)
					if (AtPosition(column, row) == null)
						return new System.Drawing.Point(column, row);

			return new System.Drawing.Point(-1, -1);
		}

		public float GetTotalDisplacement()
		{
			var result = 0f;
			for (int row = 0; row < _rowCount; row++)
				for (int column = 0; column < _columnCount; column++)
				{
					var ni = AtPosition(column, row);
					if (ni != null)
						result += ni.Displacement();
				}

			return result;
		}


	}
}
