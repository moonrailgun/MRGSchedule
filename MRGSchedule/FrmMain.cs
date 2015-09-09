﻿using LayeredSkin.DirectUI;
using MRGSchedule.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRGSchedule
{
    public partial class FrmMain : LayeredBaseForm
    {
        #region 常量
        public static Font Msgfont = new Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        public static Font font1 = new Font("微软雅黑", 9.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        public static Font font2 = new Font("微软雅黑", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        #endregion

        public DuiComboBox ScYear;
        public DuiComboBox ScMonth;
        public DuiLabel DateTimeNow;

        public int clockHandle;

        public FrmMain()
        {
            InitializeComponent();

            this.BackColor = Color.FromArgb(255 / 2, Color.White);
            this.Width = 7 * 70 + 20;
            this.Height = 6 * 50 + 85;

            MonthBaseControl.Location = new Point(10, 55);

            GetClockHandle();//获取托盘时钟的句柄

            CreatDataSelectControl();

            //HookStart();//开始hook
        }

        //创建控件
        private void CreatDataSelectControl()
        {
            #region 日期选择栏
            DataSelectControl.BackColor = Color.FromArgb(255, Color.White);
            DataSelectControl.Location = new Point(5, 5);
            DataSelectControl.Width = this.Width - 10;
            DataSelectControl.Height = 50;
            #endregion

            #region 透明度轨道
            TrackOpacity.Width = this.Width;
            TrackOpacity.Location = new Point(0, 48);
            TrackOpacity.Value = 0.8;
            #endregion

            #region 添加移动窗体栏
            DuiBaseControl title = new DuiBaseControl();
            title.Location = new Point(0, 0);
            title.Size = new Size(DataSelectControl.Width, DataSelectControl.Height);//设置大小为日期选择栏
            title.MouseDown += MoveFormMouseDown;
            title.BackColor = Color.Transparent;
            DataSelectControl.DUIControls.Add(title);
            #endregion

            #region 添加今天按钮
            DuiButton bttaday = new DuiButton();
            bttaday.Location = new Point(15, 0);
            bttaday.NormalImage = Resources.BtnTodayn;
            bttaday.HoverImage = Resources.BtnTodaye;
            bttaday.PressedImage = Resources.BtnTodayd;
            bttaday.MouseClick += BtnTodayClick;
            DataSelectControl.DUIControls.Add(bttaday);
            #endregion

            #region 添加年份列表
            ScYear = new DuiComboBox();
            ScYear.BaseColor = Color.White;
            ScYear.BackColor = Color.White;
            ScYear.Size = new Size(85, 25);
            ScYear.Location = new Point(85, 10);
            ScYear.SelectedIndexChanged += CBSelectedIndexChanged;
            for (int i = 1995; i < 2021; i++)
            {
                DuiLabel lb = new DuiLabel();
                lb.Text = string.Format("  {0}年", i);
                lb.Tag = i;
                lb.Size = new Size(84, 25);
                lb.Font = new Font("微软雅黑", 10);
                lb.TextAlign = ContentAlignment.MiddleLeft;
                lb.ForeColor = Color.DodgerBlue;
                lb.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
                lb.Location = new Point(0, i * 25);
                lb.BackColor = Color.White;
                lb.MouseEnter += DateItemsMoveEnter;
                lb.MouseLeave += DateItemsMoveLeave;
                ScYear.Items.Add(lb);
                if (i == DateTime.Now.Year)
                {
                    ScYear.SelectedIndex = i - 1995;
                    ScYear.InnerListBox.Value = (double)i / (double)(2020);
                }
            }
            ScYear.ShowBorder = false;
            ScYear.InnerListBox.Height = 200;
            ScYear.InnerListBox.Width = 85;
            ScYear.InnerListBox.RefreshList();//刷新列表
            DataSelectControl.DUIControls.Add(ScYear);
            #endregion

            #region 添加上一月按钮
            DuiButton btbefore = new DuiButton();
            btbefore.Location = new Point(180, 12);
            btbefore.NormalImage = Resources.before_n;
            btbefore.HoverImage = Resources.before_e;
            btbefore.PressedImage = Resources.before_d;
            btbefore.MouseClick += BtnBeforClick;
            DataSelectControl.DUIControls.Add(btbefore);
            #endregion

            #region 添加月份列表
            ScMonth = new DuiComboBox();
            ScMonth.BaseColor = Color.White;
            ScMonth.BackColor = Color.White;
            ScMonth.Size = new Size(65, 25);
            ScMonth.Location = new Point(205, 10);
            ScMonth.SelectedIndexChanged += CBSelectedIndexChanged;
            for (int i = 1; i <= 12; i++)
            {
                DuiLabel lb = new DuiLabel();
                lb.Text = string.Format("  {0}月", i.ToString("00"));
                lb.Tag = i;
                lb.Size = new Size(64, 25);
                lb.Font = new Font("微软雅黑", 10);
                lb.TextAlign = ContentAlignment.MiddleLeft;
                lb.ForeColor = Color.DodgerBlue;
                lb.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
                lb.Location = new Point(0, i * 25);
                lb.BackColor = Color.White;
                lb.MouseEnter += DateItemsMoveEnter;
                lb.MouseLeave += DateItemsMoveLeave;
                ScMonth.Items.Add(lb);
                if (i == DateTime.Now.Month)
                {
                    ScMonth.SelectedIndex = i - 1;
                    ScMonth.InnerListBox.Value = (double)i / (double)(12);
                }
            }
            ScMonth.ShowBorder = false;
            ScMonth.InnerListBox.Height = 200;
            ScMonth.InnerListBox.Width = 65;
            ScMonth.InnerListBox.RefreshList();
            DataSelectControl.DUIControls.Add(ScMonth);
            #endregion

            #region 添加下一月按钮
            DuiButton btnext = new DuiButton();
            btnext.Location = new Point(275, 12);
            btnext.NormalImage = Resources.next_n;
            btnext.HoverImage = Resources.next_e;
            btnext.PressedImage = Resources.next_d;
            btnext.MouseClick += BtnNextClick;
            DataSelectControl.DUIControls.Add(btnext);
            #endregion
        }

    
        #region 年月列表项鼠标事件

        private void DateItemsMoveEnter(object sender, EventArgs e)
        {
            ((DuiLabel)sender).BackColor = Color.FromArgb(45, 151, 222);
            ((DuiLabel)sender).ForeColor = Color.White;
        }
        private void DateItemsMoveLeave(object sender, EventArgs e)
        {
            ((DuiLabel)sender).BackColor = Color.White;
            ((DuiLabel)sender).ForeColor = Color.DodgerBlue;
        }
        /// <summary>
        /// 下拉项改变时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RefrshMonthDay();
            }
            catch
            {
            }
        }
        /// <summary>
        /// 更新日历
        /// </summary>
        public void RefrshMonthDay()
        {
            /*
            if (Frmdaysinfo != null)
            {
                Frmdaysinfo.Hide();
            }
            DuiLabel lbmonth = (DuiLabel)CbMonth.Items[CbMonth.SelectedIndex];
            DuiLabel lbyear = (DuiLabel)CbYear.Items[CbYear.SelectedIndex];
            DateTime dt = DateTime.Parse(string.Format("{0}-{1}", lbyear.Tag.ToString(), lbmonth.Tag.ToString()));
            RefreshMonth(dt);*/
        }
        #endregion

        
        /// <summary>
        /// 上个月
        /// </summary>
        private void BtnBeforClick(object sender, EventArgs e)
        {
            if (ScMonth.SelectedIndex == 0)
            {
                ScYear.SelectedIndex -= 1;
                ScMonth.SelectedIndex = 11;
            }
            else
            {
                ScMonth.SelectedIndex -= 1;
            }
        }
        /// <summary>
        /// 下个月
        /// </summary>
        private void BtnNextClick(object sender, EventArgs e)
        {
            try
            {
                if (ScMonth.SelectedIndex == 11)
                {
                    ScYear.SelectedIndex += 1;
                    ScMonth.SelectedIndex = 0;
                }
                else
                {
                    ScMonth.SelectedIndex += 1;
                }
            }
            catch
            {
            }
        }

        //点击【今天】按钮
        private void BtnTodayClick(object sender, DuiMouseEventArgs e)
        {
            /*
            for (int i = 0; i < CbYear.Items.Count; i++)
            {
                DuiLabel lbyear = (DuiLabel)CbYear.Items[i];
                if ((int)lbyear.Tag == DateTime.Now.Year)
                {
                    CbYear.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < CbMonth.Items.Count; i++)
            {
                DuiLabel lbmonth = (DuiLabel)CbMonth.Items[i];
                if ((int)lbmonth.Tag == DateTime.Now.Month)
                {
                    CbMonth.SelectedIndex = i;
                    break;
                }
            }
             */
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


        private void TrackOpacity_ValueChanged(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb((int)(255 * TrackOpacity.Value), Color.White);
        }

        #region 拖动无边框窗体
        /// <summary>
        /// 窗体鼠标按下事件（移动窗体）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveFormMouseDown(object sender, MouseEventArgs e)
        {
            /*
            if (Frmdaysinfo != null)
            {
                Frmdaysinfo.Hide();
            }
            */
            if (e.Button == MouseButtons.Left)
            {
                LayeredSkin.NativeMethods.MouseToMoveControl(this.Handle);
            }
        }
        #endregion
    }
}