using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using vm = UmaUmaTeio2.ViewModels;

namespace UmaUmaTeio2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// <see cref="Application"/> オブジェクトの <see cref="Application.Run"/> メソッドが呼び出されると発生します。
        /// <para>
        /// アプリケーションのエントリーポイントです。
        /// </para>
        /// </summary>
        /// <param name="e">開始イベントの引数。</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Livet.DispatcherHelper.UIDispatcher = this.Dispatcher;

            try
            {
                var vm = new vm.TesseractTestControlViewModel();
                var v = new MainWindow();

                v.DataContext = vm;

                v.ShowDialog();
            }
            finally
            {
                // リソースの開放
                Managers.OcrManager.Instance.Dispose();
            }
        }
    }
}
