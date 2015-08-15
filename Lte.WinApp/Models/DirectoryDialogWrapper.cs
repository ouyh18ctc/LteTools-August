using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Lte.WinApp.Models
{
    public abstract class DirectoryDialogWrapper
    {
        protected FolderBrowserDialog Dialog;

        public bool ShowDialog()
        {
            return Dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(Dialog.SelectedPath);
        }

        public string Directory
        {
            get { return Dialog.SelectedPath; }
        }
    }

    public class MrDirectoryDialogWrapper : DirectoryDialogWrapper
    {
        public MrDirectoryDialogWrapper()
        {
            Dialog = new FolderBrowserDialog
            {
                Description = "打开MR文件夹"
            };
        }
    }
}
