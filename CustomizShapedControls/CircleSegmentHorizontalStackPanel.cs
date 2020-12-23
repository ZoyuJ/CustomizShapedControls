using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace CustomizShapedControls {
  public class CircleSegmentHorizontalStackPanel : StackPanel {
    static CircleSegmentHorizontalStackPanel() {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(CircleSegmentHorizontalStackPanel), new FrameworkPropertyMetadata(typeof(CircleSegmentHorizontalStackPanel)));

    }


    //public Point RenderOrigin { get => (Point)GetValue(RenderOriginProperty); set => SetValue(RenderOriginProperty, value); }
    //public static readonly DependencyProperty RenderOriginProperty
    //  = DependencyProperty.Register("RenderOrigin", typeof(Point), typeof(CircleSegmentHorizontalStackPanel), new PropertyMetadata(new Point(0.0, 1.0)));
    //public double RenderScaleX { get => (double)GetValue(RenderScaleXProperty); set => SetValue(RenderScaleXProperty, value); }
    //public static readonly DependencyProperty RenderScaleXProperty
    //  = DependencyProperty.Register("RenderScaleX", typeof(double), typeof(CircleSegmentHorizontalStackPanel), new PropertyMetadata(1.0));
    //public double RenderScaleY { get => (double)GetValue(RenderScaleYProperty); set => SetValue(RenderScaleYProperty, value); }
    //public static readonly DependencyProperty RenderScaleYProperty
    //  = DependencyProperty.Register("RenderScaleY", typeof(double), typeof(CircleSegmentHorizontalStackPanel), new PropertyMetadata(1.0));
    //public double RenderAngle { get => (double)GetValue(RenderAngleProperty); set => SetValue(RenderAngleProperty, value); }
    //public static readonly DependencyProperty RenderAngleProperty
    //  = DependencyProperty.Register("RenderAngle", typeof(double), typeof(CircleSegmentHorizontalStackPanel), new PropertyMetadata(0.0));

    public Geometry SegmentGeo { get => (Geometry)GetValue(SegmentGeoProperty.DependencyProperty); protected set => SetValue(SegmentGeoProperty, value); }
    public static readonly DependencyPropertyKey SegmentGeoProperty
      = DependencyProperty.RegisterReadOnly("SegmentGeo", typeof(Geometry), typeof(CircleSegmentHorizontalStackPanel), new PropertyMetadata(null));

    public bool IsClosedStart { get => (bool)GetValue(IsClosedStartProperty); set => SetValue(IsClosedStartProperty, value); }
    public static readonly DependencyProperty IsClosedStartProperty
      = DependencyProperty.Register("IsClosedStart", typeof(bool), typeof(CircleSegmentHorizontalStackPanel), new PropertyMetadata(true));
    public bool IsClosedEnd { get => (bool)GetValue(IsClosedEndProperty); set => SetValue(IsClosedEndProperty, value); }
    public static readonly DependencyProperty IsClosedEndProperty
      = DependencyProperty.Register("IsClosedEnd", typeof(bool), typeof(CircleSegmentHorizontalStackPanel), new PropertyMetadata(true));
    public double InnerRadius { get => (double)GetValue(InnerRadiusProperty); set => SetValue(InnerRadiusProperty, value); }
    public static readonly DependencyProperty InnerRadiusProperty
      = DependencyProperty.Register("InnerRadius", typeof(double), typeof(CircleSegmentHorizontalStackPanel), new PropertyMetadata(10.0));
    public double StartAngle { get => (double)GetValue(StartAngleProperty); set => SetValue(StartAngleProperty, value); }
    public static readonly DependencyProperty StartAngleProperty
      = DependencyProperty.Register("StartAngle", typeof(double), typeof(CircleSegmentHorizontalStackPanel), new PropertyMetadata(0.0));

    static void _UpdateGeo(CircleSegmentHorizontalStackPanel dd, DependencyPropertyChangedEventArgs e) {

      dd.SegmentGeo = dd._Segment.GetGeometry();
    }

    readonly RingSegment _Segment = new RingSegment() { InnerRadius = 10.0, SegmentAngle = 0.0 };
    public RingSegment Segment { get => _Segment; }

    public double TotalAngle { get => (Children?.Cast<ICircleSegment>().Select(E => E.SegmentAngle).Sum()) ?? 0; }
    public double OuterRadius { get => (Children?.Cast<ICircleSegment>().Select(E => E.Radius).Max()) ?? InnerRadius; }

    void UpdateAngle() {
      foreach (var item in Children) {
        if (item is ICircleSegment) {

        }
      }
    }

  }
}
