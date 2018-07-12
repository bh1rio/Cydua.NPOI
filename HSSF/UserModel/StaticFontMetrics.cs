using System;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Resources;

using Cydua.NPOI.Util.Collections;

namespace Cydua.NPOI.HSSF.UserModel
{
    /**
     * Allows the user to lookup the font metrics for a particular font without
     * actually having the font on the system.  The font details are Loaded
     * as a resource from the POI jar file (or classpath) and should be contained
     * in path "/font_metrics.properties".  The font widths are for a 10 point
     * version of the font.  Use a multiplier for other sizes.
     *
     * @author Glen Stampoultzis (glens at apache.org)
     */
    class StaticFontMetrics
    {
        const string FONT_METRICS_PROPERTIES_FILE_NAME = "font_metrics.properties";

        /** The font metrics property file we're using */
        private static Properties fontMetricsProps;
        /** Our cache of font details we've alReady looked up */
        private static Hashtable fontDetailsMap = new Hashtable();

        /**
         * Retrieves the fake font details for a given font.
         * @param font  the font to lookup.
         * @return  the fake font.
         */
        public static FontDetails GetFontDetails(Font font)
        {
            // If we haven't alReady identified out font metrics file,
            //  figure out which one to use and Load it
            if (fontMetricsProps == null)
            {
                Stream metricsIn = null;
                try
                {
                    fontMetricsProps = new Properties();

                    // Check to see if the font metric file was specified
                    //  as a system property
                    String propFileName = null;
                    try
                    {
                        var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FONT_METRICS_PROPERTIES_FILE_NAME);
                        if (File.Exists(fileName))
                        {
                            propFileName = FONT_METRICS_PROPERTIES_FILE_NAME;
                        }
                    }
                    catch (Exception) { }

                    if (propFileName != null)
                    {

                        if (!File.Exists(propFileName))
                            throw new FileNotFoundException("font_metrics.properties not found at path " + Path.GetFullPath(propFileName));
                        metricsIn = typeof(StaticFontMetrics).Assembly.GetManifestResourceStream(FONT_METRICS_PROPERTIES_FILE_NAME);
                    }
                    else
                    {
                        // Use the built-in font metrics file off the classpath
                        metricsIn = typeof(StaticFontMetrics).Assembly.GetManifestResourceStream(FONT_METRICS_PROPERTIES_FILE_NAME);
                        if (metricsIn == null)
                            throw new FileNotFoundException("font_metrics.properties not found in classpath");
                    }
                    fontMetricsProps.Load(metricsIn);
                }
                catch (IOException e)
                {
                    throw new Exception("Could not Load font metrics: " + e.Message);
                }
                finally
                {
                    if (metricsIn != null)
                    {
                        try
                        {
                            metricsIn.Close();
                        }
                        catch (IOException)
                        {

                        }
                    }
                }
            }

            // Grab the base name of the font they've asked about
            String fontName = font.FontFamily.Name;

            // Some fonts support plain/bold/italic/bolditalic variants
            // Others have different font instances for bold etc
            // (eg font.dialog.plain.* vs font.Californian FB Bold.*)
            String fontStyle = "";
            //if(font.IsPlain())  fontStyle += "plain";
            if (font.Bold) fontStyle += "bold";
            if (font.Italic) fontStyle += "italic";

            // Do we have a definition for this font with just the name?
            // If not, Check with the font style Added
            if (fontMetricsProps[FontDetails.BuildFontHeightProperty(fontName)] == null &&
            fontMetricsProps[FontDetails.BuildFontHeightProperty(fontName + "." + fontStyle)] != null)
            {
                // Need to Add on the style to the font name
                fontName += "." + fontStyle;
            }

            // Get the details on this font
            if (fontDetailsMap[fontName] == null)
            {
                FontDetails fontDetails = FontDetails.Create(fontName, fontMetricsProps);
                fontDetailsMap[fontName] = fontDetails;
                return fontDetails;
            }
            else
            {
                return (FontDetails)fontDetailsMap[fontName];
            }

        }
    }
}