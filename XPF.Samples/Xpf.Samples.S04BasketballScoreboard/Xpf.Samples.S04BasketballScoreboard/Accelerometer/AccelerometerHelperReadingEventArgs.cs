namespace Xpf.Samples.S04BasketballScoreboard.Accelerometer
{
    using System;

    /// <summary>
    ///     Arguments provided by the Accelerometer Helper data event
    /// </summary>
    public class AccelerometerHelperReadingEventArgs : EventArgs
    {
        /// <summary>
        ///     Filtered and temporally averaged accelerometer data using an arithmetic mean of the last 25 "optimaly filtered" 
        ///     samples (see above), so over 500ms at 50Hz on each axis, to virtually eliminate most sensor noise. 
        ///     This provides a very stable reading but it has also a very high latency and cannot be used for rapidly reacting UI.
        /// </summary>
        public Simple3DVector AverageAcceleration { get; set; }

        /// <summary>
        ///     Filtered accelerometer data using a 1 Hz first-order low-pass on each axis to elimate the main sensor noise
        ///     while providing a medium latency. This can be used for moderatly reacting UI updates requiring a very smooth signal.
        /// </summary>
        public Simple3DVector LowPassFilteredAcceleration { get; set; }

        /// <summary>
        ///     Filtered accelerometer data using a combination of a low-pass and threshold triggered high-pass on each axis to 
        ///     elimate the majority of the sensor low amplitude noise while trending very quickly to large offsets (not perfectly
        ///     smooth signal in that case), providing a very low latency. This is ideal for quickly reacting UI updates.
        /// </summary>
        public Simple3DVector OptimalyFilteredAcceleration { get; set; }

        /// <summary>
        ///     Raw, unfiltered accelerometer data (acceleration vector in all 3 dimensions) coming directly from sensor.
        ///     This is required for updating rapidly reacting UI.
        /// </summary>
        public Simple3DVector RawAcceleration { get; set; }
    }
}