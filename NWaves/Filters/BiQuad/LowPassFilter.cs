﻿using System;
using NWaves.Filters.Base;

namespace NWaves.Filters.BiQuad
{
    /// <summary>
    /// BiQuad LP filter.
    /// The coefficients are calculated automatically according to 
    /// audio-eq-cookbook by R.Bristow-Johnson and WebAudio API.
    /// </summary>
    public class LowPassFilter : BiQuadFilter
    {
        /// <summary>
        /// Constructor computes the filter coefficients.
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="q"></param>
        public LowPassFilter(double freq, double q = 1) : base(MakeTf(freq, q))
        {
            Normalize();
        }

        /// <summary>
        /// TF generator
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static TransferFunction MakeTf(double freq, double q)
        {
            var omega = 2 * Math.PI * freq;
            var alpha = Math.Sin(omega) / (2 * q);
            var cosw = Math.Cos(omega);

            var b0 = (1 - cosw) / 2;
            var b1 = 1 - cosw;
            var b2 = b0;

            var a0 = 1 + alpha;
            var a1 = -2 * cosw;
            var a2 = 1 - alpha;

            return new TransferFunction(new[] { b0, b1, b2 }, new[] { a0, a1, a2 });
        }
    }
}
