// Copyright (c) Microsoft Corporation.  All rights reserved.
//
// Use of this source code is subject to the terms of the Microsoft
// license agreement under which you licensed this source code.
// If you did not accept the terms of the license agreement,
// you are not authorized to use this source code.
// For the terms of the license, please see the license agreement
// signed by you and Microsoft.
// THE SOURCE CODE IS PROVIDED "AS IS", WITH NO WARRANTIES OR INDEMNITIES.

namespace Xpf.Samples.S04BasketballScoreboard.Accelerometer
{
    using System;
    using System.Diagnostics;

    using Microsoft.Devices.Sensors;

    /// <summary>
    ///     Accelerometer Helper Class, providing filtering and local calibration of accelerometer sensor data
    /// </summary>
    public sealed class AccelerometerHelper : IDisposable
    {
        /// <summary>
        ///     This is the smoothing factor used for the 1st order discrete Low-Pass filter
        ///     The cut-off frequency fc = fs * K/(2*PI*(1-K))
        /// </summary>
        /// <remarks>
        ///     With a 50Hz sampling rate, this is gives a 1Hz cut-off
        /// </remarks>
        private const double LowPassFilterCoef = 0.1;

        /// <summary>
        ///     This is the maximum inclination angle variation on any axis between the average acceleration and the filtered 
        ///     acceleration beyond which the device cannot be calibrated on that particular axis.
        ///     The calibration cannot be done until this condition is met on the last contiguous samples from the accelerometer
        /// </summary>
        /// <remarks>
        ///     0.5 deg inclination delta at max
        /// </remarks>
        private const double MaximumStabilityTiltDeltaAngle = 0.5 * Math.PI / 180.0;

        /// <summary>
        ///     Maximum amplitude of noise from sample to sample. 
        ///     This is used to remove the noise selectively while allowing fast trending for larger amplitudes
        /// </summary>
        /// <remarks>
        ///     up to 0.05g deviation from filtered value is considered noise
        /// </remarks>
        private const double NoiseMaxAmplitude = 0.05;

        /// <summary>
        ///     Number of prior samples to keep for averaging.       
        ///     The higher this number, the larger the latency will be: 
        ///     At 50Hz sampling rate: Latency = 20ms * SamplesCount
        /// </summary>
        /// <remarks>
        ///     averaging and checking stability on 500ms
        /// </remarks>
        private const int SamplesCount = 25;

        /// <summary>
        ///     Corresponding lateral acceleration offset at 1g of Maximum Stability Tilt Delta Angle
        /// </summary>
        private static readonly double maximumStabilityDeltaOffset = Math.Sin(MaximumStabilityTiltDeltaAngle);

        private static readonly object syncRoot = new object();

        /// <summary>
        ///     Circular buffer of filtered samples
        /// </summary>
        private readonly Simple3DVector[] sampleBuffer = new Simple3DVector[SamplesCount];

        /// <summary>
        ///     Singleton instance for helper - prefered solution to static class to avoid static constructor (10x slower)
        /// </summary>
        private static volatile AccelerometerHelper singletonInstance;

        /// <summary>
        ///     Accelerometer is active and reading value when true
        /// </summary>
        private bool active;

        /// <summary>
        ///     Average acceleration
        ///     This is a simple arithmetic average over the entire _sampleFile (SamplesCount elements) which contains filtered readings
        ///     This is used for the calibration, to get a more steady reading of the acceleration
        /// </summary>
        private Simple3DVector averageAcceleration;

        /// <summary>
        ///     Number of samples for which the accelemeter is "stable" (filtered acceleration is within Maximum Stability Tilt 
        ///     Delta Angle of average acceleration)
        /// </summary>
        private int deviceStableCount;

        /// <summary>
        ///     Indicates that the helper has not been initialized yet.
        ///     This is used for filter past data initialization
        /// </summary>
        private bool initialized;

        /// <summary>
        ///     n-1 of low pass filter output
        /// </summary>
        private Simple3DVector previousLowPassOutput;

        /// <summary>
        ///     n-1 of optimal filter output
        /// </summary>
        private Simple3DVector previousOptimalFilterOutput;

        /// <summary>
        ///     Index in circular buffer of samples
        /// </summary>
        private int sampleIndex;

