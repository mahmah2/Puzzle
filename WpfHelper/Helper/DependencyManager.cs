using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WPFHelper
{
    public static class DependencyManager
	{
		public static Dictionary<string, List<DependencyInfo>> dependencies = new Dictionary<string, List<DependencyInfo>>();

		public static void SetupListener<T>(Expression<Func<T, object>> listener,
			Expression<Func<T, object>> reference)
		{
			var listenerName = Reflection.GetPropertyName<T>(listener);
			var referenceName = Reflection.GetPropertyName<T>(reference);

			var className = typeof(T).Name;

			if (!dependencies.ContainsKey(className))
				dependencies.Add(className, new List<DependencyInfo>());

			var pairList = dependencies[className];

			if (pairList.Count(di => di.Dependent == listenerName && di.Reference == referenceName) == 0)
				pairList.Add(new DependencyInfo(listenerName, referenceName));
		}

		public static List<string> GetListeners(Type type,
			string referenceName)
		{
			var result = new List<string>();

			try
			{
				var className = type.Name;

				if (dependencies.ContainsKey(className))
				{
					var pairs = dependencies[className];

					if (pairs != null)
					{
						result.AddRange(pairs.Where(di => di.Reference == referenceName).Select(di => di.Dependent));
					}
				}

			}
			catch
			{
			}

			return result;
		}

	}
}
