using System;
using System.Collections.Generic;

namespace Lavn.Linq
{
	/// <summary>LINQ Extension</summary>
	public static partial class Linqx
	{
		/// <summary>混在判定を行います。
		/// <para>判定対象の評価結果が全てtrueの場合、true</para>
		/// <para>判定対象の評価結果が全てfalseの場合、false</para>
		/// <para>それ以外の場合、null</para>
		/// </summary>
		/// <typeparam name="TSource">判定対象の型</typeparam>
		/// <param name="source">判定対象</param>
		/// <param name="predicate">評価式</param>
		/// <returns>
		/// <para>判定対象の評価結果が全てtrueの場合、true</para>
		/// <para>判定対象の評価結果が全てfalseの場合、false</para>
		/// <para>それ以外の場合、null</para>
		/// </returns>
		public static bool? MixedResult<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			bool? result = null;
			foreach (var item in source)
			{
				var ret = predicate(item);
				if (result.HasValue && result != ret)
				{
					return null;
				}

				result = ret;
			}

			return result;
		}
	}
}
