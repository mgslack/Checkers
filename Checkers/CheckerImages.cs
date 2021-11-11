using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

/*
 * Primary class loads and caches the checker images embedded within the program
 * assembly. Uses enums to specify which checker images to load.
 *
 * Author:  M. G. Slack
 * Written: 2014-04-01
 *
 * ----------------------------------------------------------------------------
 * 
 * Revised: yyyy-mm-dd - XXXX.                     
 *
 */
namespace Checkers
{
    /* Enum of the different checker color images available. */
    public enum CheckerColors { Black, Blue, DarkWood, Green, LightWood, Red, White, Yellow };
    /* Enum of the different checker 'crown' images available. */
    public enum CheckerCrowns { crown1, crown2, crown3 };

    public class CheckerImages
    {
        #region Private consts/fields
        private const string CHECKER_IMAGE_NAMESPACE = "Checkers.images.";
        private const string CHECKER_IMAGE_EXT = ".bmp";
        private const string CROWN_IMAGE_EXT = ".gif";

        private Dictionary<string, Bitmap> imageCache = new Dictionary<string, Bitmap>();
        #endregion

        // --------------------------------------------------------------------

        public CheckerImages() { }

        // --------------------------------------------------------------------

        #region Private Methods
        private string ConvertToName(CheckerColors color)
        {
            string name = "";

            switch (color) {
                case CheckerColors.Black :
                    name = "black_checker" + CHECKER_IMAGE_EXT;
                    break;
                case CheckerColors.Blue :
                    name = "blue_checker" + CHECKER_IMAGE_EXT;
                    break;
                case CheckerColors.DarkWood :
                    name = "darkwood_checker" + CHECKER_IMAGE_EXT;
                    break;
                case CheckerColors.Green :
                    name = "green_checker" + CHECKER_IMAGE_EXT;
                    break;
                case CheckerColors.LightWood :
                    name = "lightwood_checker" + CHECKER_IMAGE_EXT;
                    break;
                case CheckerColors.Red :
                    name = "red_checker" + CHECKER_IMAGE_EXT;
                    break;
                case CheckerColors.White :
                    name = "white_checker" + CHECKER_IMAGE_EXT;
                    break;
                case CheckerColors.Yellow :
                    name = "yellow_checker" + CHECKER_IMAGE_EXT;
                    break;
                default :
                    name = "Unknown Color (" + Enum.GetName(typeof(CheckerColors), color) + ")";
                    break;
            }

            return name;
        }

        private Bitmap LoadImage(string imageName)
        {
            string path = CHECKER_IMAGE_NAMESPACE + imageName;
            Bitmap bitmap = null;

            if (imageCache.ContainsKey(path)) { return imageCache[path]; }

            try {
                bitmap = new Bitmap(GetResourceStream(path));
                bitmap.MakeTransparent();
                if (bitmap != null) { imageCache.Add(path, bitmap); }
            }
            catch (Exception e) {
                MessageBox.Show("Image (" + imageName + "): " + e.Message, "LoadImage Error");
            }

            return bitmap;
        }
        #endregion

        // --------------------------------------------------------------------

        /*
         * Method used to get a manifest resource stream of the selected
         * resource represented by the resource path.
         */
        public static Stream GetResourceStream(string path)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            return asm.GetManifestResourceStream(path);
        }

        // --------------------------------------------------------------------

        /*
         * Method returns a checker image in the specified color.
         */
        public Bitmap GetCheckerImage(CheckerColors color)
        {
            string name = ConvertToName(color);

            return LoadImage(name);
        }

        /*
         * Method returns one of the 'crown' images used to crown a checker
         * that has made it across the board. The 'GetCrownedChecker'
         * method should be used to get a 'crowned' checker instead of this
         * call. Mainly useful for testing purposes.
         */
        public Bitmap GetCheckerCrown(CheckerCrowns crown)
        {
            return LoadImage(Enum.GetName(typeof(CheckerCrowns), crown) + CROWN_IMAGE_EXT);
        }

        /*
         * Method returns a 'disabled' image to use with the checker images.
         * Useful for implementing a 'drag-n-drop' routine.
         */
        public Bitmap GetDisabledChecker()
        {
            return LoadImage("disabled" + CHECKER_IMAGE_EXT);
        }

        /*
         * Method returns a given checker image instance with the given
         * crown image merged on it. This method creates the image each
         * time and does not use the internal cache.
         */
        public Bitmap GetCrownedChecker(Bitmap checker, CheckerCrowns crown)
        {
            Bitmap crownImage = GetCheckerCrown(crown);
            Bitmap crowned = new Bitmap(checker.Width, checker.Height);
            int x = 12, y = 0;

            switch (crown) {
                case CheckerCrowns.crown1: y = 14; break;
                case CheckerCrowns.crown2: y = 12; break;
                default: y = 15; break; // crown3
            }

            using (Graphics g = Graphics.FromImage(crowned)) {
                g.DrawImageUnscaled(checker, 0, 0);
                g.DrawImageUnscaled(crownImage, x, y);
            }

            return crowned;
        }

        /*
         * Method returns a given checker color image with a given crown
         * image merged on it. This method will use the internal cache to
         * retrieve the given image if already created.
         */
        public Bitmap GetCrownedChecker(CheckerColors color, CheckerCrowns crown)
        {
            string name = ConvertToName(color);
            string cacheName = CHECKER_IMAGE_NAMESPACE +
                   Enum.GetName(typeof(CheckerCrowns), crown) + "." + name;

            if (imageCache.ContainsKey(cacheName)) { return imageCache[cacheName]; }

            Bitmap checker = LoadImage(name);
            Bitmap crowned = GetCrownedChecker(checker, crown);

            if (crowned != null) imageCache.Add(cacheName, crowned);

            return crowned;
        }
    }
}
