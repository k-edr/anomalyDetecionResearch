using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.RowCollSensetivity
{
    class CollIntensity
    {
        public float[] Get(Mat mat)
        {
            var intensities = new float[mat.Width];

            for (int x = 0; x < mat.Width; x++)
            {
                float collIntensity = 0;

                for (int y = 0; y < mat.Height; y++)
                {
                    var pixel = mat.Get<Vec3b>(y, x);

                    var intensity = (pixel.Item0 + pixel.Item1 + pixel.Item2) / 3f;

                    collIntensity += intensity;
                }

                intensities[x] = collIntensity / mat.Height;
            }

            return intensities;
        }
    }
}
