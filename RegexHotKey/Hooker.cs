using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RegexHotKey
{
    public delegate void CharCallback(char c);

    internal sealed class Hooker
        : IDisposable
    {

        #region DLLImports
        [DllImport(Constants.HOOK_PATH,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterExternalSubscriber(IntPtr callback);
        
        [DllImport(Constants.HOOK_PATH,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern bool UnregisterExternalHandler(int charCallback);
        #endregion

        #region Singleton
        private static readonly Lazy<Hooker> lazy = new Lazy<Hooker>(() => new Hooker());

        public static Hooker Instance { get { return lazy.Value; } }
        #endregion

        private readonly object registerLock = new object();
        private int _handle = 1;
        private int _subscriberCount = 0;

        private CharCallback _keyDown;

        public event CharCallback OnKeyDown
        {
            add
            {
                if (value == null)
                    throw new ArgumentNullException("OnKeyDown delegate");

                if (value.Equals(_keyDown)) //sure hope this never happens...
                    throw new ArgumentException("OnKeyDown delegate is reference to self");

                lock (registerLock)
                {
                    if (_subscriberCount == 0)
                        RegisterKeyHandler();
                    _subscriberCount++;
                    _keyDown += value;
                }
            }
            remove
            {
                lock (registerLock)
                {
                    _subscriberCount--;
                    _keyDown -= value;

                    if(_subscriberCount < 1)
                    {
                        RemoveKeyHandler();
                    }
                }
            }
        }

        private bool RegisterKeyHandler(CharCallback charCallback = null)
        {
            if (charCallback == null)
                charCallback = KeyDownExternal;

            try
            {
                IntPtr callback = Marshal.GetFunctionPointerForDelegate(charCallback);
                _handle = RegisterExternalSubscriber(callback);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return true;
        }

        private void RemoveKeyHandler()
        {
            UnregisterExternalHandler(_handle);
        }

        void RaiseKeyDown(char c)
        {
            _keyDown?.Invoke(c);
        }

        private void KeyDownExternal(char c)
        {
            RaiseKeyDown(c);
        }


        #region IDisposable

        private bool _disposed = false;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Manual release of managed resources.
                }
                UnregisterExternalHandler(_handle);
                _disposed = true;
            }
        }

        ~Hooker() { Dispose(false); }
        #endregion

    }
}
