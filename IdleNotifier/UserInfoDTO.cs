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
    using Cassia;

    public class UserInfoDTO
    {
        #region Constructor

        public UserInfoDTO(ITerminalServicesSession session)
        {
            this.UserName = session.UserName;
            this.Status = session.ConnectionState;
            this.LoginTime = session.LoginTime;
            this.ConnectTime = session.ConnectTime;
            this.DisconnectTime = session.DisconnectTime;
            this.LastInputTime = session.LastInputTime;
            this.IdleTime = session.IdleTime;
        }

        #endregion Constructor

        #region Properties

        public string UserName
        {
            get;
            set;
        }

        public ConnectionState Status
        {
            get;
            set;
        }

        public DateTime? LoginTime
        {
            get;
            set;
        }
        
        public DateTime? ConnectTime
        {
            get;
            set;
        }

        public DateTime? DisconnectTime
        {
            get;
            set;
        }

        public DateTime? LastInputTime
        {
            get;
            set;
        }

        public TimeSpan IdleTime
        {
            get;
            set;
        }

        #endregion Properties
        
    }
}
