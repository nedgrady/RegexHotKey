using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace RegexHotKey
{
    public class Hooker : IDisposable
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public const int WH_KEYBOARD = 2;

        //LRESULT CALLBACK KeyboardProc(
        //_In_ int code,
        //_In_ WPARAM wParam,
        //_In_ LPARAM lParam (LONG_PTR)
        //);
        public delegate IntPtr HookProc(int code, UIntPtr wParam, IntPtr lparam);

        public bool RegisterKey(HookProc hookProc, IntPtr handle)
        {

            if (hookProc != null)
                Console.WriteLine(SetWindowsHookEx(WH_KEYBOARD, hookProc, handle, 0));
            return true;
        }

        public void Dispose()
        {
            
        }
    }
}
