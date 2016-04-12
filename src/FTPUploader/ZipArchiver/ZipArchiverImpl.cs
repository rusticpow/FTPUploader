using System.IO;
using System.Reflection.Emit;
using System.Text;
using Ionic.Zip;

namespace FTPUploader
{
    public class ZipArchiverImpl : IZipArchiver
    {
        public void Archivate(string path, Stream stream)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncoding = Encoding.UTF8;
                zip.AlternateEncodingUsage = ZipOption.Always;
                zip.AddDirectory(path);
                zip.UseZip64WhenSaving = Zip64Option.Always;
                zip.Save(stream);
            }
        }
    }
}