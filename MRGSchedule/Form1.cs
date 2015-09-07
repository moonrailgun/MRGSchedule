using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRGSchedule
{
    public partial class Form1 : Form
    {
        public int clockHandle;

        public Form1()
        {
            InitializeComponent();

            GetClockHandle();

            HookStart();
        }
        #region 时钟窗口句柄
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int FindWindowEx(int hwndParent, int hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(int hWndParent, CallBack lpfn, int lParam);

        [DllImport("user32.dll")]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public void GetClockHandle()
        {
            int stHandle, trayHandle;
            stHandle = FindWindow("Shell_TrayWnd", "");
            trayHandle = FindWindowEx(stHandle, 0, "TrayNotifyWnd", "");
            EnumChildWindows(trayHandle, IsClock, 0);
        }

        public delegate bool CallBack(int hwnd, int lParam);

        /// <summary>
        /// 判断是否为时钟句柄的回调函数
        /// </summary>
        private bool IsClock(int hwnd, int lParam)
        {
            StringBuilder className = new StringBuilder();
            GetClassName((IntPtr)hwnd, className, 20);

            if (className.ToString().Trim() == "TrayClockWClass")
            {
                clockHandle = hwnd;
                return false;
            }

            return true;
        }
        #endregion

        #region 钩子Hook
        // 安装钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        // 卸载钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        // 继续下一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        // 取得当前线程编号
        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        private static int hMouseHook = 0;//钩子ID

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        //钩子回调
        private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MouseButtons button = MouseButtons.None;
                int clickCount = 0;
                switch (wParam)
                {
                    case MouseEvent.WM_LBUTTONDOWN:
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case MouseEvent.WM_LBUTTONUP:
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case MouseEvent.WM_LBUTTONDBLCLK:
                        button = MouseButtons.Left;
                        clickCount = 2;
                        break;
                    case MouseEvent.WM_RBUTTONDOWN:
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case MouseEvent.WM_RBUTTONUP:
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case MouseEvent.WM_RBUTTONDBLCLK:
                        button = MouseButtons.Right;
                        clickCount = 2;
                        break;
                }

                MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                MouseEventArgs e = new MouseEventArgs(button, clickCount, MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y, 0);
                Console.WriteLine(nCode + "|" + wParam + "|" + lParam);//调试信息

                Point Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
                if (MouseActivityEvent != null)
                {
                    MouseActivityEvent(this, e);
                }
            }

            return CallNextHookEx(hMouseHook, nCode, wParam, lParam);//继续传递
        }

        // 安装钩子
        public void HookStart()
        {
            if (hMouseHook == 0)
            {
                HookProc MouseHookProcedure = new HookProc(MouseHookProc);// 创建HookProc实例
                hMouseHook = SetWindowsHookEx((int)HookType.WH_MOUSE_LL, MouseHookProcedure, IntPtr.Zero, 0);//设置线程钩子

                // 如果设置钩子失败
                if (hMouseHook == 0)
                {
                    HookStop();
                    throw new Exception("SetWindowsHookEx failed.");
                }
            }
        }

        // 卸载钩子
        public void HookStop()
        {
            bool retMouse = true;
            if (hMouseHook != 0)
            {
                retMouse = UnhookWindowsHookEx(hMouseHook);
                hMouseHook = 0;
            }
            if (!retMouse)
                throw new Exception("UnhookWindowsHookEx failed.");
        }

        //委托+事件（把钩到的消息封装为事件，由调用者处理）
        /*
        public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
        public event MouseMoveHandler MouseMoveEvent;

        public delegate void MouseClickHandler(object sender, MouseEventArgs e);
        public event MouseClickHandler MouseClickEvent;
        */
        public delegate void MouseActivityHandler(object sender, MouseEventArgs e);
        public event MouseActivityHandler MouseActivityEvent;
        #endregion
    }
}
