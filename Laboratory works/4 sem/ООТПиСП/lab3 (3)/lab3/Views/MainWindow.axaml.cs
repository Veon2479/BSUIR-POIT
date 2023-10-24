using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Avalonia.Controls;
using Avalonia.Input;
using lab3.Models;
using lab3.Models.Interfaces;
using lab3.Models.Shapes;

namespace lab3.Views
{
    public partial class MainWindow : Window
    {

        private Point _p1, _p2;
        private bool _isFirstPoint;
        private readonly ShapeFactory _shapeFactory;
        private ShapeList _shapeList;
        private readonly ShapeList _shapePreviewList;
        private readonly ShapeDrawer _shapeDrawer;
        private Shape? _selected = null;
        private Shape? _highlighting = null;

        private enum Mode{
            Dot = 0, Segment = 1, Triangle = 2, Rectangle = 3, Rhombus = 4, Ellipse = 5,
            Ext1 = 6, Ext2 = 7, Ext3 = 8, Ext4 = 9
        };
        private Mode _mode = Mode.Dot;

        private Color _color = Colors.CornflowerBlue;

        private int _thickness = 1;
        public MainWindow()
        {
            InitializeComponent();
            this._shapeFactory = ShapeFactory.GetShapeFactory();
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
                case Key.D6:
                    if (_shapeFactory.GetTypesCount() > 6)
                        _mode = Mode.Ext1;
                    break;
                case Key.D7:
                    if (_shapeFactory.GetTypesCount() > 7)
                        _mode = Mode.Ext2;
                    break;
                case Key.D8:
                    if (_shapeFactory.GetTypesCount() > 8)
                        _mode = Mode.Ext3;
                    break;
                case Key.D9:
                    if (_shapeFactory.GetTypesCount() > 9)
                        _mode = Mode.Ext4;
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
                case Key.W:
                    MoveSelectedShape(0, -1);
                    break;
                case Key.A:
                    MoveSelectedShape(-1, 0);
                    break;
                case Key.S:
                    if ((e.KeyModifiers & KeyModifiers.Control) != 0)
                    {
                        SerializeShapes();
                    }
                    else
                    {
                        MoveSelectedShape(0, 1);
                    }
                    break;
                case Key.D:
                    MoveSelectedShape(1, 0);
                    break;
                case Key.O:
                    if ((e.KeyModifiers & KeyModifiers.Control) != 0)
                    {
                        DeserializeShapes();
                    }
                    break;

                
            }
            Console.WriteLine("CHECK!" + e.ToString());
            _shapeList.WriteShapes();

            //_shapeDrawer.Draw(_shapePreviewList, canvas);
            Redraw();
        }

        private void SerializeShapes()
        {
            var sr = new ShapeListSerializer();
            using (Stream fout = new FileStream("data.my", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                sr.Serialize(_shapeList, fout);
            }
           
        }

        private void DeserializeShapes()
        {
            var sr = new ShapeListSerializer();
            using (Stream fin = new FileStream("data.my", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                if (fin.Length != 0)
                    _shapeList = sr.Deserialize(fin);
            }
        }
        
        private void MoveSelectedShape(double dx, double dy)
        {
            if (_selected != null && _selected is IMovableShape)
            {
                double speed = 3;
                IMovableShape.Move(_selected, dx*speed, dy*speed);
            }
        }
        
        
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                if (!_isFirstPoint)
                {
                    _shapeList.WriteShapes();

                    this._p1 = e.GetPosition(canvas);
                    this._p2 = this._p1;
                    if (this._mode == Mode.Dot)
                    {
                        _isFirstPoint = false;
                        var newShape = _shapeFactory.GetShapeByNumber( (int) _mode, _p1, _p2, this._color, this._thickness);
                        _shapeList.Add(newShape);
                        _shapeDrawer.Draw( _shapeList, canvas);

                    }
                    else
                    {
                        this._isFirstPoint = true;
                    }
                    _shapeList.WriteShapes();

                    Console.WriteLine("first point selected");
                }
                else
                {
                    _shapeList.WriteShapes();

                    this._p2 = e.GetPosition(canvas);
                    var newShape = _shapeFactory.GetShapeByNumber( (int) _mode, _p1, _p2, this._color, this._thickness);
                    _shapeList.Add(newShape);
                    this._isFirstPoint = false;
                    _shapeDrawer.Draw( _shapeList, canvas);
                    
                    _shapeList.WriteShapes();
                    Console.WriteLine("second point selected");

                }

            }
            else if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                Point p = e.GetPosition(canvas);
                Shape? selected = _shapeList.FindNearCollision(p);
                if (selected != null)
                {
                    _selected = selected;
                    
                }
                else
                {
                    _highlighting = null;
                    _selected = null;
                }
            }
            Console.WriteLine("=========================");
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
            
            canvas.Children.Clear();
            _shapePreviewList.Clear();
            if (_isFirstPoint)
            {   
                var newShape = _shapeFactory.GetShapeByNumber( (int) _mode, _p1, _p2, this._color, this._thickness);
                _shapePreviewList.Add(newShape);

            }

            if (_selected != null)
            {
                ISelectableShape? selectedCasted = _selected as ISelectableShape;
                _highlighting = selectedCasted?.GetHighlighting(_selected.getArea(), _selected.GetColor());
                _shapePreviewList.Add(_highlighting);

            }
            _shapeDrawer.Draw(_shapeList, canvas);
            _shapeDrawer.Draw(_shapePreviewList, canvas);
        }

    }
}