using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using lab3.Models;
using lab3.Models.Interfaces;
using lab3.Models.Shapes;

namespace lab3.Views
{
    public partial class MainWindow : Window
    {

        private Point _p1, _p2;
        private bool _isFirstPoint;
        private readonly ConfigReader _config;
        private readonly ShapeList _shapeList;
        private readonly ShapeList _shapePreviewList;
        private readonly ShapeDrawer _shapeDrawer;
        private Shape? _highlighting = null;

        private enum Mode{
            Dot = 0, Segment = 1, Triangle = 2, Rectangle = 3, Rhombus = 4, Ellipse = 5
        };
        private Mode _mode = Mode.Dot;

        private Color _color = Colors.CornflowerBlue;

        private int _thickness = 1;
        public MainWindow()
        {
            InitializeComponent();
            this._config = new ConfigReader();
            this._shapeList = new ShapeList();
            this._shapeDrawer = new ShapeDrawer();
            this._shapePreviewList = new ShapeList();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.Key)
            {
                case Key.D0:
                    _mode = Mode.Dot;
                    break;
                case Key.D1:
                    _mode = Mode.Segment;
                    break;
                case Key.D2:
                    _mode = Mode.Triangle;
                    break;
                case Key.D3:
                    _mode = Mode.Rectangle;
                    break;
                case Key.D4:
                    _mode = Mode.Rhombus;
                    break;
                case Key.D5:
                    _mode = Mode.Ellipse;
                    break;
                case Key.Up:
                    this._thickness += 1;
                    break;
                case Key.Down:
                    if (this._thickness > 1)
                        this._thickness -= 1;
                    break;
                case Key.Space:
                    var r = new Random();
                    this._color = Color.FromArgb((byte) r.Next(255), (byte) r.Next(255),
                                                (byte) r.Next(255), (byte) r.Next(255));
                    break;

            }
            //_shapeDrawer.Draw(_shapePreviewList, canvas);
            if (_isFirstPoint)
                Redraw();
        }

        
        
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                if (!_isFirstPoint)
                {
                    this._p1 = e.GetPosition(canvas);
                    this._p2 = this._p1;
                    if (this._mode == Mode.Dot)
                    {
                        _isFirstPoint = false;
                        var method = _config.getCreate( (int) _mode);
                        _shapeList.Add(  method( _p1, _p2, this._color, this._thickness ));
                        _shapeDrawer.Draw( _shapeList, canvas);

                    }
                    else
                    {
                        this._isFirstPoint = true;
                    }
                }
                else
                {
                    this._p2 = e.GetPosition(canvas);
                    var method = _config.getCreate( (int) _mode);
                    _shapeList.Add(  method( _p1, _p2, this._color, this._thickness ));
                    this._isFirstPoint = false;
                    _shapeDrawer.Draw( _shapeList, canvas);

                }

            }
            else if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                Point p = e.GetPosition(canvas);
                Shape? selected = _shapeList.FindNearCollision(p);
                if (selected != null)
                {
                    ISelectableShape? selectedCasted = selected as ISelectableShape;
                    _highlighting = selectedCasted?.GetHighlighting(selected.getArea(), selected.GetColor());
                }
                else
                {
                    _highlighting = null;
                }
            }
            Redraw();
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            if (_isFirstPoint)
            {
                _p2 = e.GetPosition(canvas);
                Redraw();
            }

        }

        private void Redraw()
        {
            var method = _config.getCreate( (int) _mode);
            canvas.Children.Clear();
            _shapePreviewList.Clear();
            _shapePreviewList.Add(  method( _p1, _p2, this._color, this._thickness ));
            if (_highlighting != null)
                _shapePreviewList.Add(_highlighting);
            _shapeDrawer.Draw(_shapeList, canvas);
            _shapeDrawer.Draw(_shapePreviewList, canvas);
        }

    }
}