using System.Collections.Generic;
using System.Linq;
using Lte.WinApp.Models;

namespace Lte.WinApp.Service
{
    public interface IFileInfoListImporter
    {
        List<ImportedFileInfo> FileInfoList { get; }

        string FileType { get; }

        void Import(ImportedFileInfo[] validFileInfos);
    }

    public interface IFileInfoListImporterAsync
    {
        List<ImportedFileInfo> FileInfoList { get; }

        string FileType { get; }
    }

    public static class QueryValidFileInfosService
    {
        public static IEnumerable<ImportedFileInfo> Query(this IFileInfoListImporter importer)
        {
            return !importer.FileInfoList.Any() ? importer.FileInfoList
                : importer.FileInfoList.Where(x => 
                    x.FileType == importer.FileType && x.CurrentState == "未读取");
        }

        public static IEnumerable<ImportedFileInfo> Query(this IFileInfoListImporterAsync importer)
        {
            return !importer.FileInfoList.Any() ? importer.FileInfoList
                : importer.FileInfoList.Where(x =>
                    x.FileType == importer.FileType && x.CurrentState == "未读取");
        }
    }
}
