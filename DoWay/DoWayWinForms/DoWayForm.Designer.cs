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
            this.panelSprites = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnSaveMap = new System.Windows.Forms.Button();
            this.btnLoadMap = new System.Windows.Forms.Button();
            this.btnExportToPng = new System.Windows.Forms.Button();
            this.btnClearMap = new System.Windows.Forms.Button();
            this.btnSetMapSize = new System.Windows.Forms.Button();
            this.panelToolBar = new System.Windows.Forms.Panel();
            this.btnAddTrees = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelSprites.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.SuspendLayout();
            this.panelToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMap
            // 
            this.panelMap.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelMap.Location = new System.Drawing.Point(12, 6);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(1297, 1060);
            this.panelMap.TabIndex = 0;
            // 
            // panelSprites
            // 
            this.panelSprites.AutoScroll = true;
            this.panelSprites.Controls.Add(this.splitContainer2);
            this.panelSprites.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelSprites.Location = new System.Drawing.Point(31, 39);
            this.panelSprites.Name = "panelSprites";
            this.panelSprites.Size = new System.Drawing.Size(490, 532);
            this.panelSprites.TabIndex = 1;
            this.panelSprites.WrapContents = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Size = new System.Drawing.Size(0, 100);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 0;
            // 
            // btnSaveMap
            // 
            this.btnSaveMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSaveMap.Location = new System.Drawing.Point(3, 12);
            this.btnSaveMap.Name = "btnSaveMap";
            this.btnSaveMap.Size = new System.Drawing.Size(163, 40);
            this.btnSaveMap.TabIndex = 2;
            this.btnSaveMap.Text = "Сохранить карту";
            this.btnSaveMap.UseVisualStyleBackColor = true;
            this.btnSaveMap.Click += new System.EventHandler(this.btnSaveMap_Click);
            // 
            // btnLoadMap
            // 
            this.btnLoadMap.Location = new System.Drawing.Point(172, 12);
            this.btnLoadMap.Name = "btnLoadMap";
            this.btnLoadMap.Size = new System.Drawing.Size(182, 40);
            this.btnLoadMap.TabIndex = 3;
            this.btnLoadMap.Text = "Загрузить карту";
            this.btnLoadMap.UseVisualStyleBackColor = true;
            this.btnLoadMap.Click += new System.EventHandler(this.btnLoadMap_Click);
            // 
            // btnExportToPng
            // 
            this.btnExportToPng.Location = new System.Drawing.Point(360, 12);
            this.btnExportToPng.Name = "btnExportToPng";
            this.btnExportToPng.Size = new System.Drawing.Size(224, 40);
            this.btnExportToPng.TabIndex = 4;
            this.btnExportToPng.Text = "Экспортировать в PNG";
            this.btnExportToPng.UseVisualStyleBackColor = true;
            this.btnExportToPng.Click += new System.EventHandler(this.btnExportToPng_Click);
            // 
            // btnClearMap
            // 
            this.btnClearMap.Location = new System.Drawing.Point(1042, 12);
            this.btnClearMap.Name = "btnClearMap";
            this.btnClearMap.Size = new System.Drawing.Size(146, 40);
            this.btnClearMap.TabIndex = 5;
            this.btnClearMap.Text = "Очистить карту";
            this.btnClearMap.UseVisualStyleBackColor = true;
            this.btnClearMap.Click += new System.EventHandler(this.btnClearMap_Click);
            // 
            // btnSetMapSize
            // 
            this.btnSetMapSize.Location = new System.Drawing.Point(1763, 12);
            this.btnSetMapSize.Name = "btnSetMapSize";
            this.btnSetMapSize.Size = new System.Drawing.Size(118, 40);
            this.btnSetMapSize.TabIndex = 6;
            this.btnSetMapSize.Text = "Применить";
            this.btnSetMapSize.UseVisualStyleBackColor = true;
            this.btnSetMapSize.Click += new System.EventHandler(this.btnSetMapSize_Click);
            // 
            // panelToolBar
            // 
            this.panelToolBar.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelToolBar.Controls.Add(this.btnAddTrees);
            this.panelToolBar.Controls.Add(this.label2);
            this.panelToolBar.Controls.Add(this.label1);
            this.panelToolBar.Controls.Add(this.txtHeight);
            this.panelToolBar.Controls.Add(this.txtWidth);
            this.panelToolBar.Controls.Add(this.btnSetMapSize);
            this.panelToolBar.Controls.Add(this.btnSaveMap);
            this.panelToolBar.Controls.Add(this.btnClearMap);
            this.panelToolBar.Controls.Add(this.btnExportToPng);
            this.panelToolBar.Controls.Add(this.btnLoadMap);
            this.panelToolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolBar.Location = new System.Drawing.Point(0, 0);
            this.panelToolBar.Name = "panelToolBar";
            this.panelToolBar.Size = new System.Drawing.Size(1893, 65);
            this.panelToolBar.TabIndex = 7;
            // 
            // btnAddTrees
            // 
            this.btnAddTrees.Location = new System.Drawing.Point(733, 12);
            this.btnAddTrees.Name = "btnAddTrees";
            this.btnAddTrees.Size = new System.Drawing.Size(224, 40);
            this.btnAddTrees.TabIndex = 11;
            this.btnAddTrees.Text = "Добавить деревья";
            this.btnAddTrees.UseVisualStyleBackColor = true;
            this.btnAddTrees.Click += new System.EventHandler(this.btnAddTrees_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1604, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "X";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1373, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Размер карты:";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(1498, 19);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(100, 26);
            this.txtHeight.TabIndex = 8;
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(1630, 19);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(100, 26);
            this.txtWidth.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 65);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelMap);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelSprites);
            this.splitContainer1.Size = new System.Drawing.Size(1893, 1069);
            this.splitContainer1.SplitterDistance = 1342;
            this.splitContainer1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1893, 1134);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelToolBar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelSprites.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panelToolBar.ResumeLayout(false);
            this.panelToolBar.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.FlowLayoutPanel panelSprites;
        private System.Windows.Forms.Button btnSaveMap;
        private System.Windows.Forms.Button btnLoadMap;
        private System.Windows.Forms.Button btnExportToPng;
        private System.Windows.Forms.Button btnClearMap;
        private System.Windows.Forms.Button btnSetMapSize;
        private System.Windows.Forms.Panel panelToolBar;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnAddTrees;
    }
}

