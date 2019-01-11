using FileDuplicationUtility;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileDuplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FileDuplication Utility");
            Console.WriteLine();

            bool bShowDuplicates = true;
            string sFileDatabaseFile = "";
            string sTarget = "";
            for(int i =0;i<args.Length;i++)
            {
                if (args[i] == "-d" && i < args.Length - 1)
                    sFileDatabaseFile = args[++i];
                else
                if (args[i] == "-q")
                    bShowDuplicates = false;
                else
                    sTarget = args[i];
            }

            FileDatabase fileDatabase = null;
            if (sFileDatabaseFile == "")
                fileDatabase = new FileDatabase();
            else
            {
                if (File.Exists(sFileDatabaseFile))
                    fileDatabase = DuplicationUtility.LoadFileDatabase(sFileDatabaseFile);
                    
                if(fileDatabase==null)
                    fileDatabase = new FileDatabase();
            }

            if (sTarget != "")
            {
                if (File.Exists(sTarget))
                {
                    DuplicationUtility.AddFileToDatabase(fileDatabase, sTarget);
                }
                if (Directory.Exists(sTarget))
                {
                    DuplicationUtility.AddDirectoryToDatabase(fileDatabase, sTarget);
                }

                if (sFileDatabaseFile != "")
                    DuplicationUtility.SaveFileDatabase(fileDatabase, sFileDatabaseFile);
            }

            if(bShowDuplicates)
            {
                List<DuplicateRecord> lstDuplicates = fileDatabase.GetDuplicateRecords();
                foreach(DuplicateRecord dr in lstDuplicates)
                {
                    Console.WriteLine(":{0}", dr.Hash);
                    foreach(FileMetaData fmd in dr.DuplicatedFiles)
                    {
                        Console.WriteLine("\t{0}", fmd.FilePath);
                    }
                }
            }

        }
    }
}
