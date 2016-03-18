using System.IO;

namespace FTPUploader
{
    public interface IZipArchiver
    {
        Stream Archivate(string path);
    }
}