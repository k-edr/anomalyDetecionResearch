using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.RowCollSensetivity
{
    class RowIntensity : IIntensity
    {
        public float[] Get(Mat mat)
        {
            var intensities = new float[mat.Height];

            for (int y = 0; y < mat.Height; y++)
            {
                float lineIntensity = 0;

                for(int x = 0;x<mat.Width; x++)
                {
                    var pixel = mat.Get<Vec3b>(y, x);

                    var intensity = (pixel.Item0 + pixel.Item1 + pixel.Item2) / 3f;

                    lineIntensity += intensity;
                }

                intensities[y] = lineIntensity / mat.Width;
            }

            return intensities;
        }
    }
}
