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

		/// <summary>階層構造を展開します</summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static IEnumerable<TSource> Descendants<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> predicate)
		{
			foreach (var item in source)
			{
				yield return item;
				var sequence = predicate(item);
				if (sequence == null)
				{
					continue;
				}

				foreach (var child in sequence.Descendants(predicate))
				{
					yield return child;
				}
			}
		}
	}
}
