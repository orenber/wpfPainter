using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Media.Animation;


namespace WpfPainter
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        #region File Menu

        #region Clear image

        // Clear the image
        private void butclear_Click(object sender, RoutedEventArgs e)
        {

            myCanvas.Children.Clear();
            myCanvas.Background = Brushes.White;


        }
       

        private void butclear_Checked(object sender, RoutedEventArgs e)
        {
            Pen.IsChecked = false;
            butrect.IsChecked = false;
            butcricle.IsChecked = false;
        }
        #endregion

        //save the file to image file

        #region Save Image
        private System.Drawing.Bitmap resultBitmap = null;
        private System.Drawing.Imaging.ImageFormat imgFormat;

        private void butsave_Click(object sender, RoutedEventArgs e)
        {
           System.Windows.Forms.SaveFileDialog SaveImageFile = new System.Windows.Forms.SaveFileDialog();
           SaveImageFile.Title = "Specify a file name and file path";
           SaveImageFile.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
           SaveImageFile.Filter += "|Bitmap Images(*.bmp)|*.bmp";
                if (SaveImageFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    string fileExtension = System.IO.Path.GetExtension(SaveImageFile.FileName).ToUpper();
                    System.Drawing.Imaging.ImageFormat imgFormat = System.Drawing.Imaging.ImageFormat.Png;

                    if (fileExtension == "BMP")
                    {
                        imgFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                    }
                    else if (fileExtension == "JPG")
                    {
                        imgFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    }

                    StreamWriter streamWriter = new StreamWriter(SaveImageFile.FileName, false);
                    resultBitmap.Save(streamWriter.BaseStream, imgFormat);
                    streamWriter.Flush();
                    streamWriter.Close();

                    resultBitmap = null;
               
                   
                }
        }
        private void CreateSaveBitmap1(string filename)
        {
            
        }
        #endregion

        private ImageBrush ib;

        // Open file image
        #region Open File image
        private void butopen_Click(object sender, RoutedEventArgs e)
        {
           System.Windows.Forms.OpenFileDialog openFileDialog1=new System.Windows.Forms.OpenFileDialog();
           openFileDialog1.Title = "Select Photos";
           
           openFileDialog1.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

           openFileDialog1.CheckFileExists = true;
           openFileDialog1.CheckPathExists = true;
          
           
          
            Stream myStream = null;
         
           

           

      
          
          if(openFileDialog1.ShowDialog() ==  System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        if ((myStream = openFileDialog1.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                string file = openFileDialog1.FileName;
                                ib = new ImageBrush();
                                ib.ImageSource = new BitmapImage(new Uri(file, UriKind.Relative));
                                myCanvas.Background = ib;
                                myCanvas.Children.Clear();
                               

                            }
                        }
                    }
        catch (Exception ex)
        {
            MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
        }
    }
        }
        #endregion 

        //Exite the program
        #region Exit
        private void butclose_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        #endregion

        #endregion

        #region Shape Menu

        #region Pen evants

        private Brush _previousFill = null;
        //Pen button click
        private Pen mypen;
        private void pen_Click(object sender, RoutedEventArgs e)
        {
            mypen = new Pen();
            mypen.Thickness = colorpiker.StrokeThickness;
            mypen.StartLineCap = 0;
           
        }

        private Point _startPoint;
        private Polyline _line;

        private void Pen_Checked(object sender, RoutedEventArgs e)
        {
            butclear.IsChecked = false;
            butrect.IsChecked = false;
            butcricle.IsChecked = false;
            butline.IsChecked = false;
        }

        #endregion

        #region Elipse Evants
        private void button1_Checked(object sender, RoutedEventArgs e)
        {
            butclear.IsChecked = false;
            butrect.IsChecked = false;
            Pen.IsChecked = false;
            butline.IsChecked = false;

            // creat new elipse
            Ellipse eli = new Ellipse();

            eli.Width = slider1.Value;
            eli.Height = slider2.Value;

            SolidColorBrush myBruse = new SolidColorBrush();
            eli.Stroke = colorpiker.Stroke;
            Brush mybruse = colorpiker.Fill;
            eli.StrokeThickness = colorpiker.StrokeThickness;

            eli.Fill = mybruse;

            // create animation 
            if (ani.IsChecked == true)
            {
                ani.Click += ani_Checked;
            }

            // add event to the new rect
            Add_event_to_shape(eli);
            myCanvas.Children.Insert(0, eli);

        }

        private Shape Add_event_to_shape(Shape object_Shape)
        {
            object_Shape.MouseLeftButtonDown += rect_MouseLeftButtonDown;
            object_Shape.MouseLeftButtonUp += rect_MouseLeftButtonUp;
            object_Shape.MouseMove += rect_MouseMove;
            return object_Shape;
        }

        #endregion
        #region Rectangle evants

        private void button2_Checked(object sender, RoutedEventArgs e)
        {

            butclear.IsChecked = false;
            Pen.IsChecked = false;
            butcricle.IsChecked = false;
            butline.IsChecked = false;

            // Create new rectengle
            
            rect= new Rectangle();
            rect.Visibility = Visibility.Visible;
            rect.Width = slider1.Value;
            rect.Height = slider2.Value;
            Brush myBrush = colorpiker.Fill;
            rect.Stroke = colorpiker.Stroke;
            rect.StrokeThickness = colorpiker.StrokeThickness;
            rect.Fill = myBrush;

           // creat animation 

            if (ani.IsChecked==true)
            {
                ani.Click += ani_Checked;
            
            }

            //  Creat event to the new rect
            Add_event_to_shape(rect);
            myCanvas.Children.Insert(0, rect);
        }
        private bool _isRectDragInProg;

        private void rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                _isRectDragInProg = true;
                if (butrect.IsChecked == true)
                {
                    Rectangle rect = sender as Rectangle;
                    rect.CaptureMouse();
                    
                }
                if (butcricle.IsChecked == true)
                {
                    Ellipse eli = sender as Ellipse;

                    eli.CaptureMouse();
                     
                }
            }
            catch
            {
                return;
            }
        }

        private void rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if (butrect.IsChecked == true)
                {
                    Rectangle rect = sender as Rectangle;
                    _isRectDragInProg = false;
                    rect.ReleaseMouseCapture();

                }
                else if (butcricle.IsChecked == true)
                {
                    Ellipse eli = sender as Ellipse;
                    _isRectDragInProg = false;
                    eli.ReleaseMouseCapture();
                }
            }

            catch
            {
                return;
            }



        }

        private void rect_MouseMove(object sender, MouseEventArgs e)
        {

            if (!_isRectDragInProg) return;
            try
            {
                // get the position of the mouse relative to the Canvas
                var mousePos = e.GetPosition(myCanvas);
                if (butcricle.IsChecked == true)
                {
                    Ellipse eli = sender as Ellipse;
                    // center the rect on the mouse
                    double left = mousePos.X - (eli.ActualWidth / 2);
                    double top = mousePos.Y - (eli.ActualHeight / 2);
                    Canvas.SetLeft(eli, left);
                    Canvas.SetTop(eli, top);
                }
                else if (butrect.IsChecked == true)
                {

                    Rectangle rect = sender as Rectangle;
                    // center the rect on the mouse
                    double left = mousePos.X - (rect.ActualWidth / 2);
                    double top = mousePos.Y - (rect.ActualHeight / 2);
                    Canvas.SetLeft(rect, left);
                    Canvas.SetTop(rect, top);

                }
            }
            catch
            {
                return;

            }


        }
        #endregion

        #endregion

        // slider evant -Select Size Shape

        #region Slider

        #endregion

        //  Color picker-Click the rectangle to select color

        #region Color picker

        private void recblack_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _line = new Polyline();
            _line.Stroke = new SolidColorBrush(Colors.Black);

        }

        #endregion

        // canvas-
        #region Canvas evant
        private Shape shape; 
        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {

            Point curentpoint = Mouse.GetPosition(myCanvas);
            xy.Content = Convert.ToString("X=" + Math.Floor(curentpoint.X) + "," + "Y=" + Math.Floor(curentpoint.Y));
            if (Pen.IsChecked == true)
            {


                if (e.LeftButton == MouseButtonState.Pressed)
                {

                    if (_startPoint != curentpoint)
                    {
                        _line.Points.Add(curentpoint);
                    }
                }
            }
                // control the size and the location of the rect by the mouse

            else if (e.RightButton == MouseButtonState.Pressed && MouseRightButtonDown==true )
            {

                if (butrect.IsChecked == true)
                {
                    shape = rect;
                }
                else if (butcricle.IsChecked == true)
                {
                    shape = eli;
                }

                var x = startpoint.X;
                var y = startpoint.Y;

                var w = (curentpoint.X - startpoint.X);
                var h = (startpoint.Y - curentpoint.Y);

                 shape.Width = Math.Abs(w);
                 shape.Height = Math.Abs(h);

                if (w < 0 && h < 0)
                {
                    Canvas.SetRight(shape,myCanvas.Width- x);
                    Canvas.SetTop(shape, y);  
                }

                else if (w < 0 && h > 0)
                {
                    Canvas.SetRight(shape,myCanvas.Width- x);
                    Canvas.SetBottom(shape,myCanvas.Height- y);
                    
                }

                else if (w > 0 && h > 0)
                {

                    Canvas.SetLeft(shape, x);
                    Canvas.SetBottom(shape,myCanvas.Height- y);
                   
                }
                else if (w > 0 && h < 0)
                {
                    Canvas.SetLeft(shape, x);
                    Canvas.SetTop(shape, y);   
                }
                else
                {
                   shape.Width = 0;
                   shape.Height = 0;
                 
                }
              
            }
            else if (butline.IsChecked == true && e.LeftButton == MouseButtonState.Pressed)
            {
                myline.X2 = curentpoint.X;
                myline.Y2 = curentpoint.Y;
            }

        }

        private Line myline;
        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(myCanvas);
            _line = new Polyline();
            _line.Stroke = colorpiker.Fill;
            _line.StrokeThickness = colorpiker.StrokeThickness;
            myCanvas.Children.Add(_line);

            #region Line

            if (butline.IsChecked == true)
            {
                startpoint = Mouse.GetPosition(myCanvas);
                myline = new Line()
                {

                    StrokeThickness = colorpiker.StrokeThickness,
                    Stroke = colorpiker.Fill,
                    X1 = startpoint.X,
                    Y1 = startpoint.Y
                };

                myCanvas.Children.Add(myline);

            }

            #endregion
        }

        //  When the mouse cursor entered into the canvas to select the tool varies depending on usage
        private void myCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            
            if (Pen.IsChecked == true)
            {
                myCanvas.Cursor = Cursors.Pen;
            }
         
            else
            {
                myCanvas.Cursor = Cursors.Cross;
            }
        }
        private void myCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            xy.Content = string.Empty;
        }

        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private bool MouseRightButtonDown;
        private Point startpoint;
        private Rectangle rect;
        private Ellipse eli;

        private void myCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (butrect.IsChecked==true)
            {

                MouseRightButtonDown = true;

                startpoint = Mouse.GetPosition(myCanvas);
                rect = new Rectangle()
                {
                    Stroke = colorpiker.Stroke,
                    StrokeThickness = colorpiker.StrokeThickness,
                    Fill = colorpiker.Fill
                };

                Add_event_to_shape(rect);
                myCanvas.Children.Add(rect);

            }

            if (butcricle.IsChecked == true)
            {

                MouseRightButtonDown = true;

                startpoint = Mouse.GetPosition(myCanvas);
                eli = new Ellipse()
                {
                    Stroke = colorpiker.Stroke,
                    StrokeThickness = colorpiker.StrokeThickness,
                    Fill = colorpiker.Fill
                };

                Add_event_to_shape(eli);
                myCanvas.Children.Add(eli);

            }
            if (Pen.IsChecked == true)
            {

                _line.Stroke = colorpiker.Stroke;

            }
        }

        #endregion

        private void myCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MouseRightButtonDown = false;
        }

        #region Animation

        private void ani_Checked(object sender, RoutedEventArgs e)
        {
            if (ani.IsChecked == true)
            {
                        
                if (butrect.IsChecked == true)
                { 
                    shape_animation(rect);
                }
                if (butcricle.IsChecked == true)
                {
                    shape_animation(eli);
                }
            }
        }

        private void shape_animation(Shape shape_object)
        {
            var da = new DoubleAnimation(360, 0, new Duration(TimeSpan.FromSeconds(1)));
            var rt = new RotateTransform();

            shape_object.RenderTransform = rt;
            shape_object.RenderTransformOrigin = new Point(0.5, 0.5);
            da.RepeatBehavior = RepeatBehavior.Forever;
            rt.BeginAnimation(RotateTransform.AngleProperty, da);

        }

        #endregion

        private void slider_strok_stikness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            colorpiker.StrokeThickness = slider_strok_stikness.Value;
        }

        private void butline_Checked(object sender, RoutedEventArgs e)
        {
            butcricle.IsChecked=false;
            butrect.IsChecked = false;
            Pen.IsChecked = false;
            butclear.IsChecked = false;

        }

        private void paintwin_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                {
                    base.OnMouseLeftButtonDown(e);

                    // Begin dragging the window
                    this.DragMove();
                }
            }
            catch
            {
                return;
            }

        }

        #region Color piker

        private void colorpiker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.ColorDialog ColorColection =new System.Windows.Forms.ColorDialog();
            ColorColection.ShowDialog();
            if (ColorColection.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
          
                
            }
        }

        private void recColorRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            {
                Rectangle rb = sender as Rectangle;
                if (rb != null)
                {
                    colorpiker.Fill = rb.Fill;
                }
            }
        }

        private void RecStroColor_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            {
                Rectangle rb = sender as Rectangle;
                if (rb != null)
                {
                    colorpiker.Stroke = rb.Fill;
                }
            }
        }
        #endregion
        private int num = 0;

        #region  Slider Strok
        private void slider_strok_stikness_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            if (e.Delta > 0)
            {
                num = num + 1;
                slider_strok_stikness.Value = num;
            }
            else if (e.Delta < 0)
            {

                num = num - 1;
                slider_strok_stikness.Value = num;
            }
        }

        private void myCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                num = num + 1;
                slider_strok_stikness.Value = num;
            }
            else if (e.Delta < 0)
            {

                num = num -1 ;
                slider_strok_stikness.Value = num;
            }

        }
        #endregion

    }
}

