using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using Lte.Evaluations.Kpi;
using Lte.Parameters.Kpi.Abstract;
using Lte.WinApp.Controls;
using Lte.WinApp.Service;

namespace Lte.WinApp.Models
{
    public abstract class FileInfoListImporterAsync<TStat, TRepository> : IFileInfoListImporterAsync
        where TRepository : class, ITopCellRepository<TStat>, new()
        where TStat : class, ITimeStat
    {
        protected ITopCellRepository<TStat> repository;

        protected abstract IStatDateImporter GenerateImporter();

        protected Func<string, StreamReader> ReadFile { private get; set; }

        public  FileListGrid FileListGrid { private get; set; }

        public List<ImportedFileInfo> FileInfoList { get; set; }

        public string FileType { get; protected set; }

        public string Result { get; private set; }

        public async void Import(IEnumerable<ImportedFileInfo> validFileInfos)
        {
            Result = "";
            repository = new TRepository();

            foreach (ImportedFileInfo file in validFileInfos)
            {
                IStatDateImporter importer = GenerateImporter();
                ImportedFileInfo fileInfo = file;
                importer.Date = fileInfo.FilePath.RetrieveFileNameBody().GetDateExtend();
                TStat stat = repository.Stats.FirstOrDefault(x => x.StatTime == importer.Date);
                if (stat == null)
                {
                    using (StreamReader reader = ReadFile(fileInfo.FilePath))
                    {
                        int count = await importer.ImportStat(reader, CsvFileDescription.CommaDescription);
                        Result += "\n" + fileInfo.FilePath + "完成导入数量：" + count;
                    }
                    fileInfo.FinishState();
                }
                else
                {
                    Result += "\n日期：" + importer.Date.ToShortDateString() + "的统计记录已导入！";
                    fileInfo.UnnecessaryState();
                }
            }
            FileListGrid.SetDataSource(FileInfoList);
        }
    }
}
