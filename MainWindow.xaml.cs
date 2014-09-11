using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using System.Diagnostics;

namespace MonoGameUtils
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _twoMGFXExecutable;
        private string _twoMGFXPath;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _twoMGFXExecutable = "2MGFX.exe";

            // By Default the 2MGFX executable is in the same directory
            _twoMGFXPath = String.Empty;

            // Or use a proper directory
            if (Directory.Exists(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "2MGFX")))
                _twoMGFXPath = "2MGFX";

            // Test if file exist
            if (!File.Exists(System.IO.Path.Combine(_twoMGFXPath, _twoMGFXExecutable)))
            {
                MessageBox.Show("2MGFX.exe is missing", "2MGFX is missing", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Launch the conversion when the user drop a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStackPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data is System.Windows.DataObject && ((System.Windows.DataObject)e.Data).ContainsFileDropList())
            {
                foreach (string filePath in ((System.Windows.DataObject)e.Data).GetFileDropList())
                {
                    if (filePath.Contains(".fx"))
                    {
                        Process process = new Process();
                        process.StartInfo.Arguments = GetCommandArgs(filePath);
                        process.StartInfo.WorkingDirectory = _twoMGFXPath;
                        process.StartInfo.FileName = _twoMGFXExecutable;
                        process.Start();
                    }
                    else
                    {
                        MessageBox.Show("This isn't a valid effect file", "Invalid file", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Get command line arguments for 2MGFX
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetCommandArgs(string filePath)
        {
            string profile = string.Empty;
            string extra = " /Profile:";

            switch (shaderProfile.SelectedIndex)
            {
                case 0:
                    profile = "dx11";
                    extra += "DirectX_11";
                    break;
                case 1:
                    profile = "ogl";
                    extra += "OpenGL";
                    break;
                case 2:
                    profile = "ps4";
                    extra += "PlayStation4";
                    break;
            }
            
            // Define the good extension
            string mgsExt = String.Format("{0}.mgfxo", profile);
             
            // Define the new path
            string newFilePath = filePath.Replace("fx", mgsExt);
            
            return filePath + " " + newFilePath + extra;
        }
    }
}
