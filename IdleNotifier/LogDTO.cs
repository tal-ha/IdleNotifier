namespace IdleNotifier.Main
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class LogDTO
    {
        #region Fields

        private string _username = null;
        private DateTime? _from = null;
        private DateTime? _to = null;

        #endregion Fields

        #region Constructors

        public LogDTO(string username, DateTime from, DateTime to)
        {
            this.Username = username;
            this.From = from;
            this.To = to;
        }

        public LogDTO(string username) : this(username, default(DateTime), default(DateTime))
        {
        }

        public LogDTO(DateTime from, DateTime to) : this(null, from, to)
        {
        }

        #endregion Constructors

        #region Properties

        public string Username 
        {
            get
            {
                return string.IsNullOrEmpty(this._username) ? Helper.CurrentUserName : this._username;
            }

            set
            {

                this._username = value;
            }
        }

        public DateTime From
        {
            get
            {
                return this._from.GetValueOrDefault();
            }

            set
            {
                if (this._to.HasValue && value > this._to.Value)
                {
                    throw new InvalidOperationException("'From' must be a value less than or equal to 'To'");
                }

                this._from = value;
            }
        }

        public DateTime To
        {
            get
            {
                return this._to.GetValueOrDefault();
            }

            set
            {
                if (!this._from.HasValue)
                {
                    throw new InvalidOperationException("'From' must be set before 'To'");
                }

                if (value < this._from.Value)
                {   
                    throw new InvalidOperationException("'To' must be a value greater than or equal to 'From'");
                }

                this._to = value;
            }
        }

        public long DiffMins
        {
            get
            {
                return Convert.ToInt64(To.Subtract(From).TotalMinutes);
            }
        }

        public long DiffSecs
        {
            get
            {
                return Convert.ToInt64(To.Subtract(From).TotalSeconds);
            }
        }

        public Color Color
        {
            get
            {
                if (this.DiffMins >= Helper.RED_COLOR_FROM)
                {
                    return Color.Red;
                }
                else if (this.DiffMins >= Helper.ORANGE_COLOR_FROM)
                {
                    return Color.Orange;
                }
                else if (this.DiffMins >= Helper.GREEN_COLOR_FROM)
                {
                    return Color.Green;
                }
                else
                {
                    return Color.Black;
                }
            }
        }

        public string LogFormat
        {
            get
            {
                return string.Format("Inactivity#{0} {1} - {2} ({3} Mins)", "{0}", this.From.ToString(Helper.DateTimeFormat), this.To.ToString(Helper.DateTimeFormat), this.DiffMins);
            }
        }

        public bool IsMyLog 
        {
            get 
            {
                return this.Username == Helper.CurrentUserName;
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return string.Format("{0}: {1} - {2} ({3} Mins)", 
                this.Username,
                this.From.ToString(Helper.DateTimeFormat), 
                this.To.ToString(Helper.DateTimeFormat),
                this.DiffMins);
        }

        #endregion Methods
    }
}
