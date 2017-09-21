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

namespace Puzzle.Controls
{
	/// <summary>
	/// Interaction logic for PuzzleButton.xaml
	/// </summary>
	public partial class PuzzleButton : UserControl
	{
		public PuzzleButton()
		{
			InitializeComponent();
			VisualStateManager.GoToState(btn1, "Glowing", true);
		}

		public void Glow()
		{
			VisualStateManager.GoToState(btn1, "Glowing", true);
		}

		private void GlowRelated_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
		{
			if (e.NewState.Name == "Glowing")
			{
				//VisualStateManager.GoToState(btn1, "NotGlowing", true);
			}
		}
	}
}
