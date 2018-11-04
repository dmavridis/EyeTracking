using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EyeXFramework;
using EyeXFramework.Wpf;
using Tobii.EyeX.Framework;
using System.ComponentModel;
using Tobii.Interaction;

namespace EyeTrackingWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<double> PointsX;
        private List<double> PointsY;
        private Window1 w1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".jpg";
            if (openFileDialog.ShowDialog() == true)
                textBox.Text = openFileDialog.FileName;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if ((DataContext as MainWindowModel).IsTrackingGaze && textBox.Text.Length > 0 && textBox.Text.Contains("jpg"))
            {
                w1 = new Window1(textBox.Text);
                w1.Show();
                (DataContext as MainWindowModel).eye = w1.eye;
                var host = (DataContext as MainWindowModel)._eyeXHost;
                var gazePointDataStream = host.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);
                PointsX = new List<double>();
                PointsY = new List<double>();
                var max_y = host.ScreenBounds.Value.Height;
                var max_x = host.ScreenBounds.Value.Width;   
                gazePointDataStream.Next += ((obj, args) => {               
                    PointsX.Add(args.X); PointsY.Add(args.Y);
                    int avnum = Math.Min(10, PointsX.Count);
                    Canvas.SetLeft(w1.eye, PointsX.GetRange(PointsX.Count - avnum, avnum).Sum()/avnum);
                    Canvas.SetTop(w1.eye, PointsY.GetRange(PointsX.Count - avnum, avnum).Sum() / avnum);     
                });
            }
            else
            {
                if (textBox.Text.Length == 0)
                {
                    MessageBox.Show("Please select file.");
                }
                else
                {
                    MessageBox.Show("Can't find gaze");
                }
            }
                
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var bubbles = new Dictionary<int[],int>(new MyEqualityComparer());
            int range = 6;
            for (int c = 0; c < PointsX.Count; c++)
            {
                var x = Convert.ToInt32(Math.Floor(PointsX[c] / range)) * range + range/2;
                var y = Convert.ToInt32(Math.Floor(PointsY[c] / range)) * range + range/2;
                if (bubbles.ContainsKey(new [] { x, y }))
                {
                    bubbles[new[] { x, y }] += 1;
                }
                else
                {
                    bubbles.Add(new[] { x, y }, 1);
                }
            }
            w1.ShowBubbles(bubbles.Keys.ToList(), bubbles.Values.ToList());
        }
    }
}
public class MyEqualityComparer : IEqualityComparer<int[]>
{
    public bool Equals(int[] x, int[] y)
    {
        if (x.Length != y.Length)
        {
            return false;
        }
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i])
            {
                return false;
            }
        }
        return true;
    }

    public int GetHashCode(int[] obj)
    {
        int result = 17;
        for (int i = 0; i < obj.Length; i++)
        {
            unchecked
            {
                result = result * 23 + obj[i];
            }
        }
        return result;
    }
}