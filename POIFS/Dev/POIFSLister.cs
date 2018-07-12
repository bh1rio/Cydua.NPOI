using System;
using System.Collections;
using System.IO;

using Cydua.NPOI.POIFS.FileSystem;

namespace Cydua.NPOI.POIFS.Dev
{
    public class POIFSLister
    {
        public static void ViewFile(String filename)
        {
            using (Stream stream = new FileStream(filename, FileMode.Open))
            {
                POIFSFileSystem fs = new POIFSFileSystem(stream);
                DisplayDirectory(fs.Root, "");
            }
        }

        public static void DisplayDirectory(DirectoryNode dir, String indent)
        {
            Console.WriteLine(indent + dir.Name + " -");
            String newIndent = indent + "  ";

            IEnumerator it = dir.Entries;
            while (it.MoveNext())
            {
                Object entry = it.Current;
                if (entry is DirectoryNode)
                {
                    DisplayDirectory((DirectoryNode)entry, newIndent);
                }
                else
                {
                    DocumentNode doc = (DocumentNode)entry;
                    String name = doc.Name;
                    if (name[0] < 10)
                    {
                        String altname = "(0x0" + (int)name[0] + ")" + name.Substring(1);
                        name = name.Substring(1) + " <" + altname + ">";
                    }
                    Console.WriteLine(newIndent + name);
                }
            }
        }
    }
}