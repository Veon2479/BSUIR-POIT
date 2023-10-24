using System;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace DSP_1.Help;

public delegate double Func(double x);

public class Plotter
{
    public PlotModel PlotModel { get; set; }

    private Func _func;
    private double _xmin, _xmax, _dx;
    private double _lastX;
    private int _points;
    private LineSeries _data;
    
    public Plotter(Func func, double xmin, double xmax, int points, string title)
    {
        _func = func;
        _xmin = xmin;
        _lastX = _xmin;
        _xmax = xmax;
        _points = points;
        _dx = (_xmax - xmin) / points;
        PlotModel = new PlotModel
        {
            Title = title
        };
        _data = new LineSeries();
        for (int i = 0; i < _points; i++)
        {
            var x = _xmin + i * _dx;
            _data.Points.Add(new DataPoint(x, _func(x)));
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
        _data.Points[^1] = new DataPoint(_data.Points[^1].X, _func(_lastX));
        PlotModel.InvalidatePlot(true);
    }
}