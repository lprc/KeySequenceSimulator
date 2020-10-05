using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySequenceSimulator
{
    class RecentFiles
    {
        private string RECENT_FILES_FILE = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "kss_files.txt");

        private List<string> files = new List<string>();

        public int MaxFilesCount { get; set; }

        public RecentFiles()
        {
            MaxFilesCount = 10;
        }

        // loads recent files list from file. Only returns existing files
        public List<string> LoadList()
        {
            // file list is stored in a text file. Each line is one file
            if (File.Exists(RECENT_FILES_FILE))
            {
                foreach (var f in File.ReadLines(RECENT_FILES_FILE))
                    if (File.Exists(f) && files.Count <= MaxFilesCount)
                        files.Add(f);
            }

            return files;
        }

        // adds file to list if it exists and updates files list file
        public void AddFile(string file)
        {
            if (File.Exists(file))
            {
                if (files.Contains(file))
                {
                    // move to first position if file already exists in list
                    files.Remove(file);
                    files.Insert(0, file);
                }
                else
                {
                    // prepend to list and crop list if needed
                    files.Insert(0, file);
                    if (files.Count > MaxFilesCount)
                        files.RemoveRange(MaxFilesCount, files.Count - MaxFilesCount);
                }
            }

            WriteList();
        }

        // writes files list to disk if it is not empty
        private void WriteList()
        {
            if (files.Count > 0)
                File.WriteAllLines(RECENT_FILES_FILE, files.ToArray());
        }
    }
}
