using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ml
{
    class OutputModel
    {
        [VectorType(3)]
        public double[] Prediction { get; set; }

        public float Intensity { get; set; }

    }
}
