﻿using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.RowCollSensetivity
{
    interface IIntensity
    {
        float[] Get(Mat mat);
    }
}
