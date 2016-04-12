using System;
using System.IO;
using System.Net;

namespace FTPUploader
{
    public class Uploader : IUploader
    {
        public Stream GetUploadStream(string url, string userName, string password)
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd__HH-mm");
            var fileName = $@"{date}.zip";

            CreateFolder(userName, password, url, fileName);

            FtpWebRequest request;
            
            request = WebRequest.Create(new Uri($"{url}/{fileName}")) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = true;
            request.Credentials = new NetworkCredential(userName, password);
            request.ConnectionGroupName = "group";

            var requestStream = request.GetRequestStream();

            return requestStream;
        }

        private void CreateFolder(string userName, string password, string path, string fileName)
        {

            try
            {
                var request = (FtpWebRequest) WebRequest.Create(new Uri(string.Format(path)));
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = true;
                request.Credentials = new NetworkCredential(userName, password);
                request.ConnectionGroupName = "group";
                using (var resp = (FtpWebResponse) request.GetResponse())
                {
                    Console.WriteLine(resp.StatusCode);
                }
            }
            catch{}
        }
    }
}