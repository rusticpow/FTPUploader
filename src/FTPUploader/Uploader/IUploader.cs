using System.IO;

namespace FTPUploader
{
    public interface IUploader
    {
        Stream GetUploadStream(string url, string userName, string password);
    }
}