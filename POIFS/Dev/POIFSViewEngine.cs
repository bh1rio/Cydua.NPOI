using System;
using System.Text;
using System.Collections;
using System.IO;
using Cydua.NPOI.POIFS.FileSystem;

namespace Cydua.NPOI.POIFS.Dev
{
    /// <summary>
    /// This class contains methods used to inspect POIFSViewable objects
    /// @author Marc Johnson (mjohnson at apache dot org)
    /// </summary>
    public class POIFSViewEngine
    {
        /// <summary>
        /// Inspect an object that may be viewable, and drill down if told to
        /// </summary>
        /// <param name="viewable">the object to be viewed</param>
        /// <param name="drilldown">if <c>true</c> and the object implements POIFSViewable, inspect the objects' contents</param>
        /// <param name="indentLevel">how far in to indent each string</param>
        /// <param name="indentString">string to use for indenting</param>
        /// <returns>a List of Strings holding the content</returns>
         public static IList InspectViewable(Object viewable,
                                       bool drilldown,
                                       int indentLevel,
                                       String indentString)
        {
            IList objects = new ArrayList();
            if (viewable is DictionaryEntry)
            {
                ProcessViewable(((DictionaryEntry)viewable).Value, drilldown, indentLevel,indentString, objects);
            }
            else if (viewable is POIFSViewable)
            {
                ProcessViewable(viewable, drilldown, indentLevel,indentString, objects);
            }
            else
            {
                objects.Add(Indent(indentLevel, indentString,
                                   viewable.ToString()));
            }
            return objects;
        }

         internal static void ProcessViewable(object viewable,
                                        bool drilldown,
                                        int indentLevel,
                                        String indentString,
                                        IList objects)
         {

             POIFSViewable inspected = (POIFSViewable)viewable;

             objects.Add(Indent(indentLevel, indentString,
                                inspected.ShortDescription));
             if (drilldown)
             {
                 if (inspected is POIFSDocument)
                 {
                     ((ArrayList)objects).AddRange(InspectViewable("POIFSDocument content is too long so ignored", drilldown,
                                                       indentLevel + 1,
                                                       indentString));
                     return;
                 }
                 if (inspected.PreferArray)
                 {
                     Array data = inspected.ViewableArray;

                     for (int j = 0; j < data.Length; j++)
                     {
                         ((ArrayList)objects).AddRange(InspectViewable(data.GetValue(j), drilldown,
                                                        indentLevel + 1,
                                                        indentString));
                     }
                 }
                 else
                 {
                     IEnumerator iter = inspected.ViewableIterator;

                     while (iter.MoveNext())
                     {
                         ((ArrayList)objects).AddRange(InspectViewable(iter.Current,
                                                        drilldown,
                                                        indentLevel + 1,
                                                        indentString));
                     }
                 }
             }
         }

         /// <summary>
         /// Indents the specified indent level.
         /// </summary>
         /// <param name="indentLevel">how far in to indent each string</param>
         /// <param name="indentString">string to use for indenting</param>
         /// <param name="data">The data.</param>
         /// <returns></returns>
        private static String Indent(int indentLevel,
                                     String indentString, String data)
        {
            StringBuilder finalBuffer  = new StringBuilder();
            StringBuilder indentPrefix = new StringBuilder();

            for (int j = 0; j < indentLevel; j++)
            {
                indentPrefix.Append(indentString);
            }

            using (StringReader reader = new StringReader(data))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    finalBuffer.Append(indentPrefix).Append(line).Append(Environment.NewLine);
                    line = reader.ReadLine();
                }
                return finalBuffer.ToString();
            }
        }
    }
}
