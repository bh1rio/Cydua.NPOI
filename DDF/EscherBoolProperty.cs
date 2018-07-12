using System;
using System.Text;
using Cydua.NPOI.Util;

namespace Cydua.NPOI.DDF
{
    /// <summary>
    /// Represents a bool property.  The actual utility of this property is in doubt because many
    /// of the properties marked as bool seem to actually contain special values.  In other words
    /// they're not true bools.
    /// @author Glen Stampoultzis
    /// </summary>
    public class EscherBoolProperty : EscherSimpleProperty
    {
        /// <summary>
        /// Create an instance of an escher bool property.
        /// </summary>
        /// <param name="propertyNumber">The property number (or id)</param>
        /// <param name="value">The 32 bit value of this bool property</param>
        public EscherBoolProperty(short propertyNumber, int value)
            : base(propertyNumber, value)
        {

        }

        /// <summary>
        /// Whether this bool property is true
        /// </summary>
        /// <value><c>true</c> if this instance is true; otherwise, <c>false</c>.</value>
        public bool IsTrue
        {
            get { return propertyValue != 0; }
        }

        /// <summary>
        /// Whether this bool property is false
        /// </summary>
        /// <value><c>true</c> if this instance is false; otherwise, <c>false</c>.</value>
        public bool IsFalse
        {
            get { return propertyValue == 0; }
        }

        //public override String ToString()
        //{
        //    return "propNum: " + PropertyNumber
        //            + ", complex: " + IsComplex
        //            + ", blipId: " + IsBlipId
        //            + ", value: " + (Value != 0);
        //}
        public override String ToXml(String tab)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tab).Append("<").Append(GetType().Name).Append(" id=\"0x").Append(HexDump.ToHex(Id))
                    .Append("\" name=\"").Append(Name).Append("\" simpleValue=\"").Append(PropertyValue).Append("\" blipId=\"")
                    .Append(IsBlipId).Append("\" value=\"").Append(IsTrue).Append("\"").Append("/>\n");
            return builder.ToString();
        }
    }
}