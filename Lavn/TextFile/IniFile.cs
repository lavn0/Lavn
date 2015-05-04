// cf. https://gist.github.com/katabamisan/5231237
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace Lavn.TextFile
{
	/// <summary>.iniファイルを読み書きするためのクラス</summary>
	public class IniFile
	{
		public const int InitialBufferExpandingSize = 256;

		private int bufferExpandingSize;

		/// <summary>ファイル名を指定してIniFileクラスを作成します。</summary>
		/// <param name="fileName">ファイル名</param>
		public IniFile(string fileName)
		{
			this.FileName = fileName;
			this.BufferExpandingSize = InitialBufferExpandingSize;
		}

		/// <summary>.iniファイルのパス。</summary>
		public string FileName { get; private set; }

		/// <summary>値やセクション読み込み時のバッファーの拡張サイズです。バイト単位で取得・設定します。</summary>
		public int BufferExpandingSize
		{
			get
			{
				return this.bufferExpandingSize;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value", "バッファーの拡張サイズには正の値を指定して下さい。");
				}

				this.bufferExpandingSize = value;
			}
		}

		/// <summary>セクション名の一覧を返します。</summary>
		/// <returns>セクション名</returns>
		public string[] GetSectionNames()
		{
			IntPtr buffer = IntPtr.Zero;
			try
			{
				int length = 0;
				int copied = 0;
				do
				{
					length += this.BufferExpandingSize;
					buffer = Marshal.ReAllocCoTaskMem(buffer, length);
					copied = (int)UnsafeNativeMethods.GetPrivateProfileString(null, null, null, buffer, (uint)length, this.FileName);
				}
				while (copied + 2 == length);

				return Marshal.PtrToStringAuto(buffer, copied - 1).Split('\0');
			}
			finally
			{
				Marshal.FreeCoTaskMem(buffer);
			}
		}

		/// <summary>セクションに属するキー名の一覧を返します。</summary>
		/// <param name="sectionName">セクション名</param>
		/// <returns>セクションに属するキー名</returns>
		public string[] GetKeyNames(string sectionName)
		{
			IntPtr buffer = IntPtr.Zero;
			try
			{
				int length = 0;
				int copied = 0;
				do
				{
					length += this.BufferExpandingSize;
					buffer = Marshal.ReAllocCoTaskMem(buffer, length);
					copied = (int)UnsafeNativeMethods.GetPrivateProfileString(sectionName, null, null, buffer, (uint)length, this.FileName);
				}
				while (copied + 2 == length);

				return Marshal.PtrToStringAuto(buffer, copied - 1).Split('\0');
			}
			finally
			{
				Marshal.FreeCoTaskMem(buffer);
			}
		}

		/// <summary>ある名前のセクションが存在するかどうかを返します。</summary>
		/// <param name="sectionName">セクション名</param>
		/// <returns>セクションが存在する場合、true</returns>
		public bool ContainsSection(string sectionName)
		{
			return Array.FindIndex<string>(
				this.GetSectionNames(),
				(string x) => sectionName.Equals(x, StringComparison.OrdinalIgnoreCase)) != -1;
		}

		/// <summary>あるセクションにある名前のキーが存在するかどうかを返します。</summary>
		/// <param name="sectionName">セクション名</param>
		/// <param name="keyName">キー名</param>
		/// <returns>セクションにキーが存在する場合、true</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "keyName")]
		public bool ContainsKey(string sectionName, string keyName)
		{
			return Array.FindIndex<string>(
				this.GetKeyNames(sectionName),
				(string x) => sectionName.Equals(x, StringComparison.OrdinalIgnoreCase)) != -1;
		}

		/// <summary>値を読み込みます。</summary>
		/// <param name="sectionName">セクション名</param>
		/// <param name="key">キー</param>
		/// <param name="defaultValue">デフォルト値</param>
		/// <returns>読み取った値</returns>
		public string ReadValue(string sectionName, string key, string defaultValue)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}

			IntPtr buffer = IntPtr.Zero;
			try
			{
				int length = 0;
				int copied = 0;
				do
				{
					length += this.BufferExpandingSize;
					buffer = Marshal.ReAllocCoTaskMem(buffer, length);
					copied = (int)UnsafeNativeMethods.GetPrivateProfileString(sectionName, key, defaultValue, buffer, (uint)length, this.FileName);
				}
				while (copied + 2 == length);

				return Marshal.PtrToStringAuto(buffer, copied);
			}
			finally
			{
				Marshal.FreeCoTaskMem(buffer);
			}
		}

		/// <summary>値を書き込みます。</summary>
		/// <param name="sectionName">書き込むセクション名</param>
		/// <param name="key">書き込むキー</param>
		/// <param name="value">書き込む値</param>
		public void WriteValue(string sectionName, string key, string value)
		{
			UnsafeNativeMethods.WritePrivateProfileString(sectionName, key, value, this.FileName);
		}

		/// <summary>セクションを読み込みます。セクションはキーと値が等号で結ばれたセットの配列から構成されます。</summary>
		/// <param name="sectionName">セクション名</param>
		/// <returns>セクション内の値</returns>
		public string[] ReadSection(string sectionName)
		{
			IntPtr buffer = IntPtr.Zero;
			try
			{
				int length = 0;
				int copied = 0;
				do
				{
					length += this.BufferExpandingSize;
					buffer = Marshal.ReAllocCoTaskMem(buffer, length);
					copied = (int)UnsafeNativeMethods.GetPrivateProfileSection(sectionName, buffer, (uint)length, this.FileName);
				}
				while (copied + 2 == length);

				return Marshal.PtrToStringAuto(buffer, copied - 1).Split('\0');
			}
			finally
			{
				Marshal.FreeCoTaskMem(buffer);
			}
		}

		/// <summary>セクションをキーと値のDictionary&lt;string, string&gt;として返します。</summary>
		/// <param name="sectionName">セクション名</param>
		/// <returns>キーと値の辞書</returns>
		public Dictionary<string, string> ReadSectionAsDictionary(string sectionName)
		{
			string[] sections = this.ReadSection(sectionName);
			Dictionary<string, string> dict = new Dictionary<string, string>();

			foreach (string section in sections)
			{
				string[] parts = section.Split('=');
				dict.Add(parts[0], parts[1]);
			}

			return dict;
		}

		/// <summary>セクションを書き込みます。セクションはキーと値を等号で結び改行で結合して構成されます。</summary>
		/// <param name="sectionName">書き込むセクション名</param>
		/// <param name="data">書き込む値</param>
		public void WriteSection(string sectionName, string data)
		{
			UnsafeNativeMethods.WritePrivateProfileSection(sectionName, data, this.FileName);
		}

		[SuppressUnmanagedCodeSecurity]
		private static class UnsafeNativeMethods
		{
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
			public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, IntPtr lpReturnedString, uint nSize, string lpFileName);

			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
			public static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);
		}
	}
}
