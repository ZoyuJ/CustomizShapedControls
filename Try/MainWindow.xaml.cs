namespace Try {
  using System;
  using System.Collections.Generic;
  using System.Globalization;
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

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }
  }

  public static class MyHelper {
    //Get the parent of an item.
    public static T FindParent<T>(FrameworkElement current)
      where T : FrameworkElement {
      do {
        current = VisualTreeHelper.GetParent(current) as FrameworkElement;
        if (current is T) {
          return (T)current;
        }
      }
      while (current != null);
      return null;
    }

    //Get the rotation angle from the value
    public static double GetAngle(double value, double maximum, double minimum) {
      double current = (value / (maximum - minimum)) * 360;
      if (current == 360)
        current = 359.999;

      return current;
    }

    //Get the rotation angle from the position of the mouse
    public static double GetAngleR(Point pos, double radius) {
      //Calculate out the distance(r) between the center and the position
      Point center = new Point(radius, radius);
      double xDiff = center.X - pos.X;
      double yDiff = center.Y - pos.Y;
      double r = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

      //Calculate the angle
      double angle = Math.Acos((center.Y - pos.Y) / r);
      Console.WriteLine("r:{0},y:{1},angle:{2}.", r, pos.Y, angle);
      if (pos.X < radius)
        angle = 2 * Math.PI - angle;
      if (Double.IsNaN(angle))
        return 0.0;
      else
        return angle;
    }
  }
  public class ValueAngleConverter : IMultiValueConverter {
    #region IMultiValueConverter Members

    public object Convert(object[] values, Type targetType, object parameter,
                  System.Globalization.CultureInfo culture) {
      double value = (double)values[0];
      double minimum = (double)values[1];
      double maximum = (double)values[2];

      return MyHelper.GetAngle(value, maximum, minimum);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
          System.Globalization.CultureInfo culture) {
      throw new NotImplementedException();
    }

    #endregion
  }

  //Convert the value to text.
  public class ValueTextConverter : IValueConverter {

    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter,
              System.Globalization.CultureInfo culture) {
      double v = (double)value;
      return String.Format("{0:F2}", v);
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture) {
      throw new NotImplementedException();
    }

    #endregion
  }
  public sealed class BorderThicknessToStorkeThickness : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      try { return ((Thickness)value).Left; } catch { return 1.0; }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
