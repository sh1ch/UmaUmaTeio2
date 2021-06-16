using Livet.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UmaUmaTeio2.ViewModels
{
    /// <summary>
    /// <see cref="TesseractTestControlViewModel"/> クラスは、VM クラスです。
    /// </summary>
    public class TesseractTestControlViewModel : Livet.ViewModel
    {
        #region Properties

        private string _FilePath;

        public string FilePath
        {
            get { return _FilePath; }
            set
            {
                _FilePath = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// ドラッグ＆ドロップしたファイルを取得します。
        /// </summary>
        public string[] DropFiles
        {
            set
            {
                if (value == null || value.Length <= 0) return;

                var extension = Path.GetExtension(value[0]);

                if (extension.ToLower() == ".png")
                {
                    FilePath = value[0];
                }
            }
        }

        public ListenerCommand<Window> SelectImageCommand => new ((window) => 
        {
            var dialog = new OpenFileDialog();

            dialog.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            dialog.Filter = "PNG Files (*.png)|*.png|All Files (*)|*.*";

            if (dialog.ShowDialog(window) ?? false)
            {
                FilePath = dialog.FileName;
            }
        });

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="TesseractTestControlViewModel"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public TesseractTestControlViewModel() { }

        #endregion

        #region Events

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

    }
}
