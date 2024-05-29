namespace _01
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.btnCreate1 = new System.Windows.Forms.Button();
			this.txtBoxProb1 = new System.Windows.Forms.TextBox();
			this.pctBoxMain = new System.Windows.Forms.PictureBox();
			this.txtBoxProb2 = new System.Windows.Forms.TextBox();
			this.lbProb1 = new System.Windows.Forms.Label();
			this.lbProb2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pctBoxMain)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCreate1
			// 
			this.btnCreate1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.btnCreate1.Location = new System.Drawing.Point(448, 867);
			this.btnCreate1.Name = "btnCreate1";
			this.btnCreate1.Size = new System.Drawing.Size(160, 50);
			this.btnCreate1.TabIndex = 0;
			this.btnCreate1.Text = "Create";
			this.btnCreate1.UseVisualStyleBackColor = true;
			this.btnCreate1.Click += new System.EventHandler(this.btnCreateClick);
			// 
			// txtBoxProb1
			// 
			this.txtBoxProb1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.txtBoxProb1.Location = new System.Drawing.Point(251, 864);
			this.txtBoxProb1.Name = "txtBoxProb1";
			this.txtBoxProb1.Size = new System.Drawing.Size(160, 43);
			this.txtBoxProb1.TabIndex = 2;
			// 
			// pctBoxMain
			// 
			this.pctBoxMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.pctBoxMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.pctBoxMain.InitialImage = null;
			this.pctBoxMain.Location = new System.Drawing.Point(0, 0);
			this.pctBoxMain.Name = "pctBoxMain";
			this.pctBoxMain.Size = new System.Drawing.Size(1902, 858);
			this.pctBoxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pctBoxMain.TabIndex = 6;
			this.pctBoxMain.TabStop = false;
			// 
			// txtBoxProb2
			// 
			this.txtBoxProb2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.txtBoxProb2.Location = new System.Drawing.Point(251, 929);
			this.txtBoxProb2.Name = "txtBoxProb2";
			this.txtBoxProb2.Size = new System.Drawing.Size(160, 43);
			this.txtBoxProb2.TabIndex = 12;
			// 
			// lbProb1
			// 
			this.lbProb1.AutoSize = true;
			this.lbProb1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lbProb1.Location = new System.Drawing.Point(18, 867);
			this.lbProb1.Name = "lbProb1";
			this.lbProb1.Size = new System.Drawing.Size(172, 38);
			this.lbProb1.TabIndex = 4;
			this.lbProb1.Text = "Probability 1";
			// 
			// lbProb2
			// 
			this.lbProb2.AutoSize = true;
			this.lbProb2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lbProb2.Location = new System.Drawing.Point(18, 934);
			this.lbProb2.Name = "lbProb2";
			this.lbProb2.Size = new System.Drawing.Size(172, 38);
			this.lbProb2.TabIndex = 13;
			this.lbProb2.Text = "Probability 2";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.ClientSize = new System.Drawing.Size(1902, 1033);
			this.Controls.Add(this.lbProb2);
			this.Controls.Add(this.txtBoxProb2);
			this.Controls.Add(this.pctBoxMain);
			this.Controls.Add(this.lbProb1);
			this.Controls.Add(this.txtBoxProb1);
			this.Controls.Add(this.btnCreate1);
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(1918, 1028);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "_01";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pctBoxMain)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Button btnCreate1;
		private TextBox txtBoxProb1;
		private PictureBox pctBoxMain;
		private TextBox txtBoxProb2;
		private Label lbProb1;
		private Label lbProb2;
	}
}