using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Moty.Utils;
using Moty.UI.Native;

namespace Moty.FolderDecorator
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			this.model = new FolderViewModel();

			this.DataContext = this.model;
		}

		private const string INIFILE_NAME = "desktop.ini";
		private const string ICON_NAME = "icon.ico";
		private const string SECTION_NAME = ".ShellClassInfo";
		private const string ICON_RESOURCE_KEY = "IconResource";
		private const string LOCALIZED_RESOURCE_NAME = "LocalizedResourceName";


		private FolderViewModel model;

		private System.Windows.Forms.FolderBrowserDialog fbd;
		private System.Windows.Forms.OpenFileDialog ofd;

		private void SelectFolder_Click(object sender, RoutedEventArgs e)
		{
			if (this.fbd == null)
			{
				this.fbd = new System.Windows.Forms.FolderBrowserDialog();
				this.fbd.Description = "选择需要个性化的目标文件夹";
				this.fbd.ShowNewFolderButton = false;
			}

			if (this.fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.model.FolderPath = fbd.SelectedPath;
				this.model.IsFolderSelected = true;
			}
		}

		private void SelectIcon_Click(object sender, RoutedEventArgs e)
		{
			if (this.ofd == null)
			{
				this.ofd = new System.Windows.Forms.OpenFileDialog();
				this.ofd.DefaultExt = "ico";
				this.ofd.Filter = "图标文件(*.ico)|*.ico";
				this.ofd.FilterIndex = 0;
				this.ofd.Title = "选择图标文件";
			}

			if (this.ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.model.FolderIcon = this.ofd.FileName;
			}
		}

		private void Custom_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (CheckFolder() && CheckIcon() && CheckName())
				{
					string folder = this.model.FolderPath;
					string iniFile = System.IO.Path.Combine(folder, INIFILE_NAME);
					
					if ((bool)ckbIcon.IsChecked)
					{
						string iconNewPath = System.IO.Path.Combine(folder, ICON_NAME);
						if (System.IO.File.Exists(iconNewPath))
						{
							System.IO.File.Delete(iconNewPath);
						}
						System.IO.File.Copy(this.model.FolderIcon, iconNewPath);
						AppIniFileHelper.WriteValue(iniFile, SECTION_NAME, ICON_RESOURCE_KEY, iconNewPath + ",0");

						var imgAttrs = System.IO.File.GetAttributes(iconNewPath);
						System.IO.File.SetAttributes(iconNewPath, imgAttrs | System.IO.FileAttributes.Hidden);
					}
					if ((bool)ckbName.IsChecked)
					{
						AppIniFileHelper.WriteValue(iniFile, SECTION_NAME, LOCALIZED_RESOURCE_NAME, this.model.FolderName);
					}
					var iniFileAttrs = System.IO.File.GetAttributes(iniFile);
					// 将desktop.ini文件设为系统文件，并且隐藏
					System.IO.File.SetAttributes(iniFile, iniFileAttrs
						| System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System);

					var folderAttrs = System.IO.File.GetAttributes(folder);
					// 要想文件夹图标改变，必须将文件夹设为系统文件夹
					System.IO.File.SetAttributes(folder, folderAttrs | System.IO.FileAttributes.System);

					MsgBox.Info("文件夹个性化成功！\r\n实际效果可能会有几秒至几十秒的延迟。");
				}
			}
			catch (Exception e1) { MsgBox.Warning(e1.StackTrace); }
		}

		private bool CheckFolder()
		{
			string folder = this.model.FolderPath;
			if (string.IsNullOrEmpty(folder))
			{
				MsgBox.Warning("请选择目标文件夹");
				this.txtFolder.Focus();
				return false;
			}
			if (!System.IO.Directory.Exists(folder))
			{
				MsgBox.Warning("目标文件夹不存在");
				this.txtFolder.Focus();
				return false;
			}

			return true;
		}

		private bool CheckIcon()
		{
			string icon = this.model.FolderIcon;

			if ((bool)ckbIcon.IsChecked)
			{
				if (string.IsNullOrEmpty(icon))
				{
					MsgBox.Warning("请选择个性化图标");
					this.txtIcon.Focus();
					return false;
				}
				if ((bool)ckbIcon.IsChecked && !System.IO.File.Exists(icon))
				{
					MsgBox.Warning("个性化图标不存在");
					this.txtIcon.Focus();
					return false;
				}
			}
			return true;
		}

		private bool CheckName()
		{
			string name = this.model.FolderName;

			if ((bool)ckbName.IsChecked)
			{
				if (string.IsNullOrEmpty(name))
				{
					MsgBox.Warning("请输入个性化名称");
					this.txtName.Focus();
					return false;
				}
				if (name.Any(p => System.IO.Path.GetInvalidPathChars().Contains(p)))
				{
					MsgBox.Warning("个性化名称不合法");
					this.txtName.Focus();
					return false;
				}
			}

			return true;
		}
	}
}
