using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LayeredSkin.DirectUI;
using LayeredSkin.Controls;
using System.Drawing.Text;
using MRGSchedule.Properties;

namespace MRGSchedule
{
    public partial class FrmSetting : LayeredBaseForm
    {
        public FrmSetting()
        {
            InitializeComponent();

            InitUI();
        }

        private void InitUI()
        {
            //添加基础UI
            LayeredBaseControl settingPage = new LayeredBaseControl();
            settingPage.Size = new Size(this.Width, this.Height);
            settingPage.BackColor = Color.Transparent;
            settingPage.MouseDown += MoveFormMouseDown;
            this.Controls.Add(settingPage);

            //标题文本
            DuiLabel titleLabel = new DuiLabel();
            titleLabel.Text = "设置";
            titleLabel.Location = new Point(10, 10);
            titleLabel.Font = FrmMain.font2;
            titleLabel.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
            titleLabel.ForeColor = Color.DodgerBlue;
            titleLabel.MouseDown += MoveFormMouseDown;
            settingPage.DUIControls.Add(titleLabel);

            #region 当前周数选择
            DuiLabel todLabel = new DuiLabel();
            todLabel.Text = "设置当前周数:";
            todLabel.Location = new Point(10, 50);
            todLabel.Font = FrmMain.font1;
            todLabel.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
            todLabel.ForeColor = Color.DodgerBlue;
            settingPage.DUIControls.Add(todLabel);

            DuiComboBox ScWeek = new DuiComboBox();
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

            DuiCheckBox bootCheck = new DuiCheckBox();
            bootCheck.Location = new Point(120, 90);
            bootCheck.CheckRectWidth = 77;
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
