using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Sdk;

namespace UmaUmaTeio2
{
    /// <summary>
    /// <see cref="NativeWrapper"/> クラスは、使用するネイティブメソッドを定義するクラスです。
    /// </summary>
    public class NativeWrapper
    {
        #region Initializes

        /// <summary>
        /// <see cref="NativeWrapper"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public NativeWrapper() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// 指定した名前のウィンドウのハンドルを取得します。
        /// </summary>
        /// <param name="className">ウィンドウのクラス名。</param>
        /// <param name="windowName">ウィンドウの名前。</param>
        /// <returns>ウィンドウのハンドル。</returns>
        public static IntPtr GetWindowHandle(string className, string windowName)
        {
            var hWnd = PInvoke.FindWindow(className, windowName);

            return hWnd.Value;
        }

        /// <summary>
        /// 指定した <see cref="handle"/> のウィンドウ画面の <see cref="Bitmap"/> データを取得します。
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル。</param>
        /// <returns>ウィンドウ画面の <see cref="Bitmap"/> データ。取得に失敗したときは <c>null</c> を返却。</returns>
        public static unsafe Bitmap GetCaptureBitmap(IntPtr handle)
        {
            Bitmap bitmap = null;
            HWND hWnd = new (handle);
            HWND zero = new (0);
            HDC desktopDC; // = new (IntPtr.Zero);
            HDC memoryDC; // = new (IntPtr.Zero);

            // ガード節
            if (handle == IntPtr.Zero)
            {
                return bitmap;
            }

            var result = PInvoke.GetClientRect(hWnd, out RECT rect);

            if (!result) throw new NullReferenceException($"指定したハンドルのウィンドウ({hWnd})を発見することができませんでした。");

            POINT point = new();
            var mapResult = PInvoke.MapWindowPoints(hWnd, zero, &point, 2);

            if (mapResult == 0) throw new NullReferenceException($"指定したハンドルのウィンドウ({hWnd})の座標空間の変換に失敗しました。");

            rect.left = point.x;
            rect.top = point.y;
            rect.right += point.x;
            rect.bottom += point.y;

            var tempRect = rect;

            desktopDC = PInvoke.GetWindowDC(zero); // デスクトップの HDC を取得

            var header = new BITMAPINFOHEADER()
            {
                biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER)),
                biWidth = tempRect.right - rect.left,
                biHeight = tempRect.bottom - rect.top,
                biPlanes = 1,
                biCompression = 0, // BitmapCompressionMode.BI_RGB = 0
                biBitCount = 24,
            };

            var info = new BITMAPINFO
            {
                bmiHeader = header,
            };

            void** bits = null;

            HBITMAP hBitmap = PInvoke.CreateDIBSection(desktopDC, &info, DIB_USAGE.DIB_RGB_COLORS, bits, new HANDLE(IntPtr.Zero), 0);

            memoryDC = PInvoke.CreateCompatibleDC(desktopDC);

            var phBitMap = PInvoke.SelectObject(memoryDC, new HGDIOBJ(hBitmap));

            PInvoke.BitBlt(memoryDC, 0, 0, header.biWidth, header.biHeight, desktopDC, rect.left, rect.top, ROP_CODE.SRCCOPY);
            PInvoke.SelectObject(memoryDC, phBitMap);

            bitmap = Bitmap.FromHbitmap(hBitmap, IntPtr.Zero);

            if (desktopDC.Value != IntPtr.Zero)
            {
                if (PInvoke.ReleaseDC(zero, desktopDC) == 0)
                {
                    Debug.WriteLine($"{nameof(desktopDC)} の開放に失敗しました。");
                }
            }

            if (memoryDC.Value != IntPtr.Zero)
            {
                if (PInvoke.ReleaseDC(hWnd, memoryDC) == 0)
                {
                    Debug.WriteLine($"{nameof(memoryDC)} の開放に失敗しました。");
                }
            }

            return bitmap;
        }

        #endregion
    }
}
