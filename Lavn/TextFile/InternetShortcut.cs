using System.Diagnostics;

namespace Lavn.TextFile
{
	/// <summary>インターネットショートカットファイルを読み書きするためのクラス</summary>
	[DebuggerDisplay("{Url}")]
	public class InternetShortcut
	{
		private const string SectionName = "InternetShortcut";

		private IniFile path;
		private string urlKey = "Url";
		private string iconFileKey = "IconFile";
		private string iconIndexKey = "IconIndex";
		////private string modifiedKey = "Modified";
		////private string hotkeyKey = "Hotkey";

		/// <summary>ファイル名を指定してインターネットショートカットIniFileクラスを作成します。</summary>
		/// <param name="path">ファイル名</param>
		public InternetShortcut(string path)
		{
			this.path = new IniFile(path);
		}

		/// <summary>ファイル名を指定してインターネットショートカットIniFileクラスを作成します。</summary>
		/// <param name="path">ファイル名</param>
		/// <param name="url">Url</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		public InternetShortcut(string path, string url)
		{
			this.path = new IniFile(path);
			this.Url = url;
		}

		/// <summary>インターネットショートカットファイルのUrl</summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		public string Url
		{
			get
			{
				return this.path.ReadValue(SectionName, this.urlKey, null);
			}

			set
			{
				this.path.WriteValue(SectionName, this.urlKey, value);
			}
		}

		/// <summary>アイコンファイルのパス</summary>
		public string IconFile
		{
			get
			{
				return this.path.ReadValue(SectionName, this.iconFileKey, null);
			}
		}

		/// <summary>アイコンファイルのインデックス(IconFileにDLLを指定した場合などに指定)</summary>
		public int? IconIndex
		{
			get
			{
				int value;

				if (int.TryParse(this.path.ReadValue(SectionName, this.iconIndexKey, null), out value))
				{
					return value;
				}

				return null;
			}
		}

		///// <summary>更新日時</summary>
		///// <remarks>18桁の16進数値で記録され、解析が複雑なため実装を省略</remarks>
		///// <see cref="http://www.atmark.gr.jp/~s2000/r/rtl/InternetShortcut.html"/>
		////public DateTime? Modified
		////{
		////	get;
		////}

		///// <summary>ショートカットキー割り当て</summary>
		////public string Hotkey
		////{
		////	get
		////	{
		////		return this.path.ReadValue(SectionName, this.hotkeyKey, null);
		////	}
		////}
	}
}
