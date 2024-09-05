using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ml.Base
{
    abstract class BaseMl
    {
        protected MLContext MLContext { get; set; }

        protected BaseMl()
        {
            MLContext = new MLContext();
        }
    }
}
