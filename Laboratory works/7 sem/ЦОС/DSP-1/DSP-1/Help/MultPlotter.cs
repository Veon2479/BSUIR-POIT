using System.Collections.Generic;
using System.Collections.ObjectModel;
using OxyPlot;
using OxyPlot.Series;

namespace DSP_1.Help;

public class MultPlotter
{
    public PlotModel PlotModel { get; set; }
    
    private double _xmin, _xmax, _dx;
    private double _ymin, _ymax;
    private double _lastX;
    private int _points, _percImp;
    private LineSeries _data;
    
    public double LastY { get; set; }

    public Dictionary<Signals, Generator> Generators { get; set; }
    private Dictionary<Signals, double> _vals;

    public ObservableCollection<bool> IsEnabled { get; set; }
    
    public MultPlotter(double xmin, double xmax, int points, string title)
    {
        _xmin = xmin;
        _xmax = xmax;
        _points = points;
        _dx = (_xmax - xmin) / points;
        _ymin = -1;
        _ymax = 1;
        _percImp = 50;

        IsEnabled = new();
        for (int i = 0; i < 5; i++)
            IsEnabled.Add(false);
        
        _vals = new();
        _vals.Add(Signals.Sin, 0);
        _vals.Add(Signals.Saw, 0);
        _vals.Add(Signals.Triangle, 0);
        _vals.Add(Signals.Impulse, 0);
        _vals.Add(Signals.Random, 0);

        Generators = new();
        Generators.Add(Signals.Sin, new SinGenerator(_dx, _dx, _ymin, _ymax));
        Generators.Add(Signals.Saw, new SawGenerator(_dx, _dx, _ymin, _ymax));
        Generators.Add(Signals.Triangle, new TriangleGenerator(_dx, _dx, _ymin, _ymax));
        Generators.Add(Signals.Impulse, new ImpulseGenerator(_dx, _dx, _ymin, _ymax, _percImp));
        Generators.Add(Signals.Random, new RandomGenerator(_dx, _dx, _ymin, _ymax));
        
        PlotModel = new PlotModel
        {
            Title = title
        };
        _data = new LineSeries();
        for (int i = 0; i < _points; i++)
        {
            var x = _xmin + i * _dx;
            _data.Points.Add(new DataPoint(x, 0));
        }
        _lastX = _data.Points[^1].X;
        PlotModel.Series.Add(_data);
    }

    public void Update()
    {
        _lastX += _dx;
        for (int i = 0; i < _data.Points.Count - 1; i++)
        {
            _data.Points[i] = new DataPoint(_data.Points[i].X, _data.Points[i + 1].Y);
        }

        double y = 0;
        for (int i = 0; i < Generators.Keys.Count; i++)
        {
            var sig = (Signals)i;
            double val = Generators[sig].Generate(_lastX);
            if (IsEnabled[(int)sig])
                _vals[sig] = val;
            else
                _vals[sig] = 0;
            y += _vals[sig];
        }
        _data.Points[^1] = new DataPoint(_data.Points[^1].X, y);
        LastY = y;
        PlotModel.InvalidatePlot(true);
    }
    

}