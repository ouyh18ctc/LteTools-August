using System.Linq;
using Lte.WinApp.Models;

namespace Lte.WinApp.Service
{
    public static class RuMrCdrImportService
    {
        public static void ImportRu(this IFileInfoListImporter ruImporter)
        {
            ImportedFileInfo[] validRuFileInfos = ruImporter.Query().ToArray();
            if (validRuFileInfos.Any()) ruImporter.Import(validRuFileInfos);
        }

        public static void ImportCdr(this IFileInfoListImporter cdrImporter)
        {
            ImportedFileInfo[] validCdrFileInfos = cdrImporter.Query().ToArray();
            if (validCdrFileInfos.Any()) cdrImporter.Import(validCdrFileInfos);
        }
    }
}
