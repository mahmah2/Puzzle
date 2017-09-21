using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace WPFHelper
{
	public class Reflection
	{
		public static List<string> GetListeners(Type t,
				[System.Runtime.CompilerServices.CallerMemberName] string referenceName = "")
		{
			var result = new List<string>();

			foreach (var prop in t.GetProperties())
			{
				foreach (var attr in prop.GetCustomAttributes(typeof(ListensToAttribute), false))
				{
					if (((ListensToAttribute)attr).ListensTo.Equals(referenceName))
						result.Add(prop.Name);
				}
			}

			return result;
		}
		
		public static string GetPropertyName<T>(Expression<Func<T, object>> propertySelector)
		{
            var memberExpression = propertySelector.Body as MemberExpression;

            if (memberExpression == null)
            {
                //It should be a convert exprssion
                var ue = propertySelector.Body as UnaryExpression;

                if (ue == null)
                    return string.Empty;

                var o = ue.Operand;
                memberExpression = o as MemberExpression;

                if (memberExpression == null)
                    return string.Empty;
            }

            return memberExpression.Member.Name;
		}
	}

	
}
