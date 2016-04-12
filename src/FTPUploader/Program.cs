using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Practices.Unity;

namespace FTPUploader
{
    class Program
    {
        private readonly static IUnityContainer _unityContainer = new UnityContainer();

        static void Main(string[] args)
        {
            Register();

            var options = new Options();
            var telegram = new TelegramSender();

            try
            {

                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    // consume Options instance properties

                    Console.WriteLine("Archiving...");

                    Start(options.FtpHost, options.FtpUserName, options.FtpPassword, options.FilePath);

                    Console.WriteLine("Complete");

                    if (!string.IsNullOrWhiteSpace(options.TelegramBotToken))
                    {
                        telegram.Send("Backup!", options.TelegramClientId, options.TelegramBotToken).GetAwaiter().GetResult();
                    }
                }
                else
                {
                    // Display the default usage information
                    Console.WriteLine(options.GetUsage());
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);

                telegram.Send(e.Message, options.TelegramClientId, options.TelegramBotToken).GetAwaiter().GetResult();
                telegram.Send(e.StackTrace, options.TelegramClientId, options.TelegramBotToken).GetAwaiter().GetResult();


                Console.WriteLine(options.GetUsage());
            }

        }


        public static void Start(string url, string userName, string password, string file)
        {
            var archiver = _unityContainer.Resolve<IZipArchiver>();
            var uploader = _unityContainer.Resolve<IUploader>();

            
            var requestStream = uploader.GetUploadStream(url, userName, password);
            archiver.Archivate(file, requestStream);
        }

        private static  void Register()
        {
            _unityContainer.RegisterType<IZipArchiver, ZipArchiverImpl>();
            _unityContainer.RegisterType<IUploader, Uploader>();
        }
    }

    class Options
    {
        [Option('h', "host", Required = true, HelpText = "FtpUrl")]
        public string FtpHost { get; set; }

        [Option('u', "username")]
        public string FtpUserName { get; set; }
        [Option('p', "password")]
        public string FtpPassword { get; set; }

        [Option('f', "file", Required = true)]
        public string FilePath { get; set; }

        [Option("tgrmclient")]
        public int TelegramClientId { get; set; }

        [Option("tgrmtoken")]
        public string TelegramBotToken { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            //  or using HelpText.AutoBuild
            var usage = new StringBuilder();
            usage.AppendLine("Quickstart Application 1.0");
            usage.AppendLine("Read user manual for usage instructions...");
            return usage.ToString();
        }
    }

    public class TelegramSender
    {
        public async Task Send(string message, int chatId, string token)
        {
             var Url =
            $"https://api.telegram.org/bot{token}/sendMessage";
            using (var web = new HttpClient())
            {
                await web.PostAsync(Url, new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("chat_id", chatId.ToString()),
                    new KeyValuePair<string, string>("text", message)
                }));
            }
        }
    }
}
