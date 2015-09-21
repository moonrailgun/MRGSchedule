using System;
using System.Drawing;
using System.Windows.Forms;
using LayeredSkin.DirectUI;
using LayeredSkin.Controls;
using System.Drawing.Text;
using MRGSchedule.Properties;
using System.IO;
using Microsoft.Win32;

namespace MRGSchedule
{
    public partial class FrmSetting : LayeredBaseForm
    {
        private FrmMain mainFrmInstance;
        private string inipath;

        private DuiComboBox ScWeek;
        private DuiCheckBox bootCheck;

        private bool lastBootChecked;

        public FrmSetting(FrmMain mainFrm)
        {
            InitializeComponent();

            this.mainFrmInstance = mainFrm;
            this.inipath = mainFrmInstance.iniPath;

            InitUI();

            //从INI文件中获取数据
            if (File.Exists(inipath))
            {
                string currentWeekIndex = INI.GetIniContentValue("setting", "currentWeekIndex", inipath);
                int weekIndex = mainFrmInstance.ScWeek.SelectedIndex;
                ScWeek.SelectedIndex = currentWeekIndex != "" ? Convert.ToInt32(currentWeekIndex) : weekIndex;

                string isBoot = INI.GetIniContentValue("setting", "isBoot", inipath);
                bootCheck.Checked = lastBootChecked = isBoot != "" ? Convert.ToBoolean(isBoot) : false;
            }
        }

        private void InitUI()
        {
            //settingPageUI
            LayeredBaseControl settingPage = new LayeredBaseControl();
            settingPage.Size = new Size(this.Width, this.Height);
            settingPage.BackColor = Color.Transparent;
            this.Controls.Add(settingPage);

            #region 添加标题栏
            DuiBaseControl titleBar = new DuiBaseControl();
            titleBar.Size = new Size(settingPage.Width, 26);
            titleBar.BackColor = Color.Transparent;
            titleBar.MouseMove += MoveFormMouseDown;
            settingPage.DUIControls.Add(titleBar);

            DuiLabel titleLabel = new DuiLabel();
            titleLabel.Text = "设置";
            titleLabel.Location = new Point(10, 10);
            titleLabel.Font = FrmMain.font2;
            titleLabel.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
            titleLabel.ForeColor = Color.DodgerBlue;
            titleLabel.MouseDown += MoveFormMouseDown;
            settingPage.DUIControls.Add(titleLabel);

            //添加关闭按钮
            DuiButton btClose = new DuiButton();
            btClose.NormalImage = Resources.CloseN;
            btClose.HoverImage = Resources.CloseE;
            btClose.PressedImage = Resources.CloseD;
            btClose.Location = new Point(settingPage.Width - 40, 13);
            btClose.MouseClick += (sender, e) => { this.Close(); };
            settingPage.DUIControls.Add(btClose);
            #endregion

            //标题文本


            #region 当前周数选择
            DuiLabel todLabel = new DuiLabel();
            todLabel.Text = "设置当前周数:";
            todLabel.Location = new Point(10, 50);
            todLabel.Font = FrmMain.font1;
            todLabel.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
            todLabel.ForeColor = Color.DodgerBlue;
            settingPage.DUIControls.Add(todLabel);

            ScWeek = new DuiComboBox();
            ScWeek.BaseColor = Color.White;
            ScWeek.BackColor = Color.White;
            ScWeek.Size = new Size(65, 25);
            ScWeek.Location = new Point(120, 47);
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
                lb.MouseEnter += (sender, e) =>
                {
                    ((DuiLabel)sender).BackColor = Color.FromArgb(45, 151, 222);
                    ((DuiLabel)sender).ForeColor = Color.White;
                };
                lb.MouseLeave += (sender, e) =>
                {
                    ((DuiLabel)sender).BackColor = Color.White;
                    ((DuiLabel)sender).ForeColor = Color.DodgerBlue;
                };
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
            settingPage.DUIControls.Add(ScWeek);
            #endregion

            #region 是否开机自启动
            DuiLabel bootLabel = new DuiLabel();
            bootLabel.Text = "是否开机自启动:";
            bootLabel.Location = new Point(10, 90);
            bootLabel.Font = FrmMain.font1;
            bootLabel.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
            bootLabel.ForeColor = Color.DodgerBlue;
            settingPage.DUIControls.Add(bootLabel);

            bootCheck = new DuiCheckBox();
            bootCheck.Location = new Point(120, 88);
            bootCheck.CheckRectWidth = 62;
            bootCheck.CheckedNormal = Resources.CheckboxE;
            bootCheck.UncheckedNormal = Resources.CheckboxD;
            settingPage.DUIControls.Add(bootCheck);
            #endregion

            DuiButton confirmButton = new DuiButton();
            confirmButton.Text = "确认";
            confirmButton.Font = FrmMain.Msgfont;
            confirmButton.Size = new Size(150, 50);
            confirmButton.ForeColor = Color.White;
            confirmButton.BaseColor = Color.DodgerBlue;
            confirmButton.Location = new Point(this.Width / 2 - confirmButton.Width / 2, this.Height - 90);
            confirmButton.MouseClick += (sender, e) =>
            {
                //处理开机自启动
                if (bootCheck.Checked != lastBootChecked)
                {
                    try
                    {
                        //与之前不同
                        string path = Application.ExecutablePath;
                        RegistryKey rk = Registry.LocalMachine;
                        RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                        if (bootCheck.Checked)
                        {
                            //设置开机自启动
                            rk2.SetValue("SKR", path);
                        }
                        else
                        {
                            //取消开机自启动
                            rk2.DeleteValue("SKR", false);
                        }
                        rk2.Close();
                        rk.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("设置开机自启动失败，可能是权限不足：" + ex.ToString());
                        bootCheck.Checked = lastBootChecked;
                    }
                }

                //改写Index
                INI.WritePrivateProfileString("setting", "currentWeekIndex", ScWeek.SelectedIndex.ToString(), inipath);
                INI.WritePrivateProfileString("setting", "currentWeekSetTime", DateTime.Now.ToString("yyyy-MM-dd"), inipath);
                INI.WritePrivateProfileString("setting", "isBoot", bootCheck.Checked.ToString(), inipath);

                //改写主页面的index
                mainFrmInstance.ScWeek.SelectedIndex = ScWeek.SelectedIndex;


                //关闭页面
                this.Close();
            };
            settingPage.DUIControls.Add(confirmButton);
        }

        #region 拖动无边框窗体
        /// <summary>
        /// 窗体鼠标按下事件（移动窗体）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveFormMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LayeredSkin.NativeMethods.MouseToMoveControl(this.Handle);
            }
        }
        #endregion
    }
}
