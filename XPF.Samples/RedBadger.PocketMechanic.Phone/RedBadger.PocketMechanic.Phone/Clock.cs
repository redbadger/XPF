#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

/*namespace RedBadger.PocketMechanic.Phone
{
    using System;
    using System.ComponentModel;

    using Microsoft.Phone.Reactive;

    public class Clock : INotifyPropertyChanged
    {
        private readonly IObservable<long> timer = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1));

        private string time;

        public Clock()
        {
            this.timer.ObserveOnDispatcher().Subscribe(
                Observer.Create<long>(
                    l =>
                        {
                            DateTime dateTime = DateTime.Now;
                            this.Time = dateTime.Second % 2 == 0
                                            ? dateTime.ToLongTimeString()
                                            : dateTime.ToShortTimeString();
                        }));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Time
        {
            get
            {
                return this.time;
            }

            set
            {
                this.time = value;
                this.InvokePropertyChanged(new PropertyChangedEventArgs<TProperty, TOwner>("Time"));
            }
        }

        public void InvokePropertyChanged(PropertyChangedEventArgs<TProperty, TOwner> e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}*/

