using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;



namespace WpfPainter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void paint_Click(object sender, RoutedEventArgs e)
        {
          // creat paint window
            Window1 paintwin = new Window1();
            paintwin.Show();
          //close the main windows
            this.Close();
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            MainWindow.DragOverEvent();
        }

        private static void DragOverEvent()
        {
            MainWindow.DragOverEvent();
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            MainWindow.DragEnterEvent();
        }

        private static void DragEnterEvent()
        {
            throw new NotImplementedException();
        }

        private void Window_Activated(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 360;
            da.Duration = new Duration(TimeSpan.FromSeconds(3));
            
            RotateTransform rt = new RotateTransform();
            button2.RenderTransform = rt;
           
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
           
        }

        private void button2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void button2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 30;
            da.To = 100;
            da.AutoReverse = true;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            button2.BeginAnimation(Button.HeightProperty, da);
        }

        private void Win_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
    }
}
