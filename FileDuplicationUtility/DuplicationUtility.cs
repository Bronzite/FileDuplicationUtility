using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileDuplicationUtility
{
    static public class DuplicationUtility
    {
        public static FileDatabase LoadFileDatabase(Stream streamData)
        {
            JsonReader jsonReader = new JsonTextReader(new StreamReader(streamData));
            JsonSerializer serializer = JsonSerializer.Create();
            object o= serializer.Deserialize<FileDatabase>(jsonReader);
            FileDatabase retval = o as FileDatabase;
            if (retval != null) 
                retval.UpdateCache();
            return retval;
        }

        public static void SaveFileDatabase(FileDatabase fileDatabase, Stream streamData)
        {
            using (JsonWriter jsonWriter = new JsonTextWriter(new StreamWriter(streamData)))
            {
                JsonSerializer serializer = JsonSerializer.Create();
                serializer.Serialize(jsonWriter, fileDatabase);
                jsonWriter.Flush();
            }
        }

        public static void SaveFileDatabase(FileDatabase fileDatabase, string sPath)
        {
            using (Stream s = File.OpenWrite(sPath))
            {
                SaveFileDatabase(fileDatabase, s);
            }
        }

        public static FileDatabase LoadFileDatabase(string sPath)
        {
            using (Stream s = File.OpenRead(sPath))
            {
                return LoadFileDatabase(s);
            }
        }

        public static void AddFileToDatabase(FileDatabase fileDatabase, string sPath)
        {
            FileMetaData fmd = new FileMetaData(sPath);

            fileDatabase.AddFileMetaData(fmd);
            fileDatabase.LastUpdate = DateTime.Now;
        }

        public static void AddDirectoryToDatabase(FileDatabase fileDatabase, string sPath)
        {
            DirectoryInfo DInfo = new DirectoryInfo(sPath);
            foreach(DirectoryInfo curDirInfo in DInfo.GetDirectories())
            {
                AddDirectoryToDatabase(fileDatabase, curDirInfo.FullName);
            }
            foreach(FileInfo curFileInfo in DInfo.GetFiles())
            {
                AddFileToDatabase(fileDatabase, curFileInfo.FullName);
            }
        }
    }
}
