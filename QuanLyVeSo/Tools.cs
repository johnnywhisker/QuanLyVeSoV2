using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuanLyVeSo
{
    namespace ConsoleTables {
        public class ConsoleTable
        {
            public IList<object> Columns { get; set; }
            public IList<object[]> Rows { get; protected set; }

            public ConsoleTableOptions Options { get; protected set; }

            public ConsoleTable(params string[] columns)
                : this(new ConsoleTableOptions { Columns = new List<string>(columns) })
            {
            }

            public ConsoleTable(ConsoleTableOptions options)
            {
                Options = options ?? throw new ArgumentNullException("options");
                Rows = new List<object[]>();
                Columns = new List<object>(options.Columns);
            }

            public ConsoleTable AddColumn(IEnumerable<string> names)
            {
                foreach (var name in names)
                    Columns.Add(name);
                return this;
            }

            public ConsoleTable AddRow(params object[] values)
            {
                if (values == null)
                    throw new ArgumentNullException(nameof(values));

                if (!Columns.Any())
                    throw new Exception("Please set the columns first");

                if (Columns.Count != values.Length)
                    throw new Exception(
                        $"The number columns in the row ({Columns.Count}) does not match the values ({values.Length}");

                Rows.Add(values);
                return this;
            }

            public static ConsoleTable From<T>(IEnumerable<T> values)
            {
                var table = new ConsoleTable();

                var columns = GetColumns<T>();

                table.AddColumn(columns);

                foreach (var propertyValues in values.Select(value => columns.Select(column => GetColumnValue<T>(value, column))))
                    table.AddRow(propertyValues.ToArray());

                return table;
            }

            public override string ToString()
            {
                var builder = new StringBuilder();

                // find the longest column by searching each row
                var columnLengths = ColumnLengths();

                // create the string format with padding
                var format = Enumerable.Range(0, Columns.Count)
                    .Select(i => " | {" + i + ",-" + columnLengths[i] + "}")
                    .Aggregate((s, a) => s + a) + " |";

                // find the longest formatted line
                var maxRowLength = Math.Max(0, Rows.Any() ? Rows.Max(row => string.Format(format, row).Length) : 0);
                var columnHeaders = string.Format(format, Columns.ToArray());

                // longest line is greater of formatted columnHeader and longest row
                var longestLine = Math.Max(maxRowLength, columnHeaders.Length);

                // add each row
                var results = Rows.Select(row => string.Format(format, row)).ToList();

                // create the divider
                var divider = " " + string.Join("", Enumerable.Repeat("-", longestLine - 1)) + " ";

                builder.AppendLine(divider);
                builder.AppendLine(columnHeaders);

                foreach (var row in results)
                {
                    builder.AppendLine(divider);
                    builder.AppendLine(row);
                }

                builder.AppendLine(divider);

                if (Options.EnableCount)
                {
                    builder.AppendLine("");
                    builder.AppendFormat("So Luong: {0}", Rows.Count);
                }

                return builder.ToString();
            }

            public string ToMarkDownString()
            {
                return ToMarkDownString('|');
            }

            private string ToMarkDownString(char delimiter)
            {
                var builder = new StringBuilder();

                // find the longest column by searching each row
                var columnLengths = ColumnLengths();

                // create the string format with padding
                var format = Format(columnLengths, delimiter);

                // find the longest formatted line
                var columnHeaders = string.Format(format, Columns.ToArray());

                // add each row
                var results = Rows.Select(row => string.Format(format, row)).ToList();

                // create the divider
                var divider = Regex.Replace(columnHeaders, @"[^|]", "-");

                builder.AppendLine(columnHeaders);
                builder.AppendLine(divider);
                results.ForEach(row => builder.AppendLine(row));

                return builder.ToString();
            }

            public string ToMinimalString()
            {
                return ToMarkDownString(char.MinValue);
            }

            public string ToStringAlternative()
            {
                var builder = new StringBuilder();

                // find the longest column by searching each row
                var columnLengths = ColumnLengths();

                // create the string format with padding
                var format = Format(columnLengths);

                // find the longest formatted line
                var columnHeaders = string.Format(format, Columns.ToArray());

                // add each row
                var results = Rows.Select(row => string.Format(format, row)).ToList();

                // create the divider
                var divider = Regex.Replace(columnHeaders, @"[^|]", "-");
                var dividerPlus = divider.Replace("|", "+");

                builder.AppendLine(dividerPlus);
                builder.AppendLine(columnHeaders);

                foreach (var row in results)
                {
                    builder.AppendLine(dividerPlus);
                    builder.AppendLine(row);
                }
                builder.AppendLine(dividerPlus);

                return builder.ToString();
            }

            private string Format(List<int> columnLengths, char delimiter = '|')
            {
                var delimiterStr = delimiter == char.MinValue ? string.Empty : delimiter.ToString();
                var format = (Enumerable.Range(0, Columns.Count)
                    .Select(i => " " + delimiterStr + " {" + i + ",-" + columnLengths[i] + "}")
                    .Aggregate((s, a) => s + a) + " " + delimiterStr).Trim();
                return format;
            }

            private List<int> ColumnLengths()
            {
                var columnLengths = Columns
                    .Select((t, i) => Rows.Select(x => x[i])
                        .Union(Columns)
                        .Where(x => x != null)
                        .Select(x => x.ToString().Length).Max())
                    .ToList();
                return columnLengths;
            }

            public void Write(Format format = ConsoleTables.Format.Default)
            {
                switch (format)
                {
                    case ConsoleTables.Format.Default:
                        Console.WriteLine(ToString());
                        break;
                    case ConsoleTables.Format.MarkDown:
                        Console.WriteLine(ToMarkDownString());
                        break;
                    case ConsoleTables.Format.Alternative:
                        Console.WriteLine(ToStringAlternative());
                        break;
                    case ConsoleTables.Format.Minimal:
                        Console.WriteLine(ToMinimalString());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }
            }

            private static IEnumerable<string> GetColumns<T>()
            {
                return typeof(T).GetProperties().Select(x => x.Name).ToArray();
            }

            private static object GetColumnValue<T>(object target, string column)
            {
                return typeof(T).GetProperty(column).GetValue(target, null);
            }
        }

        public class ConsoleTableOptions
        {
            public IEnumerable<string> Columns { get; set; } = new List<string>();
            public bool EnableCount { get; set; } = true;
        }

        public enum Format
        {
            Default = 0,
            MarkDown = 1,
            Alternative = 2,
            Minimal = 3
        }
    }
}

