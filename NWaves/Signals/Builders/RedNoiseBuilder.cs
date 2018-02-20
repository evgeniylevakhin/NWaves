﻿using System;
using System.Collections.Generic;

namespace NWaves.Signals.Builders
{
    /// <summary>
    /// Class for a red (Brownian) noise generator
    /// </summary>
    public class RedNoiseBuilder : SignalBuilder
    {
        /// <summary>
        /// Lower amplitude level
        /// </summary>
        private double _low;

        /// <summary>
        /// Upper amplitude level
        /// </summary>
        private double _high;

        /// <summary>
        /// Root-Mean-Square (RMS) value
        /// </summary>
        private double? _rms;

        public RedNoiseBuilder()
        {
            ParameterSetters = new Dictionary<string, Action<double>>
            {
                { "low, lo, min",  param => _low = param },
                { "high, hi, max", param => _high = param },
                { "rms", param => _rms = param }
            };

            _low = -1.0;
            _high = 1.0;
        }

        /// <summary>
        /// Method implements fancy filtering for obtaining the red noise.
        /// </summary>
        /// <returns></returns>
        protected override DiscreteSignal Generate()
        {
            if (_high < _low)
            {
                throw new FormatException("Upper level must be greater than the lower one!");
            }

            var rand = new Random();

            double scaleCoeff = _rms.HasValue ? 10.75 : 3.5;

            double prev = 0;

            var red = new double[Length];
            for (var n = 0; n < Length; n++)
            {
                var white = rand.NextDouble() * (_high - _low) + _low;

                red[n] = (prev + (0.02 * white)) / 1.02;
                prev = red[n];
                red[n] *= scaleCoeff;
            }

            return new DiscreteSignal(SamplingRate, red);
        }
    }
}
