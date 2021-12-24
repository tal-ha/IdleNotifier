namespace IdleNotifier.Main
{
    using Cassia;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public static class Helper
    {
        #region Fields

#if DEBUG
        public static string FOLDER_NAME = "log";
        public static FormWindowState WINDOW_STATE = FormWindowState.Normal;

        public static int LOG_NUM_OFFSET = 777;
        public static int INTERVAL_MS = Helper.SecToMs(3);
        public static long LIMIT_SECS = 10;

        public static bool FIRST_LAUNCH = false;
        public static bool IGNORE_FILE_IO = true;
        public static bool ENABLE_LOGGING = true;
        public static bool NOTIFY_LOGIN_LOGOFF = false;

        public static int RED_COLOR_FROM = 2; // minutes
        public static int ORANGE_COLOR_FROM = 1;
        public static int GREEN_COLOR_FROM = 0;

        public static int TOOLTIP_DURATION = 1000000;

        public static View FIRST_VIEW = View.INACTIVITIES;
        public static View CURRENT_VIEW = Helper.FIRST_VIEW;
#else
        public static string FOLDER_NAME = "log";
        public static FormWindowState WINDOW_STATE = FormWindowState.Minimized;

        public static int LOG_NUM_OFFSET = 0;
        public static int INTERVAL_MS = Helper.SecToMs(5);
        public static long LIMIT_SECS = Helper.MinToSec(5);
        
        public static bool FIRST_LAUNCH = false;
        public static bool IGNORE_FILE_IO = false;
        public static bool ENABLE_LOGGING = false;
        public static bool NOTIFY_LOGIN_LOGOFF = false;

        public static int RED_COLOR_FROM = 60; // minutes
        public static int ORANGE_COLOR_FROM = 15;
        public static int GREEN_COLOR_FROM = 5;

        public static int TOOLTIP_DURATION = 1000000;

        public static View FIRST_VIEW = View.INACTIVITIES;
        public static View CURRENT_VIEW = Helper.FIRST_VIEW;
#endif
        public const string DateTimeRegex = @"(0[1-9]|[12][0-9]|3[01])[.](0[1-9]|1[012])[.](19|20)\d\d[ ]([0-9]|0[0-9]|1[0-9]|2[0-3])[:]([0-5][0-9])[:]([0-5][0-9])";
        public const string DateTimeFormat = "dd.MM.yyyy HH:mm:ss";

        public enum LogLevel
        {
            INFO,
            WARNING,
            ERROR
        }

        public enum View
        {
            INACTIVITIES,
            GRAPH,
            TASKS,
            ALL_INACTIVITIES
        }

        #endregion Fields

        #region Methods

        public static void Log(string msg, LogLevel level = LogLevel.INFO)
        {
            if (Helper.ENABLE_LOGGING)
            {
                Console.WriteLine($"[{level}] --> {msg}");
            }
        }
        
        public static string FilePath
        {
            get
            {
                return string.Format("{0}\\log-{1}.txt", Helper.FOLDER_NAME, DateTime.Now.ToShortDateString());
            }
        }

        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;

            box.AppendText(text);
            box.AppendText(Environment.NewLine);
            //box.ScrollToCaret();
        }

        public static bool IsEqualTo(this DateTime dt1, DateTime dt2, int secBuffer = 0)
        {
            DateTime dt1UpperBound = dt1.TrimMilliSecs().AddSeconds(secBuffer);
            DateTime dt1LowerBound = dt1.TrimMilliSecs().AddSeconds(secBuffer * -1);
            dt2 = dt2.TrimMilliSecs();

            if (dt2 > dt1UpperBound || dt2 < dt1LowerBound)
            {
                return false;
            }

            return true;
        }

        private static DateTime TrimMilliSecs(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
        }

        public static int SecToMs(int sec)
        {
            if (sec <= 0)
            {
                return 0;
            }
            return 1000 * sec;
        }

        public static int MinToMs(int min)
        {
            if (min <= 0)
            {
                return 0;
            }
            return 1000 * 60 * min;
        }

        public static int MinToSec(int min)
        {
            if (min <= 0)
            {
                return 0;
            }
            return 60 * min;
        }

        public static long DiffMins(DateTime from, DateTime to)
        {
            return Convert.ToInt64(to.Subtract(from).TotalMinutes);
        }

        public static string FormatDateTime(DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return string.Empty;
            }

            string date;
            if (dt?.Date == DateTime.Now.Date)
            {
                date = "Today";
            }
            else if (dt?.Date == DateTime.Now.Date.AddDays(-1))
            {
                date = "Yesterday";
            }
            else
            {
                date = dt?.Date.ToString("dd MMM");
            }

            return string.Format("{0} at {1}", date, dt?.ToShortTimeString());
        }

        public static string FormatTimeSpan(TimeSpan interval)
        {
            if (interval == null)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            if (interval.Days > 0)
            {
                builder.Append(string.Format("{0} {1} ", interval.Days, interval.Days > 1 ? "Days" : "Day"));
            }

            if (interval.Hours > 0)
            {
                builder.Append(string.Format("{0} {1} ", interval.Hours, interval.Hours > 1 ? "Hrs" : "Hr"));
            }

            if (interval.Minutes > 0)
            {
                builder.Append(string.Format("{0} {1} ", interval.Minutes, interval.Minutes > 1 ? "Mins" : "Min"));
            }

            return builder.ToString().Trim();
        }

        public static void FixDataGridRowNumbers(ref DataGridView dataGridView)
        {
            int i = 1;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                row.Cells[0].Value = i++;
            }
        }

        public static void AddLog(LogDTO logDto, int num, ref RichTextBox infoBox, bool skipFile = false)
        {
            Helper.Log("Adding log");

            string text = string.Format(logDto.LogFormat, num + Helper.LOG_NUM_OFFSET);
            infoBox.AppendText(text, logDto.Color);

            if (!skipFile)
            {
                Helper.WriteToFile(text);
            }
        }

        public static void UpdateLog(IList<LogDTO> logger, ref RichTextBox infoBox, bool skipFile = false)
        {
            Helper.Log("Updating log");

            infoBox.Clear();
            int num = 0;
            foreach (LogDTO logDto in logger)
            {
                num++;
                string text = string.Format(logDto.LogFormat, num + Helper.LOG_NUM_OFFSET);
                infoBox.AppendText(text, logDto.Color);
            }

            if (!skipFile)
            {
                LogDTO lastLogDto = logger[logger.Count - 1];
                Helper.RemoveLastLineFromFile();
                Helper.WriteToFile(string.Format(lastLogDto.LogFormat, logger.Count));
            }
        }

        public static void UpdateLogForUser(IList<LogDTO> logger, ref RichTextBox infoBox)
        {
            Helper.Log("Updating log");

            infoBox.Clear();
            foreach (LogDTO logDto in logger)
            {
                infoBox.AppendText(logDto.ToString(), logDto.Color);
            }
        }

        public static List<LogDTO> GetLogsFromFile()
        {
            if (Helper.IGNORE_FILE_IO)
            {
                return new List<LogDTO>();
            }

            List<LogDTO> logger = new List<LogDTO>();
            try
            {
                if (!File.Exists(Helper.FilePath))
                {
                    return logger;
                }

                string[] allLines = File.ReadAllLines(Helper.FilePath);
                foreach (string line in allLines)
                {
                    LogDTO logDto = Helper.GetDtoFromLine(line);
                    if (logDto != null)
                    {
                        logger.Add(logDto);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message);
            }

            return logger;
        }

        private static void WriteToFile(string text)
        {
            if (Helper.IGNORE_FILE_IO)
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(Helper.FOLDER_NAME);
                using (TextWriter writer = File.AppendText(Helper.FilePath))
                {
                    writer.WriteLine(text);
                }
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message);
            }
        }

        private static void RemoveLastLineFromFile()
        {
            if (Helper.IGNORE_FILE_IO)
            {
                return;
            }

            try
            {
                if (!File.Exists(Helper.FilePath))
                {
                    return;
                }

                string[] allLines = File.ReadAllLines(Helper.FilePath);
                File.WriteAllLines(Helper.FilePath, allLines.Take(allLines.Length - 1).ToArray());
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message);
            }
        }

        private static LogDTO GetDtoFromLine(string line)
        {
            // example line = Inactivity#3 29.12.2017 16:26:46 - 29.12.2017 16:26:57 (0 Mins)
            string[] parts = line.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return null;
            }

            Match m1 = Regex.Match(parts[0], Helper.DateTimeRegex);
            Match m2 = Regex.Match(parts[1], Helper.DateTimeRegex);
            if (m1.Success && m2.Success)
            {
                return new LogDTO(
                    DateTime.ParseExact(m1.Value, Helper.DateTimeFormat, CultureInfo.InvariantCulture),
                    DateTime.ParseExact(m2.Value, Helper.DateTimeFormat, CultureInfo.InvariantCulture)
                );
            }
            else
            {
                return null;
            }
        }

        #endregion Methods

        #region Properties

        public static string CurrentUserName
        { 
            get 
            {
                ITerminalServicesManager manager = new TerminalServicesManager();
                return manager?.CurrentSession?.UserName ?? string.Empty;
            } 
        }

        #endregion Properties
    }
}
