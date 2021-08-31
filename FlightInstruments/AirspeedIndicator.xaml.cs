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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MSFS.Planes;

namespace FlightInstruments
{
    /// <summary>
    /// Interaction logic for AirspeedIndicator.xaml
    /// </summary>
    public partial class AirspeedIndicator : UserControl
    {
        private DispatcherTimer timer = new DispatcherTimer();
        //private double SpeedOffset = -163; //for starting below 30
        private double AirSpeedGagueOffset = 165;
        private double AirSpeedRangeOffset = 165;
        private int minSpeed = 20; //This will be set per plane. For now set to 20 as per G100 Manual. Independant of 
        private int maxSpeed = 330;
        private double LastSpeedIndicated = 0;
        private double pixelOffset = 5.4887;
        public AirspeedIndicator()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += new EventHandler(OnTick);
            timer.Start();

            //Populate Airspeed Gagues up to max speed
            //1760px for 345Max speed. 25.507px per 5 KTS
            AirspeedGague.Height = (110 * (maxSpeed / 20));
            AirSpeedGagueOffset = -AirspeedGague.Height + AirSpeedGagueOffset;
            Canvas.SetTop(AirspeedGague, AirSpeedGagueOffset);//Add +3 to fix offset opf image
            for (int i = 0; i < maxSpeed; i += 20)
            {
                Image Img = new Image();
                Img.Source = new BitmapImage(new Uri("pack://application:,,,/FlightInstruments;component/Images/AirspeedGague.png"));
                AirspeedGague.Children.Add(Img);
            }

            //Populate Airspeed Ranges
            int topPadding = 14;
            AirspeedRange.Height = (55 * ((maxSpeed / 10)));
            AirSpeedRangeOffset = -AirspeedRange.Height + AirSpeedRangeOffset + 18; //18 for middle of text object
            Canvas.SetTop(AirspeedRange, AirSpeedRangeOffset); //Add half of textblock offset
            for (int i = maxSpeed + 10; i > 10; i -= 10)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = i.ToString();
                textBlock.FontFamily = new FontFamily(new Uri("pack://application:,,,/FlightInstruments;component/Fonts/"), "./#Roboto Mono");
                textBlock.Foreground = new SolidColorBrush(Colors.White);
                textBlock.FontSize = 30;
                textBlock.HorizontalAlignment = HorizontalAlignment.Right;
                textBlock.Padding = new Thickness { Right = 7, Top = topPadding };
                textBlock.Height = 55;
                AirspeedRange.Children.Add(textBlock);
            }

