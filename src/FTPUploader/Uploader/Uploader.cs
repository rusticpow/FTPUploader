using System;
using System.IO;
using System.Net;

namespace FTPUploader
{
    public class Uploader : IUploader
    {
        public Stream GetUploadStream(string url, string userName, string password)
        {
            WebClient client = new WebClient();
            
            
                client.Credentials = new NetworkCredential(userName, password);
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri($"{url}/{userName}"));
                reqFTP.GetRequestStream();

                Stream ftpStream = reqFTP.GetRequestStream();

                return ftpStream;
            
        }
    }
}