using System;
using System.Collections.Generic;
using System.Text;

namespace FileDuplicationUtility
{
    public class DuplicateRecord
    {
        public DuplicateRecord()
        {
            DuplicatedFiles = new List<FileMetaData>();
        }
        public string Hash { get; set; }
        public List<FileMetaData> DuplicatedFiles { get; set; }
    }
}
