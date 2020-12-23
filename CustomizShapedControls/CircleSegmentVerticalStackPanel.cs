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

namespace CustomizShapedControls {


  /// <summary>
  /// Vertical means From Origin to around or reserved;
  /// </summary>
  [Obsolete("No Impl",true)]
  public class CircleSegmentVerticalStackPanel : ItemsControl {
    static CircleSegmentVerticalStackPanel() {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(CircleSegmentVerticalStackPanel), new FrameworkPropertyMetadata(typeof(CircleSegmentVerticalStackPanel)));
    }

    public CircleSegmentVerticalStackPanel() : base() { }

 

    //public Brush BorderBrush { get => (Brush)GetValue(BorderBrushProperty); set => SetValue(BorderBrushProperty, value); }
    //public static readonly DependencyProperty BorderBrushProperty
    //  = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(CircleSegmentVerticalStackPanel), new PropertyMetadata(null));
    //public Thickness BorderThickness { get => (Thickness)GetValue(BorderThicknessProperty); set => SetValue(BorderThicknessProperty, value); }
    //public static readonly DependencyProperty BorderThicknessProperty
    //  = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(CircleSegmentVerticalStackPanel), new PropertyMetadata(new Thickness()));

    public Point RenderOrigin { get => (Point)GetValue(RenderOriginProperty); set => SetValue(RenderOriginProperty, value); }
    public static readonly DependencyProperty RenderOriginProperty
      = DependencyProperty.Register("RenderOrigin", typeof(Point), typeof(CircleSegmentVerticalStackPanel), new PropertyMetadata(new Point(0.0, 1.0)));
    public double RenderScaleX { get => (double)GetValue(RenderScaleXProperty); set => SetValue(RenderScaleXProperty, value); }
    public static readonly DependencyProperty RenderScaleXProperty
      = DependencyProperty.Register("RenderScaleX", typeof(double), typeof(CircleSegmentVerticalStackPanel), new PropertyMetadata(1.0));
    public double RenderScaleY { get => (double)GetValue(RenderScaleYProperty); set => SetValue(RenderScaleYProperty, value); }
    public static readonly DependencyProperty RenderScaleYProperty
      = DependencyProperty.Register("RenderScaleY", typeof(double), typeof(CircleSegmentVerticalStackPanel), new PropertyMetadata(1.0));
    public double RenderAngle { get => (double)GetValue(RenderAngleProperty); set => SetValue(RenderAngleProperty, value); }
    public static readonly DependencyProperty RenderAngleProperty
      = DependencyProperty.Register("RenderAngle", typeof(double), typeof(CircleSegmentVerticalStackPanel), new PropertyMetadata(0.0));

    public Geometry SegmentGeo { get => (Geometry)GetValue(SegmentGeoProperty.DependencyProperty); protected set => SetValue(SegmentGeoProperty, value); }
    public static readonly DependencyPropertyKey SegmentGeoProperty
      = DependencyProperty.RegisterReadOnly("SegmentGeo", typeof(Geometry), typeof(CircleSegmentVerticalStackPanel), new PropertyMetadata(null));

    public bool IsClosedStart { get => (bool)GetValue(IsClosedStartProperty); set => SetValue(IsClosedStartProperty, value); }
    public static readonly DependencyProperty IsClosedStartProperty
      = DependencyProperty.Register("IsClosedStart", typeof(bool), typeof(CircleSegmentVerticalStackPanel), new PropertyMetadata(true, ___UpdateMatrix));
    public bool IsClosedEnd { get => (bool)GetValue(IsClosedEndProperty); set => SetValue(IsClosedEndProperty, value); }
    public static readonly DependencyProperty IsClosedEndProperty
      = DependencyProperty.Register("IsClosedEnd", typeof(bool), typeof(CircleSegmentVerticalStackPanel), new PropertyMetadata(true, ___UpdateMatrix));
    public double InnerRadius { get => (double)GetValue(InnerRadiusProperty); set => SetValue(InnerRadiusProperty, value); }
    public static readonly DependencyProperty InnerRadiusProperty
      = DependencyProperty.Register("InnerRadius", typeof(double), typeof(CircleSegmentVerticalStackPanel), new PropertyMetadata(0.0, ___UpdateMatrix));
    public double StartAngle { get => (double)GetValue(StartAngleProperty); set => SetValue(StartAngleProperty, value); }
    public static readonly DependencyProperty StartAngleProperty
      = DependencyProperty.Register("StartAngle", typeof(double), typeof(CircleSegmentVerticalStackPanel), new PropertyMetadata(0.0, ___UpdateMatrix));

