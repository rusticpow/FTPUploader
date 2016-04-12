using System.IO;

namespace FTPUploader
{
    public interface IUploader
    {
        Stream GetUploadStream(string uri, string userName, string password);
    }
}