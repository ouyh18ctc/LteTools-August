using System.Windows.Forms;
using Lte.WinApp.Properties;

namespace Lte.WinApp.Models
{
    public abstract class FileDialogWrapper
    {
        protected FileDialog Dialog;

        public bool ShowDialog()
        {
            return Dialog.ShowDialog() == DialogResult.OK && Dialog.FileNames.Length > 0 && Dialog.FileNames[0] != null;
        }

        public string FileName
        {
            get { return Dialog.FileName; }
        }

        public string[] FileNames
        {
            get { return Dialog.FileNames; }
        }
    }

    public class OpenRuFileDialogWrapper : FileDialogWrapper
    {
        public OpenRuFileDialogWrapper()
        {
            Dialog = new OpenFileDialog
            {
                Title = Resources.OpenRuFileDialogTitle,
                Filter = Resources.OpenRuFileDialogFilter,
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = Resources.DefaultRuFileExt,
                Multiselect = true
            };
        }
    }

    public class OpenMrFileDialogWrapper : FileDialogWrapper
    {
        public OpenMrFileDialogWrapper()
        {
            Dialog = new OpenFileDialog
            {
                Title = Resources.OpenMrFileDialogTiltle,
                Filter = Resources.OpenMrFileDialogFilter,
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = Resources.DefaultMrFileExt,
                Multiselect = true
            };
        }
    }

    public class OpenCdrFileDialogWrapper : FileDialogWrapper
    {
        public OpenCdrFileDialogWrapper()
        {
            Dialog = new OpenFileDialog
            {
                Title = Resources.OpenCdrFileDialogTitle,
                Filter = Resources.OpenCdrFileDialogFilter,
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = Resources.DefaultCdrFileExt,
                Multiselect = true
            };
        }
    }

    public class OpenKpiFileDialogWrapper : FileDialogWrapper
    {
        public OpenKpiFileDialogWrapper()
        {
            Dialog = new OpenFileDialog
            {
                Title = Resources.OpenKpiFileDialogTitle,
                Filter = Resources.OpenKpiFileDialogFilter,
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = Resources.DefaultKpiFileExt,
                Multiselect = true
            };
        }
    }

    public class OpenPreciseFileDialogWrapper : FileDialogWrapper
    {
        public OpenPreciseFileDialogWrapper()
        {
            Dialog = new OpenFileDialog
            {
                Title = Resources.OpenPreciseFileDialogTitle,
                Filter = Resources.OpenPreciseFileDialogFilter,
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = Resources.DefaultKpiFileExt,
                Multiselect = true
            };
        }
    }

    public class OpenLteNeighborFileDialogWrapper : FileDialogWrapper
    {
        public OpenLteNeighborFileDialogWrapper()
        {
            Dialog = new OpenFileDialog
            {
                Title = Resources.OpenLteNeighborDialogTitle,
                Filter = Resources.OpenLteNeighborDialogFilter,
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = Resources.DefaultKpiFileExt,
                Multiselect = true
            };
        }
    }

    public class OpenLteFileDialogWrapper : FileDialogWrapper
    {
        public OpenLteFileDialogWrapper()
        {
            Dialog = new OpenFileDialog
            {
                Title = Resources.OpenLteFileDialogTitle,
                Filter = Resources.OpenParametersFileDialogFilter,
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = Resources.DefaultParametersFileExt,
                Multiselect = false
            };
        }
    }

    public class OpenCdmaFileDialogWrapper : FileDialogWrapper
    {
        public OpenCdmaFileDialogWrapper()
        {
            Dialog = new OpenFileDialog
            {
                Title = Resources.OpenCdmaFileDialogTitle,
                Filter = Resources.OpenParametersFileDialogFilter,
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = Resources.DefaultParametersFileExt,
                Multiselect = false
            };
        }
    }

    public class OpenMmlFileDialogWrapper : FileDialogWrapper
    {
        public OpenMmlFileDialogWrapper()
        {
            Dialog = new OpenFileDialog
            {
                Title = Resources.OpenMmlFileDialogTitle,
                Filter = Resources.OpenMmlFileDialogFilter,
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = Resources.DefaultMmlFileExt,
                Multiselect = true
            };
        }
    }
}
