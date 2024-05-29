using System;
using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Vector = Avalonia.Vector;

namespace _3DViewer;

public partial class MainWindow : Window
{
    private State _state;

    public WriteableBitmap Bitmap { get => _bitmap;
        set { _bitmap = value; }
    }
    
    private const float sized = 2.0f;
    
    private WriteableBitmap _bitmap;
    private Renderer _renderer;

    private Image _imgc;
    public MainWindow()
    {
        var parser = new ObjParser();

        // var path = "/home/andmin/Workspace/3DViewer/Models/";
        // var file = "lenin.obj";
        // var file = "man.obj";
        // var file = "wolf.obj";
        // var file = "plane.obj";
        // var file = "pol.obj";
        // var file = "cube-b.obj";
        // var file = "admech.obj";

        var path = "/home/andmin/Workspace/3DViewer/Models/head/";
        var file = "african_head.obj";
        
        var model = parser.ParseObjFile(path, file);
        
        SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;
        Cursor = new Cursor(StandardCursorType.None);
        InitializeBitmap();

        DataContext = this;
        InitializeComponent();
        
        _imgc = this.FindControl<Image>("imageControl");

        _state = new State(Screens.Primary.Bounds.Width / sized, Screens.Primary.Bounds.Height / sized, Vector3.Zero);
        
        _renderer = new(ref model, in _bitmap, in _state);
        _renderer.Render(ref _bitmap, in _state);
    }
    
    private void InitializeBitmap()
    {
        Size windowSize = ClientSize;
        var width = (int)(Screens.Primary.Bounds.Width / sized);
        var height = (int)(Screens.Primary.Bounds.Height / sized);
        var dpi = 96;
        
        Console.WriteLine($"Working Area has height {height} and width {width}");   

        _bitmap = new WriteableBitmap(
            new PixelSize(width, height), 
            new Vector(dpi, dpi), 
            PixelFormat.Rgba8888,
            AlphaFormat.Premul);

    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        float scaleStep = 0.005f, posStep = 0.01f, rotStep = 0.05f, stateStep = 0.1f;
        bool isOwn = true;
        switch (e.Key)
        {
            case Key.Add:
            case Key.OemPlus:
                _state.Scale += scaleStep;
                break;
            case Key.Subtract:
            case Key.OemMinus:
                _state.Scale -= scaleStep;
                break;
            
            case Key.Q:
                _state.Position.X += posStep;
                break;
            case Key.A:
                _state.Position.X -= posStep;
                break;
            case Key.W:
                _state.Position.Y += posStep;
                break;
            case Key.S:
                _state.Position.Y -= posStep;
                break;
            case Key.E:
                _state.Position.Z += posStep;
                break;
            case Key.D:
                _state.Position.Z -= posStep;
                break;
            
            case Key.U:
                _state.Rotate.X += rotStep;
                break;
            case Key.J:
                _state.Rotate.X -= rotStep;
                break;
            case Key.I:
                _state.Rotate.Y += rotStep;
                break;
            case Key.K:
                _state.Rotate.Y -= rotStep;
                break;
            case Key.O:
                _state.Rotate.Z += rotStep;
                break;
            case Key.L:
                _state.Rotate.Z -= rotStep;
                break;
            
            case Key.Z:
                _state.LightDirection -= new Vector3(stateStep, 0, 0);
                break;
            case Key.X:
                _state.LightDirection -= new Vector3(0, stateStep, 0);
                break;
            case Key.C:
                _state.LightDirection -= new Vector3(0, 0, stateStep);
                break;
            case Key.B:
                _state.LightDirection += new Vector3(stateStep, 0, 0);
                break;
            case Key.N:
                _state.LightDirection += new Vector3(0, stateStep, 0);
                break;
            case Key.M:
                _state.LightDirection += new Vector3(0, 0, stateStep);
                break;
            
            case Key.R:
                _state.Reset();
                break;
            
            case Key.Escape:
                Close();
                break;
            
            default:
                isOwn = false;
                break;
        }

        if (isOwn)
        {
            _renderer.Render(ref _bitmap, in _state);
            _imgc.InvalidateVisual();
        }
    }
}
