namespace Kohonen
{
    partial class Form1
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.menuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsStart = new System.Windows.Forms.ToolStripMenuItem();
			this.tsRepeat = new System.Windows.Forms.ToolStripMenuItem();
			this.tsStop = new System.Windows.Forms.ToolStripMenuItem();
			this.tsSave = new System.Windows.Forms.ToolStripMenuItem();
			this.saveFile = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.White;
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(384, 361);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStart,
            this.tsRepeat,
            this.tsStop,
            this.tsSave});
			this.menuStrip.Name = "contextMenuStrip1";
			this.menuStrip.Size = new System.Drawing.Size(136, 92);
			// 
			// tsStart
			// 
			this.tsStart.Name = "tsStart";
			this.tsStart.Size = new System.Drawing.Size(135, 22);
			this.tsStart.Text = "Начать";
			this.tsStart.Click += new System.EventHandler(this.tsStartClick);
			// 
			// tsRepeat
			// 
			this.tsRepeat.Name = "tsRepeat";
			this.tsRepeat.Size = new System.Drawing.Size(135, 22);
			this.tsRepeat.Text = "Повтор";
			this.tsRepeat.Click += new System.EventHandler(this.tsRepeatClick);
			// 
			// tsStop
			// 
			this.tsStop.Name = "tsStop";
			this.tsStop.Size = new System.Drawing.Size(135, 22);
			this.tsStop.Text = "Остановить";
			this.tsStop.Click += new System.EventHandler(this.tsStopClick);
			// 
			// tsSave
			// 
			this.tsSave.Name = "tsSave";
			this.tsSave.Size = new System.Drawing.Size(135, 22);
			this.tsSave.Text = "Сохранить";
			this.tsSave.Click += new System.EventHandler(this.tsSaveClick);
			// 
			// saveFile
			// 
			this.saveFile.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileOk);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 361);
			this.Controls.Add(this.pictureBox1);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Сеть Кохонена";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.menuStrip.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsStart;
        private System.Windows.Forms.ToolStripMenuItem tsRepeat;
        private System.Windows.Forms.ToolStripMenuItem tsStop;
        private System.Windows.Forms.ToolStripMenuItem tsSave;
        private System.Windows.Forms.SaveFileDialog saveFile;
    }
}

