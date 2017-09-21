using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle.ViewModel
{
	public class BoardActionCollection : Stack<BoardAction> {

	}
	public class BoardAction
	{
		public BoardAction(int fromX, int fromY, int toX, int toY)
		{
			From = new System.Drawing.Point(fromX, fromY);

			To = new System.Drawing.Point(toX, toY);
		}
		public System.Drawing.Point From { get; set; }
		public System.Drawing.Point To { get; set; }
	}
}
