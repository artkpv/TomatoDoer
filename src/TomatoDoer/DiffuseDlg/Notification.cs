using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Abhinaba.TransDlg;

namespace DiffuseDlgDemo
{
	public class Notification : TransDialog
	{
        #region Ctor, init code and dispose
		public Notification()
            : base(true)
		{
			InitializeComponent();
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
        #endregion // Ctor and init code

        #region Event handler
        private void Notification_Load(object sender, System.EventArgs e)
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.Left = screenWidth - this.Width;
            this.Top = screenHeight - this.Height;

            timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            this.Close();
        }

        #endregion // Event handler
        
        #region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 70);
            this.label1.TabIndex = 0;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Notification
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(234, 88);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TomatoDoer";
            this.Text = "TomatoDoer";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Notification_Load);
            this.ResumeLayout(false);

        }
		#endregion

        #region Designer generated variables

        private System.Windows.Forms.Timer timer1;
        private Label label1;
        private System.ComponentModel.IContainer components;
        #endregion

        public void SetMessage(string message)
        {
            label1.Text = message;
        }
	}
}
