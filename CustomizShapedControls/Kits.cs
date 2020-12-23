namespace CustomizShapedControls {
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows;
  using System.Windows.Media;

  internal static class Kits {
    public const double Deg2Rad = (Math.PI / 180.0);
    public const double Rad2Deg = (180.0 / Math.PI);

    public static readonly double Cos45Deg = Math.Cos(45.0 * Deg2Rad);

    public static readonly Point Zero = new Point(0.0, 0.0);
    public static readonly Point One = new Point(1.0, 1.0);
    public static readonly Point Half = new Point(0.5, 0.5);
    public static readonly Point Up = new Point(0.0, -1.0);
    public static readonly Point Right = new Point(1.0, 0.0);
    public static readonly Point Left = new Point(-1.0, 0.0);
    public static readonly Point Down = new Point(0.0, 1.0);

    public static readonly Matrix CalculateCordToRendering = new Matrix(1, 0, 0, -1, 0, 0);

    public static double Degree(in this Vector V1, in Vector V2) {
      return Math.Acos(Clamp(Vector.Multiply(V1, V2), -1.0, 1.0)) * Rad2Deg;
    }
    public static double Clamp(double f, double low, double high) {
      return (f < low) ? low : (f > high) ? high : f;
    }
  }
}
