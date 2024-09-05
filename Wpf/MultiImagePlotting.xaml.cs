using Microsoft.Win32;
using OpenCvSharp;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using Wpf.RowCollSensetivity;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MultiImagePlotting.xaml
    /// </summary>
    public partial class MultiImagePlotting : System.Windows.Window
    {
        Actions Actions;

        Random random;

        private PlotModel _rowPlotModel { get; set; }

        private PlotModel _collPlotModel { get; set; }

        public MultiImagePlotting()
        {
            _rowPlotModel = new PlotModel() { Title = "RowPlot" };
            _collPlotModel = new PlotModel() { Title = "CollPlot" };
            Actions = new Actions(this);
            random = new Random();
            InitializeComponent();
        }

        private void OriginalImage_Button_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new OpenFolderDialog();

            if ((bool)folderDialog.ShowDialog()!)
            {
                var files = Directory.GetFiles(folderDialog.FolderName);

                _rowPlotModel = new PlotModel() { Title = "RowPlot" };
                _collPlotModel = new PlotModel() { Title = "CollPlot" };

                foreach (var file in files)
                {
                    using var mat = new Mat(file);

                    _rowPlotModel.Series.Add(Actions.GetLineSerie(
                        new RowIntensity().Get(mat),
                        OxyColor.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255))));

                    _collPlotModel.Series.Add(Actions.GetLineSerie(
                        new CollIntensity().Get(mat),
                        OxyColor.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255))));                                  
                }

                Actions.UpdatePlotView(_rowPlotModel);
                Actions.UpdatePlotView(_collPlotModel);
            }
        }
    }
}
