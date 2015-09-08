﻿namespace MRGSchedule
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.TrackOpacity = new LayeredSkin.Controls.LayeredTrackBar();
            this.DataSelectControl = new LayeredSkin.Controls.LayeredBaseControl();
            this.MonthBaseControl = new LayeredSkin.Controls.LayeredBaseControl();
            this.SuspendLayout();
            // 
            // TrackOpacity
            // 
            this.TrackOpacity.AdaptImage = true;
            this.TrackOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackOpacity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.TrackOpacity.BackImage = null;
            this.TrackOpacity.BackLineColor = System.Drawing.Color.Transparent;
            this.TrackOpacity.Borders.BottomColor = System.Drawing.Color.Empty;
            this.TrackOpacity.Borders.BottomWidth = 1;
            this.TrackOpacity.Borders.LeftColor = System.Drawing.Color.Empty;
            this.TrackOpacity.Borders.LeftWidth = 1;
            this.TrackOpacity.Borders.RightColor = System.Drawing.Color.Empty;
            this.TrackOpacity.Borders.RightWidth = 1;
            this.TrackOpacity.Borders.TopColor = System.Drawing.Color.Empty;
            this.TrackOpacity.Borders.TopWidth = 1;
            this.TrackOpacity.Canvas = ((System.Drawing.Bitmap)(resources.GetObject("TrackOpacity.Canvas")));
            this.TrackOpacity.ControlRectangle = new System.Drawing.Rectangle(5, 5, 280, 6);
            this.TrackOpacity.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TrackOpacity.LineWidth = 2;
            this.TrackOpacity.Location = new System.Drawing.Point(2, 43);
            this.TrackOpacity.MouseCanControl = true;
            this.TrackOpacity.Name = "TrackOpacity";
            this.TrackOpacity.Orientation = LayeredSkin.Controls.Orientations.Horizontal;
            this.TrackOpacity.PointImage = null;
            this.TrackOpacity.PointImageHover = null;
            this.TrackOpacity.PointImagePressed = null;
            this.TrackOpacity.PointState = LayeredSkin.Controls.ControlStates.Normal;
            this.TrackOpacity.Size = new System.Drawing.Size(290, 16);
            this.TrackOpacity.SurfaceImage = null;
            this.TrackOpacity.SurfaceLineColor = System.Drawing.Color.DodgerBlue;
            this.TrackOpacity.TabIndex = 6;
            this.TrackOpacity.Text = "layeredTrackBar1";
            this.TrackOpacity.Value = 0.5D;
            this.TrackOpacity.ValueChanged += new System.EventHandler(this.TrackOpacity_ValueChanged);
            // 
            // DataSelectControl
            // 
            this.DataSelectControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.DataSelectControl.Borders.BottomColor = System.Drawing.Color.Empty;
            this.DataSelectControl.Borders.BottomWidth = 1;
            this.DataSelectControl.Borders.LeftColor = System.Drawing.Color.Empty;
            this.DataSelectControl.Borders.LeftWidth = 1;
            this.DataSelectControl.Borders.RightColor = System.Drawing.Color.Empty;
            this.DataSelectControl.Borders.RightWidth = 1;
            this.DataSelectControl.Borders.TopColor = System.Drawing.Color.Empty;
            this.DataSelectControl.Borders.TopWidth = 1;
            this.DataSelectControl.Canvas = ((System.Drawing.Bitmap)(resources.GetObject("DataSelectControl.Canvas")));
            this.DataSelectControl.Location = new System.Drawing.Point(12, 12);
            this.DataSelectControl.Name = "DataSelectControl";
            this.DataSelectControl.Size = new System.Drawing.Size(280, 25);
            this.DataSelectControl.TabIndex = 3;
            this.DataSelectControl.Text = "layeredBaseControl2";
            // 
            // MonthBaseControl
            // 
            this.MonthBaseControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MonthBaseControl.Borders.BottomColor = System.Drawing.Color.Empty;
            this.MonthBaseControl.Borders.BottomWidth = 1;
            this.MonthBaseControl.Borders.LeftColor = System.Drawing.Color.Empty;
            this.MonthBaseControl.Borders.LeftWidth = 1;
            this.MonthBaseControl.Borders.RightColor = System.Drawing.Color.Empty;
            this.MonthBaseControl.Borders.RightWidth = 1;
            this.MonthBaseControl.Borders.TopColor = System.Drawing.Color.Empty;
            this.MonthBaseControl.Borders.TopWidth = 1;
            this.MonthBaseControl.Canvas = ((System.Drawing.Bitmap)(resources.GetObject("MonthBaseControl.Canvas")));
            this.MonthBaseControl.Location = new System.Drawing.Point(23, 91);
            this.MonthBaseControl.Name = "MonthBaseControl";
            this.MonthBaseControl.Size = new System.Drawing.Size(247, 157);
            this.MonthBaseControl.TabIndex = 2;
            this.MonthBaseControl.Text = "layeredBaseControl1";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.ClientSize = new System.Drawing.Size(304, 302);
            this.Controls.Add(this.TrackOpacity);
            this.Controls.Add(this.DataSelectControl);
            this.Controls.Add(this.MonthBaseControl);
            this.Name = "FrmMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.ResumeLayout(false);

        }

        #endregion

        private LayeredSkin.Controls.LayeredBaseControl MonthBaseControl;
        private LayeredSkin.Controls.LayeredBaseControl DataSelectControl;
        private LayeredSkin.Controls.LayeredTrackBar TrackOpacity;
    }
}

