using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Moty.FolderDecorator
{
	public class FolderViewModel : INotifyPropertyChanged
	{
		private string folderPath = string.Empty;
		public string FolderPath
		{
			get { return folderPath; }
			set
			{
				if (folderPath.Equals(value))
					return;
				folderPath = value;
				NotifyPropertyChanged("FolderPath");
			}
		}

		private string folderIcon = string.Empty;
		public string FolderIcon
		{
			get { return folderIcon; }
			set
			{
				if (folderIcon.Equals(value))
					return;
				folderIcon = value;
				NotifyPropertyChanged("FolderIcon");
			}
		}

		private string folderName = string.Empty;
		public string FolderName
		{
			get { return folderName; }
			set
			{
				if (folderName.Equals(value))
					return;
				folderName = value;
				NotifyPropertyChanged("FolderName");
			}
		}

		private bool isFolderSelected = false;
		public bool IsFolderSelected
		{
			get { return isFolderSelected; }
			set
			{
				if (isFolderSelected == value)
					return;
				isFolderSelected = value;
				NotifyPropertyChanged("IsFolderSelected");
			}
		}


		/// <summary>
		/// 属性更改事件。
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		/// <summary>
		/// 向外通知属性已经更改。
		/// </summary>
		/// <param name="callerPropertyName">调用该方法的属性的属性名。</param>
		protected void NotifyPropertyChanged(string callerPropertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(callerPropertyName));
			}
		}
	}
}
