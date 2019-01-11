using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FileDuplicationUtility
{
    [Serializable]
    public class FileMetaData
    {
        public FileMetaData() { }
        public FileMetaData(string sPath):this()
        {
            
            if(File.Exists(sPath))
            {
                FileName = Path.GetFileName(sPath);
                FilePath = sPath;
                FileSize = new FileInfo(sPath).Length;

                using (SHA256 hasher = SHA256.Create())
                {
                    FileStream fStream = File.OpenRead(sPath);
                    hasher.ComputeHash(fStream);
                    FileHash = Convert.ToBase64String(hasher.Hash);
                }
            }
        }

        [JsonProperty]
        public string FileName { get; set; }
        [JsonProperty]
        public string FilePath { get; set; }
        [JsonProperty]
        public long FileSize { get; set; }
        [JsonProperty]
        public string FileHash { get; set; }
    }
}
