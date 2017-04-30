using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MagicSquare
{
    public partial class MainWindow : Window
    {
        [STAThread]
        public static void Main()
        {
            var window = new MainWindow();
            var app = new Application();
            app.Run(window);
        }

        public MainWindow()
        {
            InitializeComponent();
            Init();
            KeyUp += MainWindow_KeyUp;
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        Border[,] m;
        int selectedColumn = 1;
        int selectedRow = 1;

        int width = 5;
        int height = 5;
        int F(int i, int j) => i + j;
        string operation = "+";

        Brush emptyBrush = Brushes.AliceBlue;
        Brush emptyRowBrush = Brushes.MintCream;
        Brush emptyColumnBrush = Brushes.MintCream;
        Brush selectedColumnBrush = Brushes.LavenderBlush;
        Brush selectedRowBrush = Brushes.LavenderBlush;
        Brush selectedHeaderColumnBrush = Brushes.Yellow;
        Brush selectedHeaderRowBrush = Brushes.Yellow;
        Brush answerBrush = Brushes.Yellow;

        private void Init()
        {
            m = new Border[width + 1, height + 1];

            matrix.ColumnDefinitions.Add(new ColumnDefinition());
            matrix.RowDefinitions.Add(new RowDefinition());

            for (int i = 1; i < width + 1; i++)
            {
                matrix.ColumnDefinitions.Add(new ColumnDefinition());
                var cell = GetCell(i.ToString());
                var captured = i;
                cell.MouseDown += (o, e) => SelectColumn(captured);
                Grid.SetColumn(cell, i);
                matrix.Children.Add(cell);
                m[i, 0] = cell;
            }

            for (int i = 1; i < height + 1; i++)
            {
                matrix.RowDefinitions.Add(new RowDefinition());
                var cell = GetCell(i.ToString());
                var captured = i;
                cell.MouseDown += (o, e) => SelectRow(captured);
                Grid.SetRow(cell, i);
                matrix.Children.Add(cell);
                m[0, i] = cell;
            }

            for (int i = 1; i < width + 1; i++)
            {
                for (int j = 1; j < height + 1; j++)
                {
                    var text = F(i, j).ToString();
                    var cell = GetCell(text);
                    var ci = i;
                    var cj = j;
                    cell.MouseDown += (o, e) =>
                    {
                        SelectColumn(ci);
                        SelectRow(cj);
                    };
                    Grid.SetColumn(cell, i);
                    Grid.SetRow(cell, j);
                    matrix.Children.Add(cell);
                    m[i, j] = cell;
                }
            }

            UpdateColors();
        }

        private void SelectColumn(int c)
        {
            selectedColumn = c;
            UpdateColors();
        }

        private void SelectRow(int c)
        {
            selectedRow = c;
            UpdateColors();
        }

        private void UpdateColors()
        {
            for (int i = 1; i < width + 1; i++)
            {
                if (i == selectedColumn)
                {
                    m[i, 0].Background = selectedHeaderColumnBrush;
                }
                else
                {
                    m[i, 0].Background = emptyColumnBrush;
                }
            }

            for (int j = 1; j < height + 1; j++)
            {
                if (j == selectedRow)
                {
                    m[0, j].Background = selectedHeaderRowBrush;
                }
                else
                {
                    m[0, j].Background = emptyRowBrush;
                }
            }

            for (int i = 1; i < width + 1; i++)
            {
                for (int j = 1; j < height + 1; j++)
                {
                    var b = emptyBrush;
                    if (i == selectedColumn)
                    {
                        if (j == selectedRow)
                        {
                            b = answerBrush;
                        }
                        else if (j > selectedRow)
                        {
                            b = emptyBrush;
                        }
                        else
                        {
                            b = selectedColumnBrush;
                        }
                    }
                    else if (i > selectedColumn)
                    {
                        b = emptyBrush;
                    }
                    else
                    {
                        if (j == selectedRow)
                        {
                            b = selectedRowBrush;
                        }
                        else 
                        {
                            b = emptyBrush;
                        }
                    }

                    m[i, j].Background = b;
                }
            }

            answerText.Text = $"{selectedRow} {operation} {selectedColumn} = {F(selectedRow, selectedColumn)}";
        }

        private static Border GetCell(string text)
        {
            var border = new Border()
            {
                Margin = new Thickness(4),
            };
            var textBlock = new TextBlock
            {
                Text = text,
                Foreground = Brushes.DarkGray,
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            border.Child = textBlock;
            return border;
        }
    }
}
