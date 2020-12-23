using System;
using System.Collections.Generic;
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
  [Obsolete("No Impl",true)]
  public class CircleSegmentScrollBar : RangeBase {
    static CircleSegmentScrollBar() {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(CircleSegmentScrollBar), new FrameworkPropertyMetadata(typeof(CircleSegmentScrollBar)));
    }

    public CircleSegmentScrollBar() : base() {

    }

    //ContentPresenter PART_Bar;
    //Path PART_InteractableArea;

    readonly RingSegment _Segment = new RingSegment() { InnerRadius = 30.0, OuterRadius = 70.0, SegmentAngle = 90.0 };
    readonly CircleSegement _MiddlePath = new CircleSegement() { SegmentAngle = 90.0, Radius = 50.0};

    public Point RenderOrigin { get => (Point)GetValue(RenderOriginProperty); set => SetValue(RenderOriginProperty, value); }
    public static readonly DependencyProperty RenderOriginProperty
      = DependencyProperty.Register("RenderOrigin", typeof(Point), typeof(CircleSegmentScrollBar), new PropertyMetadata(new Point(0.0, 1.0)));
    public double RenderScaleX { get => (double)GetValue(RenderScaleXProperty); set => SetValue(RenderScaleXProperty, value); }
    public static readonly DependencyProperty RenderScaleXProperty
      = DependencyProperty.Register("RenderScaleX", typeof(double), typeof(CircleSegmentScrollBar), new PropertyMetadata(1.0));
    public double RenderScaleY { get => (double)GetValue(RenderScaleYProperty); set => SetValue(RenderScaleYProperty, value); }
    public static readonly DependencyProperty RenderScaleYProperty
      = DependencyProperty.Register("RenderScaleY", typeof(double), typeof(CircleSegmentScrollBar), new PropertyMetadata(1.0));
    public double RenderAngle { get => (double)GetValue(RenderAngleProperty); set => SetValue(RenderAngleProperty, value); }
    public static readonly DependencyProperty RenderAngleProperty
      = DependencyProperty.Register("RenderAngle", typeof(double), typeof(CircleSegmentScrollBar), new PropertyMetadata(0.0));

    public object Bar { get => (object)GetValue(BarProperty); set => SetValue(BarProperty, value); }
    public static readonly DependencyProperty BarProperty
      = DependencyProperty.Register("Bar", typeof(object), typeof(CircleSegmentScrollBar), new PropertyMetadata(null));

    public Geometry SegmentGeo { get => (Geometry)GetValue(SegmentGeoProperty.DependencyProperty); protected set => SetValue(SegmentGeoProperty, value); }
    public static readonly DependencyPropertyKey SegmentGeoProperty
      = DependencyProperty.RegisterReadOnly("SegmentGeo", typeof(Geometry), typeof(CircleSegmentScrollBar), new PropertyMetadata(null));

    public double SegmentAngle { get => (double)GetValue(SegmentAngleProperty); set => SetValue(SegmentAngleProperty, value); }
    public static readonly DependencyProperty SegmentAngleProperty
      = DependencyProperty.Register("SegmentAngle", typeof(double), typeof(CircleSegmentScrollBar),
        new PropertyMetadata(90.0, (d, e) => {
          var dd = (CircleSegmentScrollBar)d;
          dd._Segment.SegmentAngle = (double)e.NewValue;
          dd._MiddlePath.SegmentAngle = (double)e.NewValue;
          _UpdateGeo(dd, e);
        }));
    public double InnerRadius { get => (double)GetValue(InnerRadiusProperty); set => SetValue(InnerRadiusProperty, value); }
    public static readonly DependencyProperty InnerRadiusProperty
      = DependencyProperty.Register("InnerRadius", typeof(double), typeof(CircleSegmentScrollBar),
        new PropertyMetadata(30.0, (d, e) => {
          var dd = (CircleSegmentScrollBar)d;
          dd._Segment.InnerRadius = (double)e.NewValue;
          dd._MiddlePath.Radius = (dd.OuterRadius - (double)e.NewValue) * 0.5 + (double)e.NewValue;
          _UpdateGeo(dd, e);
        }));
    public double OuterRadius { get => (double)GetValue(OuterRadiusProperty); set => SetValue(OuterRadiusProperty, value); }
    public static readonly DependencyProperty OuterRadiusProperty
      = DependencyProperty.Register("OuterRadius", typeof(double), typeof(CircleSegmentScrollBar),
        new PropertyMetadata(50.0, (d, e) => {
          var dd = (CircleSegmentScrollBar)d;
          dd._Segment.OuterRadius = (double)e.NewValue;
          dd._MiddlePath.Radius = ((double)e.NewValue - dd.InnerRadius) * 0.5 + dd.InnerRadius;
          _UpdateGeo(dd, e);
        }));
    static void _UpdateGeo(CircleSegmentScrollBar dd, DependencyPropertyChangedEventArgs e) {
      dd.SegmentGeo = dd._Segment.GetGeometry();
      dd.CalToRender = dd._Segment.CalculatToRenderingMatrix;
      dd.RenderToCal = dd._Segment.RenderingToCalculatMatrix;
    }


    //public double BarPosX { get => (double)GetValue(BarPosXProperty.DependencyProperty); protected set => SetValue(BarPosXProperty, value); }
    //public static readonly DependencyPropertyKey BarPosXProperty
    //  = DependencyProperty.RegisterReadOnly("BarPosX", typeof(double), typeof(CircleSegmentScrollBar), new PropertyMetadata(0.0));
    //public double BarPosY { get => (double)GetValue(BarPosYProperty.DependencyProperty); protected set => SetValue(BarPosYProperty, value); }
    //public static readonly DependencyPropertyKey BarPosYProperty
    //  = DependencyProperty.RegisterReadOnly("BarPosY", typeof(double), typeof(CircleSegmentScrollBar), new PropertyMetadata(0.0));
    public double BarAngle { get => (double)GetValue(BarAngleProperty.DependencyProperty); protected set => SetValue(BarAngleProperty, value); }
    public static readonly DependencyPropertyKey BarAngleProperty
      = DependencyProperty.RegisterReadOnly("BarAngle", typeof(double), typeof(CircleSegmentScrollBar), new PropertyMetadata(0.0));

    private Matrix RenderToCal;
    private Matrix CalToRender;
    private bool _IsDown;
    private void Handle_MouseDown(object sender, MouseButtonEventArgs e) {
      _IsDown = true;
    }
    private void Handle_MouseUp(object sender, MouseButtonEventArgs e) {
      _IsDown = false;

    }
    private void Handle_MouseMove(object sender, MouseEventArgs e) {
      if (_IsDown) {
        //var CP = RenderToCal.Transform(e.GetPosition((IInputElement)sender));
        //var RawAng = Vector.AngleBetween((CP - Kits.Zero), Kits.Right - Kits.Zero);
        //var Ang = ((RawAng < 0.0 ? 180.0 - RawAng : RawAng) - _MiddlePath.StartAngle) / _MiddlePath.SegmentAngle;
        //Value = (Maximum - Minimum) * Ang;
        //var PonP = _MiddlePath.PointOnArc(Ang);
        //var RenPonP = CalToRender.Transform(PonP);
        //BarPosX = RenPonP.X;
        //BarPosY = RenPonP.Y;
      }
    }
    protected override void OnValueChanged(double oldValue, double newValue) {

    }

  }
}
