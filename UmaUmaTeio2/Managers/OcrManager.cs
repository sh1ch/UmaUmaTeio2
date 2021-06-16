using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaUmaTeio2.Managers
{
    /// <summary>
    /// <see cref="OcrManager"/> クラスは、OCR の機能を管理するクラスです。
    /// </summary>
    public sealed class OcrManager : IDisposable
    {
        #region Properties

        /// <summary>
        /// <see cref="OcrManager"/> クラスの唯一のインスタンスを取得します。
        /// </summary>
        public static OcrManager Instance { get; } = new ();

        public Tesseract.TesseractEngine DigitEngine { get; }

        public Tesseract.TesseractEngine JpnEngine { get; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="OcrManager"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        private OcrManager()
        {
            var exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            DigitEngine = new Tesseract.TesseractEngine(System.IO.Path.Combine(exePath, "tessdata", "ubuntu"), "eng");

            DigitEngine.SetVariable("tessedit_char_whitelist", "0123456789");
            DigitEngine.SetVariable("tessedit_char_blacklist", "!?@#$%&*()<>_-+=/:;'\"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
            DigitEngine.SetVariable("classify_bln_numeric_mode", "1");

            JpnEngine = new Tesseract.TesseractEngine(System.IO.Path.Combine(exePath, "tessdata", "ubuntu"), "jpn");
        }

        /// <summary>
        /// 使用したリソースをすべて開放します。
        /// </summary>
        public void Dispose()
        {
            DigitEngine?.Dispose();
            JpnEngine?.Dispose();
        }

        #endregion

        #region Events

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}
