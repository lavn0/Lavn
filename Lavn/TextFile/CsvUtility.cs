// cf. http://www.atmarkit.co.jp/fdotnet/dotnettips/487csvparser/csvparser.html
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Lavn.TextFile
{
	/// <summary>CSVを処理するためのユーティリティ</summary>
	public static class CsvUtility
	{
		/// <summary>CSV文字列をCSVの値に分割します</summary>
		/// <param name="csvStr">CSV文字列</param>
		/// <returns>CSVの値</returns>
		public static List<string[]> GetCsv(string csvStr)
		{
			using (var sr = new MemoryStream(Encoding.UTF8.GetBytes(csvStr)))
			{
				return GetCsv(sr);
			}
		}

		/// <summary>CSV文字列をストリームから読み出し、CSVの値に分割します</summary>
		/// <param name="sr">CSV文字列を返すストリーム(UTF8)</param>
		/// <returns>CSVの値</returns>
		public static List<string[]> GetCsv(Stream sr)
		{
			return GetCsv(sr, Encoding.UTF8);
		}

		/// <summary>CSV文字列をストリームから読み出し、CSVの値に分割します</summary>
		/// <param name="sr">CSV文字列を返すストリーム(UTF8)</param>
		/// <param name="encoding">ストリームの文字コード</param>
		/// <returns>CSVの値</returns>
		public static List<string[]> GetCsv(Stream sr, Encoding encoding)
		{
			var parser = new TextFieldParser(sr, encoding);
			var result = new List<string[]>();

			using (parser)
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(","); // 区切り文字はコンマ

				//// parser.HasFieldsEnclosedInQuotes = false;
				//// parser.TrimWhiteSpace = false;

				while (!parser.EndOfData)
				{
					string[] row = parser.ReadFields(); // 1行読み込み
					result.Add(row);
				}
			}

			return result;
		}
	}
}
