using System;
using System.Collections;
using System.Configuration;

namespace Cydua.NPOI.Util
{
    public class POILogFactory
    {

        /**
         * Map of POILogger instances, with classes as keys
         */
        private static Hashtable _loggers = new Hashtable();

        /**
         * A common instance of NullLogger, as it does nothing
         *  we only need the one
         */
        private static POILogger _nullLogger = new NullLogger();
        /**
         * The name of the class to use. Initialised the
         *  first time we need it
         */
        private static String _loggerClassName = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="POILogFactory"/> class.
        /// </summary>
        private POILogFactory()
        {
        }

        /// <summary>
        /// Get a logger, based on a class name
        /// </summary>
        /// <param name="type">the class whose name defines the log</param>
        /// <returns>a POILogger for the specified class</returns>
        public static POILogger GetLogger(Type type)
        {
            return GetLogger(type.Name);
        }
        
        /// <summary>
        /// Get a logger, based on a String
        /// </summary>
        /// <param name="cat">the String that defines the log</param>
        /// <returns>a POILogger for the specified class</returns>
        public static POILogger GetLogger(String cat)
        {
            POILogger logger = null;
            
            // If we haven't found out what logger to use yet,
            //  then do so now
            // Don't look it up until we're first asked, so
            //  that our users can set the system property
            //  between class loading and first use
            if(_loggerClassName == null) {

        	    // Use the default logger if none specified,
        	    //  or none could be fetched
        	    if(_loggerClassName == null) {
        		    _loggerClassName = _nullLogger.GetType().Name;
        	    }
            }
            
            // Short circuit for the null logger, which
            //  ignores all categories
            if(_loggerClassName.Equals(_nullLogger.GetType().Name)) {
        	    return _nullLogger;
            }

            
            // Fetch the right logger for them, creating
            //  it if that's required 
            if (_loggers.ContainsKey(cat)) {
                logger = (POILogger)_loggers[cat];
            } else {
                try {
                    //logger=assembly.CreateInstance(_loggerClassName) as POILogger;
                    Type loggerClass = Type.GetType(_loggerClassName);
                    logger =  Activator.CreateInstance(loggerClass) as POILogger;
                    logger.Initialize(cat);
                } catch(Exception) {
                  // Give up and use the null logger
                  logger = _nullLogger;
                }
                
                // Save for next time
                _loggers[cat] = logger;
            }
            return logger;
        }
    }
}
