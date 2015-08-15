using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.WinApp.Models;

namespace Lte.WinApp.Service
{
    public class FinishValidFilesStateService
    {
        private readonly IEnumerable<ImportedFileInfo> _validFileInfos;

        public FinishValidFilesStateService(IEnumerable<ImportedFileInfo> validFileInfos)
        {
            _validFileInfos = validFileInfos;
        }

        public void Finish()
        {
            foreach (ImportedFileInfo fileInfo in _validFileInfos)
            {
                fileInfo.FinishState();
            }
        }
    }
}
