using System;
using System.Drawing;

namespace IvyGameWinForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button StartNewGameButton;
        private System.Windows.Forms.ComboBox DifficultySelector;
        private System.Windows.Forms.Panel CollectionPanel;
        private System.Windows.Forms.Panel GamePanel;

        private void InitializeComponent()
        {
            this.StartNewGameButton = new System.Windows.Forms.Button();
            this.DifficultySelector = new System.Windows.Forms.ComboBox();
            this.GamePanel = new System.Windows.Forms.Panel();
            this.CollectionPanel = new System.Windows.Forms.Panel();
            this.buttonMinus = new System.Windows.Forms.Button();
            this.buttonPlus = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartNewGameButton
            // 
            this.StartNewGameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StartNewGameButton.BackColor = System.Drawing.Color.SandyBrown;
            this.StartNewGameButton.Location = new System.Drawing.Point(757, 12);
            this.StartNewGameButton.Name = "StartNewGameButton";
            this.StartNewGameButton.Size = new System.Drawing.Size(152, 36);
            this.StartNewGameButton.TabIndex = 0;
            this.StartNewGameButton.Text = "Начать игру";
            this.StartNewGameButton.UseVisualStyleBackColor = false;
            this.StartNewGameButton.Click += new System.EventHandler(this.StartNewGameButton_Click);
            // 
            // DifficultySelector
            // 
            this.DifficultySelector.BackColor = System.Drawing.Color.SandyBrown;
            this.DifficultySelector.ForeColor = System.Drawing.SystemColors.MenuText;
            this.DifficultySelector.Items.AddRange(new object[] {
            "Легкий",
            "Средний",
            "Сложный"});
            this.DifficultySelector.Location = new System.Drawing.Point(20, 20);
            this.DifficultySelector.Name = "DifficultySelector";
            this.DifficultySelector.Size = new System.Drawing.Size(150, 28);
            this.DifficultySelector.TabIndex = 1;
            this.DifficultySelector.SelectedIndexChanged += new System.EventHandler(this.DifficultySelector_SelectedIndexChanged);
            // 
            // GamePanel
            // 
            this.GamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GamePanel.BackColor = System.Drawing.Color.Linen;
            this.GamePanel.Location = new System.Drawing.Point(20, 60);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(889, 431);
            this.GamePanel.TabIndex = 3;
            // 
            // CollectionPanel
            // 
            this.CollectionPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CollectionPanel.AutoScroll = true;
            this.CollectionPanel.BackColor = System.Drawing.Color.Tan;
            this.CollectionPanel.Location = new System.Drawing.Point(218, 524);
            this.CollectionPanel.Name = "CollectionPanel";
            this.CollectionPanel.Size = new System.Drawing.Size(463, 83);
            this.CollectionPanel.TabIndex = 2;
            // 
            // buttonMinus
            // 
            this.buttonMinus.BackColor = System.Drawing.Color.LightCoral;
            this.buttonMinus.Location = new System.Drawing.Point(330, 12);
            this.buttonMinus.Name = "buttonMinus";
            this.buttonMinus.Size = new System.Drawing.Size(104, 36);
            this.buttonMinus.TabIndex = 4;
            this.buttonMinus.Text = "Меньше";
            this.buttonMinus.UseVisualStyleBackColor = false;
            this.buttonMinus.Click += new System.EventHandler(this.buttonMinus_Click);
            // 
            // buttonPlus
            // 
            this.buttonPlus.BackColor = System.Drawing.Color.LightGreen;
            this.buttonPlus.Location = new System.Drawing.Point(452, 12);
            this.buttonPlus.Name = "buttonPlus";
            this.buttonPlus.Size = new System.Drawing.Size(100, 36);
            this.buttonPlus.TabIndex = 5;
            this.buttonPlus.Text = "Больше";
            this.buttonPlus.UseVisualStyleBackColor = false;
            this.buttonPlus.Click += new System.EventHandler(this.buttonPlus_Click);
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.Linen;
            this.ClientSize = new System.Drawing.Size(929, 631);
            this.Controls.Add(this.buttonPlus);
            this.Controls.Add(this.buttonMinus);
            this.Controls.Add(this.StartNewGameButton);
            this.Controls.Add(this.DifficultySelector);
            this.Controls.Add(this.CollectionPanel);
            this.Controls.Add(this.GamePanel);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mahjong";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            GamePanel.Width = this.ClientSize.Width - 40;
            GamePanel.Height = this.ClientSize.Height - 160;

            int collectionWidth = CollectionPanel.Width;
            int collectionHeight = CollectionPanel.Height;

            int offsetX = (this.ClientSize.Width - collectionWidth) / 2;
            int offsetY = this.ClientSize.Height - collectionHeight - 20;

            CollectionPanel.Location = new Point(offsetX, offsetY);

            RenderGameField();
        }

        private System.Windows.Forms.Button buttonMinus;
        private System.Windows.Forms.Button buttonPlus;
    }
}
