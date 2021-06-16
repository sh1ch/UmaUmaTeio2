using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaUmaTeio2.Drawing
{
    /// <summary>
    /// <see cref="BitmapBuilder"/> クラスは、<see cref="Bitmap"/> データの画素などに効果を加えるためのクラスです。
    /// </summary>
    public class BitmapBuilder
    {
        #region Public Methods

        /// <summary>
        /// <see cref="Bitmap"/> データを拡大/縮小した <see cref="Bitmap"/> データを取得します。
        /// </summary>
        /// <param name="bitmap">拡大/縮小するビットマップ データ。</param>
        /// <param name="scale">拡大/縮小する比率。</param>
        /// <param name="interpolationMode">拡大/縮小する際の画素補完法。</param>
        /// <returns>拡大/縮小した <see cref="Bitmap"/> データ。</returns>
        public static Bitmap Resize(Bitmap bitmap, double scale, InterpolationMode interpolationMode = InterpolationMode.NearestNeighbor)
        {
            var resize = new Bitmap((int)(bitmap.Width * scale), (int)(bitmap.Height * scale));
            using var g = Graphics.FromImage(resize);

            g.InterpolationMode = interpolationMode;
            g.DrawImage(bitmap, 0, 0, (int)(bitmap.Width * scale), (int)(bitmap.Height * scale));

            return resize;
        }

        /// <summary>
        /// <see cref="Bitmap"/> データを指定した領域 <see cref="Drawing.Rect"/> に切り取ります。
        /// </summary>
        /// <param name="bitmap">切り取りするビットマップ データ。</param>
        /// <param name="rect">切り取りする領域。</param>
        /// <returns>切り出された <see cref="Bitmap"/> データ。</returns>
        public static Bitmap Cut(Bitmap bitmap, Drawing.Rect rect) => Cut(bitmap, rect, bitmap.PixelFormat);

        /// <summary>
        /// <see cref="Bitmap"/> データを指定した領域 <see cref="Drawing.Rect"/> に切り取ります。
        /// </summary>
        /// <param name="bitmap">切り取りするビットマップ データ。</param>
        /// <param name="rect">切り取りする領域。</param>
        /// <param name="pixelFormat">切り出した画像のフォーマット。</param>
        /// <returns>切り出された <see cref="Bitmap"/> データ。</returns>
        public static Bitmap Cut(Bitmap bitmap, Drawing.Rect rect, PixelFormat pixelFormat)
        {
            return bitmap.Clone(new System.Drawing.Rectangle(rect.X1, rect.Y1, rect.Width, rect.Height), pixelFormat);
        }

        /// <summary>
        /// <see cref="Bitmap"/> に余白を加えた <see cref="Bitmap"/> を生成します。
        /// </summary>
        /// <param name="bitmap">余白（左右上下）を与える画像データ。</param>
        /// <param name="margin">余白の設定。</param>
        /// <param name="backgroundColor">背景色。</param>
        /// <returns>余白を加えた <see cref="Bitmap"/> データ。</returns>
        public static Bitmap Margin(Bitmap bitmap, int margin, System.Drawing.Color backgroundColor)
        {
            var resize = new Bitmap(bitmap.Width + margin * 2, bitmap.Height + margin * 2);
            using var g = Graphics.FromImage(resize);

            g.Clear(backgroundColor);
            g.DrawImage(bitmap, margin, margin, bitmap.Width, bitmap.Height);

            return resize;
        }

        /// <summary>
        /// <see cref="Bitmap"/> データの反転（ネガティブ）データを生成します。
        /// </summary>
        /// <param name="bitmap">ビットマップ データ。</param>
        /// <returns>画素を反転させた <see cref="Bitmap"/> データ。</returns>
        public static Bitmap Negative(Bitmap bitmap)
        {
            var negative = new Bitmap(bitmap.Width, bitmap.Height);
            using var g = Graphics.FromImage(negative);

            var matrix = new System.Drawing.Imaging.ColorMatrix();
            var attributes = new System.Drawing.Imaging.ImageAttributes();

            matrix.Matrix00 = -1;
            matrix.Matrix11 = -1;
            matrix.Matrix22 = -1;
            matrix.Matrix33 = +1;
            matrix.Matrix40 = +1;
            matrix.Matrix41 = +1;
            matrix.Matrix42 = +1;
            matrix.Matrix44 = +1;

            attributes.SetColorMatrix(matrix);
            g.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);

            return negative;
        }

        #endregion
    }
}
