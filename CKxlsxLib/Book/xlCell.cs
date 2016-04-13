﻿using XLOC.Utility;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace XLOC.Book
{
    public class xlCell
    {
        #region Variables
        //=================================================
        int row, column;
        //=================================================
        #endregion

        #region Constructor
        //=================================================
        internal xlCell()
        {

        }
        //=================================================
        #endregion

        #region Private
        //=================================================
        internal T getAs<T>() where T : struct
        {
            return (T)Value;
        }

        internal static string GetReference(int col, int row)
        {
            int m;
            string name = string.Empty;
            while (col > 0)
            {
                m = (col - 1) % 26;
                name = (char)(65 + m) + name;
                col = (int)((col - m) / 26);
            }
            return name + row;
        }
        //=================================================
        #endregion

        #region Properties
        //=================================================
        public object Value { get; set; }
        public int Row
        {
            get { return row; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("Индекс строки должен быть больше 1");
                row = value;
            }
        }
        public int Col
        {
            get { return column; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("Индекс столбца должен быть больше 1");
                column = value;
            }
        }
        public string Reference
        {
            get
            {
                return GetReference(column, row);
            }
            set
            {
                if (!Regex.IsMatch(value, @"^[A-Z]+\d+$"))
                    throw new ArgumentException("Адрес ячейки должен соответсвовать формату [A-Z]+[0-9]+");

                int i = Regex.Match(value, "^[A-Z]+").Value.Count() - 1;
                int col = 0;
                foreach (var item in Regex.Match(value, "^[A-Z]+").Value.Select(x => (byte)x - 64))
                {
                    col += (int)(item * Math.Pow(26, i--));
                }
                Col = col;
                Row = int.Parse(Regex.Match(value, @"\d+$").Value);
            }
        }
        public xlContentType? Type { get; set; }
        public int? SharedId { get; internal set; }
        //=================================================
        #endregion

        #region Methods
        //=================================================
        public static implicit operator xlCell(CellInfo local)
        {
            return new xlCell()
            {
                Reference = local.Reference,
                Type = local.ContentType,
                Value = local.Value,
                SharedId = local.SharedId
            };
        }

        public static implicit operator CellInfo(xlCell local)
        {
            return new CellInfo(local.Reference, local.Type, local.Value, local.SharedId);
        }
        //=================================================
        #endregion
    }
}