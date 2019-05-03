using System.Windows.Forms;

namespace Moty.UI.Native
{
	/// <summary>
	/// 封装了一系列MessageBox，简化操作。
	/// </summary>
	public static class MsgBox
	{
		/// <summary>
		/// 弹出警告框，并显示指定文本。
		/// </summary>
		public static DialogResult Warning(string text)
		{
			return MessageBox.Show(text, "警告提示",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
		
		/// <summary>
		/// 弹出错误框，并显示指定文本。
		/// </summary>
		public static DialogResult Error(string text)
		{
			return MessageBox.Show(text, "错误提示",
				MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// 弹出提示框，并显示指定文本。
		/// </summary>
		public static DialogResult Info(string text)
		{
			return MessageBox.Show(text, "信息提示", 
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// <summary>
		/// 弹出确认框，并显示指定文本。
		/// </summary>
		public static DialogResult Confirm(string text)
		{
			return MessageBox.Show(text, "确认提示",
				MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
		}

		/// <summary>
		/// 弹出确认框，并显示指定文本。
		/// </summary>
		public static DialogResult Confirm(string text, MessageBoxDefaultButton btn = MessageBoxDefaultButton.Button1)
		{
			return MessageBox.Show(text, "确认提示",
				MessageBoxButtons.OKCancel, MessageBoxIcon.Question, btn);
		}
	}
}
