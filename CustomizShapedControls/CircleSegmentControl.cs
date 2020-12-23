namespace CustomizShapedControls {
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Text;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Data;
  using System.Windows.Media;

  public class CircleSegmentControl : ContentControl, ICircleSegment {
    static CircleSegmentControl() {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(CircleSegmentControl), new FrameworkPropertyMetadata(typeof(CircleSegmentControl)));
    }

    public CircleSegmentControl() : base() {

      this.Width = double.NaN;
      this.Height = double.NaN;
    }


    //public Point RenderOrigin {
    //  get => (Point)GetValue(RenderOriginProperty);
    //  set => SetValue(RenderOriginProperty, value);
    //}
    //public static readonly DependencyProperty RenderOriginProperty
    //  = DependencyProperty.Register("RenderOrigin", typeof(Point), typeof(CircleSegmentControl),
    //    new PropertyMetadata(new Point(0.0, 1.0)));
    //public double RenderScaleX {
    //  get => (double)GetValue(RenderScaleXProperty);
    //  set => SetValue(RenderScaleXProperty, value);
    //}
    //public static readonly DependencyProperty RenderScaleXProperty
    //  = DependencyProperty.Register("RenderScaleX", typeof(double), typeof(CircleSegmentControl),
    //    new PropertyMetadata(1.0));
    //public double RenderScaleY {
    //  get => (double)GetValue(RenderScaleYProperty);
    //  set => SetValue(RenderScaleYProperty, value);
    //}
    //public static readonly DependencyProperty RenderScaleYProperty
    //  = DependencyProperty.Register("RenderScaleY", typeof(double), typeof(CircleSegmentControl),
    //    new PropertyMetadata(1.0));
    //public double RenderAngle {
    //  get => (double)GetValue(RenderAngleProperty);
    //  set => SetValue(RenderAngleProperty, value);
    //}
    //public static readonly DependencyProperty RenderAngleProperty
    //  = DependencyProperty.Register("RenderAngle", typeof(double), typeof(CircleSegmentControl),
    //    new PropertyMetadata(0.0));
    public double HalfChord { get => (double)GetValue(HalfChordProperty.DependencyProperty); protected set => SetValue(HalfChordProperty, value); }
    public static readonly DependencyPropertyKey HalfChordProperty
      = DependencyProperty.RegisterReadOnly("HalfChord", typeof(double), typeof(CircleSegmentControl), new PropertyMetadata(0.0));

    public Geometry SegmentGeo {
      get => (Geometry)GetValue(SegmentGeoProperty.DependencyProperty);
      protected set => SetValue(SegmentGeoProperty, value);
    }
    public static readonly DependencyPropertyKey SegmentGeoProperty
      = DependencyProperty.RegisterReadOnly("SegmentGeo", typeof(Geometry), typeof(CircleSegmentControl),
        new PropertyMetadata(null));

    public bool IsClosed {
      get => (bool)GetValue(IsClosedProperty);
      set => SetValue(IsClosedProperty, value);
    }
    public static readonly DependencyProperty IsClosedProperty
      = DependencyProperty.Register("IsClosed", typeof(bool), typeof(CircleSegmentControl),
        new PropertyMetadata(true, (d, e) => { var dd = (CircleSegmentControl)d; dd._Segment.IsClosed = (bool)e.NewValue; __CalculatSegment(dd, e); }));
    public double Radius {
      get => (double)GetValue(RadiusProperty);
      set => SetValue(RadiusProperty, value);
    }
    public static readonly DependencyProperty RadiusProperty
      = DependencyProperty.Register("Radius", typeof(double), typeof(CircleSegmentControl),
        new PropertyMetadata(80.0, (d, e) => { var dd = (CircleSegmentControl)d; dd._Segment.Radius = (double)e.NewValue; __CalculatSegment(dd, e); }));
    public double SegmentAngle {
      get => (double)GetValue(SegmentAngleProperty);
      set => SetValue(SegmentAngleProperty, value);
    }
    public static readonly DependencyProperty SegmentAngleProperty
      = DependencyProperty.Register("SegmentAngle", typeof(double), typeof(CircleSegmentControl),
        new PropertyMetadata(60.0, (d, e) => { var dd = (CircleSegmentControl)d; dd._Segment.SegmentAngle = (double)e.NewValue; __CalculatSegment(dd, e); }));

    static void __CalculatSegment(CircleSegmentControl dd, DependencyPropertyChangedEventArgs e) {
      try {
        dd.SegmentGeo = dd._Segment.GetGeometry();
        dd.CalculatToRenderingMatrix = dd._Segment.CalculatToRenderingMatrix;
        dd.HalfChord = dd.Segment.ChordLength * 0.5;
        __UpdateContentRange(dd, e);
      }
      catch { }
    }


    public double ContentOffsetX { get => (double)GetValue(ContentOffsetXProperty.DependencyProperty); set => SetValue(ContentOffsetXProperty, value); }
    public static readonly DependencyPropertyKey ContentOffsetXProperty
      = DependencyProperty.RegisterReadOnly("ContentOffsetX", typeof(double), typeof(CircleSegmentControl), new PropertyMetadata(0.0));
    public double ContentOffsetY { get => (double)GetValue(ContentOffsetYProperty.DependencyProperty); set => SetValue(ContentOffsetYProperty, value); }
    public static readonly DependencyPropertyKey ContentOffsetYProperty
      = DependencyProperty.RegisterReadOnly("ContentOffsetY", typeof(double), typeof(CircleSegmentControl), new PropertyMetadata(0.0));

    public VerticalAlignment ContentVerticalAlignment { get => (VerticalAlignment)GetValue(ContentVerticalAlignmentProperty.DependencyProperty); protected set => SetValue(ContentVerticalAlignmentProperty, value); }
    public static readonly DependencyPropertyKey ContentVerticalAlignmentProperty
      = DependencyProperty.RegisterReadOnly("ContentVerticalAlignment", typeof(VerticalAlignment), typeof(CircleSegmentControl), new PropertyMetadata(VerticalAlignment.Bottom));

    public double ContentRotateDegree { get => (double)GetValue(ContentRotateDegreeProperty); set => SetValue(ContentRotateDegreeProperty, value); }
    public static readonly DependencyProperty ContentRotateDegreeProperty
      = DependencyProperty.Register("ContentRotateDegree", typeof(double), typeof(CircleSegmentControl), new PropertyMetadata(0.0));
    public double ContentOffsetAspect { get => (double)GetValue(ContentOffsetAspectProperty); set => SetValue(ContentOffsetAspectProperty, value); }
    public static readonly DependencyProperty ContentOffsetAspectProperty
      = DependencyProperty.Register("ContentOffsetAspect", typeof(double), typeof(CircleSegmentControl),
        new PropertyMetadata(0.5, __UpdateContentRange));
    static void __UpdateContentRange(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      var dd = (CircleSegmentControl)d;
      var Orig = dd._Segment.ContentOffset(dd.ContentOffsetAspect, out var Align);
      dd.ContentVerticalAlignment = Align;
      Orig = Kits.CalculateCordToRendering.Transform(Orig);
      dd.ContentOffsetX = Orig.X;
      dd.ContentOffsetY = Orig.Y;
    }

    public Matrix CalculatToRenderingMatrix { get; protected set; }

    readonly CircleSegement _Segment = new CircleSegement();
    public CircleSegement Segment { get => _Segment; }

  }
  public sealed class HalfChordLengthToCanvasLeftOffset : IMultiValueConverter {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
      var CL = (double)values[0];
      var HCL = (double)values[1];
      return (CL * 0.5 - HCL);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
