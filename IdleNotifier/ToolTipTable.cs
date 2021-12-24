namespace IdleNotifier.Main
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class ToolTipTable
    {
        #region Constructor

        public ToolTipTable(bool firstRowHeader = true)
        {
            this.Rows = new List<TableRow>();
            this.FirstRowHeader = firstRowHeader;
        }

        #endregion Constructor

        #region Properties

        public bool FirstRowHeader
        {
            get;
            set;
        }

        public IList<TableRow> Rows
        {
            get;
            set;
        }

        public int MaxCols
        {
            get
            {
                return this.Rows.Max(r => r.Cols.Count);
            }
        }

        public int MinCols
        {
            get
            {
                return this.Rows.Min(r => r.Cols.Count);
            }
        }

        #endregion Properties

        #region Methods

        public void AddRow(string[] cols)
        {
            this.Rows.Add(new TableRow(cols));
        }

        /// <summary>
        /// Return column text length at given index
        /// </summary>
        /// <param name="index">Zero based index</param>
        /// <param name="padding">Optional padding to add to maximum length</param>
        /// <returns></returns>
        public int MaxTextLenAtCol(int index, int padding = 0)
        {
            int maxLen = 0;
            foreach (var r in this.Rows)
            {
                if (r.Cols.Count > index)
                {
                    int colTextLen = r.Cols[index].TextLen;
                    maxLen = colTextLen > maxLen ? colTextLen : maxLen;
                }
            }

            return maxLen + padding;
        }

        public override string ToString()
        {
            if (this.Rows == null || this.Rows.Count == 0)
            {
                return string.Empty;
            }

            int[] colLens = new int[this.MaxCols];
            for (int i = 0; i < this.MaxCols; i++)
            {
                colLens[i] = this.MaxTextLenAtCol(i, 4);
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < this.Rows.Count; i++)
            {
                if (i == 1 && this.FirstRowHeader)
                {
                    int ttlLen = colLens.Sum();
                    builder.AppendLine(new string('-', ttlLen));
                }

                var row = this.Rows[i];
                for (int j = 0; j < row.Cols.Count; j++)
                {
                    var col = row.Cols[j];
                    int diff = colLens[j] - col.TextLen;
                    builder.Append(col.Text);
                    builder.Append(new string(' ', diff));
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        #endregion Methods
    }

    #region Nested types

    public class TableRow
    {
        #region Constructor

        public TableRow()
        {
            this.Cols = new List<TableCol>();
        }

        public TableRow(string[] cols) : this()
        {
            this.Cols = cols.Select(c => new TableCol(c)).ToList();
        }

        #endregion Constructor

        #region Properties

        public IList<TableCol> Cols
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public void AddCols(string[] cols)
        {
            this.Cols = cols.Select(c => new TableCol(c)).ToList();
        }

        #endregion Methods
    }

    public class TableCol
    {
        #region Constructor

        public TableCol(string text)
        {
            this.Text = text;
        }

        #endregion Constructor

        #region Properties

        public string Text
        {
            get;
            set;
        }

        public int TextLen
        {
            get
            {
                return this.Text.Length;
            }
        }

        #endregion Properties
    }

    #endregion Nested types
}
