using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DSP_1.Help;

namespace DSP_1
{
    public partial class MainWindow : Window
    {
        
        public Plotter SinPlot { get; set; }
        public Plotter RandomPlot { get; set; }
        public Plotter SawPlot { get; set; }
        public Plotter TrianglePlot { get; set; }
        public Plotter ImpulsePlot { get; set; }
        
        public Plotter SinPlotMod { get; set; }
        public Plotter RandomPlotMod { get; set; }
        public Plotter SawPlotMod { get; set; }
        public Plotter TrianglePlotMod { get; set; }
        public Plotter ImpulsePlotMod { get; set; }

        public MultPlotter MultPlotter { get; set; }
        
        public double Ymin { get; set; }
        public double Ymax { get; set; }
        public int PercImp { get; set; }

        private DispatcherTimer _timer;
        private double _xmin, _xmax, _dx;
        private int _points;

        private Generator sawGenerator, triangleGenerator, impulseGenerator, sinGenerator, randomGenerator;
        private Generator sawGeneratorMod, triangleGeneratorMod, impulseGeneratorMod, sinGeneratorMod, randomGeneratorMod;

        private double _yminmod, _ymaxmod;
        
        public MainWindow()
        {
            DataContext = this;

            Ymin = 0;
            Ymax = 1;
            PercImp = 60;
            
            _xmin = -5;
            _xmax = 5;
            _points = 1000;
            _dx = Math.Abs((_xmax - _xmin) / _points);

            

            sawGenerator = new SawGenerator(_dx, _dx, Ymin, Ymax);
            triangleGenerator = new TriangleGenerator(_dx, _dx, Ymin, Ymax);
            impulseGenerator = new ImpulseGenerator(_dx, _dx, Ymin, Ymax, PercImp);
            sinGenerator = new SinGenerator(_dx, _dx, Ymin, Ymax);
            randomGenerator = new RandomGenerator(_dx, _dx, Ymin, Ymax);
            
            SinPlot = new Plotter(x => sinGenerator.Generate(x), _xmin, _xmax, _points, "Sin Signal");
            RandomPlot = new Plotter(x => randomGenerator.Generate(x), _xmin, _xmax, _points,
                "Noise Signal");
            SawPlot = new Plotter(x => sawGenerator.Generate(x), _xmin, _xmax, _points, "Saw Signal");
            ImpulsePlot = new Plotter(x => impulseGenerator.Generate(x), _xmin, _xmax, _points, "Impulse Signal");
            TrianglePlot = new Plotter(x => triangleGenerator.Generate(x), _xmin, _xmax, _points, "Triangle Signal");
            
            _yminmod = -1;
            _ymaxmod = 1;
            
            sawGeneratorMod = new SawGenerator(_dx, _dx, _yminmod, _ymaxmod);
            triangleGeneratorMod = new TriangleGenerator(_dx, _dx, _yminmod, _ymaxmod);
            impulseGeneratorMod = new ImpulseGenerator(_dx, _dx, _yminmod, _ymaxmod, PercImp);
            sinGeneratorMod = new SinGenerator(_dx, _dx, _yminmod, _ymaxmod);
            randomGeneratorMod = new RandomGenerator(_dx, _dx, _yminmod, _ymaxmod);
            
            SinPlotMod = new Plotter(x => sinGeneratorMod.Generate(x), _xmin, _xmax, _points, "Sin Signal");
            RandomPlotMod = new Plotter(x => randomGeneratorMod.Generate(x), _xmin, _xmax, _points,
                "Noise Signal");
            SawPlotMod = new Plotter(x => sawGeneratorMod.Generate(x), _xmin, _xmax, _points, "Saw Signal");
            ImpulsePlotMod = new Plotter(x => impulseGeneratorMod.Generate(x), _xmin, _xmax, _points, "Impulse Signal");
            TrianglePlotMod = new Plotter(x => triangleGeneratorMod.Generate(x), _xmin, _xmax, _points, "Triangle Signal");

            MultPlotter = new MultPlotter(_xmin, _xmax, _points, "Polyphonic Signal");
     
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Tick += TimerTick;
            _timer.Start();
            InitializeComponent();

        }
        
        
        
        private void TimerTick(object sender, EventArgs e)
        {
            SinPlot.Update();
            RandomPlot.Update();
            SawPlot.Update();
            TrianglePlot.Update();
            ImpulsePlot.Update();
            
            MultPlotter.Update();

            double a = MultPlotter.LastY;
            
            sinGeneratorMod.setMax(_ymaxmod * a);
            sinGeneratorMod.setMin(_yminmod * a);
            
            sawGeneratorMod.setMax(_ymaxmod * a);
            sawGeneratorMod.setMin(_yminmod * a);
            
            triangleGeneratorMod.setMax(_ymaxmod * a);
            triangleGeneratorMod.setMin(_yminmod * a);
            
            randomGeneratorMod.setMax(_ymaxmod * a);
            randomGeneratorMod.setMin(_yminmod * a);
            
            impulseGeneratorMod.setMax(_ymaxmod * a);
            impulseGeneratorMod.setMin(_yminmod * a);
            
            SinPlotMod.Update();
            RandomPlotMod.Update();
            SawPlotMod.Update();
            TrianglePlotMod.Update();
            ImpulsePlotMod.Update();

        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SubmitButtonT1_Click(object? sender, RoutedEventArgs e)
        {
            double val;
            bool f = false;
            if (double.TryParse(this.FindControl<TextBox>("YminField").Text, out val))
            {
                Ymin = val;
                sinGenerator.setMin(Ymin);
                sawGenerator.setMin(Ymin);
                triangleGenerator.setMin(Ymin);
                impulseGenerator.setMin(Ymin);
                randomGenerator.setMin(Ymin);
            }
            if (double.TryParse(this.FindControl<TextBox>("YmaxField").Text, out val))
            {
                Ymax = val;
                sinGenerator.setMax(Ymax);
                sawGenerator.setMax(Ymax);
                triangleGenerator.setMax(Ymax);
                impulseGenerator.setMax(Ymax);
                randomGenerator.setMax(Ymax);
            }

            int tmp;
            if (int.TryParse(this.FindControl<TextBox>("ImpPercField").Text, out tmp))
            {
                PercImp = tmp;
                (impulseGenerator as ImpulseGenerator)!.SetPercImp(PercImp);
            }
        }
    }
}