            //Airspeed 100's
            AirspeedHundred.Height = (55 * ((maxSpeed / 10)));
            Canvas.SetTop(AirspeedHundred, AirSpeedRangeOffset); //Add half of textblock offset
            for (int i = maxSpeed + 10; i > 10; i -= 10)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = i.ToString();
                textBlock.FontFamily = new FontFamily(new Uri("pack://application:,,,/FlightInstruments;component/Fonts/"), "./#Roboto Mono");
                textBlock.Foreground = new SolidColorBrush(Colors.White);
                textBlock.FontSize = 30;
                textBlock.HorizontalAlignment = HorizontalAlignment.Right;
                textBlock.Padding = new Thickness { Right = 7, Top = topPadding };
                textBlock.Height = 55;
                AirspeedHundred.Children.Add(textBlock);
            }


            //Add V Speed Reference Colors
            //5.353 is 1KT per 1px
            VSpeedRef.Height = AirspeedGague.Height;
            Canvas.SetTop(VSpeedRef, AirSpeedGagueOffset);

            Rectangle VneRectangle = new Rectangle();
            VneRectangle.Width = 14;
            VneRectangle.Height = 54 +((maxSpeed - Data.Cesna172S().Vne) * pixelOffset); //maxspeed is always offset by 15
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/FlightInstruments;component/Images/Vne.png"));
            imageBrush.TileMode = TileMode.Tile;
            imageBrush.ViewportUnits = BrushMappingMode.Absolute;
            imageBrush.Viewport = new Rect(0, 0,25,100);
            VneRectangle.Fill = imageBrush;
            VSpeedRef.Children.Add(VneRectangle);

            Console.WriteLine(VSpeedRef.Height);

            //Add reference rectangle that will display over top of speed gague.
            VneRef.Height = AirspeedGague.Height;
            Canvas.SetTop(VneRef, AirSpeedGagueOffset);

            VneRectangle = new Rectangle();
            VneRectangle.Width = 14;
            VneRectangle.Height = 54 + ((maxSpeed - Data.Cesna172S().Vne) * pixelOffset);
            imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/FlightInstruments;component/Images/Vne.png"));
            imageBrush.TileMode = TileMode.Tile;
            imageBrush.ViewportUnits = BrushMappingMode.Absolute;
            imageBrush.Viewport = new Rect(0, 0, 25, 100);
            VneRectangle.Fill = imageBrush;
            VneRef.Children.Add(VneRectangle);

            //Yellow
            VneRectangle = new Rectangle();
            VneRectangle.Width = 14;
            VneRectangle.Height = (Data.Cesna172S().Vne - Data.Cesna172S().Vno) * pixelOffset;
            VneRectangle.Fill = new SolidColorBrush(Colors.Yellow);
            VSpeedRef.Children.Add(VneRectangle);

            //Green
            VneRectangle = new Rectangle();
            VneRectangle.Width = 14;
            VneRectangle.Height = (Data.Cesna172S().Vno - Data.Cesna172S().Vsl) * pixelOffset;
            VneRectangle.Fill = new SolidColorBrush(Colors.Green);
            VSpeedRef.Children.Add(VneRectangle);

            //Blank
            VneRectangle = new Rectangle();
            VneRectangle.Width = 14;
            VneRectangle.Height = (Data.Cesna172S().Vsl - Data.Cesna172S().Vso) * pixelOffset;
            VSpeedRef.Children.Add(VneRectangle);

            //Red
            VneRectangle = new Rectangle();
            VneRectangle.Width = 14;
            VneRectangle.Height = (Data.Cesna172S().Vso - 20) * pixelOffset;
            VneRectangle.Fill = new SolidColorBrush(Colors.Red);
            VSpeedRef.Children.Add(VneRectangle);
        }
        private void OnTick(object sender, EventArgs e)
        {
            //AirspeedIndicated.Content = SimVars.ID.airspeedIndicated;
            TrueAirSpeed.Text = "TAS " + (Convert.ToInt32(SimVars.ID.airspeedTrue) < 100 ? (" " + Convert.ToInt32(SimVars.ID.airspeedTrue)) : (Convert.ToInt32(SimVars.ID.airspeedTrue)).ToString()) + "KT";
            if (SimVars.ID.airspeedIndicated >= minSpeed)
            {
                //Idicators
                double currentAirSpeed = (((SimVars.ID.airspeedIndicated - 20) / 10) * 55);//51 pixels per 10mph +4 for bar width center
               
                //Airspeed Gague Animation
                NewStoryboard(AirspeedGague, LastSpeedIndicated + AirSpeedGagueOffset, currentAirSpeed + AirSpeedGagueOffset, "AirspeedGague", .1, new PropertyPath(Canvas.TopProperty));
                NewStoryboard(VneRef, LastSpeedIndicated + AirSpeedGagueOffset, currentAirSpeed + AirSpeedGagueOffset, "VneRef", .1, new PropertyPath(Canvas.TopProperty));
                NewStoryboard(VSpeedRef, LastSpeedIndicated + AirSpeedGagueOffset, currentAirSpeed + AirSpeedGagueOffset, "VSpeedRef", .1, new PropertyPath(Canvas.TopProperty));
               
                //Speed Range
                DoubleAnimation SpeedAnimation = new DoubleAnimation();
                SpeedAnimation.From = LastSpeedIndicated + AirSpeedRangeOffset;
                SpeedAnimation.To = currentAirSpeed + AirSpeedRangeOffset;
                SpeedAnimation.Duration = new Duration(TimeSpan.FromSeconds(.1));
                Storyboard.SetTargetName(SpeedAnimation, "AirspeedRange");
                Storyboard.SetTargetProperty(SpeedAnimation, new PropertyPath(Canvas.TopProperty));
                Storyboard SpeedStoryboard = new Storyboard();
                SpeedStoryboard.Children.Add(SpeedAnimation);
                SpeedStoryboard.Begin(AirspeedRange);

                //Speed Hundered
                DoubleAnimation SpeedAnimationHundered = new DoubleAnimation();
                SpeedAnimationHundered.From = ((int)(LastSpeedIndicated / 55)) * 55 + AirSpeedRangeOffset - 143;
                SpeedAnimationHundered.To = ((int)(currentAirSpeed / 55)) * 55 + AirSpeedRangeOffset - 143;
                SpeedAnimationHundered.Duration = new Duration(TimeSpan.FromSeconds(.2));
                Storyboard.SetTargetName(SpeedAnimationHundered, "AirspeedHundred");
                Storyboard.SetTargetProperty(SpeedAnimationHundered, new PropertyPath(Canvas.TopProperty));
                Storyboard SpeedHunderedStoryboard = new Storyboard();
                SpeedHunderedStoryboard.Children.Add(SpeedAnimationHundered);
                SpeedHunderedStoryboard.Begin(AirspeedHundred);


                LastSpeedIndicated = currentAirSpeed;
            }
            else
            {
                //Canvas.SetTop(Airspeed_3, -1154);
            }

            //recSpeed.BeginAnimation(Canvas.TopProperty, doubleAnimation);

            /*RollScale.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotateTransform = new RotateTransform(SimVars.ID.attitude_roll);
            RollScale.RenderTransform = rotateTransform;*/

            //If the overall speed is above Vne then change the speed indicator to RED
            if(SimVars.ID.airspeedIndicated > Data.Cesna172S().Vne)
            {
                IndicatedSpeedImg.Source = new BitmapImage(new Uri("pack://application:,,,/FlightInstruments;component/Images/IndicatedSpeedRed.png"));
            }
            else
            {
                IndicatedSpeedImg.Source = new BitmapImage(new Uri("pack://application:,,,/FlightInstruments;component/Images/IndicatedSpeed.png"));
            }
        }
               

        private void NewStoryboard(FrameworkElement Element, double From, double To, string TargetName, double Duration, PropertyPath propertyPath)
        {
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.From = From;
            Animation.To = To;
            Animation.Duration = new Duration(TimeSpan.FromSeconds(Duration));
            Storyboard.SetTargetName(Animation, TargetName);
            Storyboard.SetTargetProperty(Animation, propertyPath);
            Storyboard StoryboardAnimation = new Storyboard();
            StoryboardAnimation.Children.Add(Animation);
            StoryboardAnimation.Begin(Element);
        }
    }
}
