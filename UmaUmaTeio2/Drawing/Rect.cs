using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaUmaTeio2.Drawing
{
    /// <summary>
    /// <see cref="Rect"/> クラスは、OCR を実行する領域を表すクラスです。
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public class Rect : IEquatable<Rect>
    {
        #region Fields

        private int _X;
        private int _Y;
        private int _Width;
        private int _Height;

        #endregion

        #region Properties

        public int X1 => _X;
        public int Y1 => _Y;
        public int X2 => _X + _Width;
        public int Y2 => _Y + _Height;

        public int Width => _Width;
        public int Height => _Height;


        public override string ToString() =>
            string.Format("[Rect X={0}, Y={1}, Width={2}, Height={3}]", _X, _Y, _Width, _Height);

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="Rect"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Rect(int x, int y, int width, int height)
        {
            _X = x;
            _Y = y;
            _Width = width;
            _Height = height;
        }

        /// <summary>
        /// 座標系から <see cref="Rect"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public static Rect FromCoords(int x1, int y1, int x2, int y2) => new Rect(x1, y1, x2 - x1, y2 - y1);

        #endregion

        #region Operators

        public static bool operator ==(Rect lhs, Rect rhs) => lhs.Equals(rhs);
        public static bool operator !=(Rect lhs, Rect rhs) => !(lhs == rhs);

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            // obj の型が OcrRect であること＋すべての座標が一致すること
            return (obj is Rect) && Equals((Rect)obj);
        }

        public bool Equals(Rect other)
        {
            return _X == other._X && 
                   _Y == other._Y &&
                   _Width == other._Width &&
                   _Height == other._Height;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            unchecked
            {
                hashCode += 1000000007 * _X.GetHashCode();
                hashCode += 1000000009 * _Y.GetHashCode();
                hashCode += 1000000021 * _Width.GetHashCode();
                hashCode += 1000000033 * _Height.GetHashCode();
            }

            return hashCode;
        }

        #endregion
    }
}
