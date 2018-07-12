using Cydua.NPOI.POIFS.FileSystem;
using System.Collections;
using System.Collections.Generic;
using System;
namespace NPOI.Util
{
    public class POIUtils
    {

        /**
         * Copies an Entry into a target POIFS directory, recursively
         */

        public static void CopyNodeRecursively(Entry entry, DirectoryEntry target)
        {
            // System.err.println("copyNodeRecursively called with "+entry.GetName()+
            // ","+tarGet.getName());
            DirectoryEntry newTarget = null;
            if (entry.IsDirectoryEntry)
            {
                newTarget = target.CreateDirectory(entry.Name);
                IEnumerator entries = ((DirectoryEntry)entry).Entries;

                while (entries.MoveNext())
                {
                    CopyNodeRecursively((Entry)entries.Current, newTarget);
                }
            }
            else
            {
                DocumentEntry dentry = (DocumentEntry)entry;
                using (DocumentInputStream dstream = new DocumentInputStream(dentry))
                {
                    target.CreateDocument(dentry.Name, dstream);
                    //now part of usings call to Dispose: dstream.Close();
                }
            }
        }

        /**
         * Copies nodes from one POIFS to the other minus the excepts
         * 
         * @param source
         *            is the source POIFS to copy from
         * @param target
         *            is the target POIFS to copy to
         * @param excepts
         *            is a list of Strings specifying what nodes NOT to copy
         */
        public static void CopyNodes(DirectoryEntry sourceRoot,
                DirectoryEntry targetRoot, List<String> excepts)
        {
            IEnumerator entries = sourceRoot.Entries;
            while (entries.MoveNext())
            {
                Entry entry = (Entry)entries.Current;
                if (!excepts.Contains(entry.Name))
                {
                    CopyNodeRecursively(entry, targetRoot);
                }
            }
        }

        /**
         * Copies nodes from one POIFS to the other minus the excepts
         * 
         * @param source
         *            is the source POIFS to copy from
         * @param target
         *            is the target POIFS to copy to
         * @param excepts
         *            is a list of Strings specifying what nodes NOT to copy
         */
        public static void CopyNodes(POIFSFileSystem source,
                POIFSFileSystem target, List<String> excepts)
        {
            // System.err.println("CopyNodes called");
            CopyNodes(source.Root, target.Root, excepts);
        }
    }

}