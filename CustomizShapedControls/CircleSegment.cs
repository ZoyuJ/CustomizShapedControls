namespace CustomizShapedControls {
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows;
  using System.Windows.Media;

  public class CircleSegement {
    public Point P0 { get => PointOnArc(0.0); }
    public Point P1 { get => PointOnArc(1.0); }
    public Matrix CalculatToRenderingMatrix {
      get {
        var CalculatToRenderingMatrix = new Matrix();
        CalculatToRenderingMatrix.Scale(1, -1);
        CalculatToRenderingMatrix.Translate(Math.Abs(P1.X), Radius);
        return CalculatToRenderingMatrix;
      }
    }
    public double ChordLength { get => Math.Abs(P0.X) + Math.Abs(P1.X); }
    public bool IsLargeArc { get => SegmentAngle > 180.0; }
    public double StartAngle { get => 90.0 - SegmentAngle * 0.5; }
    public double EndAngle { get => SegmentAngle + StartAngle; }

    public Point PointOnArc(double t) {
      double theta = (1 - t) * StartAngle + (t) * EndAngle;
      theta *= Kits.Deg2Rad;
      double c = Math.Cos(theta), s = Math.Sin(theta);
      return new Point(0.0 + Radius * c, 0.0 + Radius * s);
    }


    public static CircleSegement FromSize(Size _Size, in double Offset) {
      var WP5 = _Size.Width * 0.5;
      var AngleP5 = Math.Asin(WP5 / Offset);
      return new CircleSegement() {
        SegmentAngle = 2.0 * AngleP5,
        Radius = WP5 / Math.Sin(AngleP5 * Kits.Deg2Rad),
      };
    }
    public static CircleSegement FromRect(Rect _Rect) {
      return FromSize(_Rect.Size, _Rect.Bottom);
    }

    public bool IsClosed = true;
    public double Radius;
    public double SegmentAngle;

    public Geometry GetGeometry() {
      if (SegmentAngle < 0 || SegmentAngle > 360) throw new System.IO.InvalidDataException("Angle ∈ [0,360]");
      return new GeometryGroup {
        Children = SegmentAngle == 360.0
          ? new GeometryCollection(new EllipseGeometry[] {
            new EllipseGeometry(Kits.Zero,Radius,Radius),
          })
          : new GeometryCollection(new PathGeometry[] {
            new PathGeometry(new PathFigureCollection(
              new PathFigure[] {
                new PathFigure(Kits.CalculateCordToRendering.Transform(P0),
                  new PathSegment[] {
                    new ArcSegment(Kits.CalculateCordToRendering.Transform(P1), new Size(Radius, Radius),SegmentAngle, IsLargeArc, SweepDirection.Counterclockwise, true),
                    new LineSegment(Kits.Zero,IsClosed),
                  },
                  IsClosed) }))
            })
      };
    }
    public Point ContentOffset(in double ContentOffsetAspect, out VerticalAlignment Aligment) {
      if (SegmentAngle <= 180) {
        Aligment = VerticalAlignment.Bottom;
        return new Point(0, ContentOffsetAspect * Radius);
      }
      else if (SegmentAngle < 360) {
        Aligment = VerticalAlignment.Top;
        return new Point(0, ContentOffsetAspect * Radius);
      }
      else {
        Aligment = VerticalAlignment.Center;
        return new Point(0, 0);
      }
    }

  }
}
