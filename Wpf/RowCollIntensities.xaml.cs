using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
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
using System.Windows.Shapes;
using Wpf.Ml;
using Wpf.RowCollSensetivity;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for RowCollIntensities.xaml
    /// </summary>
    public partial class RowCollIntensities : System.Windows.Window
    {
        private Actions Actions;

        private PlotModel _rowPlotModel { get; set; }

        private PlotModel _collPlotModel { get; set; }

        private string _operatedImagePath;

        private float[] _originalRowsData;

        private float[] _originalCollsData;

        public RowCollIntensities()
        {
            _rowPlotModel = new PlotModel() { Title = "RowPlot" };
            _collPlotModel = new PlotModel() { Title = "CollPlot" };
            Actions = new Actions(this);

            InitializeComponent();
        }

        public virtual void VideoSelect_Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            if ((bool)fileDialog.ShowDialog()!)
            {
                _operatedImagePath = fileDialog.FileName;

                using var mat = new Mat(fileDialog.FileName);

                _rowPlotModel = Actions.CreatePlot(new RowIntensity().Get(mat), _originalRowsData, "RowPlot");
                _collPlotModel = Actions.CreatePlot(new CollIntensity().Get(mat), _originalCollsData, "CollPlot");

                Actions.UpdatePlotView(_rowPlotModel);
                Actions.UpdatePlotView(_collPlotModel);

                Image_Image.Source = mat.ToBitmapSource();
            }
        }
             
        private void OriginalImage_Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            if ((bool)fileDialog.ShowDialog()!)
            {
                using var mat = new Mat(fileDialog.FileName);

                _originalRowsData = new RowIntensity().Get(mat);
                _originalCollsData = new CollIntensity().Get(mat);
            }
        }

        private void AnomalyCheck_Button_Click(object sender, RoutedEventArgs e)
        {
            //oper, orig

            using var operated = new Mat(_operatedImagePath);

            var rowsAnomalies = new AnomalyDetection().ChangePointDetection(0.95f, new RowIntensity().Get(operated));
            var collsAnomalies = new AnomalyDetection().ChangePointDetection(0.95f, new CollIntensity().Get(operated));
        
            _rowPlotModel.Series.Add(Actions.GetScatterSerie(rowsAnomalies));
            _collPlotModel.Series.Add(Actions.GetScatterSerie(collsAnomalies));

            Actions.UpdatePlotView(_rowPlotModel);
            Actions.UpdatePlotView(_collPlotModel);
        }

        
    }
}
