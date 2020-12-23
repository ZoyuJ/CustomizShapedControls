namespace Try {
  using System;
  using System.Collections.Generic;
  using System.Text;
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
  /// Interaction logic for CircleSegmentPath.xaml
  /// </summary>
  public partial class CircleSegmentPath : UserControl {
    public CircleSegmentPath() {
      DataContext = this;
      InitializeComponent();
      Loaded += OnLoaded;
    }
    private void OnLoaded(object sender, RoutedEventArgs e) {
      _Fig = new PathFigure(new Point(0.0, 0.0), null, true);
      _ArcPart = new ArcSegment(P1, new Size(Radius, Radius), EndAngle - StartAngle, IsLargeArc, SweepDirection.Clockwise, true);
      _LinePart = new LineSegment(Origin, true);
      Segs.Add(_ArcPart);
      if (IsClosed) Segs.Add(_LinePart);
      _Fig.Segments = Segs;
      Figs.Add(_Fig);
    }

    public double RotateAngle { get => (double)GetValue(RotateAngleProperty); set => SetValue(RotateAngleProperty, value); }
    public static readonly DependencyProperty RotateAngleProperty = DependencyProperty.Register("RotateAngle", typeof(double), typeof(CircleSegmentPath), new PropertyMetadata(0.0));
    public Point RotateOrigin { get => (Point)GetValue(RotateOriginProperty); set => SetValue(RotateOriginProperty, value); }
    public static readonly DependencyProperty RotateOriginProperty = DependencyProperty.Register("RotateOrigin", typeof(Point), typeof(CircleSegmentPath), new PropertyMetadata(new Point(0.5, 0.5)));
    public bool IsClosed { get => (bool)GetValue(IsClosedProperty); set => SetValue(IsClosedProperty, value); }
    public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register("IsClosed", typeof(bool), typeof(CircleSegmentPath), new PropertyMetadata(true));

    public PathFigureCollection Figs { get; set; } = new PathFigureCollection();
    private readonly PathSegmentCollection Segs = new PathSegmentCollection();

    //public PathSegmentCollection Segments { get => Figs.Segments; }

    public double Radius { get => (double)GetValue(RadiusProperty); set => SetValue(RadiusProperty, value); }
    public static readonly DependencyProperty RadiusProperty
      = DependencyProperty.Register("Radius", typeof(double), typeof(CircleSegmentPath), new PropertyMetadata(80.0, CalculatSegment));
    public double StartAngle { get => (double)GetValue(StartAngleProperty); set => SetValue(StartAngleProperty, value); }
    public static readonly DependencyProperty StartAngleProperty
      = DependencyProperty.Register("StartAngle", typeof(double), typeof(CircleSegmentPath), new PropertyMetadata(0.0, CalculatSegment));
    public double EndAngle { get => (double)GetValue(EndAngleProperty); set => SetValue(EndAngleProperty, value); }
    public static readonly DependencyProperty EndAngleProperty
      = DependencyProperty.Register("EndAngle", typeof(double), typeof(CircleSegmentPath), new PropertyMetadata(60.0, CalculatSegment));
    public Point Origin { get => (Point)GetValue(OriginProperty); set => SetValue(OriginProperty, value); }
    public static readonly DependencyProperty OriginProperty
      = DependencyProperty.Register("Origin", typeof(Point), typeof(CircleSegmentPath), new PropertyMetadata(new Point(0.0, 0.0), CalculatSegment));

    static void CalculatSegment(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      try {
        var dd = (CircleSegmentPath)d;
        //dd.Figs.StartPoint = dd.P0;
        //dd.Segments.Clear();
        //dd._ArcPart = new ArcSegment(dd.P1, new Size(dd.Radius, dd.Radius), dd.EndAngle - dd.StartAngle, dd.IsLargeArc, SweepDirection.Clockwise, true);
        //dd.Segments.Add(dd._ArcPart);
        //if (dd.IsClosed) {
        //  dd._LinePart = new LineSegment(dd.Origin, true);
        //  dd.Segments.Add(dd._ArcPart);
        //}
        dd._Fig.StartPoint = dd.P0;
        dd.Segs.Clear();
        dd._ArcPart = new ArcSegment(dd.P1, new Size(dd.Radius, dd.Radius), dd.EndAngle - dd.StartAngle, dd.IsLargeArc, SweepDirection.Clockwise, true);
        dd.Segs.Add(dd._ArcPart);
        if (dd.IsClosed) {
          dd._LinePart = new LineSegment(dd.Origin, true);
          dd.Segs.Add(dd._ArcPart);
        }
      }
      catch { }
    }
    //static void CalculatSegmentByStartAngle(DependencyObject d, DependencyPropertyChangedEventArgs e) {
    //  var dd = (CircleSegmentPath)d;
    //  dd.Figs.StartPoint = dd.P0;
    //}
    //static void CalculatSegmentByEndAngle(DependencyObject d, DependencyPropertyChangedEventArgs e) {
    //  var dd = (CircleSegmentPath)d;
    //  dd._ArcPart.Point = dd.P1;
    //}
    //static void CalculatSegmentByRadius(DependencyObject d, DependencyPropertyChangedEventArgs e) {
    //  var dd = (CircleSegmentPath)d;
    //  dd.Figs.StartPoint = dd.P0;
    //  dd._ArcPart.Point = dd.P1;
    //}
    //static void CalculatSegmentByOrigin(DependencyObject d, DependencyPropertyChangedEventArgs e) {
    //  var dd = (CircleSegmentPath)d;
    //  dd.Figs.StartPoint = dd.P0;
    //  dd._ArcPart.Point = dd.P1;
    //  dd._LinePart.Point = dd.Origin;
    //}
    private ArcSegment _ArcPart;
    private LineSegment _LinePart;
    private PathFigure _Fig;

    public bool IsLargeArc { get => EndAngle - StartAngle > 180.0; }

    public Point P0 { get => PointOnArc(0.0); }
    public Point P1 { get => PointOnArc(1.0); }
    public Point PointOnArc(double t) {
      double theta = (1 - t) * StartAngle + (t) * EndAngle;
      theta = theta * MathUtil.Deg2Rad;
      double c = Math.Cos(theta), s = Math.Sin(theta);
      return new Point(Origin.X + Radius * c, Origin.Y + Radius * s);
    }

  }
  public struct Arc2d {
    public Vector Center;
    public double Radius;
    public double AngleStartDeg;
    public double AngleEndDeg;
    public bool IsReversed;   // use ccw orientation instead of cw


    public Arc2d(Vector center, double radius, double startDeg, double endDeg) {
      IsReversed = false;
      Center = center;
      Radius = radius;
      AngleStartDeg = startDeg;
      AngleEndDeg = endDeg;
      if (AngleEndDeg < AngleStartDeg)
        AngleEndDeg += 360;

      // [TODO] handle full arcs, which should be circles?
    }


    /// <summary>
    /// Create Arc around center, **clockwise** from start to end points.
    /// Points must both be the same distance from center (ie on circle)
    /// </summary>
    public Arc2d(Vector vCenter, Vector vStart, Vector vEnd) {
      IsReversed = false;
      //SetFromCenterAndPoints(vCenter, vStart, vEnd);
      Vector ds = vStart - vCenter;
      Vector de = vEnd - vCenter;
      //Debug.Assert(Math.Abs(ds.LengthSquared - de.LengthSquared) < MathUtil.ZeroTolerancef);
      AngleStartDeg = Math.Atan2(ds.Y, ds.X) * MathUtil.Rad2Deg;
      AngleEndDeg = Math.Atan2(de.Y, de.X) * MathUtil.Rad2Deg;
      if (AngleEndDeg < AngleStartDeg)
        AngleEndDeg += 360;
      Center = vCenter;
      Radius = ds.Length;
    }


    /// <summary>
    /// Initialize Arc around center, **clockwise** from start to end points.
    /// Points must both be the same distance from center (ie on circle)
    /// </summary>
    public void SetFromCenterAndPoints(Vector vCenter, Vector vStart, Vector vEnd) {
      Vector ds = vStart - vCenter;
      Vector de = vEnd - vCenter;
      //Debug.Assert(Math.Abs(ds.LengthSquared - de.LengthSquared) < MathUtil.ZeroTolerancef);
      AngleStartDeg = Math.Atan2(ds.Y, ds.X) * MathUtil.Rad2Deg;
      AngleEndDeg = Math.Atan2(de.Y, de.X) * MathUtil.Rad2Deg;
      if (AngleEndDeg < AngleStartDeg)
        AngleEndDeg += 360;
      Center = vCenter;
      Radius = ds.Length;
    }



    public Vector P0 {
      get { return SampleT(0.0); }
    }
    public Vector P1 {
      get { return SampleT(1.0); }
    }

    public double Curvature {
      get { return 1.0 / Radius; }
    }
    public double SignedCurvature {
      get { return (IsReversed) ? (-1.0 / Radius) : (1.0 / Radius); }
    }


    // t in range[0,1] spans arc
    public Vector SampleT(double t) {
      double theta = (IsReversed) ?
        (1 - t) * AngleEndDeg + (t) * AngleStartDeg :
        (1 - t) * AngleStartDeg + (t) * AngleEndDeg;
      theta = theta * MathUtil.Deg2Rad;
      double c = Math.Cos(theta), s = Math.Sin(theta);
      return new Vector(Center.X + Radius * c, Center.Y + Radius * s);
    }


    public Vector TangentT(double t) {
      double theta = (IsReversed) ?
        (1 - t) * AngleEndDeg + (t) * AngleStartDeg :
        (1 - t) * AngleStartDeg + (t) * AngleEndDeg;
      theta = theta * MathUtil.Deg2Rad;
      Vector tangent = new Vector(-Math.Sin(theta), Math.Cos(theta));
      if (IsReversed)
        tangent = -tangent;
      tangent.Normalize();
      return tangent;
    }


    public bool HasArcLength { get { return true; } }

    public double ArcLength {
      get {
        return (AngleEndDeg - AngleStartDeg) * MathUtil.Deg2Rad * Radius;
      }
    }

    public Vector SampleArcLength(double a) {
      if (ArcLength < MathUtil.Epsilon)
        return (a < 0.5) ? SampleT(0) : SampleT(1);
      double t = a / ArcLength;
      double theta = (IsReversed) ?
        (1 - t) * AngleEndDeg + (t) * AngleStartDeg :
        (1 - t) * AngleStartDeg + (t) * AngleEndDeg;
      theta = theta * MathUtil.Deg2Rad;
      double c = Math.Cos(theta), s = Math.Sin(theta);
      return new Vector(Center.X + Radius * c, Center.Y + Radius * s);
    }

    public void Reverse() {
      IsReversed = !IsReversed;
    }

    public ArcSegment ToPathSegment(SweepDirection Direction = SweepDirection.Clockwise) {
      return new ArcSegment(new Point(P1.X, P1.Y), new Size(Radius, Radius), AngleEndDeg - AngleStartDeg, AngleEndDeg - AngleStartDeg > 180.0, Direction, false);
    }


    //public bool IsTransformable { get { return true; } }
    //public void Transform(ITransform2 xform) {
    //  Vector2d vCenter = xform.TransformP(Center);
    //  Vector2d vStart = xform.TransformP((IsReversed) ? P1 : P0);
    //  Vector2d vEnd = xform.TransformP((IsReversed) ? P0 : P1);

    //  SetFromCenterAndPoints(vCenter, vStart, vEnd);
    //}





    public double Distance(Vector point) {
      Vector PmC = point - Center;
      double lengthPmC = PmC.Length;
      if (lengthPmC > MathUtil.Epsilon) {
        Vector dv = PmC / lengthPmC;
        double theta = Math.Atan2(dv.Y, dv.X) * MathUtil.Rad2Deg;
        if (!(theta >= AngleStartDeg && theta <= AngleEndDeg)) {
          double ctheta = MathUtil.ClampAngleDeg(theta, AngleStartDeg, AngleEndDeg);
          double radians = ctheta * MathUtil.Deg2Rad;
          double c = Math.Cos(radians), s = Math.Sin(radians);
          Vector pos = new Vector(Center.X + Radius * c, Center.Y + Radius * s);
          return pos.Distance(point);
        }
        else {
          return Math.Abs(lengthPmC - Radius);
        }
      }
      else {
        return Radius;
      }
    }


    public Vector NearestPoint(Vector point) {
      Vector PmC = point - Center;
      double lengthPmC = PmC.Length;
      if (lengthPmC > MathUtil.Epsilon) {
        Vector dv = PmC / lengthPmC;
        double theta = Math.Atan2(dv.Y, dv.X);
        theta *= MathUtil.Rad2Deg;
        theta = MathUtil.ClampAngleDeg(theta, AngleStartDeg, AngleEndDeg);
        theta = MathUtil.Deg2Rad * theta;
        double c = Math.Cos(theta), s = Math.Sin(theta);
        return new Vector(Center.X + Radius * c, Center.Y + Radius * s);
      }
      else
        return SampleT(0.5);        // all points equidistant
    }


  }
  public static class MathUtil {
    public const double Deg2Rad = (Math.PI / 180.0);
    public const double Rad2Deg = (180.0 / Math.PI);
    public const double TwoPI = 2.0 * Math.PI;
    public const double FourPI = 4.0 * Math.PI;
    public const double HalfPI = 0.5 * Math.PI;
    public const double ZeroTolerance = 1e-08;
    public const double Epsilon = 2.2204460492503131e-016;
    public const double SqrtTwo = 1.41421356237309504880168872420969807;
    public const double SqrtTwoInv = 1.0 / SqrtTwo;
    public const double SqrtThree = 1.73205080756887729352744634150587236;

    public const float Deg2Radf = (float)(Math.PI / 180.0);
    public const float Rad2Degf = (float)(180.0 / Math.PI);
    public const float PIf = (float)(Math.PI);
    public const float TwoPIf = 2.0f * PIf;
    public const float HalfPIf = 0.5f * PIf;
    public const float SqrtTwof = 1.41421356237f;

    public const float ZeroTolerancef = 1e-06f;
    public const float Epsilonf = 1.192092896e-07F;
    public static double DistanceSquared(this Vector V1, Vector V2) {
      double dx = V2.X - V1.X, dy = V2.Y - V1.Y;
      return dx * dx + dy * dy;
    }
    public static double Distance(this Vector V1, Vector V2) {
      double dx = V2.X - V1.X, dy = V2.Y - V1.Y;
      return Math.Sqrt(dx * dx + dy * dy);
    }
    public static double Dot(this Vector V1, Vector V2) {
      return V1.X * V2.X + V1.Y * V2.Y;
    }
    public static double ClampAngleDeg(double theta, double min, double max) {
      // convert interval to center/extent - [c-e,c+e]
      double c = (min + max) * 0.5;
      double e = max - c;

      // get rid of extra rotations
      theta = theta % 360;

      // shift to origin, then convert theta to +- 180
      theta -= c;
      if (theta < -180)
        theta += 360;
      else if (theta > 180)
        theta -= 360;

      // clamp to extent
      if (theta < -e)
        theta = -e;
      else if (theta > e)
        theta = e;

      // shift back
      return theta + c;
    }
  }
}
