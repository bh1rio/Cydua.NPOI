using System;
using System.Text;
using System.IO;
using System.Collections;
using Cydua.NPOI.POIFS.FileSystem;

namespace Cydua.NPOI.POIFS.Dev
{
    public class POIFSViewer
    {
        public static void ViewFile(String filename, bool printName)
        {
            if (printName)
            {
                StringBuilder flowerbox = new StringBuilder();

                flowerbox.Append(".");
                for (int j = 0; j < filename.Length; j++)
                {
                    flowerbox.Append("-");
                }
                flowerbox.Append(".");
                Console.WriteLine(flowerbox);
                Console.WriteLine("|" + filename + "|");
                Console.WriteLine(flowerbox);
            }
            try
            {
                using (Stream fileStream = File.OpenRead(filename))
                {
                    POIFSViewable fs = (POIFSViewable)new POIFSFileSystem(fileStream);
                
                    IList strings = POIFSViewEngine.InspectViewable(fs, true,
                                                0, "  ");
                    IEnumerator iter = strings.GetEnumerator();

                    while (iter.MoveNext())
                    {
                        Console.Write(iter.Current);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
