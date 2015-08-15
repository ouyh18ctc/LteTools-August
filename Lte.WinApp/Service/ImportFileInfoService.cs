using System.Collections.Generic;
using System.Linq;
using Lte.WinApp.Models;

namespace Lte.WinApp.Service
{
    public static class ImportFileInfoService
    {
        public static void ImportFiles(this IFileInfoListImporter importer, IEnumerable<string> fileNames)
        {
            foreach (string fileName in fileNames.Where(
                fileName => importer.FileInfoList.All(x => x.FilePath != fileName)))
            {
                importer.FileInfoList.Add(new ImportedFileInfo
                {
                    FilePath = fileName,
                    FileType = importer.FileType,
                    IsSelected = true
                });
            }
        }

        public static void ImportFiles(this IFileInfoListImporterAsync importer, IEnumerable<string> fileNames)
        {
            foreach (string fileName in fileNames.Where(
                fileName => importer.FileInfoList.All(x => x.FilePath != fileName)))
            {
                importer.FileInfoList.Add(new ImportedFileInfo
                {
                    FilePath = fileName,
                    FileType = importer.FileType,
                    IsSelected = true
                });
            }
        }
    }
}
