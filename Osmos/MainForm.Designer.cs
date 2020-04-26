namespace Osmos
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.gameField = new System.Windows.Forms.PictureBox();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.gameModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replusionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cycleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gameField)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameField
            // 
            this.gameField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gameField.Location = new System.Drawing.Point(12, 34);
            this.gameField.Margin = new System.Windows.Forms.Padding(0);
            this.gameField.Name = "gameField";
            this.gameField.Size = new System.Drawing.Size(1477, 686);
            this.gameField.TabIndex = 0;
            this.gameField.TabStop = false;
            this.gameField.Paint += new System.Windows.Forms.PaintEventHandler(this.Draw);
            this.gameField.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DownMouse);
            this.gameField.MouseUp += new System.Windows.Forms.MouseEventHandler(this.UpMouse);
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Interval = 15;
            this.Timer.Tick += new System.EventHandler(this.TickTimer);
            // 
            // MenuStrip
            // 
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameModeToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1501, 28);
            this.MenuStrip.TabIndex = 1;
            this.MenuStrip.Text = "MenuStrip";
            // 
            // gameModeToolStripMenuItem
            // 
            this.gameModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replusionToolStripMenuItem,
            this.cycleToolStripMenuItem});
            this.gameModeToolStripMenuItem.Name = "gameModeToolStripMenuItem";
            this.gameModeToolStripMenuItem.Size = new System.Drawing.Size(105, 24);
            this.gameModeToolStripMenuItem.Text = "Game mode";
            // 
            // replusionToolStripMenuItem
            // 
            this.replusionToolStripMenuItem.Name = "replusionToolStripMenuItem";
            this.replusionToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            this.replusionToolStripMenuItem.Text = "Repulsion";
            this.replusionToolStripMenuItem.Click += new System.EventHandler(this.ClickRepulsion);
            // 
            // cycleToolStripMenuItem
            // 
            this.cycleToolStripMenuItem.Name = "cycleToolStripMenuItem";
            this.cycleToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            this.cycleToolStripMenuItem.Text = "Cycle";
            this.cycleToolStripMenuItem.Click += new System.EventHandler(this.ClickCycle);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1501, 732);
            this.Controls.Add(this.gameField);
            this.Controls.Add(this.MenuStrip);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.MenuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Osmos";
            ((System.ComponentModel.ISupportInitialize)(this.gameField)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox gameField;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem gameModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replusionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cycleToolStripMenuItem;
    }
}

