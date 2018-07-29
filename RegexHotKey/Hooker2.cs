using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System;

namespace RegexHotKey
{
    public delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    class Hooker2
        :IDisposable
    {
        #region DLLImports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
           KeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int HC_ACTION = 0;
        #endregion

        #region Singleton
        private static readonly Lazy<Hooker2> lazy = new Lazy<Hooker2>(() => new Hooker2());

        public static Hooker2 Instance { get { return lazy.Value; } }
        #endregion

        //private readonly KeyboardProc _proc;
        private readonly IntPtr _hHook;

        public event CharCallback OnKeyDown;

        private Hooker2()
        {
            //_proc  = KeyboardCallback;
            _hHook = SetHook(KeyboardCallback);
        }

        private IntPtr KeyboardCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }

            if(nCode == HC_ACTION && wParam.ToInt32() == WM_KEYDOWN)
            {
                int virtualKeyCode = Marshal.ReadInt32(lParam);
                //Console.WriteLine((Keys)virtualKeyCode);
                //string s = (Keys)virtualKeyCode
                
            }
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        void RaiseKeyDown(char c)
        {
            OnKeyDown?.Invoke(c);
        }

        #region HookAndUnhook
        private IntPtr SetHook(KeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private bool Unhook()
        {
            return UnhookWindowsHookEx(_hHook);
        }
        #endregion

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
                UnhookWindowsHookEx(_hHook);
                _disposed = true;
            }
        }

        ~Hooker2() { Dispose(false); }
        #endregion
    }
}
