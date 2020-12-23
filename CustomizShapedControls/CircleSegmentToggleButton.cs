using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomizShapedControls {

  public class CircleSegmentToggleButton : ToggleButton, ICircleSegment {
    static CircleSegmentToggleButton() {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(CircleSegmentToggleButton), new FrameworkPropertyMetadata(typeof(CircleSegmentToggleButton)));
    }

    public CircleSegmentToggleButton() : base() { }


    //public Point RenderOrigin { get => (Point)GetValue(RenderOriginProperty); set => SetValue(RenderOriginProperty, value); }
    //public static readonly DependencyProperty RenderOriginProperty
    //  = DependencyProperty.Register("RenderOrigin", typeof(Point), typeof(CircleSegmentToggleButton), new PropertyMetadata(new Point(0.0, 1.0)));
    //public double RenderScaleX { get => (double)GetValue(RenderScaleXProperty); set => SetValue(RenderScaleXProperty, value); }
    //public static readonly DependencyProperty RenderScaleXProperty
    //  = DependencyProperty.Register("RenderScaleX", typeof(double), typeof(CircleSegmentToggleButton), new PropertyMetadata(1.0));
    //public double RenderScaleY { get => (double)GetValue(RenderScaleYProperty); set => SetValue(RenderScaleYProperty, value); }
    //public static readonly DependencyProperty RenderScaleYProperty
    //  = DependencyProperty.Register("RenderScaleY", typeof(double), typeof(CircleSegmentToggleButton), new PropertyMetadata(1.0));
    //public double RenderAngle { get => (double)GetValue(RenderAngleProperty); set => SetValue(RenderAngleProperty, value); }
    //public static readonly DependencyProperty RenderAngleProperty
    //  = DependencyProperty.Register("RenderAngle", typeof(double), typeof(CircleSegmentToggleButton), new PropertyMetadata(0.0));
    public double HalfChord { get => (double)GetValue(HalfChordProperty.DependencyProperty); protected set => SetValue(HalfChordProperty, value); }
    public static readonly DependencyPropertyKey HalfChordProperty
      = DependencyProperty.RegisterReadOnly("HalfChord", typeof(double), typeof(CircleSegmentToggleButton), new PropertyMetadata(0.0));

    public Geometry SegmentGeo { get => (Geometry)GetValue(SegmentGeoProperty.DependencyProperty); protected set => SetValue(SegmentGeoProperty, value); }
    public static readonly DependencyPropertyKey SegmentGeoProperty
      = DependencyProperty.RegisterReadOnly("SegmentGeo", typeof(Geometry), typeof(CircleSegmentToggleButton), new PropertyMetadata(null));

    public bool IsClosed { get => (bool)GetValue(IsClosedProperty); set => SetValue(IsClosedProperty, value); }
    public static readonly DependencyProperty IsClosedProperty
      = DependencyProperty.Register("IsClosed", typeof(bool), typeof(CircleSegmentToggleButton),
        new PropertyMetadata(true, (d, e) => { var dd = (CircleSegmentToggleButton)d; dd.Segment.IsClosed = (bool)e.NewValue; __CalculatSegment(dd, e); }));
    public double Radius { get => (double)GetValue(RadiusProperty); set => SetValue(RadiusProperty, value); }
    public static readonly DependencyProperty RadiusProperty
      = DependencyProperty.Register("Radius", typeof(double), typeof(CircleSegmentToggleButton),
        new PropertyMetadata(80.0, (d, e) => { var dd = (CircleSegmentToggleButton)d; dd.Segment.Radius = (double)e.NewValue; __CalculatSegment(dd, e); }));
    public double SegmentAngle { get => (double)GetValue(SegmentAngleProperty); set => SetValue(SegmentAngleProperty, value); }
    public static readonly DependencyProperty SegmentAngleProperty
      = DependencyProperty.Register("SegmentAngle", typeof(double), typeof(CircleSegmentToggleButton),
        new PropertyMetadata(60.0, (d, e) => {
          var dd = (CircleSegmentToggleButton)d;
          dd.Segment.SegmentAngle = (double)e.NewValue;
          __CalculatSegment(dd, e);
        }));

    static void __CalculatSegment(CircleSegmentToggleButton dd, DependencyPropertyChangedEventArgs e) {
      try {
        dd.SegmentGeo = dd.Segment.GetGeometry();
        dd.CalculatToRenderingMatrix = dd.Segment.CalculatToRenderingMatrix;
        dd.HalfChord = Math.Abs(dd.Segment.P1.X);
        __UpdateContentRange(dd, e);
      }
      catch { }
    }

    public double ContentOffsetX { get => (double)GetValue(ContentOffsetXProperty.DependencyProperty); set => SetValue(ContentOffsetXProperty, value); }
    public static readonly DependencyPropertyKey ContentOffsetXProperty
      = DependencyProperty.RegisterReadOnly("ContentOffsetX", typeof(double), typeof(CircleSegmentToggleButton), new PropertyMetadata(0.0));
    public double ContentOffsetY { get => (double)GetValue(ContentOffsetYProperty.DependencyProperty); set => SetValue(ContentOffsetYProperty, value); }
    public static readonly DependencyPropertyKey ContentOffsetYProperty
      = DependencyProperty.RegisterReadOnly("ContentOffsetY", typeof(double), typeof(CircleSegmentToggleButton), new PropertyMetadata(0.0));

    public VerticalAlignment ContentVerticalAlignment { get => (VerticalAlignment)GetValue(ContentVerticalAlignmentProperty.DependencyProperty); protected set => SetValue(ContentVerticalAlignmentProperty, value); }
    public static readonly DependencyPropertyKey ContentVerticalAlignmentProperty
      = DependencyProperty.RegisterReadOnly("ContentVerticalAlignment", typeof(VerticalAlignment), typeof(CircleSegmentToggleButton), new PropertyMetadata(VerticalAlignment.Bottom));

    public double ContentRotateDegree { get => (double)GetValue(ContentRotateDegreeProperty); set => SetValue(ContentRotateDegreeProperty, value); }
    public static readonly DependencyProperty ContentRotateDegreeProperty
      = DependencyProperty.Register("ContentRotateDegree", typeof(double), typeof(CircleSegmentToggleButton), new PropertyMetadata(0.0));
    public double ContentOffsetAspect { get => (double)GetValue(ContentOffsetAspectProperty); set => SetValue(ContentOffsetAspectProperty, value); }
    public static readonly DependencyProperty ContentOffsetAspectProperty
      = DependencyProperty.Register("ContentOffsetAspect", typeof(double), typeof(CircleSegmentToggleButton),
        new PropertyMetadata(0.5, __UpdateContentRange));
    static void __UpdateContentRange(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      var dd = (CircleSegmentToggleButton)d;
      var Orig = dd.Segment.ContentOffset(dd.ContentOffsetAspect, out var Align);
      dd.ContentVerticalAlignment = Align;
      Orig = Kits.CalculateCordToRendering.Transform(Orig);
      dd.ContentOffsetX = Orig.X;
      dd.ContentOffsetY = Orig.Y;
    }

    public Matrix CalculatToRenderingMatrix { get; protected set; }
    public CircleSegement Segment { get; } = new CircleSegement() { SegmentAngle = 60, IsClosed = true, Radius = 80.0 };


  }
}
