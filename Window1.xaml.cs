using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EyeTrackingWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        string image_url;
        public Image i;
        public Ellipse eye = new Ellipse { Width = 100, Height = 100 };
        public Window1(string file)
        {
            InitializeComponent();
            image_url = file;
            MaximizeToSecondaryMonitor();
        }

        public void MaximizeToSecondaryMonitor()
        {
            var secondaryScreen = Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (secondaryScreen != null)
            {
                var workingArea = secondaryScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                this.Margin = new Thickness(this.Left, this.Top, workingArea.Right, workingArea.Bottom);
            }
        }
        void CreateEllipse(double width, double height, double desiredCenterX, double desiredCenterY)
        {
            Ellipse ellipse = new Ellipse { Width = width, Height = height };
            double left = desiredCenterX - (width / 2);
            double top = desiredCenterY - (height / 2);
            ellipse.Fill = new SolidColorBrush() { Color = Color.FromRgb(0, 0, 255), Opacity = 0.2 };
            canvas.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);
            ellipse.Visibility = Visibility.Visible;
            Canvas.SetZIndex(ellipse, 10);
        }
        public void ShowBubbles(List<int[]> centres, List<int> sizes)
        {
            for (int j = 0; j < centres.Count; j++)
            {
                CreateEllipse(sizes[j], sizes[j], centres[j][0], centres[j][1]);
            }
        }
        public void ShowHeatmap(List<int[]> centres, List<int> sizes)
        {
            for (int j = 0; j < centres.Count; j++)
            {
                CreateEllipse(sizes[j], sizes[j], centres[j][0], centres[j][1]);
            }
        }




        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            i = new Image();
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(image_url, UriKind.Absolute);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            i.Source = src;
            i.Stretch = Stretch.Uniform;
            //int q = src.PixelHeight;        // Image loads here
            canvas.Children.Add(i);

            eye.Fill = new SolidColorBrush() { Color = Color.FromRgb(255, 0, 0), Opacity = 0.2 };
            canvas.Children.Add(eye);
            Canvas.SetZIndex(eye, 100);
        }
    }
}
