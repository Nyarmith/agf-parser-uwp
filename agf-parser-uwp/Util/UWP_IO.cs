using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

namespace agf_parser_uwp
{
    public class UWPIO
    {
        public const string SAVEDIR = "SaveDir";
        public const string GAMEDIR = "GameDir";

        // all paths are relative to LocalFolder
        public static async Task<List<string>> listFiles(string path)
        {
            StorageFolder local = getLocal();
            StorageFolder f = await local.GetFolderAsync(path);
            IReadOnlyList<IStorageItem> items = await f.GetItemsAsync();

            List<string> ret = new List<string>();
            foreach(IStorageItem i in items)
            {
                ret.Add(i.Name);
            }
            return ret;
        }

        public static async Task<bool> exists(string path)
        {
            StorageFolder local = getLocal();
            IStorageItem item = await local.TryGetItemAsync(path);

            return (item != null);
        }

        public static async Task makeDir(string path)
        {
            StorageFolder local = getLocal();
            await local.CreateFolderAsync(path);
        }

        //TODO create folders in path if they're not there
        public static async Task createFile(string path, string contents)
        {
            StorageFolder local = getLocal();
            StorageFile f = await local.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(f, contents);
        }

        public static async Task<string> readFile(string path)
        {
            StorageFolder local = getLocal();
            StorageFile f = await local.GetFileAsync(path);

            IBuffer buffer = await FileIO.ReadBufferAsync(f);
            byte[] fileData = buffer.ToArray();
            Encoding enc = Encoding.UTF8;
            string content = enc.GetString(fileData, 0, fileData.Length);

            return content;
        }


        public static async Task<string> dateCreatedAsync(string path)
        {
            StorageFolder local = getLocal();
            StorageFile f = await local.GetFileAsync(path);
            return f.DateCreated.DateTime.ToShortDateString();
        }

        public static async Task<string> dateModifiedAsync(string path)
        {
            StorageFolder local = getLocal();
            StorageFile f = await local.GetFileAsync(path);
            BasicProperties p = await f.GetBasicPropertiesAsync();
            return p.DateModified.DateTime.ToShortDateString();

        }


        //internal utils

        private static string init(string fpath)
        {
            string[] s = fpath.Split('\\');
            return String.Join("\\", s, 0, s.Length - 1);
        }

        private static string last(string fpath)
        {
            string[] s = fpath.Split('\\');
            return s[s.Length - 1];
        }

        private static StorageFolder getLocal()
        {
            StorageFolder local = ApplicationData.Current.LocalFolder;
            return local;
        }

        //basically check if these exist or not, then create them and copy over asset if they don't
        public static async Task initLocalOnStartupAsync()
        {
            StorageFolder local = getLocal();

            IStorageItem item = await local.TryGetItemAsync(GAMEDIR);
            if (item == null)
            {
                await local.CreateFolderAsync(GAMEDIR);

                string oldpath = Package.Current.InstalledLocation.Path;
                StorageFolder dir = await StorageFolder.GetFolderFromPathAsync(oldpath + "\\Assets\\Adventures\\");
                IReadOnlyList<StorageFile> fileList = await dir.GetFilesAsync();

                foreach (StorageFile file in fileList)
                {
                    System.IO.File.Copy(file.Path, local.Path + "\\" + GAMEDIR + "\\" + last(file.Name));
                }
            }

            item = await local.TryGetItemAsync(SAVEDIR);
            if (item == null)
            {
                await local.CreateFolderAsync(SAVEDIR);
            }

        }

        public static async Task<string> storageFileToString(StorageFile sf)
        {

            IBuffer buffer = await FileIO.ReadBufferAsync(sf);
            byte[] fileData = buffer.ToArray();
            Encoding enc = Encoding.UTF8;
            string content = enc.GetString(fileData, 0, fileData.Length);
            return content;
        }
    }
}
