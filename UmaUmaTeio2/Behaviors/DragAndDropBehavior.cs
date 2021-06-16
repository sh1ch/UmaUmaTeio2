using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UmaUmaTeio2.Behaviors
{
    /// <summary>
    /// <see cref="DragAndDropBehavior"/> クラスは、添付プロパティを定義するクラスです。
    /// <para>
    /// <see cref="UIElement"/> へのファイルのドラッグ＆ドロップをサポートします。
    /// </para>
    /// </summary>
    public class DragAndDropBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty DropFilesProperty =
            DependencyProperty.Register(
                nameof(DropFiles),
                typeof(string[]),
                typeof(DragAndDropBehavior),
                new FrameworkPropertyMetadata(default(string[]), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );


        #region Public Methods

        public string[] DropFiles
        {
            get
            {
                return (string[])this.GetValue(DropFilesProperty);
            }
            
            set
            {
                SetValue(DropFilesProperty, value);
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnAttached()
        {
            base.OnAttached();

            // AssociatedObject.PreviewDragEnter += AssociatedObject_PreviewDragEnter;
            AssociatedObject.PreviewDragOver += AddDragDropEffects;

            AssociatedObject.Drop += SetDropFiles;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            // AssociatedObject.PreviewDragEnter -= AssociatedObject_PreviewDragEnter;
            AssociatedObject.PreviewDragOver -= AddDragDropEffects;

            AssociatedObject.Drop -= SetDropFiles;
        }

        private void AddDragDropEffects(object sender, DragEventArgs e)
        {
            e.Effects = (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None);

            // e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void SetDropFiles(object sender, DragEventArgs e)
        {
            DropFiles = e.Data.GetData(DataFormats.FileDrop) as string[];
        }

        #endregion
    }
}
