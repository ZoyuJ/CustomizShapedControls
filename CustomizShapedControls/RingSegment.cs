using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace CustomizShapedControls {
  public class RingSegment {
    public Point OuterP0 { get => PointOnOuterArc(0.0); }
    public Point OuterP1 { get => PointOnOuterArc(1.0); }
    public Point InnerP0 { get => PointOnInnerArc(0.0); }
    public Point InnerP1 { get => PointOnInnerArc(1.0); }
    public Matrix CalculatToRenderingMatrix {
      get {
        var CalculatToRenderingMatrix = new Matrix();
        CalculatToRenderingMatrix.Scale(1, -1);
        CalculatToRenderingMatrix.Translate(Math.Abs(OuterP1.X), OuterRadius);
        return CalculatToRenderingMatrix;
      }
    }
    public Matrix RenderingToCalculatMatrix {
      get {
        var CalculatToRenderingMatrix = new Matrix();
        CalculatToRenderingMatrix.Translate(-Math.Abs(OuterP1.X), -OuterRadius);
        CalculatToRenderingMatrix.Scale(1, -1);
        return CalculatToRenderingMatrix;
      }
    }
    public bool IsLargeArc { get => SegmentAngle > 180.0; }
    protected double StartAngle { get => 90.0 - SegmentAngle * 0.5; }
    public double EndAngle { get => SegmentAngle + StartAngle; }

    public Point PointOnInnerArc(double t) {
      double theta = (1 - t) * StartAngle + (t) * EndAngle;
      theta *= Kits.Deg2Rad;
      double c = Math.Cos(theta), s = Math.Sin(theta);
      return new Point(0.0 + InnerRadius * c, 0.0 + InnerRadius * s);
    }
    public Point PointOnOuterArc(double t) {
      double theta = (1 - t) * StartAngle + (t) * EndAngle;
      theta *= Kits.Deg2Rad;
      double c = Math.Cos(theta), s = Math.Sin(theta);
      return new Point(0.0 + OuterRadius * c, 0.0 + OuterRadius * s);
    }

    public static RingSegment FromSize(Size _Size, in double Offset) {
      var WP5 = _Size.Width * 0.5;
      var AngleP5 = Math.Asin(WP5 / Offset);
      return new RingSegment() {
        SegmentAngle = 2.0 * AngleP5,
        OuterRadius = WP5 / Math.Sin(AngleP5 * Kits.Deg2Rad),
        InnerRadius = Offset,
      };
    }
    public static RingSegment FromRect(Rect _Rect) {
      return FromSize(_Rect.Size, _Rect.Bottom);
    }

    public bool IsClosed { get => IsClosedEnd && IsClosedStart; }
    public double Radius { get => OuterRadius; }

    public bool IsClosedStart = true, IsClosedEnd = true;
    public double OuterRadius;
    public double InnerRadius;
    public double SegmentAngle;


    public Geometry GetGeometry() {
      if (SegmentAngle < 0 || SegmentAngle > 360) throw new System.IO.InvalidDataException("Angle ∈ [0,360]");
      return SegmentAngle <= 0 ? null : new GeometryGroup {
        Children = SegmentAngle == 360.0
          ? new GeometryCollection(new GeometryGroup[] {
          new GeometryGroup() {
            FillRule=FillRule.EvenOdd,
            Children=new GeometryCollection(new EllipseGeometry[] {
            new EllipseGeometry(Kits.Zero, InnerRadius, InnerRadius),
            new EllipseGeometry(Kits.Zero, OuterRadius, OuterRadius),
          }) }
          })
          : new GeometryCollection(new PathGeometry[] {
            new PathGeometry(new PathFigureCollection(
              new PathFigure[] {
                new PathFigure(Kits.CalculateCordToRendering.Transform( OuterP0),
                  new PathSegment[] {
                    new ArcSegment(Kits.CalculateCordToRendering.Transform( OuterP1), new Size( OuterRadius,  OuterRadius), SegmentAngle,  IsLargeArc, SweepDirection.Counterclockwise, true),
                    new LineSegment(Kits.CalculateCordToRendering.Transform( InnerP1), IsClosedStart),
                    new ArcSegment(Kits.CalculateCordToRendering.Transform( InnerP0), new Size( InnerRadius,  InnerRadius), SegmentAngle,  IsLargeArc, SweepDirection.Clockwise, true),
                    new LineSegment(Kits.CalculateCordToRendering.Transform( OuterP0), IsClosedEnd),
                  },
                  false) }))
            })
      };
    }



  }
}
