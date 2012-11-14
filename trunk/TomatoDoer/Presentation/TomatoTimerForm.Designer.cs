namespace TomatoDoer
{
	partial class TomatoTimerForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TomatoTimerForm));
			this.StartOrSquashButton = new System.Windows.Forms.Button();
			this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sendToEvernoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.analyzeLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.timerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.smallDurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mediumDurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.largeDurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
			this.ContinueButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// StartOrSquashButton
			// 
			this.StartOrSquashButton.Location = new System.Drawing.Point(173, 27);
			this.StartOrSquashButton.Name = "StartOrSquashButton";
			this.StartOrSquashButton.Size = new System.Drawing.Size(89, 44);
			this.StartOrSquashButton.TabIndex = 0;
			this.StartOrSquashButton.Text = "Start";
			this.StartOrSquashButton.UseVisualStyleBackColor = true;
			// 
			// maskedTextBox1
			// 
			this.maskedTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.maskedTextBox1.Location = new System.Drawing.Point(22, 27);
			this.maskedTextBox1.Mask = "00:00:00";
			this.maskedTextBox1.Name = "maskedTextBox1";
			this.maskedTextBox1.Size = new System.Drawing.Size(145, 44);
			this.maskedTextBox1.TabIndex = 2;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.timerToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(380, 24);
			this.menuStrip1.TabIndex = 7;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.analyzeLogToolStripMenuItem,
            this.quitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToEvernoteToolStripMenuItem});
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.exportToolStripMenuItem.Text = "Export";
			// 
			// sendToEvernoteToolStripMenuItem
			// 
			this.sendToEvernoteToolStripMenuItem.Name = "sendToEvernoteToolStripMenuItem";
			this.sendToEvernoteToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.sendToEvernoteToolStripMenuItem.Text = "Send to Evernote";
			this.sendToEvernoteToolStripMenuItem.Click += new System.EventHandler(this.sendToEvernoteToolStripMenuItem_Click);
			// 
			// analyzeLogToolStripMenuItem
			// 
			this.analyzeLogToolStripMenuItem.Name = "analyzeLogToolStripMenuItem";
			this.analyzeLogToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.analyzeLogToolStripMenuItem.Text = "Analyze log";
			this.analyzeLogToolStripMenuItem.Click += new System.EventHandler(this.CallAnalysisOfLog);
			// 
			// quitToolStripMenuItem
			// 
			this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			this.quitToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.quitToolStripMenuItem.Text = "Quit";
			this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
			// 
			// timerToolStripMenuItem
			// 
			this.timerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smallDurationToolStripMenuItem,
            this.mediumDurationToolStripMenuItem,
            this.largeDurationToolStripMenuItem});
			this.timerToolStripMenuItem.Name = "timerToolStripMenuItem";
			this.timerToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
			this.timerToolStripMenuItem.Text = "Duration";
			// 
			// smallDurationToolStripMenuItem
			// 
			this.smallDurationToolStripMenuItem.Name = "smallDurationToolStripMenuItem";
			this.smallDurationToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.smallDurationToolStripMenuItem.Text = "Small";
			this.smallDurationToolStripMenuItem.Click += new System.EventHandler(this.smallDurationToolStripMenuItem_Click);
			// 
			// mediumDurationToolStripMenuItem
			// 
			this.mediumDurationToolStripMenuItem.Name = "mediumDurationToolStripMenuItem";
			this.mediumDurationToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.mediumDurationToolStripMenuItem.Text = "Medium";
			this.mediumDurationToolStripMenuItem.Click += new System.EventHandler(this.mediumDurationToolStripMenuItem_Click);
			// 
			// largeDurationToolStripMenuItem
			// 
			this.largeDurationToolStripMenuItem.Name = "largeDurationToolStripMenuItem";
			this.largeDurationToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.largeDurationToolStripMenuItem.Text = "Large";
			this.largeDurationToolStripMenuItem.Click += new System.EventHandler(this.largeDurationToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// richTextBoxLog
			// 
			this.richTextBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBoxLog.Location = new System.Drawing.Point(22, 77);
			this.richTextBoxLog.Name = "richTextBoxLog";
			this.richTextBoxLog.Size = new System.Drawing.Size(335, 350);
			this.richTextBoxLog.TabIndex = 8;
			this.richTextBoxLog.Text = "";
			// 
			// ContinueButton
			// 
			this.ContinueButton.Location = new System.Drawing.Point(268, 27);
			this.ContinueButton.Name = "ContinueButton";
			this.ContinueButton.Size = new System.Drawing.Size(89, 44);
			this.ContinueButton.TabIndex = 9;
			this.ContinueButton.Text = "Continue";
			this.ContinueButton.UseVisualStyleBackColor = true;
			this.ContinueButton.Click += new System.EventHandler(this.buttonContinue_Click);
			// 
			// TomatoTimerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(380, 439);
			this.Controls.Add(this.ContinueButton);
			this.Controls.Add(this.richTextBoxLog);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.StartOrSquashButton);
			this.Controls.Add(this.maskedTextBox1);
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(396, 477);
			this.Name = "TomatoTimerForm";
			this.Text = "Tomato doer";
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
		private System.Windows.Forms.Button StartOrSquashButton;
		private System.Windows.Forms.MaskedTextBox maskedTextBox1;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem analyzeLogToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sendToEvernoteToolStripMenuItem;
		private System.Windows.Forms.RichTextBox richTextBoxLog;
		private System.Windows.Forms.ToolStripMenuItem timerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem smallDurationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mediumDurationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem largeDurationToolStripMenuItem;
		private System.Windows.Forms.Button ContinueButton;
	}
}