    public bool IsLargeArc { get => SegmentAngle > 180.0; }
    static void __CalculatSegment(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      try {
        var dd = (CircleSegmentVerticalStackPanel)d;
        var Gp = dd.SegmentAngle <= 0 ? null : new GeometryGroup {
          Children = dd.SegmentAngle == 360.0
          ? new GeometryCollection(new GeometryGroup[] {
          new GeometryGroup() {
            FillRule=FillRule.EvenOdd,
            Children=new GeometryCollection(new EllipseGeometry[] {
            new EllipseGeometry(Kits.Zero,dd.InnerRadius,dd.InnerRadius),
            new EllipseGeometry(Kits.Zero,dd.OuterRadius,dd.OuterRadius),
          }) }
          })
          : new GeometryCollection(new PathGeometry[] {
            new PathGeometry(new PathFigureCollection(
              new PathFigure[] {
                new PathFigure(Kits.CalculateCordToRendering.Transform(dd.OuterP0),
                  new PathSegment[] {
                    new ArcSegment(Kits.CalculateCordToRendering.Transform(dd.OuterP1), new Size(dd.OuterRadius, dd.OuterRadius),dd.SegmentAngle, dd.IsLargeArc, SweepDirection.Counterclockwise, true),
                    new LineSegment(Kits.CalculateCordToRendering.Transform(dd.InnerP1),dd.IsClosedStart),
                    new ArcSegment(Kits.CalculateCordToRendering.Transform(dd.InnerP0), new Size(dd.InnerRadius, dd.InnerRadius),dd.SegmentAngle, dd.IsLargeArc, SweepDirection.Clockwise, true),
                    new LineSegment(Kits.CalculateCordToRendering.Transform(dd.OuterP0),dd.IsClosedEnd),
                  },
                  false) }))
            })
        };
        dd.SegmentGeo = Gp;
      }
      catch { }
    }
    static void ___UpdateMatrix(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      var dd = (CircleSegmentVerticalStackPanel)d;
      if (dd.SegmentAngle > 360 || dd.SegmentAngle < 0) throw new System.IO.InvalidDataException("Angle ∈ [0,360]");
      dd.AllocatCalculatToRenderingMatrix();
      __CalculatSegment(d, e);
    }

    public double SegmentAngle { get => (ItemsSource ?? Enumerable.Empty<ICircleSegment>()).Cast<ICircleSegment>().Select(E => E.SegmentAngle).Max(); }
    public double OuterRadius { get => (ItemsSource ?? Enumerable.Empty<ICircleSegment>()).Cast<ICircleSegment>().Select(E => E.Radius).Sum() + InnerRadius; }
    //public double OuterRadius { get => (ItemsSource ?? Enumerable.Empty<ICircleSegment>()).Cast<ICircleSegment>().Select(E => E.Radius).Max(); }
    public double EndAngle { get => SegmentAngle + StartAngle; }
    public double Radius { get => OuterRadius; }
    public bool IsClosed { get => IsClosedStart && IsClosedEnd; }





    public Point OuterP0 { get => PointOnOuterArc(0.0); }
    public Point OuterP1 { get => PointOnOuterArc(1.0); }
    public Point PointOnOuterArc(double t) {
      double theta = (1 - t) * StartAngle + (t) * EndAngle;
      theta *= Kits.Deg2Rad;
      double c = Math.Cos(theta), s = Math.Sin(theta);
      return new Point(0.0 + OuterRadius * c, 0.0 + OuterRadius * s);
    }
    public Point InnerP0 { get => PointOnInnerArc(0.0); }
    public Point InnerP1 { get => PointOnInnerArc(1.0); }
    public Point PointOnInnerArc(double t) {
      double theta = (1 - t) * StartAngle + (t) * EndAngle;
      theta *= Kits.Deg2Rad;
      double c = Math.Cos(theta), s = Math.Sin(theta);
      return new Point(0.0 + InnerRadius * c, 0.0 + InnerRadius * s);
    }
    public Matrix CalculatToRenderingMatrix { get; protected set; }
    public void AllocatCalculatToRenderingMatrix() {
      var CalculatToRenderingMatrix = new Matrix();
      CalculatToRenderingMatrix.Scale(1, -1);
      CalculatToRenderingMatrix.Translate(Math.Abs(OuterP1.X), OuterRadius);
      this.CalculatToRenderingMatrix = CalculatToRenderingMatrix;
    }


  }

  public enum CircleStackDirector : byte {
    InnerToOuter = 0,
    OuterToInner = 1,
  }

  public enum AngleAlignment : byte {
    Center = 0,
    Strech = 1,
    Start = 2,
    End=3
  }

  public interface ICircleSegment {
    double SegmentAngle { get; }
    double Radius { get; }


    bool IsClosed { get; }

  }

}
