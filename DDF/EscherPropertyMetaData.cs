using System;

namespace Cydua.NPOI.DDF
{
    /// <summary>
    /// This class stores the type and description of an escher property.
    /// @author Glen Stampoultzis (glens at apache.org)
    /// </summary>
    public class EscherPropertyMetaData
    {
        // Escher property types.
        public const byte TYPE_UNKNOWN = (byte)0;
        public const byte TYPE_bool = (byte)1;
        public const byte TYPE_RGB = (byte)2;
        public const byte TYPE_SHAPEPATH = (byte)3;
        public const byte TYPE_SIMPLE = (byte)4;
        public const byte TYPE_ARRAY = (byte)5;

        private String description;
        private byte type;


        /// <summary>
        /// Initializes a new instance of the <see cref="EscherPropertyMetaData"/> class.
        /// </summary>
        /// <param name="description">The description of the escher property.</param>
        public EscherPropertyMetaData(String description)
        {
            this.description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EscherPropertyMetaData"/> class.
        /// </summary>
        /// <param name="description">The description of the escher property.</param>
        /// <param name="type">The type of the property.</param> 
        public EscherPropertyMetaData(String description, byte type)
        {
            this.description = description;
            this.type = type;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public String Description
        {
            get { return description; }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public byte Type
        {
            get { return type; }
        }

    }
}