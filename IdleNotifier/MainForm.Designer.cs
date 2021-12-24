namespace IdleNotifier.Main
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.CheckIdleTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InfoBox = new System.Windows.Forms.RichTextBox();
            this.infoBoxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showHideGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideTasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersInfoToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tasksDataGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.notifyIconContextMenu.SuspendLayout();
            this.infoBoxContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tasksDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CheckIdleTimer
            // 
            this.CheckIdleTimer.Interval = 1000;
            this.CheckIdleTimer.Tick += new System.EventHandler(this.CheckIdleTimer_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyIconContextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "IdleNotifier";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // notifyIconContextMenu
            // 
            this.notifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CloseToolStripMenuItem});
            this.notifyIconContextMenu.Name = "notifyIconContextMenu";
            this.notifyIconContextMenu.Size = new System.Drawing.Size(104, 26);
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.CloseToolStripMenuItem.Text = "Close";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // InfoBox
            // 
            this.InfoBox.ContextMenuStrip = this.infoBoxContextMenu;
            this.InfoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox.HideSelection = false;
            this.InfoBox.Location = new System.Drawing.Point(0, 0);
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.ReadOnly = true;
            this.InfoBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.InfoBox.Size = new System.Drawing.Size(500, 261);
            this.InfoBox.TabIndex = 2;
            this.InfoBox.Text = "";
            this.InfoBox.MouseHover += new System.EventHandler(this.InfoBox_MouseHover);
            // 
            // infoBoxContextMenu
            // 
            this.infoBoxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHideGraphToolStripMenuItem,
            this.showHideTasksToolStripMenuItem,
            this.showHideAllToolStripMenuItem});
            this.infoBoxContextMenu.Name = "infoBoxContextMenu";
            this.infoBoxContextMenu.Size = new System.Drawing.Size(168, 70);
            // 
            // showHideGraphToolStripMenuItem
            // 
            this.showHideGraphToolStripMenuItem.Name = "showHideGraphToolStripMenuItem";
            this.showHideGraphToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.showHideGraphToolStripMenuItem.Text = "Show/Hide graph";
            this.showHideGraphToolStripMenuItem.Click += new System.EventHandler(this.showHideGraphToolStripMenuItem_Click);
            // 
            // showHideTasksToolStripMenuItem
            // 
            this.showHideTasksToolStripMenuItem.Name = "showHideTasksToolStripMenuItem";
            this.showHideTasksToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.showHideTasksToolStripMenuItem.Text = "Show/Hide tasks";
            this.showHideTasksToolStripMenuItem.Click += new System.EventHandler(this.showHideTasksToolStripMenuItem_Click);
            // 
            // showHideAllToolStripMenuItem
            // 
            this.showHideAllToolStripMenuItem.Name = "showHideAllToolStripMenuItem";
            this.showHideAllToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.showHideAllToolStripMenuItem.Text = "Show/Hide all";
            this.showHideAllToolStripMenuItem.Click += new System.EventHandler(this.showHideAllToolStripMenuItem_Click);
            // 
            // usersInfoToolTip
            // 
            this.usersInfoToolTip.AutoPopDelay = 5000;
            this.usersInfoToolTip.InitialDelay = 1000;
            this.usersInfoToolTip.OwnerDraw = true;
            this.usersInfoToolTip.ReshowDelay = 100;
            this.usersInfoToolTip.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.toolTip1_Draw);
            this.usersInfoToolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ContextMenuStrip = this.infoBoxContextMenu;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(500, 261);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "chart1";
            this.chart1.Visible = false;
            this.chart1.VisibleChanged += new System.EventHandler(this.chart1_VisibleChanged);
            // 
            // tasksDataGridView
            // 
            this.tasksDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tasksDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.tasksDataGridView.ContextMenuStrip = this.infoBoxContextMenu;
            this.tasksDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tasksDataGridView.Location = new System.Drawing.Point(0, 0);
            this.tasksDataGridView.Name = "tasksDataGridView";
            this.tasksDataGridView.Size = new System.Drawing.Size(500, 261);
            this.tasksDataGridView.TabIndex = 4;
            this.tasksDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.tasksDataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.tasksDataGridView_RowsAdded);
            this.tasksDataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.tasksDataGridView_RowsRemoved);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "No.";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 50;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Task";
            this.Column2.Name = "Column2";
            this.Column2.Width = 205;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "";
            this.Column3.Name = "Column3";
            this.Column3.Text = "Start";
            this.Column3.UseColumnTextForButtonValue = true;
            this.Column3.Width = 50;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "";
            this.Column4.Name = "Column4";
            this.Column4.Text = "Pause";
            this.Column4.UseColumnTextForButtonValue = true;
            this.Column4.Width = 50;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "";
            this.Column5.Name = "Column5";
            this.Column5.Text = "Stop";
            this.Column5.UseColumnTextForButtonValue = true;
            this.Column5.Width = 50;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "";
            this.Column6.Name = "Column6";
            this.Column6.Text = "Delete";
            this.Column6.UseColumnTextForButtonValue = true;
            this.Column6.Width = 50;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 261);
            this.Controls.Add(this.tasksDataGridView);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.InfoBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IdleNotifier";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.notifyIconContextMenu.ResumeLayout(false);
            this.infoBoxContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tasksDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer CheckIdleTimer;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        private System.Windows.Forms.RichTextBox InfoBox;
        private System.Windows.Forms.ToolTip usersInfoToolTip;
        private System.Windows.Forms.ContextMenuStrip infoBoxContextMenu;
        private System.Windows.Forms.ToolStripMenuItem showHideGraphToolStripMenuItem;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ToolStripMenuItem showHideTasksToolStripMenuItem;
        private System.Windows.Forms.DataGridView tasksDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewButtonColumn Column3;
        private System.Windows.Forms.DataGridViewButtonColumn Column4;
        private System.Windows.Forms.DataGridViewButtonColumn Column5;
        private System.Windows.Forms.DataGridViewButtonColumn Column6;
        private System.Windows.Forms.ToolStripMenuItem showHideAllToolStripMenuItem;
    }
}

