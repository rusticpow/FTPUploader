using Unity;

namespace FTPUploader
{
    class Program
    {
        private readonly static IUnityContainer _unityContainer = new UnityContainer();

        static void Main(string[] args)
        {
            Register();

            Start();
        }


        private static void Start()
        {
            var archiver = _unityContainer.Resolve<IZipArchiver>();
            var uploader = _unityContainer.Resolve<IUploader>();

            var requestStream = uploader.GetUploadStream("")
        }

        private static  void Register()
        {
            _unityContainer.RegisterType<IZipArchiver, ZipArchiverImpl>();
            _unityContainer.RegisterType<IUploader, Uploader>();
        }
    }
}
