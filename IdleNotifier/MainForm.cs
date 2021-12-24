namespace IdleNotifier.Main
{
    using Cassia;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Windows.Forms.DataVisualization.Charting;
    using Win32API;

    /// <summary>
    /// Partial class MainForm
    /// </summary>
    public partial class MainForm : Form
    {
        #region Fields

        private IList<LogDTO> allInactivities;
        private IDictionary<string, UserInfoDTO> users;
        private bool closingFromNotifyIcon;
        private string toolTipText;
        private IDictionary<Helper.View, Control> viewsDict;

        #endregion Fields

        #region Constructor

        public MainForm()
        {
            this.InitializeComponent();
            Helper.FIRST_LAUNCH = true;

            this.allInactivities = new List<LogDTO>();
            this.users = new Dictionary<string, UserInfoDTO>();
            this.CheckIdleTimer.Interval = Helper.INTERVAL_MS;
            this.notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            this.notifyIcon.BalloonTipTitle = "Idle Notifier";
            this.viewsDict = new Dictionary<Helper.View, Control>()
            {
                { Helper.View.INACTIVITIES, this.InfoBox},
                { Helper.View.GRAPH, this.chart1},
                { Helper.View.TASKS, this.tasksDataGridView}
            };
        }

        #endregion Constructor

        #region Methods

        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.SetVersion();
            this.LoadLogFromFile();
            this.CheckIdleTimer.Start();
            this.WindowState = Helper.WINDOW_STATE;
            if (Helper.WINDOW_STATE == FormWindowState.Minimized)
            {
                this.Hide();
            }

            this.ShowView(Helper.FIRST_VIEW);
        }

        private void ShowView(Helper.View view)
        {
            if (!this.viewsDict.ContainsKey(view))
            {
                return;
            }

            foreach (var item in this.viewsDict)
            {
                if (item.Key == view)
                {
                    item.Value.Show();
                    Helper.CURRENT_VIEW = view;
                }
                else
                {
                    item.Value.Hide();
                }
            }
        }

        private void ShowNotifyIcon(string msg)
        {
            this.notifyIcon.BalloonTipText = msg;
            this.notifyIcon.Visible = true;
            this.notifyIcon.ShowBalloonTip(500);
        }

        private void UpdateUsersLoginLogoffInfo(IList<ITerminalServicesSession> sessions) 
        {
            IList<string> loggedInUsers = new List<string>();
            StringBuilder builder = new StringBuilder();
            foreach (ITerminalServicesSession session in sessions)
            {
                loggedInUsers.Add(session.UserName);
                if (this.users.ContainsKey(session.UserName))
                {
                    // existing user: update info only
                    UserInfoDTO infoDTO = this.users[session.UserName];
                    if (infoDTO.Status != session.ConnectionState)
                    {
                        builder.AppendLine($"{session.UserName} is {session.ConnectionState}");
                    }

                    infoDTO.Status = session.ConnectionState;
                    infoDTO.LoginTime = session.LoginTime;
                    infoDTO.LastInputTime = session.LastInputTime;
                    infoDTO.IdleTime = session.IdleTime;
                }
                else
                {
                    // new logged in user: add to list
                    if (!Helper.FIRST_LAUNCH)
                    {
                        builder.AppendLine($"{session.UserName} logged in");
                    }

                    this.users.Add(session.UserName, new UserInfoDTO(session));
                }
            }

            // check for logged out users
            if (!Helper.FIRST_LAUNCH)
            {
                IList<string> loggedOutUsers = this.users.Keys.Where(k => !loggedInUsers.Contains(k)).ToList();
                if (loggedOutUsers.Count > 0)
                {
                    builder.AppendLine($"{string.Join(",", loggedOutUsers)} logged out");
                    foreach (var loggedOutUser in loggedOutUsers)
                    {
                        this.users.Remove(loggedOutUser);
                    }
                }
            }

            if (Helper.FIRST_LAUNCH)
            {
                Helper.FIRST_LAUNCH = false;
            }

            if (Helper.NOTIFY_LOGIN_LOGOFF)
            {
                this.ShowNotifyIcon(builder.ToString());
            }
        }

        private void UpdateUsersLoginLogoffInfo()
        {
            try
            {
                ITerminalServicesManager manager = new TerminalServicesManager();
                using (ITerminalServer server = manager.GetRemoteServer(System.Environment.MachineName))
                {
                    server.Open();
                    this.UpdateUsersLoginLogoffInfo(server.GetSessions().Where(s => !string.IsNullOrEmpty(s.UserName)).ToList());
                }
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message);
            }
        }

        private void SetVersion()
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            string version = $"v{info.FileMajorPart}.{info.FileMinorPart}";
            this.Text = string.Format("{0} {1}", this.Text, version);
#if DEBUG
            this.Text = string.Format("{0} {1}", this.Text, "[WARNING! DEBUG MODE]");