        /// <summary>
        ///     Sum of all the filtered samples in the circular buffer file
        /// </summary>
        /// <remarks>
        ///     assume start flat: -1g in z axis
        /// </remarks>
        private Simple3DVector sampleSum = new Simple3DVector(
            0.0 * SamplesCount, 0.0 * SamplesCount, -1.0 * SamplesCount);

        /// <summary>
        ///     Accelerometer sensor
        /// </summary>
        private Accelerometer sensor;

        /// <summary>
        ///     Private constructor,
        ///     Use Instance property to get singleton instance
        /// </summary>
        private AccelerometerHelper()
        {
            // Set up buckets for calculating rolling average of the accelerations
            this.sampleIndex = 0;
        }

        /// <summary>
        ///     New raw and processed accelerometer data available event.
        ///     Fires every 20ms.
        /// </summary>
        public event EventHandler<AccelerometerHelperReadingEventArgs> ReadingChanged;

        /// <summary>
        ///     Singleton instance of the Accelerometer Helper class
        /// </summary>
        public static AccelerometerHelper Instance
        {
            get
            {
                if (singletonInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (singletonInstance == null)
                        {
                            singletonInstance = new AccelerometerHelper();
                        }
                    }
                }

                return singletonInstance;
            }
        }

        /// <summary>
        ///     Accelerometer is active and reading value when true
        /// </summary>
        public bool Active
        {
            get
            {
                return this.active;
            }

            set
            {
                if (!this.NoAccelerometer)
                {
                    if (value)
                    {
                        if (!this.active)
                        {
                            this.StartAccelerometer();
                        }
                    }
                    else
                    {
                        if (this.active)
                        {
                            this.StopAccelerometer();
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     True when the device is "stable" (no movement for about 0.5 sec)
        /// </summary>
        public bool DeviceStable
        {
            get
            {
                return this.deviceStableCount >= SamplesCount;
            }
        }

        /// <summary>
        ///     Accelerometer is not present on device
        /// </summary>
        public bool NoAccelerometer { get; private set; }

        /// <summary>
        ///     Release sensor resource if not already done
        /// </summary>
        public void Dispose()
        {
            if (this.sensor != null)
            {
                this.sensor.Dispose();
            }
        }

        /// <summary>
        ///     discrete low-magnitude fast low-pass filter used to remove noise from raw accelerometer while allowing fast trending on high amplitude changes
        /// </summary>
        /// <param name = "newInputValue">New input value (latest sample)</param>
        /// <param name = "priorOutputValue">The previous (n-1) output value (filtered, one sampling period ago)</param>
        /// <returns>The new output value</returns>
        private static double FastLowAmplitudeNoiseFilter(double newInputValue, double priorOutputValue)
        {
            double newOutputValue = newInputValue;
            if (Math.Abs(newInputValue - priorOutputValue) <= NoiseMaxAmplitude)
            {
                // Simple low-pass filter
                newOutputValue = priorOutputValue + (LowPassFilterCoef * (newInputValue - priorOutputValue));
            }

            return newOutputValue;
        }

        /// <summary>
        ///     1st order discrete low-pass filter used to remove noise from raw accelerometer.
        /// </summary>
        /// <param name = "newInputValue">New input value (latest sample)</param>
        /// <param name = "priorOutputValue">The previous output value (filtered, one sampling period ago)</param>
        /// <returns>The new output value</returns>
        private static double LowPassFilter(double newInputValue, double priorOutputValue)
        {
            double newOutputValue = priorOutputValue + (LowPassFilterCoef * (newInputValue - priorOutputValue));
            return newOutputValue;
        }

        /// <summary>
        ///     Initialize Accelerometer sensor and start sampling
        /// </summary>
        private void StartAccelerometer()
        {
            try
            {
                this.sensor = new Accelerometer();
                this.sensor.ReadingChanged += this.SensorReadingChanged;
                this.sensor.Start();
                this.active = true;
                this.NoAccelerometer = false;
            }
            catch (Exception e)
            {
                this.active = false;
                this.NoAccelerometer = true;
                Debug.WriteLine("Exception creating Accelerometer: " + e.Message);
            }
        }

        /// <summary>
        ///     Stop smpling and release accelerometer sensor
        /// </summary>
        private void StopAccelerometer()
        {
            try
            {
                if (this.sensor != null)
                {
                    this.sensor.ReadingChanged -= this.SensorReadingChanged;
                    this.sensor.Stop();
                    this.sensor = null;
                    this.active = false;
                    this.initialized = false;
                }
            }
            catch (Exception e)
            {
                this.active = false;
                this.NoAccelerometer = true;
                Debug.WriteLine("Exception deleting Accelerometer: " + e.Message);
            }
        }

        /// <summary>
        ///     Called on accelerometer sensor sample available.
        ///     Main accelerometer data filtering routine
        /// </summary>
        /// <param name = "sender">Sender of the event.</param>
        /// <param name = "e">AccelerometerReadingAsyncEventArgs</param>
        private void SensorReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            Simple3DVector lowPassFilteredAcceleration;
            Simple3DVector optimalFilteredAcceleration;
            Simple3DVector averagedAcceleration;
            var rawAcceleration = new Simple3DVector(e.X, e.Y, e.Z);

            lock (this.sampleBuffer)
            {
                if (!this.initialized)
                {
                    // Initialize file with 1st value
                    this.sampleSum = rawAcceleration * SamplesCount;
                    this.averageAcceleration = rawAcceleration;

                    // Initialize file with 1st value
                    for (int i = 0; i < SamplesCount; i++)
                    {
                        this.sampleBuffer[i] = this.averageAcceleration;
                    }

                    this.previousLowPassOutput = this.averageAcceleration;
                    this.previousOptimalFilterOutput = this.averageAcceleration;

                    this.initialized = true;
                }

                // low-pass filter
                lowPassFilteredAcceleration =
                    new Simple3DVector(
                        LowPassFilter(rawAcceleration.X, this.previousLowPassOutput.X), 
                        LowPassFilter(rawAcceleration.Y, this.previousLowPassOutput.Y), 
                        LowPassFilter(rawAcceleration.Z, this.previousLowPassOutput.Z));
                this.previousLowPassOutput = lowPassFilteredAcceleration;

                // optimal filter
                optimalFilteredAcceleration =
                    new Simple3DVector(
                        FastLowAmplitudeNoiseFilter(rawAcceleration.X, this.previousOptimalFilterOutput.X), 
                        FastLowAmplitudeNoiseFilter(rawAcceleration.Y, this.previousOptimalFilterOutput.Y), 
                        FastLowAmplitudeNoiseFilter(rawAcceleration.Z, this.previousOptimalFilterOutput.Z));
                this.previousOptimalFilterOutput = optimalFilteredAcceleration;

                // Increment circular buffer insertion index
                this.sampleIndex++;
                if (this.sampleIndex >= SamplesCount)
                {
                    this.sampleIndex = 0;
                        
                        // if at max SampleCount then wrap samples back to the beginning position in the list
                }

                // Add new and remove old at _sampleIndex
                Simple3DVector newVect = optimalFilteredAcceleration;
                this.sampleSum += newVect;
                this.sampleSum -= this.sampleBuffer[this.sampleIndex];
                this.sampleBuffer[this.sampleIndex] = newVect;

                averagedAcceleration = this.sampleSum / SamplesCount;
                this.averageAcceleration = averagedAcceleration;

                // Stablity check
                // If current low-pass filtered sample is deviating for more than 1/100 g from average (max of 0.5 deg inclination noise if device steady)
                // then reset the stability counter.
                // The calibration will be prevented until the counter is reaching the sample count size (calibration enabled only if entire 
                // sampling buffer is "stable"
                Simple3DVector deltaAcceleration = averagedAcceleration - optimalFilteredAcceleration;
                if ((Math.Abs(deltaAcceleration.X) > maximumStabilityDeltaOffset) ||
                    (Math.Abs(deltaAcceleration.Y) > maximumStabilityDeltaOffset) ||
                    (Math.Abs(deltaAcceleration.Z) > maximumStabilityDeltaOffset))
                {
                    // Unstable
                    this.deviceStableCount = 0;
                }
                else
                {
                    if (this.deviceStableCount < SamplesCount)
                    {
                        ++this.deviceStableCount;
                    }
                }
            }

            if (this.ReadingChanged != null)
            {
                var readingEventArgs = new AccelerometerHelperReadingEventArgs
                    {
                        RawAcceleration = rawAcceleration,
                        LowPassFilteredAcceleration = lowPassFilteredAcceleration,
                        OptimalyFilteredAcceleration = optimalFilteredAcceleration,
                        AverageAcceleration = averagedAcceleration
                    };

                this.ReadingChanged(this, readingEventArgs);
            }
        }
    }
}