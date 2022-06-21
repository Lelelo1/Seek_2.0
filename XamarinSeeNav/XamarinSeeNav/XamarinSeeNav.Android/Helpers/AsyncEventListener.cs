﻿using Android.Hardware.Camera2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seek.Droid.Helpers
{
    public class AsyncEventListener
    {
        private readonly Func<bool> _predicate;

        public AsyncEventListener() : this(() => true)
        {

        }

        public AsyncEventListener(Func<bool> predicate)
        {
            _predicate = predicate;
            Successfully = new Task(() => { });
        }

        public void Listen()
        {
            if (!Successfully.IsCompleted && _predicate.Invoke())
            {
                Successfully.RunSynchronously();
            }
        }

        public Task Successfully { get; }
    }
}
