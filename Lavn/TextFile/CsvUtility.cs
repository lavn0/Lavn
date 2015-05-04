using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Lavn.TextFile
{
	public static class CsvUtility
	{
		private const char DOUBLE_QUOT_C = '\"';
		private const string DOUBLE_QUOT = @"""";

		/// <summary>
		/// DataTableをCSV用の文字列に変換する
		/// </summary>
		/// <param name="dt">変換対象のDataTable</param>
		/// <param name="outCaption">ヘッダを出力する場合、true</param>
		/// <returns>CSV変換結果</returns>
		public static string GetCsvData(DataTable dt, bool outCaption)
		{
			var ret = new StringBuilder();

			// ヘッダを書き込む
			if (outCaption)
			{
				var captions = dt.Columns.Cast<DataColumn>().Select(column => GetField(column.Caption));
				var headerLine = string.Join(",", captions);
				ret.AppendLine(headerLine);
			}

			// レコードを書き込む
			foreach (var row in dt.Rows.Cast<DataRow>())
			{
				var fields = row.ItemArray.Select(cell => GetField(cell.ToString()));
				var fieldLine = string.Join(",", fields);
				ret.AppendLine(fieldLine);
			}

			return ret.ToString();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static List<List<string>> GetCsv(string str)
		{
			var open = false;
			var index = 0;
			var result = new List<List<string>>();
			var entry = new List<string>();

			for (var i = 0; i < str.Length; i++)
			{
				switch (str[i])
				{
					case '\"':
						open = !open;
						break;

					case ',':
						if (!open)
						{
							entry.Add(ParseField(str.Substring(index, i - index)));
							index = i + 1;
						}

						break;

					case '\n':
						if (!open)
						{
							var length = i - index + (i > 0 ? str[i - 1] == '\r' ? -1 : 0 : 0);	// "\r\n"対応
							entry.Add(ParseField(str.Substring(index, length)));
							result.Add(entry);
							entry = new List<string>();
							index = i + 1;
						}

						break;
				}
			}

			entry.Add(ParseField(str.Substring(index, str.Length - index)));
			result.Add(entry);
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static IEnumerable<IEnumerable<string>> GetCsv(StreamReader sr)
		{
			// ストリームの末尾まで繰り返す
			while (!sr.EndOfStream)
			{
				// ファイルから一行読み込む
				var line = sr.ReadLine();

				// 読み込んだ一行をカンマ毎に分けて配列に格納する
				var values = SplitLine(line);
				yield return values;
			}
		}

		private static IEnumerable<string> SplitLine(string line)
		{
			var open = false;
			var index = 0;

			for (var i = 0; i < line.Length; i++)
			{
				switch (line[i])
				{
					case '\"':
						open = !open;
						break;

					case ',':
						if (!open)
						{
							yield return line.Substring(index, i - index).Replace(DOUBLE_QUOT + DOUBLE_QUOT, DOUBLE_QUOT);
							index = i + 1;
							continue;
						}

						break;
				}
			}

			if (open)
			{
				throw new ArgumentException("ダブルクォーテーションの囲みが異常です。\t" + line);
			}

			yield return line.Substring(index, line.Length - index).Replace(DOUBLE_QUOT + DOUBLE_QUOT, DOUBLE_QUOT);
		}

		private static string ParseField(string quotedField)
		{
			var quoted = !string.IsNullOrEmpty(quotedField) &&
				'\"' == quotedField[0] &&
				'\"' == quotedField[quotedField.Length - 1];
			var unQuoted = quoted ? quotedField.Substring(1, quotedField.Length - 2) : quotedField;
			var unEscaped = quoted ? unQuoted.Replace(DOUBLE_QUOT + DOUBLE_QUOT, DOUBLE_QUOT) : unQuoted;
			return unEscaped;
		}

		/// <summary>
		/// CSVデータのフィールドとして用いるべき値に変換して返却する
		/// </summary>
		/// <param name="field">値候補</param>
		/// <returns>CSVフィールドデータ</returns>
		private static string GetField(string field)
		{
			if (IsNeedToRoundByDoubleQuot(field))
			{
				return GetRoundedFieldByDoubleQuot(field);
			}

			return field;
		}

		/// <summary>
		/// "で囲まれた文字列を取得する
		/// </summary>
		/// <param name="field">"で囲む文字列</param>
		/// <returns>"で囲まれた文字列</returns>
		private static string GetRoundedFieldByDoubleQuot(string field)
		{
			return DOUBLE_QUOT + field.Replace(DOUBLE_QUOT, DOUBLE_QUOT + DOUBLE_QUOT) + DOUBLE_QUOT;
		}

		/// <summary>
		/// "で囲む必要があるか判定する
		/// </summary>
		/// <param name="field">判定対象</param>
		/// <returns>"で囲む必要がある場合、true</returns>
		private static bool IsNeedToRoundByDoubleQuot(string field)
		{
			return
				field[0] == ' ' ||
				field[0] == '\t' ||
				field[field.Length - 1] == ' ' ||
				field[field.Length - 1] == '\t' ||
				field.IndexOf('"') >= 0 ||
				field.IndexOf(',') >= 0 ||
				field.IndexOf('\r') >= 0 ||
				field.IndexOf('\n') >= 0;
		}
	}
}
