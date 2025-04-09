using Cairo;
using Gdk;
using Point = Gdk.Point;

namespace Lucas.SubtitleEdit.CairoExpansions
{
    public static class ContextExtensions
    {
        /// <summary>
        /// Draw the <paramref name="image"/> in <paramref name="tlCorner"/> whith shear effect
        /// </summary>
        /// <param name="cr">The context to use</param>
        /// <param name="image">The Pixbuf to draw</param>
        /// <param name="tlCorner">Top-Right corner location</param>
        /// <param name="urCorner">Top-Left corner location</param>
        /// <param name="dlCorner">Down-Left corner location</param>
        private static void DrawImageShear(this Context cr, Pixbuf image, Point tlCorner, Point urCorner, Point dlCorner)
        {                
            double xx, yx, xy, yy; 
            
            // Width scale
            double width = urCorner.X - tlCorner.X;
            xx = width / image.Width;
            
            // Height scale
            double height = dlCorner.Y - tlCorner.Y;
            yy = height / image.Height;

            // Moving scale of the top right corner
            yx = (urCorner.Y - tlCorner.Y) / height;
            
            // Moving scale of the down left corner
            xy = (dlCorner.X - tlCorner.X) / width;
            
            cr.Save();
            cr.Transform(new Matrix(xx, xy, yx, yy, tlCorner.X, tlCorner.Y));
            Gdk.CairoHelper.SetSourcePixbuf(cr, image, 0, 0);
            cr.Paint();
            cr.Restore();
        }
    }
}