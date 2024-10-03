namespace wfaAnchor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.Location = new Point(122, 137);
            button1.Name = "button1";
            button1.Size = new Size(178, 57);
            button1.TabIndex = 0;
            button1.Text = "Печать *";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = Properties.Resources.flud;
            pictureBox1.Location = new Point(513, 76);
            pictureBox1.MinimumSize = new Size(275, 284);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(322, 284);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightCoral;
            panel1.Controls.Add(textBox2);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 451);
            panel1.Name = "panel1";
            panel1.Size = new Size(847, 67);
            panel1.TabIndex = 2;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(23, 15);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(416, 31);
            textBox2.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(122, 76);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(178, 31);
            textBox1.TabIndex = 3;
            // 
            // button2
            // 
            button2.BackColor = Color.Transparent;
            button2.Location = new Point(122, 223);
            button2.Name = "button2";
            button2.Size = new Size(178, 57);
            button2.TabIndex = 4;
            button2.Text = "Печать $";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(192, 255, 192);
            ClientSize = new Size(847, 518);
            Controls.Add(button2);
            Controls.Add(textBox1);
            Controls.Add(panel1);
            Controls.Add(pictureBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private PictureBox pictureBox1;
        private Panel panel1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button2;
    }
}
