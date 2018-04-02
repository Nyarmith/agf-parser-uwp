using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Google.Apis.Discovery;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using System.Threading;

namespace agf_parser_uwp
{

    public class WebAPI
    {
        private const int KB = 0x400;
        private const int DownloadChunkSize = 256 * KB;
        private const string DownloadDir = @"agf_repo";
        private const string ContentType = @"text/json";
        private DriveService gdrive;

        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "adventure-game-uwp-client";

        public WebAPI()
        {
            //authenticate
            GoogleCredential cred = GoogleCredential.FromStream(new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read));

            gdrive = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = cred.CreateScoped(Scopes),
                ApplicationName = ApplicationName,
            });
        }

        public List<string> listFiles()
        {
            List<string> ret = new List<string>();

            //define parameters of request
            FilesResource.ListRequest listRequest = gdrive.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id,name)";
            var result = listRequest.Execute();
            IList<Google.Apis.Drive.v3.Data.File> files = result.Files;

            if (files != null && files.Count > 0)
                foreach (var file in files)
                    ret.Add(file.Name);
            else
                ret.Add("No files found.");

            return ret;
        }

        /*
        public async Task<string> downloadFile(string fileName)
        {
            //download file to local adventures
        }

        public async Task<string> getManifestFile()
        {
            //request the metadata file, to get info for other files
        }
        */


        /*  Far future todo
        public void uploadAdventure()
        {
            //request the metadata file, to get info for other files
        }
        */
    }
}
