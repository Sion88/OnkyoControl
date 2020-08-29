namespace Onkyo
{
    partial class SoundBalance
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
            this.LeftBtn = new System.Windows.Forms.Button();
            this.RightBtn = new System.Windows.Forms.Button();
            this.AllBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LeftBtn
            // 
            this.LeftBtn.Font = new System.Drawing.Font("新細明體", 18F);
            this.LeftBtn.Location = new System.Drawing.Point(12, 12);
            this.LeftBtn.Name = "LeftBtn";
            this.LeftBtn.Size = new System.Drawing.Size(144, 61);
            this.LeftBtn.TabIndex = 1;
            this.LeftBtn.Text = "Left";
            this.LeftBtn.UseVisualStyleBackColor = true;
            this.LeftBtn.Click += new System.EventHandler(this.LeftBtn_Click);
            // 
            // RightBtn
            // 
            this.RightBtn.Font = new System.Drawing.Font("新細明體", 18F);
            this.RightBtn.Location = new System.Drawing.Point(159, 12);
            this.RightBtn.Name = "RightBtn";
            this.RightBtn.Size = new System.Drawing.Size(144, 61);
            this.RightBtn.TabIndex = 2;
            this.RightBtn.Text = "Right";
            this.RightBtn.UseVisualStyleBackColor = true;
            this.RightBtn.Click += new System.EventHandler(this.RightBtn_Click);
            // 
            // AllBtn
            // 
            this.AllBtn.Font = new System.Drawing.Font("新細明體", 18F);
            this.AllBtn.Location = new System.Drawing.Point(12, 79);
            this.AllBtn.Name = "AllBtn";
            this.AllBtn.Size = new System.Drawing.Size(291, 66);
            this.AllBtn.TabIndex = 3;
            this.AllBtn.Text = "All";
            this.AllBtn.UseVisualStyleBackColor = true;
            this.AllBtn.Click += new System.EventHandler(this.AllBtn_Click);
            // 
            // SoundBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 157);
            this.Controls.Add(this.AllBtn);
            this.Controls.Add(this.RightBtn);
            this.Controls.Add(this.LeftBtn);
            this.Name = "SoundBalance";
            this.Text = "SoundBalance";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LeftBtn;
        private System.Windows.Forms.Button RightBtn;
        private System.Windows.Forms.Button AllBtn;
    }
}