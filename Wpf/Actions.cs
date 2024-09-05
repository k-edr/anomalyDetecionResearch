using OxyPlot.Wpf;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OxyPlot.Axes;
using OxyPlot.Series;
using Wpf.Ml;

namespace Wpf
{
    class Actions
    {
       
        private Window _window;

        public Actions(Window window)
        {
            _window = window;
        }

        public void UpdatePlotView(PlotModel model)
        {
            PlotView? plotView = _window.FindName(model.Title) as PlotView;
            if (plotView != null)
            {
                plotView.Model = model;
                plotView.Model.InvalidatePlot(true);
            }
            //MessageBox.Show("Updated");
        }

        public PlotModel CreatePlot(float[] data, float[] original, string title)
        {
            if (data.Length != original.Length)
            {
                var min = Math.Min(data.Length, original.Length);

                data = data.Take(min).ToArray();
                original = original.Take(min).ToArray();
            }

            var lineSeries = GetLineSerie(data, OxyColor.FromRgb(255, 0, 0));

            var originalLineSeries = GetLineSerie(original, OxyColor.FromRgb(0, 255, 0));

            var model = new PlotModel() { Title = title };

            model.Series.Add(lineSeries);
            model.Series.Add(originalLineSeries);

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            return model;
        }

        public LineSeries GetLineSerie(float[] data, OxyColor color)
        {
            var lineSerie = new LineSeries() { Color = color };

            for (int i = 0; i < data.Length; i++)
            {
                var sample = data[i];

                lineSerie.Points.Add(new DataPoint(i, sample));
            }

            return lineSerie;
        }

        public ScatterSeries GetScatterSerie(OutputModel[] models)
        {
            var scatterSerie = new ScatterSeries() { MarkerType = MarkerType.Triangle, Title = "Spike", MarkerSize = 5d };

            for (int i = 0; i < models.Length; i++)
            {
                var sample = models[i];

                if (sample.Prediction[0] == 1)
                {
                    scatterSerie.Points.Add(new ScatterPoint(i, sample.Intensity));
                }
            }

            return scatterSerie;
        }
    }
}
