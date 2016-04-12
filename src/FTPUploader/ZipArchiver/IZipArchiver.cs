using System.IO;

namespace FTPUploader
{
    public interface IZipArchiver
    {
        void Archivate(string path, Stream stream);
    }
}