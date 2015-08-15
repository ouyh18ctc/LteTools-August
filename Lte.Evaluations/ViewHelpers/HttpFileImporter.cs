using System;
using System.IO;
using System.Linq;
using System.Web;

namespace Lte.Evaluations.ViewHelpers
{
    public abstract class HttpImporter : IDisposable
    {
        public string FileName { get; private set; }

        public StreamReader Reader { get; protected set; }

        public bool Success { get; set; }

        public string FilePath { get; protected set; }

        public abstract string BaseDirectory { get; }

        protected HttpImporter(HttpPostedFileBase file)
        {
            if (!file.IsValid())
            {
                Success = false;
            }
            else
            {
                Success = true;
                FileName = Path.GetFileName(file.FileName);
            }
        }

        protected abstract void ConstructReader(HttpPostedFileBase file);

        public void Dispose()
        {
            if (Reader != null)
            {
                Reader.Close();
            }
        }
    }

    public sealed class HttpFileImporter : HttpImporter
    {
        public HttpFileImporter(HttpPostedFileBase file)
            : base(file)
        {
            FilePath = Path.Combine(BaseDirectory, FileName ?? "");
            ConstructReader(file);
        }

        public override string BaseDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory + "uploads\\"; }
        }

        protected override void ConstructReader(HttpPostedFileBase file)
        {
            file.SaveAs(FilePath);
            Reader = new StreamReader(FilePath);
        }
    }

    public sealed class ImageFileImporter : HttpImporter
    {
        private readonly string _category;
        public ImageFileImporter(HttpPostedFileBase file, string category) : base(file)
        {
            _category = category;
            FilePath = Path.Combine(BaseDirectory, FileName ?? "");
            ConstructReader(file);
        }

        public override string BaseDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory + "uploads\\" + _category; }
        }

        protected override void ConstructReader(HttpPostedFileBase file)
        {
            DirectoryInfo imgDirectoryInfo=new DirectoryInfo(BaseDirectory);
            if (!imgDirectoryInfo.Exists)
            {
                imgDirectoryInfo.Create();
            }
            file.SaveAs(FilePath);
        }
    }

    public static class ImportOperations
    {
        public static TResult ImportInfo<TResult>(this HttpPostedFileBase attachedFile,
            Func<string, TResult> ImportAction, string keyword)
        {
            TResult result;
            using (HttpFileImporter importer = new HttpFileImporter(attachedFile))
            {
                if (importer.Success)
                {
                    result = ImportAction(importer.FilePath);
                }
                else
                {
                    string file = Directory.GetFiles(importer.BaseDirectory).FirstOrDefault(
                        x => x.IndexOf(keyword, StringComparison.Ordinal) > 0);
                    if (file != null)
                    {
                        importer.Success = true;
                    }
                    result = ImportAction(file);
                }
            }
            return result;
        }

        public static bool IsValid(this HttpPostedFileBase httpPostedFileBase)
        {
            return httpPostedFileBase != null && (httpPostedFileBase.FileName != "");
        }
    }
}
