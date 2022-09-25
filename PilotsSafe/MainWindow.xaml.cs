using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace PilotsSafe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const int MAX_SIZE = 10;

        private List<Button> _listButton = new List<Button>();

        private string _textInTextBox = $"Введите размерность до {MAX_SIZE}";

        private DispatcherTimer _timer = new DispatcherTimer();

        private int _countMixing = 0;

        public int CurrentSize { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            TextBox1.Text = _textInTextBox;

            _timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            _timer.Tick += _timer_Tick;

            GenerateHilt(4);

        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            var rand = new Random();
            var value = rand.Next(0, CurrentSize);

            Button btn;
            if (_countMixing % 2 == 0)
                btn = _listButton.First(x => ((State)x.Tag).Column == value);
            else
                btn = _listButton.First(x => ((State)x.Tag).Row == value);

            Button_Click(btn, null);

            if (++_countMixing > 3)
            {
                _timer.Stop();
                _countMixing = 0;
            }

        }



        private void GenerateHilt(int size)
        {
            CurrentSize = size;
            _listButton.Clear();
            Grid1.ColumnDefinitions.Clear();
            Grid1.RowDefinitions.Clear();
            Grid1.Children.Clear();

            for (int i = 0; i < size; i++)
            {
                var col = new ColumnDefinition();
                col.Width = new System.Windows.GridLength(60);
                Grid1.ColumnDefinitions.Add(col);

                var row = new RowDefinition();
                row.Height = new System.Windows.GridLength(60);
                Grid1.RowDefinitions.Add(row);

            }


            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    Button button = new Button() { Width = 20, Height = 40, Background = new SolidColorBrush(Colors.Blue) };
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    button.Click += Button_Click;

                    button.Tag = new State(i, j, false);
                    _listButton.Add(button);
                    Grid1.Children.Add(button);
                }

            _timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            var changeBtns = _listButton.Where(x => ((State)x.Tag).Column == ((State)btn.Tag).Column || ((State)x.Tag).Row == ((State)btn.Tag).Row);
            foreach (var button in changeBtns)
            {
                if (button.RenderTransform is RotateTransform rotateTransform)
                {
                    if (rotateTransform.Angle == 90)
                    {
                        button.RenderTransform = new RotateTransform(0, 0, 0);
                        ((State)button.Tag).IsTurn = false;
                    }
                    else
                    {
                        button.RenderTransform = new RotateTransform(90, button.Width / 2, button.Height / 2);
                        ((State)button.Tag).IsTurn = true;
                    }
                }
                else
                {
                    button.RenderTransform = new RotateTransform(90, button.Width / 2, button.Height / 2);
                    ((State)button.Tag).IsTurn = true;
                }
            }

            if (e != null && (_listButton.All(x => ((State)x.Tag).IsTurn == false) || _listButton.All(x => ((State)x.Tag).IsTurn == true)))
                MessageBox.Show("Поздравляем! Вы победили!");
        }

        #region Работа с TextBox

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                int size = Int32.Parse(((TextBox)sender).Text);
                if (size > MAX_SIZE)
                {
                    MessageBox.Show($"Введите размерность до {MAX_SIZE}!");
                    return;
                }
                GenerateHilt(size);

            }
        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void TextBox1_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (((TextBox)sender).Text == _textInTextBox)
            {
                TextBox1.Text = "";
                TextBox1.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBox1_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                TextBox1.Text = _textInTextBox;
                TextBox1.Foreground = new SolidColorBrush(Colors.DimGray);
            }
        }

        #endregion
    }
}
