using System;
using System.Collections.Generic;
using System.Linq;

namespace Lavn.Linq
{
	/// <summary>LINQ Extension</summary>
	public static partial class Linqx
	{
		/// <summary>指定した個数より多いか評価します</summary>
		/// <typeparam name="TSource">判定対象の型</typeparam>
		/// <param name="source">判定対象</param>
		/// <param name="count">個数</param>
		/// <returns>指定した個数より多い場合、true</returns>
		public static bool CountOver<TSource>(this IEnumerable<TSource> source, int count)
		{
			return source.ElementAtOrDefault(count) != null;
		}

		/// <summary>指定した個数より多いか評価します</summary>
		/// <typeparam name="TSource">判定対象の型</typeparam>
		/// <param name="source">判定対象</param>
		/// <param name="count">個数</param>
		/// <param name="predicate">条件式</param>
		/// <returns>指定した個数より多い場合、true</returns>
		public static bool CountOver<TSource>(this IEnumerable<TSource> source, int count, Func<TSource, bool> predicate)
		{
			return source.Where(predicate).ElementAtOrDefault(count) != null;
		}

		/// <summary>指定した個数より少ないか評価します</summary>
		/// <typeparam name="TSource">判定対象の型</typeparam>
		/// <param name="source">判定対象</param>
		/// <param name="count">個数</param>
		/// <returns>指定した個数より少ない場合、true</returns>
		public static bool CountUnder<TSource>(this IEnumerable<TSource> source, int count)
		{
			return count <= 1 ? false : source.ElementAtOrDefault(count - 1) == null;
		}

		/// <summary>指定した個数より少ない評価します</summary>
		/// <typeparam name="TSource">判定対象の型</typeparam>
		/// <param name="source">判定対象</param>
		/// <param name="count">個数</param>
		/// <param name="predicate">条件式</param>
		/// <returns>指定した個数より少ない場合、true</returns>
		public static bool CountUnder<TSource>(this IEnumerable<TSource> source, int count, Func<TSource, bool> predicate)
		{
			return count <= 1 ? false : source.Where(predicate).ElementAtOrDefault(count - 1) == null;
		}

		/// <summary>指定した個数かどうか評価します</summary>
		/// <typeparam name="TSource">判定対象の型</typeparam>
		/// <param name="source">判定対象</param>
		/// <param name="count">個数</param>
		/// <returns>指定した個数の場合、true</returns>
		public static bool CountEquals<TSource>(this IEnumerable<TSource> source, int count)
		{
			if (count == 0)
			{
				return !source.Any();
			}

			var result = source;
			if (count - 1 > 0)
			{
				result = result.Skip(count - 1);
			}

			return result.Take(2).Count() == 1;
		}

		/// <summary>指定した個数かどうか評価します</summary>
		/// <typeparam name="TSource">判定対象の型</typeparam>
		/// <param name="source">判定対象</param>
		/// <param name="count">個数</param>
		/// <param name="predicate">条件式</param>
		/// <returns>指定した個数の場合、true</returns>
		public static bool CountEquals<TSource>(this IEnumerable<TSource> source, int count, Func<TSource, bool> predicate)
		{
			if (count == 0)
			{
				return !source.Where(predicate).Any();
			}

			var result = source.Where(predicate);
			if (count - 1 > 0)
			{
				result = result.Skip(count - 1);
			}

			return result.Take(2).Count() == 1;
		}
	}
}
