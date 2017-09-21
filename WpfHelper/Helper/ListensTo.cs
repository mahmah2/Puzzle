using System;



namespace WPFHelper
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	public class ListensToAttribute : Attribute
	{
		public string ListensTo { get; set; }
		public ListensToAttribute(string reference)
		{
			ListensTo = reference;
		}
	}
}
