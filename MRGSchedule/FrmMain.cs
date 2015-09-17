using LayeredSkin.DirectUI;
using MRGSchedule.Properties;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
        public DuiCheckBox ScSeason;
        public DuiComboBox ScWeek;
        public DuiLabel DateTimeNow;
        public FrmSetting settingFrm;
        public string dataRootPath = Path.Combine(Application.StartupPath, "scheduleData");
        public string iniPath = Path.Combine(Application.StartupPath, "config.ini");

        public int clockHandle;

        public FrmMain()
        {
            InitializeComponent();
            try
            {
                //创建数据目录
                if (!Directory.Exists(dataRootPath))
                {
                    Directory.CreateDirectory(dataRootPath);
                }


                //设置窗体
                this.BackColor = Color.FromArgb(255 / 2, Color.White);
                ScheduleBaseControl.Location = new Point(10, 55);
                ScheduleBaseControl.Width = 7 * 70;
                ScheduleBaseControl.Height = 5 * 90 + 60;
                this.Width = 7 * 70 + 20;
                this.Height = ScheduleBaseControl.Height + 60;
                this.FormClosing += FrmMain_FormClosing;

                Schedule sc = new Schedule();



                CreatDataSelectControl();
                /*
                GetClockHandle();//获取托盘时钟的句柄
                
                //HookStart();//开始hook
                */

                //设置初始数据
                if (File.Exists(iniPath))
                {
                    try
                    {
                        string yearIndex = INI.GetIniContentValue("config", "yearIndex", iniPath);
                        ScYear.SelectedIndex = yearIndex != "" ? Convert.ToInt32(yearIndex) : ScYear.Controls.Count - 1;

                        string lastWeekIndex = INI.GetIniContentValue("config", "weekIndex", iniPath);
                        string currentWeekIndex = INI.GetIniContentValue("setting", "currentWeekIndex", iniPath);
                        if (currentWeekIndex == "")
                        {
                            //如果用户没设置当前周
                            ScWeek.SelectedIndex = lastWeekIndex != "" ? Convert.ToInt32(lastWeekIndex) : 0;
                        }
                        else
                        {
                            //如果用户设置了当前周，计算后得出现在的当前周
                            string currentWeekSetTime = INI.GetIniContentValue("setting", "currentWeekSetTime", iniPath);
                            DateTime setTime = Convert.ToDateTime(currentWeekSetTime);
                            int i = setTime.DayOfWeek - DayOfWeek.Monday;
                            if(i == -1)
                                i = 6;//处理周日
                            TimeSpan ts = new TimeSpan(i,0,0,0);
                            DateTime mondayDate = setTime.Subtract(ts);//该周周一时间
                            double dayInterval = (DateTime.Now - mondayDate).TotalDays;
                            int realWeekIndex = (int)(dayInterval / (double)7) + Convert.ToInt32(currentWeekIndex);
                            if (realWeekIndex >= 20) realWeekIndex = 20;//处理超值
                            ScWeek.SelectedIndex = realWeekIndex;
                        }

                        string seasonIndex = INI.GetIniContentValue("config", "seasonIndex", iniPath);
                        ScSeason.Checked = seasonIndex != "" ? Convert.ToBoolean(seasonIndex) : false;
                    }
                    catch { }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                INI.WritePrivateProfileString("config", "yearIndex", ScYear.SelectedIndex.ToString(), iniPath);
                INI.WritePrivateProfileString("config", "weekIndex", ScWeek.SelectedIndex.ToString(), iniPath);
                INI.WritePrivateProfileString("config", "seasonIndex", ScSeason.Checked.ToString(), iniPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //创建菜单栏控件
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
            DuiButton bttoday = new DuiButton();
            bttoday.Location = new Point(15, 0);
            bttoday.NormalImage = Resources.BtnTodayn;
            bttoday.HoverImage = Resources.BtnTodaye;
            bttoday.PressedImage = Resources.BtnTodayd;
            bttoday.MouseClick += BtnTodayClick;
            DataSelectControl.DUIControls.Add(bttoday);
            #endregion

            #region 添加春秋季标识
            ScSeason = new DuiCheckBox();
            ScSeason.Location = new Point(65, 12);
            ScSeason.CheckRectWidth = 20;
            ScSeason.UncheckedNormal = Resources.Autumn;//未选中为秋，选中为春。
            ScSeason.CheckedNormal = Resources.Spring;
            ScSeason.CheckedChanged += CBSelectedIndexChanged;
            DataSelectControl.DUIControls.Add(ScSeason);
            #endregion

            #region 添加年份列表
            int latestYear = DateTime.Now.Year;
            ScYear = new DuiComboBox();
            ScYear.BaseColor = Color.White;
            ScYear.BackColor = Color.White;
            ScYear.Size = new Size(85, 25);
            ScYear.Location = new Point(90, 10);
            ScYear.SelectedIndexChanged += CBSelectedIndexChanged;
            for (int i = 1995; i <= latestYear; i++)
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
                    ScYear.InnerListBox.Value = (double)i / (double)(latestYear);
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
            btbefore.Location = new Point(185, 12);
            btbefore.NormalImage = Resources.before_n;
            btbefore.HoverImage = Resources.before_e;
            btbefore.PressedImage = Resources.before_d;
            btbefore.MouseClick += BtnBeforClick;
            DataSelectControl.DUIControls.Add(btbefore);
            #endregion

            #region 添加周数列表
            ScWeek = new DuiComboBox();
            ScWeek.BaseColor = Color.White;
            ScWeek.BackColor = Color.White;
            ScWeek.Size = new Size(65, 25);
            ScWeek.Location = new Point(210, 10);
            ScWeek.SelectedIndexChanged += CBSelectedIndexChanged;
            for (int i = 1; i <= 20; i++)
            {
                DuiLabel lb = new DuiLabel();
                lb.Text = string.Format("第{0}周", i.ToString());
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
                ScWeek.Items.Add(lb);
                if (i == DateTime.Now.Month)
                {
                    ScWeek.SelectedIndex = i - 1;
                    ScWeek.InnerListBox.Value = (double)i / (double)(12);
                }
            }
            ScWeek.ShowBorder = false;
            ScWeek.InnerListBox.Height = 200;
            ScWeek.InnerListBox.Width = 65;
            ScWeek.InnerListBox.RefreshList();
            DataSelectControl.DUIControls.Add(ScWeek);
            #endregion

            #region 添加下一月按钮
            DuiButton btnext = new DuiButton();
            btnext.Location = new Point(280, 12);
            btnext.NormalImage = Resources.next_n;
            btnext.HoverImage = Resources.next_e;
            btnext.PressedImage = Resources.next_d;
            btnext.MouseClick += BtnNextClick;
            DataSelectControl.DUIControls.Add(btnext);
            #endregion

            #region 添加实时时间
            DateTimeNow = new DuiLabel();
            DateTimeNow.Text = DateTime.Now.ToString("HH:mm:ss");
            DateTimeNow.Font = new Font("微软雅黑", 18);
            DateTimeNow.TextAlign = ContentAlignment.MiddleLeft;
            DateTimeNow.ForeColor = Color.DodgerBlue;
            DateTimeNow.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
            DateTimeNow.Size = new Size(110, 20);
            DateTimeNow.Location = new Point(315, 12);
            DataSelectControl.DUIControls.Add(DateTimeNow);
            Timer dateTimer = new Timer();//添加计时器
            dateTimer.Tick += (sender, e) => { DateTimeNow.Text = DateTime.Now.ToString("HH:mm:ss"); };
            dateTimer.Interval = 1000;
            dateTimer.Start();
            #endregion

            #region 添加设置按钮
            DuiButton btSetting = new DuiButton();
            btSetting.NormalImage = Resources.SettingN;
            btSetting.HoverImage = Resources.SettingE;
            btSetting.PressedImage = Resources.SettingD;
            btSetting.Location = new Point(DataSelectControl.Width - 70, 8);
            btSetting.MouseClick += OpenSettingFrm;
            DataSelectControl.DUIControls.Add(btSetting);
            #endregion

            #region 添加关闭按钮
            DuiButton btClose = new DuiButton();
            btClose.NormalImage = Resources.CloseN;
            btClose.HoverImage = Resources.CloseE;
            btClose.PressedImage = Resources.CloseD;
            btClose.Location = new Point(DataSelectControl.Width - 35, 8);
            btClose.MouseClick += (sender, e) => { this.Close(); };
            DataSelectControl.DUIControls.Add(btClose);
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
                RefrshSchedule();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 更新日历
        /// </summary>
        public void RefrshSchedule()
        {
            DuiLabel lbmonth = (DuiLabel)ScWeek.Items[ScWeek.SelectedIndex];
            DuiLabel lbyear = (DuiLabel)ScYear.Items[ScYear.SelectedIndex];

            //清除控件
            ScheduleBaseControl.DUIControls.Clear();

            DuiBaseControl weekTitle = new DuiBaseControl();
            weekTitle.BackgroundImage = Resources.WeekTitle;
            weekTitle.BackColor = Color.Transparent;
            weekTitle.Size = new Size(490, 50);
            weekTitle.BackgroundImageLayout = ImageLayout.Center;
            weekTitle.Location = new Point(0, 0);
            ScheduleBaseControl.DUIControls.Add(weekTitle);

            //获取日期数据
            DuiLabel year = (DuiLabel)this.ScYear.Items[ScYear.SelectedIndex];
            DuiLabel week = (DuiLabel)this.ScWeek.Items[ScWeek.SelectedIndex];
            bool season = this.ScSeason.Checked;
            UpdateScheduleData(string.Format("{0}{1}", year.Tag.ToString(), Convert.ToInt32(season).ToString("00")), Convert.ToInt32(week.Tag));//更新课程表信息
        }

        /// <summary>
        /// 更新课程表信息
        /// </summary>
        /// <param name="scheduleDate">请求的课程表名，格式为Schedule+年份+春秋级(秋季01春季02)(e.g.201501)</param>
        private void UpdateScheduleData(string scheduleDate, int weekNum)
        {
            string filePath = dataRootPath + "\\Schedule" + scheduleDate + ".sch";

            if (!File.Exists(filePath))
            {
                //如果不存在数据文件。显示导入按钮
                DuiButton button = new DuiButton();
                button.Text = "导入课程表文件";
                button.Size = new Size(100, 40);
                button.Location = new Point(ScheduleBaseControl.Width / 2 - 50, ScheduleBaseControl.Height / 2 - 20);
                button.MouseClick += (sender, e) =>
                {
                    OpenFileDialog fd = new OpenFileDialog();
                    fd.Filter = "课程表文件(*.doc)|*.doc|课程表数据文件(*.sch)|*.sch|All files (*.*)|*.*"; //过滤文件类型
                    fd.InitialDirectory = Path.Combine(Application.StartupPath, "scheduleData");//设定初始目录
                    fd.ShowReadOnly = true; //设定文件是否只读
                    DialogResult r = fd.ShowDialog();
                    if (r == DialogResult.OK)
                    {
                        try
                        {
                            string fileName = fd.FileName;
                            string extensionName = Path.GetExtension(fileName);
                            StreamReader sr = new StreamReader(fileName);//读取数据流
                            if (extensionName == ".sch")
                            {
                                //课程表数据文件-拷贝并导入
                                Schedule sch = Schedule.Deserialize(sr.BaseStream);
                                File.Copy(fileName, filePath);
                                UpdateScheduleData(sch, weekNum);//更新UI数据
                            }
                            else
                            {
                                //课程表文件
                                Schedule sch = new Schedule();
                                sch.ImportSchedule(fileName);//导入数据
                                Schedule.Serialize(sch, filePath);//保存数据

                                UpdateScheduleData(sch, weekNum);//更新UI数据
                            }

                            sr.Close();

                            ScheduleBaseControl.DUIControls.Remove(button);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("导入数据失败请重试");
                        }
                    }
                };
                ScheduleBaseControl.DUIControls.Add(button);
            }
            else
            {
                //有数据文件
                StreamReader sr = new StreamReader(filePath);//读取数据流
                Schedule sch = Schedule.Deserialize(sr.BaseStream);
                sr.Close();
                UpdateScheduleData(sch, weekNum);
            }
        }
        private void UpdateScheduleData(Schedule schedule, int selectWeekNum)
        {
            for (int weekNum = 0; weekNum <= 6; weekNum++)
            {
                for (int classNum = 1; classNum <= 5; classNum++)
                {
                    LessonInfo info = null;
                    foreach (Lesson lesson in schedule.GetLessonList())
                    {
                        if (lesson.weekNum == (WeekDay)weekNum && lesson.classNum == classNum)
                        {
                            info = lesson.lessonInfo;
                            break;
                        }
                    }

                    //项目基础框
                    DuiBaseControl baseControl = new DuiBaseControl();
                    baseControl.Size = new Size(70, 90);
                    baseControl.Location = new Point(weekNum * 70, (classNum - 1) * 90 + 55);//位置
                    baseControl.BackColor = (weekNum + classNum) % 2 == 0 ? Color.FromArgb(30, Color.Gainsboro) : Color.FromArgb(10, Color.Black);//背景色（间隔）
                    baseControl.MouseEnter += LessonItemsMoveEnter;
                    baseControl.MouseLeave += LessonItemsMoveLeave;
                    baseControl.MouseClick += LessonItemsMoveClick;

                    if (info != null)
                    {
                        baseControl.Tag = info;

                        DuiLabel lb = new DuiLabel();
                        lb.Text = info.lessonName;
                        lb.Font = font1;
                        lb.Location = new Point(0, 10);
                        lb.Size = new Size(70, 50);
                        if (weekNum == (int)WeekDay.星期日 || weekNum == (int)WeekDay.星期六)
                        {
                            //假期变黄
                            lb.ForeColor = Color.DarkOrange;
                        }
                        if (info.lessonStartWeek > selectWeekNum || info.lessonOverWeek < selectWeekNum)
                        {
                            //未开课|结课为灰度
                            lb.ForeColor = Color.Gray;
                        }
                        lb.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
                        lb.TextAlign = ContentAlignment.TopCenter;
                        baseControl.Controls.Add(lb);


                        lb = new DuiLabel();
                        lb.Text = info.lessonSite;
                        lb.Font = font1;
                        lb.Location = new Point(0, 60);
                        lb.Size = new Size(70, 25);
                        if (weekNum == (int)WeekDay.星期日 || weekNum == (int)WeekDay.星期六)
                        {
                            //假期变黄
                            lb.ForeColor = Color.DarkOrange;
                        }
                        if (info.lessonStartWeek > selectWeekNum || info.lessonOverWeek < selectWeekNum)
                        {
                            //未开课|结课为灰度
                            lb.ForeColor = Color.Gray;
                        }
                        lb.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
                        lb.TextAlign = ContentAlignment.MiddleCenter;
                        baseControl.Controls.Add(lb);
                    }

                    ScheduleBaseControl.DUIControls.Add(baseControl);
                }
            }
        }
        #endregion


        /// <summary>
        /// 上个月
        /// </summary>
        private void BtnBeforClick(object sender, EventArgs e)
        {
            if (ScWeek.SelectedIndex == 0)
            {
                ScYear.SelectedIndex -= 1;
                ScWeek.SelectedIndex = 11;
            }
            else
            {
                ScWeek.SelectedIndex -= 1;
            }
        }
        /// <summary>
        /// 下个月
        /// </summary>
        private void BtnNextClick(object sender, EventArgs e)
        {
            try
            {
                if (ScWeek.SelectedIndex == 11)
                {
                    ScYear.SelectedIndex += 1;
                    ScWeek.SelectedIndex = 0;
                }
                else
                {
                    ScWeek.SelectedIndex += 1;
                }
            }
            catch
            {
            }
        }

        //点击【今天】按钮
        private void BtnTodayClick(object sender, DuiMouseEventArgs e)
        {
            string currentWeek = INI.GetIniContentValue("setting", "currentWeek", iniPath);
            if (currentWeek == "")
            {
                DialogResult result = MessageBox.Show("尚未设定当前周，请在设置页面中设置后选择");
                if (result == DialogResult.OK)
                {
                    this.OpenSettingFrm(null, null);
                }
            }
            else
            {
                ScWeek.SelectedIndex = Convert.ToInt32(currentWeek);
            }
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

        #region 课程表项目事件
        /// <summary>
        /// 鼠标进入用户控件时触发事件
        /// </summary>
        private void LessonItemsMoveEnter(object sender, EventArgs e)
        {
            ((DuiBaseControl)sender).Borders.LeftWidth =
                ((DuiBaseControl)sender).Borders.RightWidth =
                    ((DuiBaseControl)sender).Borders.TopWidth = ((DuiBaseControl)sender).Borders.BottomWidth = 1;
            ((DuiBaseControl)sender).Borders.LeftColor =
                ((DuiBaseControl)sender).Borders.RightColor =
                    ((DuiBaseControl)sender).Borders.TopColor = ((DuiBaseControl)sender).Borders.BottomColor = Color.FromArgb(160, 208, 240);
        }
        /// <summary>
        /// 鼠标离开用户控件时触发事件
        /// </summary>
        private void LessonItemsMoveLeave(object sender, EventArgs e)
        {
            ((DuiBaseControl)sender).Borders.LeftWidth =
                ((DuiBaseControl)sender).Borders.RightWidth =
                    ((DuiBaseControl)sender).Borders.TopWidth = ((DuiBaseControl)sender).Borders.BottomWidth = 0;
        }

        /// <summary>
        /// 鼠标点击用户控件时触发事件
        /// </summary>
        private void LessonItemsMoveClick(object sender, EventArgs e)
        {
            try
            {
                LessonInfo info = (LessonInfo)((DuiBaseControl)sender).Tag;

                MessageBox.Show(info.lessonName);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        private void OpenSettingFrm(object sender, EventArgs e)
        {
            if (settingFrm == null || settingFrm.IsDisposed == true)
                settingFrm = new FrmSetting(this);
            settingFrm.Show();
        }
    }
}
