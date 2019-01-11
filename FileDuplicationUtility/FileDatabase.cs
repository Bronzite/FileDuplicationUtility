using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileDuplicationUtility
{
    [Serializable]
    public class FileDatabase
    {
        public FileDatabase()
        {
            PathsCache = new HashSet<string>();
            HashCache = new HashSet<string>();
            FileMetaData = new List<FileMetaData>();
        }

        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public List<FileMetaData> FileMetaData {get;set;}
        [JsonProperty]
        public DateTime LastUpdate { get; set; }

        private HashSet<string> PathsCache = new HashSet<string>();
        private HashSet<string> HashCache = new HashSet<string>();

        public void UpdateCache()
        {
            foreach(FileMetaData fmd in FileMetaData)
            {
                if (!PathsCache.Contains(fmd.FilePath))
                    PathsCache.Add(fmd.FilePath);
                if (!HashCache.Contains(fmd.FileHash))
                    HashCache.Add(fmd.FileHash);
            }
        }

        public bool AddFileMetaData(FileMetaData fmd)
        {
            if (PathsCache.Contains(fmd.FilePath)) return false;
            if (!HashCache.Contains(fmd.FileHash)) HashCache.Add(fmd.FileHash);
            FileMetaData.Add(fmd);
            return true;
        }

        public List<DuplicateRecord> GetDuplicateRecords()
        {
            List<DuplicateRecord> retval = new List<DuplicateRecord>();
            FileMetaData.Sort((a, b) => a.FileHash.CompareTo(b.FileHash));
            DuplicateRecord dr = new DuplicateRecord();
            for(int i=0;i<FileMetaData.Count;i++)
            {
                if (dr.Hash == "") dr.Hash = FileMetaData[i].FileHash;
                if(FileMetaData[i].FileHash == dr.Hash)
                {
                    dr.DuplicatedFiles.Add(FileMetaData[i]);
                }
                else
                {
                    if (dr.DuplicatedFiles.Count > 1)
                        retval.Add(dr);
                    dr = new DuplicateRecord();
                    dr.Hash = FileMetaData[i].FileHash;
                }
            }
            if (dr.DuplicatedFiles.Count > 1) retval.Add(dr);
            return retval;
        }
    }
}
