using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Moty.Utils
{
	public static class AppIniFileHelper
	{
		#region 导入Win32 API
		/// <summary>
		/// 取得初始化文件中指定条目的字串。
		/// </summary>
		/// <param name="sectionName">欲在其中查找条目的小节名称。</param>
		/// <param name="keyName">欲获取的项名或条目名。</param>
		/// <param name="defaultReturnedString">指定的条目没有找到时返回的默认值。</param>
		/// <param name="returnedString">指定一个字串缓冲区，长度至少为size。</param>
		/// <param name="size">指定装载到returnedString缓冲区的最大字符数量。</param>
		/// <param name="fileName">初始化文件的名字。</param>
		/// <returns>
		/// 复制到returnedString缓冲区的字节数量，其中不包括那些NULL中止字符。
		/// 如returnedString缓冲区不够大，不能容下全部信息，就返回size-1
		/// （若sectionName或keyName为NULL，则返回size-2）
		/// </returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern uint GetPrivateProfileString(
			string sectionName,
			string keyName,
			string defaultReturnedString,
			[In, Out]char[] returnedString,
			uint size,
			string fileName);

		/// <summary>
		/// 为初始化文件指定小节设置一个字串。
		/// </summary>
		/// <param name="sectionName">要在其中写入新字串的小节名称。</param>
		/// <param name="keyName">要设置的项名或条目名。</param>
		/// <param name="writtenString">指定为这个项写入的字串值。</param>
		/// <param name="fileName">初始化文件的名字。</param>
		/// <returns>设置成功返回true，失败返回false。</returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool WritePrivateProfileString(
			string sectionName,
			string keyName,
			string writtenString,
			string fileName); 
		#endregion

		private const string VALUE_NOT_FOUND = "!!!Sorry : Not Found!!!";

		/// <summary>
		/// 判断结果字符串是否是未找到。
		/// </summary>
		/// <param name="value">结果字符串。</param>
		/// <returns></returns>
		public static bool IsValueNotFound(string value)
		{
			return VALUE_NOT_FOUND.Equals(value);
		}

		/// <summary>
		/// 读取指定文件指定节点指定键对应的值。
		/// </summary>
		/// <param name="fileName">文件名。</param>
		/// <param name="sectionName">节点名。(当为null时返回所有节点名称。)</param>
		/// <param name="keyName">键名。(当为null时返回所有键名称。)</param>
		/// <returns>指定键对应的值。</returns>
		public static string ReadValue(string fileName, string sectionName, string keyName)
		{
			string returnedString;
			string defaultString = VALUE_NOT_FOUND;
			try 
			{
				uint size = (uint)Math.Max(GetCharNumberFromFile(fileName), VALUE_NOT_FOUND.Length);
				char[] chs = new char[size];
				GetPrivateProfileString(sectionName, keyName, defaultString, chs, size, fileName);
				returnedString = new string(chs).Replace("\0", " ").Trim();
			}
			catch (Exception) { returnedString = defaultString; }
			return returnedString;
		}

		/// <summary>
		/// 向指定文件的指定节点的指定键写入值。
		/// </summary>
		/// <param name="fileName">文件名。</param>
		/// <param name="sectionName">节点名。</param>
		/// <param name="keyName">键名。(当为null时会去掉该键所在节点的所有键值对。)</param>
		/// <param name="writtenString">待写入的值。(当为null时会去掉该项键值对。)</param>
		/// <returns>写入成功返回true，失败返回false。</returns>
		public static bool WriteValue(string fileName, string sectionName, string keyName, string writtenString)
		{
			try { return WritePrivateProfileString(sectionName, keyName, writtenString, fileName); }
			catch (Exception) { return false; }
		}

		/// <summary>
		/// 获取文件中字符个数。
		/// </summary>
		/// <param name="fileName">文件名。</param>
		/// <returns>文件中字符个数。</returns>
		/// <exception cref="Exception"></exception>
		private static uint GetCharNumberFromFile(string fileName)
		{
			uint size;
			try
			{
				using (StreamReader sr = new StreamReader(fileName))
				{
					size = (uint)sr.ReadToEnd().Length;
				}
			}
			catch (Exception) { throw; }
			return size;
		}
	}
}