#endif
        }

        private void LoadLogFromFile()
        {
            this.allInactivities = Helper.GetLogsFromFile();
            Helper.UpdateLog(this.MyInactivities, ref InfoBox, true);
        }

        private void UpdateToolTipText()
        {
            if (this.users == null || this.users.Count == 0)
            {
                this.toolTipText = "Please wait.";
                return;
            }
            
            ToolTipTable tbl = new ToolTipTable();
            tbl.AddRow(new string[] { "Username", "Status", "Login time", "Connect time", "Disconnect time", "Last input time", "Idle since" });

            foreach (var item in this.users.OrderBy(u => u.Value.Status).ThenBy(u => u.Key))
            {
                UserInfoDTO infoDTO = item.Value;
                tbl.AddRow(new string[] 
                {
                    item.Key,
                    infoDTO.Status.ToString(),
                    Helper.FormatDateTime(infoDTO.LoginTime),
                    Helper.FormatDateTime(infoDTO.ConnectTime),
                    Helper.FormatDateTime(infoDTO.DisconnectTime),
                    Helper.FormatDateTime(infoDTO.LastInputTime),
                    Helper.FormatTimeSpan(infoDTO.IdleTime)
                });
            }

            this.toolTipText = tbl.ToString();
        }

        private void RerenderChart()
        {
            this.chart1.Series.Clear();

            Series series = new Series();
            series.Name = "series_1";
            series.Color = Color.Red;
            series.IsVisibleInLegend = false;
            series.ChartType = SeriesChartType.Column;
            
            this.chart1.Series.Add(series);
            IList<LogDTO> logs = this.MyInactivities;
#if DEBUG
            logs = this.GetMockedInactivities();
#endif
            foreach (var log in logs)
            {
                int index = series.Points.AddY(log.DiffMins);
                series.Points[index].Color = log.Color;
                series.Points[index].Label = string.Format("({0} Mins)", log.DiffMins.ToString());
            }

            this.chart1.Invalidate();
        }

        private IList<LogDTO> GetMockedInactivities()
        {
            IList<LogDTO> logs = new List<LogDTO>();

            logs.Add(new LogDTO(DateTime.Now.AddSeconds(Helper.MinToSec(30) * -1), DateTime.Now.AddSeconds(Helper.MinToSec(20) * -1))); // 10min inactive
            logs.Add(new LogDTO(DateTime.Now.AddSeconds(Helper.MinToSec(15) * -1), DateTime.Now.AddSeconds(Helper.MinToSec(12) * -1))); // 3min inactive
            logs.Add(new LogDTO(DateTime.Now.AddSeconds(Helper.MinToSec(10) * -1), DateTime.Now.AddSeconds(Helper.MinToSec(2) * -1))); // 8min inactive
            logs.Add(new LogDTO(DateTime.Now.AddSeconds(Helper.MinToSec(1) * -1), DateTime.Now)); // 1min inactive

            return logs;
        }

        private void ClockTick()
        {
            try
            {
                ITerminalServicesManager manager = new TerminalServicesManager();
                using (ITerminalServer server = manager.GetRemoteServer(Environment.MachineName))
                {
                    server.Open();
                    IList<ITerminalServicesSession> sessions = server.GetSessions().Where(s => !string.IsNullOrEmpty(s.UserName)).ToList();
                    foreach (ITerminalServicesSession session in sessions)
                    {
                        if (!session.LastInputTime.HasValue || session.ConnectionState != Cassia.ConnectionState.Active)
                        {
                            continue;
                        }

                        LogDTO log = new LogDTO(session.UserName, session.LastInputTime.Value, DateTime.Now);
                        if (log.DiffSecs <= Helper.LIMIT_SECS)
                        {
                            continue;
                        }
                        
                        LogDTO lastLog = this.allInactivities.LastOrDefault(ll => ll.Username == log.Username);
                        if (lastLog == null || !lastLog.From.IsEqualTo(log.From, 3))
                        {
                            this.allInactivities.Add(log);
                            if (log.IsMyLog && Helper.CURRENT_VIEW == Helper.View.INACTIVITIES)
                            {
                                Helper.AddLog(log, this.MyInactivities.Count, ref InfoBox);
                            }
                        }
                        else 
                        {
                            lastLog.To = log.To;
                            if (log.IsMyLog && Helper.CURRENT_VIEW == Helper.View.INACTIVITIES)
                            {
                                Helper.UpdateLog(this.MyInactivities, ref InfoBox);
                            }
                        }
                    }

                    this.UpdateUsersLoginLogoffInfo(sessions);
                }
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message);
            }
        }

        private void ClockTickOld()
        {
            long idleTimeSec = Win32.GetIdleTimeInSecs();
            Helper.Log($"idle since {idleTimeSec}secs, limit is {Helper.LIMIT_SECS}secs");

            if (idleTimeSec > Helper.LIMIT_SECS)
            {
                DateTime from = DateTime.Now.AddSeconds(idleTimeSec * -1);
                DateTime to = DateTime.Now;

                int count = this.allInactivities.Count;
                if (count == 0)
                {
                    LogDTO item = new LogDTO(from, to);
                    this.allInactivities.Add(item);
                    Helper.AddLog(item, this.allInactivities.Count, ref InfoBox);
                }
                else
                {
                    LogDTO lastLog = this.allInactivities[count - 1];
                    if (lastLog.From.IsEqualTo(from, 1))
                    {
                        lastLog.To = to;
                        Helper.UpdateLog(this.MyInactivities, ref InfoBox);
                    }
                    else
                    {
                        LogDTO item = new LogDTO(from, to);
                        this.allInactivities.Add(item);
                        Helper.AddLog(item, this.MyInactivities.Count, ref InfoBox);
                    }
                }
            }

            this.UpdateUsersLoginLogoffInfo();
        }

        #region Events

        private void CheckIdleTimer_Tick(object sender, EventArgs e)
        {
            this.ClockTick();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.closingFromNotifyIcon = true;
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.closingFromNotifyIcon)
            {
                e.Cancel = true;
                //this.ShowNotifyIcon("Running...");
                this.Hide();
            }
        }

        private void InfoBox_MouseHover(object sender, EventArgs e)
        {
            this.UpdateToolTipText();
            this.usersInfoToolTip.Show(this.toolTipText, this.InfoBox, Helper.TOOLTIP_DURATION);

            //var control = (Control)sender;
            //this.toolTip1.Show(this.toolTipText, this.InfoBox, control.PointToClient(new Point(Cursor.Position.X - 500, Cursor.Position.Y)), Helper.TOOLTIP_DURATION);
            //Point pt = control.PointToClient(new Point(Cursor.Position.X - 300, Cursor.Position.Y));
            //this.toolTip1.Show(this.toolTipText, this.InfoBox, pt.X, pt.Y);
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = TextRenderer.MeasureText(this.toolTipText, new Font("Courier New", 10.0f, FontStyle.Regular));
        }

        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {
            DrawToolTipEventArgs newArgs = new DrawToolTipEventArgs(e.Graphics, e.AssociatedWindow, e.AssociatedControl, e.Bounds, e.ToolTipText, this.usersInfoToolTip.BackColor, Color.Blue, new Font("Courier New", 10.0f, FontStyle.Regular));
            newArgs.DrawBackground();
            newArgs.DrawBorder();
            newArgs.DrawText(TextFormatFlags.TextBoxControl);
        }

        private void showHideGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.chart1.Visible)
            {
                this.ShowView(Helper.View.INACTIVITIES);
            }
            else
            {
                this.ShowView(Helper.View.GRAPH);
            }
        }

        private void showHideTasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tasksDataGridView.Visible)
            {
                this.ShowView(Helper.View.INACTIVITIES);
            }
            else
            {
                this.ShowView(Helper.View.TASKS);
            }
        }

        private void showHideAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.InfoBox.Visible)
            {
                this.ShowView(Helper.View.INACTIVITIES);
            }

            if (Helper.CURRENT_VIEW == Helper.View.INACTIVITIES)
            {
                Helper.UpdateLogForUser(this.allInactivities, ref InfoBox);
                Helper.CURRENT_VIEW = Helper.View.ALL_INACTIVITIES;
            }
            else 
            {
                Helper.UpdateLog(this.MyInactivities, ref InfoBox, skipFile: true);
                Helper.CURRENT_VIEW = Helper.View.INACTIVITIES;
            }
        }

        private void chart1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.chart1.Visible)
            {
                this.RerenderChart();
            }
        }

        private void tasksDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Helper.FixDataGridRowNumbers(ref this.tasksDataGridView);
        }

        private void tasksDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            Helper.FixDataGridRowNumbers(ref this.tasksDataGridView);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                // Start clicked
                // Stopwatch watch = new Stopwatch();
                // watch.Start();
            }

            if (e.ColumnIndex == 3)
            {
                // Pause clicked
            }

            if (e.ColumnIndex == 4)
            {
                // Stop clicked
            }

            if (e.ColumnIndex == 5)
            {
                // Delete clicked
                DataGridViewRow currentRow = this.tasksDataGridView.Rows[e.RowIndex];
                if (!currentRow.IsNewRow)
                {
                    this.tasksDataGridView.Rows.Remove(currentRow);
                }
            }
        }

        #endregion Events

        #endregion Methods

        #region Properties

        public IList<LogDTO> MyInactivities
        {
            get
            {
                if (this.allInactivities == null || this.allInactivities.Count == 0)
                {
                    return new List<LogDTO>();
                }

                return this.allInactivities.Where(i => string.IsNullOrEmpty(i.Username) || i.Username == Helper.CurrentUserName).ToList();
            }
        }

        #endregion Properties
    }
}