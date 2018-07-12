using System;
using System.Text;

namespace Cydua.NPOI.POIFS.FileSystem
{
    /// <summary>
    /// Class DocumentDescriptor
    /// @author Marc Johnson (mjohnson at apache dot org)
    /// </summary>
    public class DocumentDescriptor
    {
        private POIFSDocumentPath path;
        private String            name;
        private int               hashcode = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDescriptor"/> class.
        /// </summary>
        /// <param name="path">the Document path</param>
        /// <param name="name">the Document name</param>
        public DocumentDescriptor(POIFSDocumentPath path, String name)
        {
            if (path == null)
            {
                throw new NullReferenceException("path must not be null");
            }
            if (name == null)
            {
                throw new NullReferenceException("name must not be null");
            }
            if (name.Length== 0)
            {
                throw new ArgumentException("name cannot be empty");
            }
            this.path = path;
            this.name = name;
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get { return this.path.ToString(); }
        }
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// equality. Two DocumentDescriptor instances are equal if they
        /// have equal paths and names
        /// </summary>
        /// <param name="o">the object we're checking equality for</param>
        /// <returns>true if the object is equal to this object</returns>
        public override bool Equals(Object o)
        {
            bool rval = false;

            if ((o != null) && (o.GetType()== this.GetType()))
            {
                if (this == o)
                {
                    rval = true;
                }
                else
                {
                    DocumentDescriptor descriptor = ( DocumentDescriptor ) o;

                    rval = this.path.Equals(descriptor.path)
                           && this.name.Equals(descriptor.name);
                }
            }
            return rval;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// hashcode
        /// </returns>
        public override int GetHashCode()
        {
            if (hashcode == 0)
            {
                hashcode = path.GetHashCode() ^ name.GetHashCode();
            }
            return hashcode;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override String ToString()
        {
            StringBuilder buffer = new StringBuilder(40 * (path.Length + 1));

            for (int j = 0; j < path.Length; j++)
            {
                buffer.Append(path.GetComponent(j)).Append("/");
            }
            buffer.Append(name);
            return buffer.ToString();
        }
    }
}
