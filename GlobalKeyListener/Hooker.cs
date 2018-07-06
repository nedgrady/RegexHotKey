using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace GlobalKeyListener
{
    public delegate void CharCallback(char c);

    internal class Hooker
        : IDisposable
    {
        private int _handle = 0;

        readonly object _registerLock = new object();
        static readonly object externalLock = new object();

        [DllImport("E:\\Code\\PDT\\RegexHotKey\\HooksUnmanaged\\HooksUnmanaged.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterExternalSubscriber(IntPtr callback);

        [DllImport("E:\\Code\\PDT\\RegexHotKey\\HooksUnmanaged\\HooksUnmanaged.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern bool UnregisterExternalHandler(int charCallback);

        public event CharCallback KeyDown
        {
            add
            {
                lock(_registerLock)
                {
                    KeyDown += value;
                    RegisterKeyHandler(value);
                }
            }

            remove
            {
                lock (_registerLock)
                {
                    KeyDown += value;
                    RemoveKeyHandler();
                }
            }
        }


        private bool RegisterKeyHandler(CharCallback charCallback)
        {
            if (charCallback == null)
                throw new ArgumentNullException("charCallback");

            IntPtr callback = Marshal.GetFunctionPointerForDelegate(charCallback);
            _handle = RegisterExternalSubscriber(callback);

            return true;
        }

        private bool RemoveKeyHandler()
        {

        }
/*
        #region IDisposable

        private bool _disposed = false;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Manual release of managed resources.
                }
                UnregisterExternalHandler()
                _disposed = true;
            }
        }

        ~Hooker() { Dispose(false); }
        #endregion
        */
    }
}
