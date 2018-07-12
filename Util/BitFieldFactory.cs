using System.Collections;

namespace Cydua.NPOI.Util
{
    /// <summary>
    /// Returns immutable Btfield instances.
    /// @author Jason Height (jheight at apache dot org)
    /// </summary>
    public class BitFieldFactory
    {
        //use Hashtable to replace HashMap
        private static Hashtable instances = new Hashtable();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns></returns>
        public static BitField GetInstance(int mask)
        {
            BitField f = (BitField)instances[mask];
            if (f == null)
            {
                f = new BitField(mask);
                instances[mask] = f;
            }
            return f;
        }
    }
}
