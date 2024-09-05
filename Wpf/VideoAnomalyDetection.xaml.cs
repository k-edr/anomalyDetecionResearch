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
using Wpf.Ml;
using Wpf.RowCollSensetivity;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for VideoAnomalyDetection.xaml
    /// </summary>
    public partial class VideoAnomalyDetection : System.Windows.Window
    {
        Actions Actions;

        private PlotModel _rowPlotModel { get; set; }

        private PlotModel _collPlotModel { get; set; }

        public VideoAnomalyDetection()
        {
            _rowPlotModel = new PlotModel() { Title = "RowPlot" };
            _collPlotModel = new PlotModel() { Title = "CollPlot" };
            Actions = new Actions(this);
            InitializeComponent();
        }

        private void OpenFramesFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new OpenFolderDialog();

            if ((bool)folderDialog.ShowDialog()!)
            {
                var files = Directory.GetFiles(folderDialog.FolderName);

                var rowsFilesAsIntensities = new float[files.Length][];
                var collsFilesAsIntensities = new float[files.Length][];

                for (int i = 0; i < files.Length; i++)
                {
                    var file = files[i];

                    using var mat = new Mat(file);

                    rowsFilesAsIntensities[i] = new RowIntensity().Get(mat);
                    collsFilesAsIntensities[i] = new CollIntensity().Get(mat);
                }


                float[] rowPlotData = GetDetected(rowsFilesAsIntensities);
                float[] collPlotData = GetDetected(collsFilesAsIntensities);

                var rowLineSerie = Actions.GetLineSerie(rowPlotData, OxyColor.FromRgb(255, 0, 0));
                var collLineSerie = Actions.GetLineSerie(collPlotData, OxyColor.FromRgb(255, 0, 0));

                _rowPlotModel.Series.Add(rowLineSerie);
                _collPlotModel.Series.Add(collLineSerie);

                Actions.UpdatePlotView(_rowPlotModel);
                Actions.UpdatePlotView(_collPlotModel);

                MessageBox.Show("Done");
            }
        }

        private static float[] GetDetected(float[][] fileIntensities)
        {
            var samples = new List<float[]>();

            int lineDataCount = fileIntensities[0].Length;
            for (int i = 0; i < lineDataCount; i++)
            {
                var arr = new float[fileIntensities.Length];

                for (int k = 0; k < fileIntensities.Length; k++)
                {
                    arr[k] = fileIntensities[k][i];
                }

                samples.Add(arr);
            }

            int index = 0;
            var rowPlotData = new float[samples.Count];
            foreach (var sample in samples)
            {
                var res = new AnomalyDetection().SpikeDetection(0.95f, sample);

                var val = 0;

                if (res.Any(x => x.Prediction[0] == 1))
                {
                    val = 1;
                }

                rowPlotData[index] = val;
            }

            return rowPlotData;
        }
    }
}
