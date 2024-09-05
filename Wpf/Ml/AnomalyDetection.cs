using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using Wpf.Ml.Base;

namespace Wpf.Ml
{
    class AnomalyDetection:BaseMl
    {
        public OutputModel[] ChangePointDetection(double confidence, IEnumerable<InputModel> models)
        {
            var pipeline = MLContext.Transforms.DetectIidChangePoint(
                outputColumnName: nameof(OutputModel.Prediction),
                inputColumnName: nameof(InputModel.Intensity),
                confidence: confidence,
                changeHistoryLength: models.Count()/4);

            return Detect(pipeline, models);
        }

        public OutputModel[] ChangePointDetection(double confidence, float[] data)
        {           
            return ChangePointDetection(confidence, GetAsModels(data));
        }

        public OutputModel[] SpikeDetection(double confidence, IEnumerable<InputModel> models)
        {
            var pipeline = MLContext.Transforms.DetectIidSpike(
                outputColumnName: nameof(OutputModel.Prediction),
                inputColumnName: nameof(InputModel.Intensity),
                confidence: confidence,
                pvalueHistoryLength: models.Count()/4);

            return Detect(pipeline, models);
        }

        public OutputModel[] SpikeDetection(double confidence, float[] data)
        {
            return SpikeDetection(confidence, GetAsModels(data));
        }

        private OutputModel[] Detect<T>(TrivialEstimator<T> pipeline , IEnumerable<InputModel> models) where T : class,ITransformer
        {
            var data = MLContext.Data.LoadFromEnumerable(models);

            var model = pipeline.Fit(data);

            var predictions = model.Transform(data);

            var scoredData = MLContext.Data.CreateEnumerable<OutputModel>(predictions, reuseRowObject: false);

            return scoredData.ToArray();
        }

        private InputModel[] GetAsModels(float[] data)
        {
            var list = new List<InputModel>();

            foreach (var input in data)
            {
                list.Add(new InputModel() { Intensity = input });
            }

            return list.ToArray();
        }
    }
}
