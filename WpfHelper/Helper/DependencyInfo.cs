using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHelper
{
	public class DependencyInfo
	{
		public string Dependent { get; set; }
		public string Reference { get; set; }
		public DependencyInfo(string dependent, string reference)
		{
			Dependent = dependent;
			Reference = reference;
		}
	}
}