namespace SimpleCMenu.Menu
{
    public class MenuItem
    {
        #region Properties

        public int ID { get; set; }
        public string Text { get; set; }
        public Action Action { get; set; }

        #endregion

        #region Constructors

        public MenuItem()
        {

        }

        public MenuItem(int id, string text, Action action)
        {
            this.ID = id;
            this.Text = text;
            this.Action = action;
        }

        #endregion
    }
    public class ConsoleMenu
    {
        #region Public var

        public ConsoleMenu ParentMenu { get; set; }

        public string Header { get; set; }
        public string SubTitle { get; set; }
        public string CursorText { get; set; }
        public ConsoleColor CursorColor { get; set; }
        public ConsoleColor HeaderColor { get; set; }
        public ConsoleColor ForeColor { get; set; }
        public ConsoleColor MenuItemColor { get; set; }
        public ConsoleColor SubTitleColor { get; set; }

        #endregion

        #region Private var

        private List<MenuItem> menuItemList;

        private int cursor;
        private bool exit;

        #endregion

        #region Constructor

        public ConsoleMenu(string cursorText = "->")
        {
            menuItemList = new List<MenuItem>();
            cursor = 0;

            this.CursorText = cursorText;
            CursorColor = ConsoleColor.Yellow;
            HeaderColor = ConsoleColor.Blue;
            ForeColor = ConsoleColor.Green;
            MenuItemColor = ConsoleColor.White;
            SubTitleColor = ConsoleColor.White;
        }

        #endregion 

        #region Public methods

        public bool addMenuItem(int id, string text, Action action)
        {
            // check if it dosen't already exists
            if (!menuItemList.Any(item => item.ID == id))
            {
                menuItemList.Add(new MenuItem(id, text, action));
                return true;
            }
            return false;
        }

        public bool removeMenuItem(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Virtual methods

        public virtual void drawHeader()
        {
            Console.ForegroundColor = HeaderColor;
            Console.WriteLine(Header);
            Console.ForegroundColor = ForeColor;
        }

        public virtual void drawWithHeader()
        {
            drawHeader();
            draw();
        }

        public virtual void draw()
        {
            Console.WriteLine(SubTitle);

            for (int i = 0; i < menuItemList.Count; i++)
            {
                if (i == cursor)
                {
                    Console.ForegroundColor = CursorColor;
                    Console.Write(CursorText + " ");
                    Console.WriteLine(menuItemList[i].Text);
                    Console.ForegroundColor = ForeColor;
                }
                else
                {
                    Console.Write(new string(' ', (CursorText.Length + 1)));
                    Console.WriteLine(menuItemList[i].Text);
                }
            }
        }

        public virtual void clear()
        {
            Console.Clear();
        }

        public virtual void clearWithoutHeader()
        {
            Console.Clear();
            drawHeader();
        }

        public virtual void showMenu()
        {
            Console.CursorVisible = false;
            Console.Clear();
            drawWithHeader();
            exit = false;
            while (!exit)
            {
                updateMenu();
            }
        }

        public virtual void hideMenu()
        {
            exit = true;
        }

        public virtual void updateMenu()
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    {
                        if (cursor > 0)
                        {
                            cursor--;
                            Console.Clear();
                            drawWithHeader();
                        }
                    }
                    break;
                case ConsoleKey.DownArrow:
                    {
                        if (cursor < (menuItemList.Count - 1))
                        {
                            cursor++;
                            Console.Clear();
                            drawWithHeader();
                        }
                    }
                    break;
                case ConsoleKey.Escape:
                    {
                        if (ParentMenu != null)
                        {

                            hideMenu();
                        }
                    }
                    break;
                case ConsoleKey.Enter:
                    {
                        Console.Clear();
                        drawHeader();
                        Console.CursorVisible = true;
                        menuItemList[cursor].Action();
                        Console.CursorVisible = false;
                        Console.Clear();
                        drawWithHeader();
                    }
                    break;
                default:
                    {
                        // Unsuported key. Do nothing....
                    }
                    break;
            }
        }

        #endregion
    }
}
