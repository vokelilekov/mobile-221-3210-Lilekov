namespace DoWayWinForms
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMap = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panelSprites = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnSetMapSize = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.pictureBoxSelectedSprite = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.afqToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьКартуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьКToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.экспортВPNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.инструментыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ластикToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.очиститьКартуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьДеревьяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.инструкцияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSprites.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSelectedSprite)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMap
            // 
            this.panelMap.BackColor = System.Drawing.SystemColors.Control;
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMap.Location = new System.Drawing.Point(30, 83);
            this.panelMap.Margin = new System.Windows.Forms.Padding(20);
            this.panelMap.Name = "panelMap";
            this.panelMap.Padding = new System.Windows.Forms.Padding(20);
            this.panelMap.Size = new System.Drawing.Size(1271, 986);
            this.panelMap.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(1324, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(274, 53);
            this.label4.TabIndex = 0;
            this.label4.Text = "Выбор участка дороги";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(1604, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(276, 53);
            this.label5.TabIndex = 1;
            this.label5.Text = "Текущий элемент";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSprites
            // 
            this.panelSprites.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelSprites.AutoScroll = true;
            this.panelSprites.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panelSprites.Controls.Add(this.splitContainer2);
            this.panelSprites.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelSprites.Location = new System.Drawing.Point(1324, 66);
            this.panelSprites.Name = "panelSprites";
            this.panelSprites.Padding = new System.Windows.Forms.Padding(10);
            this.panelSprites.Size = new System.Drawing.Size(274, 262);
            this.panelSprites.TabIndex = 1;
            this.panelSprites.WrapContents = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(13, 13);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Size = new System.Drawing.Size(0, 100);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 0;
            // 
            // btnSetMapSize
            // 
            this.btnSetMapSize.Location = new System.Drawing.Point(1152, 3);
            this.btnSetMapSize.Name = "btnSetMapSize";
            this.btnSetMapSize.Size = new System.Drawing.Size(114, 26);
            this.btnSetMapSize.TabIndex = 6;
            this.btnSetMapSize.Text = "Применить";
            this.btnSetMapSize.UseVisualStyleBackColor = true;
            this.btnSetMapSize.Click += new System.EventHandler(this.btnSetMapSize_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1000, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "X";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(769, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Размер карты:";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(894, 3);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(100, 26);
            this.txtHeight.TabIndex = 8;
            this.txtHeight.Text = "10";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(1026, 3);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(100, 26);
            this.txtWidth.TabIndex = 7;
            this.txtWidth.Text = "10";
            // 
            // pictureBoxSelectedSprite
            // 
            this.pictureBoxSelectedSprite.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxSelectedSprite.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBoxSelectedSprite.Location = new System.Drawing.Point(1614, 73);
            this.pictureBoxSelectedSprite.Margin = new System.Windows.Forms.Padding(10);
            this.pictureBoxSelectedSprite.Name = "pictureBoxSelectedSprite";
            this.pictureBoxSelectedSprite.Size = new System.Drawing.Size(256, 256);
            this.pictureBoxSelectedSprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSelectedSprite.TabIndex = 13;
            this.pictureBoxSelectedSprite.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelMap, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelSprites, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxSelectedSprite, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 35);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1893, 1099);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(13, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(1305, 53);
            this.label6.TabIndex = 14;
            this.label6.Text = "Редактор дорог";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.afqToolStripMenuItem,
            this.инструментыToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 9, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1893, 35);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // afqToolStripMenuItem
            // 
            this.afqToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьКартуToolStripMenuItem,
            this.загрузитьКToolStripMenuItem,
            this.экспортВPNGToolStripMenuItem});
            this.afqToolStripMenuItem.Name = "afqToolStripMenuItem";
            this.afqToolStripMenuItem.Size = new System.Drawing.Size(69, 29);
            this.afqToolStripMenuItem.Text = "Файл";
            // 
            // сохранитьКартуToolStripMenuItem
            // 
            this.сохранитьКартуToolStripMenuItem.Name = "сохранитьКартуToolStripMenuItem";
            this.сохранитьКартуToolStripMenuItem.Size = new System.Drawing.Size(236, 34);
            this.сохранитьКартуToolStripMenuItem.Text = "Экспорт карты";
            this.сохранитьКартуToolStripMenuItem.Click += new System.EventHandler(this.сохранитьКартуToolStripMenuItem_Click);
            // 
            // загрузитьКToolStripMenuItem
            // 
            this.загрузитьКToolStripMenuItem.Name = "загрузитьКToolStripMenuItem";
            this.загрузитьКToolStripMenuItem.Size = new System.Drawing.Size(236, 34);
            this.загрузитьКToolStripMenuItem.Text = "Импорт карты";
            this.загрузитьКToolStripMenuItem.Click += new System.EventHandler(this.загрузитьКToolStripMenuItem_Click);
            // 
            // экспортВPNGToolStripMenuItem
            // 
            this.экспортВPNGToolStripMenuItem.Name = "экспортВPNGToolStripMenuItem";
            this.экспортВPNGToolStripMenuItem.Size = new System.Drawing.Size(236, 34);
            this.экспортВPNGToolStripMenuItem.Text = "Экспорт в PNG";
            this.экспортВPNGToolStripMenuItem.Click += new System.EventHandler(this.экспортВPNGToolStripMenuItem_Click);
            // 
            // инструментыToolStripMenuItem
            // 
            this.инструментыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ластикToolStripMenuItem,
            this.очиститьКартуToolStripMenuItem,
            this.добавитьДеревьяToolStripMenuItem});
            this.инструментыToolStripMenuItem.Name = "инструментыToolStripMenuItem";
            this.инструментыToolStripMenuItem.Size = new System.Drawing.Size(138, 29);
            this.инструментыToolStripMenuItem.Text = "Инструменты";
            // 
            // ластикToolStripMenuItem
            // 
            this.ластикToolStripMenuItem.Name = "ластикToolStripMenuItem";
            this.ластикToolStripMenuItem.Size = new System.Drawing.Size(264, 34);
            this.ластикToolStripMenuItem.Text = "Ластик";
            this.ластикToolStripMenuItem.Click += new System.EventHandler(this.ластикToolStripMenuItem_Click);
            // 
            // очиститьКартуToolStripMenuItem
            // 
            this.очиститьКартуToolStripMenuItem.Name = "очиститьКартуToolStripMenuItem";
            this.очиститьКартуToolStripMenuItem.Size = new System.Drawing.Size(264, 34);
            this.очиститьКартуToolStripMenuItem.Text = "Очистить карту";
            this.очиститьКартуToolStripMenuItem.Click += new System.EventHandler(this.очиститьКартуToolStripMenuItem_Click);
            // 
            // добавитьДеревьяToolStripMenuItem
            // 
            this.добавитьДеревьяToolStripMenuItem.Name = "добавитьДеревьяToolStripMenuItem";
            this.добавитьДеревьяToolStripMenuItem.Size = new System.Drawing.Size(264, 34);
            this.добавитьДеревьяToolStripMenuItem.Text = "Добавить деревья";
            this.добавитьДеревьяToolStripMenuItem.Click += new System.EventHandler(this.добавитьДеревьяToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.инструкцияToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(97, 32);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // инструкцияToolStripMenuItem
            // 
            this.инструкцияToolStripMenuItem.Name = "инструкцияToolStripMenuItem";
            this.инструкцияToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.инструкцияToolStripMenuItem.Text = "Инструкция";
            this.инструкцияToolStripMenuItem.Click += new System.EventHandler(this.инструкцияToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1893, 1134);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnSetMapSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DoWay";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp_1);
            this.panelSprites.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSelectedSprite)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.FlowLayoutPanel panelSprites;
        private System.Windows.Forms.Button btnSetMapSize;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBoxSelectedSprite;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem afqToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьКартуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьКToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem экспортВPNGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem инструментыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ластикToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem очиститьКартуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьДеревьяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem инструкцияToolStripMenuItem;
    }
}

