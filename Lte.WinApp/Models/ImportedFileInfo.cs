using Lte.Parameters.Kpi.Abstract;

namespace Lte.WinApp.Models
{
    public class ImportedFileInfo : PropertyChangedViewModel
    {
        public string FileType { get; set; }

        public string FilePath { get; set; }

        public string CurrentState { get; private set; }

        public string Result { get; private set; }

        private bool _isSelected;

        public bool IsSelected {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public ImportedFileInfo()
        {
            CurrentState = "未读取";
            Result = "未知";
        }

        public void FinishState()
        {
            Result = "导入成功";
            CurrentState = "已读取";
        }

        public void UnnecessaryState()
        {
            Result = "无需导入";
            CurrentState = "已读取";
        }
    }

    public class ParametersDumpConfig : PropertyChangedViewModel, IParametersDumpConfig
    {
        public bool ImportBts { get; set; }

        public bool UpdateBts { get; set; }

        public bool ImportCdmaCell { get; set; }

        public bool UpdateCdmaCell { get; set; }

        public bool ImportENodeb { get; set; }

        public bool UpdateENodeb { get; set; }

        public bool ImportLteCell { get; set; }

        public bool UpdateLteCell { get; set; }

        public bool UpdatePci { get; set; }
    }

}