// DemoAvalonia (c) 2021/23 Baltasar MIT License <jbgarcia@uvigo.es>


using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace TallerDIA.Models;

/// <summary>Support for charts</summary>
public class Chart : Control
{
    /// <summary>Select the type of chart.</summary>
    public enum ChartType { Lines, Bars }

    /// <summary>Support for fonts</summary>
    public struct Font
    {
        public Font(double fontSize)
        {
            Family = FontFamily.Default;
            Style = FontStyle.Normal;
            Weight = FontWeight.Normal;
            Size = fontSize;
        }

        public FontFamily Family { get; set; }
        public FontStyle Style { get; set; }
        public FontWeight Weight { get; set; }
        public double Size { get; set; }
    }

    /// <summary>Parameterless constructor. Change the properties.</summary>
    public Chart()
    {
        // Create the other properties
        _values = new List<int>();
        _labels = new List<string>();
        LegendX = "Value";
        LegendY = "Date";
        _normalizedData = Array.Empty<int>();
        FrameWidth = 30;
        Type = ChartType.Lines;

        AxisPen = new Pen(Brushes.White, 4);
        DataPen = new Pen(Brushes.Yellow, 2);
        GridPen = new Pen(Brushes.Gray);
        Background = Brushes.Black;
        LegendBrush = Brushes.White;




        DataFont = new Font(12) { Family = FontFamily.Default };
        LabelFont = new Font(12) { Family = FontFamily.Default };
        LegendFont = new Font(12) { Family = FontFamily.Default };
        DrawGrid = true;

    }

    public override void Render(DrawingContext graphs)
    {

        base.Render(graphs);
        Draw(graphs);
    }

    public void Draw()
    {
        if (!isInitialized)
        {
            InitialColors();
        }
        InvalidateVisual();
    }

    /// <summary>Redraws the chart</summary>
    private void Draw(DrawingContext graphs)
    {
        _graphics = graphs;

        ClearCanvas();

        // Frame
        DrawRectangle(
            new Pen(Background, FrameWidth),
            0, 0,
            Width,
            Height);

        // Chart components
        DrawAxis();
        DrawData();
        DrawLegends();
    }

    private void DrawLegends()
    {
        // Legend for X
        DrawString(
                LegendFont,
                LegendBrush,
                Width / 2
                  - (int)DataOrgPosition.X / 2,
                (int)(FramedEndPosition.Y + 20),
                LegendX,
                vertical: false);

        // Legend for Y
        DrawString(
                LegendFont,
                LegendBrush,
                (int)-FramedEndPosition.Y,
                (int)FramedOrgPosition.X - 20,
                LegendY,
                vertical: true);
    }

    private void DrawData()
    {
        NormalizeData();

        int numValues = _normalizedData.Length;
        int baseLine = (int)DataOrgPosition.Y;
        int xGap = (int)((double)GraphWidth / (numValues + 1));

        // Start drawing point
        var currentPoint = new Point(
                        x: DataOrgPosition.X + xGap,
                        y: baseLine - _normalizedData[0]);

        // Next points
        for (int i = 0; i < numValues; ++i)
        {
            string tag = _values[i].ToString();

            var nextPoint = new Point(
                x: DataOrgPosition.X + xGap * (i + 1),
                y: baseLine - _normalizedData[i]
            );

            if (Type == ChartType.Bars)
            {
                currentPoint = new Point(nextPoint.X, baseLine);
            }

            DrawLine(DataPen, currentPoint, nextPoint);
            DrawString(
                        font: DataFont,
                        DataPen.Brush ?? Brushes.Black,
                        x: (int)(nextPoint.X - DataPen.Thickness / 2),
                        y: (int)(nextPoint.Y - DataPen.Thickness),
                        msg: tag);

            currentPoint = nextPoint;
        }
    }

    private void DrawAxis()
    {
        // Grid
        if (DrawGrid)
        {
            int numValues = _values.Count;
            int xGap = (int)((double)GraphWidth / (numValues + 1));
            int yGap = GraphHeight / 10;

            // Labels available?
            if (_labels.Count == 0)
            {
                _labels.AddRange(
                    Enumerable.Range(1, _values.Count)
                        .Select(x => Convert.ToString(x)));
            }

            // Vertical lines going right
            for (int i = 0; i < numValues + 1; ++i)
            {
                int columnPos = (int)DataOrgPosition.X + xGap * i;

                // Y axis (1 per value)
                DrawLine(
                    pen: GridPen,
                    x1: columnPos,
                    y1: (int)DataOrgPosition.Y,
                    x2: columnPos,
                    y2: (int)FramedOrgPosition.Y);

                // The label
                if (i - 1 >= 0
                  && i - 1 < _labels.Count)
                {
                    DrawString(
                        font: LabelFont,
                        LegendBrush,
                        x: columnPos,
                        y: (int)(FramedEndPosition.Y + 2),
                        msg: _labels[i - 1]
                    );
                }
            }

            // Horizontal lines going up
            for (int i = 0; i < 10; ++i)
            {
                int rowPos = (int)DataOrgPosition.Y - yGap * i;

                // X axis (tenth line)
                DrawLine(
                    GridPen,
                    (int)DataOrgPosition.X,
                    rowPos,
                    (int)FramedEndPosition.X,
                    rowPos);
            }
        }

        // Y axis
        DrawLine(AxisPen,
                       (int)FramedOrgPosition.X,
                       (int)FramedOrgPosition.Y,
                       (int)FramedOrgPosition.X,
                       (int)FramedEndPosition.Y);

        // X axis
        DrawLine(AxisPen,
                       (int)FramedOrgPosition.X,
                       (int)FramedEndPosition.Y,
                       (int)FramedEndPosition.X,
                       (int)FramedEndPosition.Y);
    }

    private void NormalizeData()
    {
        int numValues = _values.Count;
        int maxValue = _values.Max();
        if (maxValue == 0)
        {
            _normalizedData = _values.ToArray();

            for (int i = 0; i < numValues; ++i)
            {
                _normalizedData[i] = 0;
            }

            return;
        }
        _normalizedData = _values.ToArray();

        for (int i = 0; i < numValues; ++i)
        {
            _normalizedData[i] =
                                _values[i] * GraphHeight / maxValue;
        }

        return;
    }

    /// <summary>
    /// Gets or sets the values used as data.
    /// </summary>
    /// <value>The values.</value>
    public IEnumerable<int> Values
    {
        get => _values.ToArray();
        set
        {
            _values.Clear();
            _values.AddRange(value);
        }
    }

    /// <summary>
    /// Gets or sets the labels used for the data
    /// under the X axis.
    /// </summary>
    /// <value>The labels.</value>
    public IEnumerable<string> Labels
    {
        get => _labels.ToArray();

        set
        {
            _labels.Clear();
            _labels.AddRange(value);
        }
    }

    /// <summary>
    /// Gets the framed origin.
    /// </summary>
    /// <value>The origin <see cref="Point"/>.</value>
    public Point DataOrgPosition
    {
        get
        {
            int margin = (int)(AxisPen.Thickness * 2);

            return new Point(
                FramedOrgPosition.X + margin,
                FramedEndPosition.Y - margin);
        }
    }

    /// <summary>
    /// Gets or sets the width of the frame around the chart.
    /// </summary>
    /// <value>The width of the frame.</value>
    public int FrameWidth
    {
        get; set;
    }

    /// <summary>
    /// Gets the framed origin.
    /// </summary>
    /// <value>The origin <see cref="Point"/>.</value>
    public Point FramedOrgPosition => new(FrameWidth, FrameWidth);

    /// <summary>
    /// Gets the framed end.
    /// </summary>
    /// <value>The end <see cref="Point"/>.</value>
    public Point FramedEndPosition => new(Width - FrameWidth,
                                            Height - FrameWidth);

    /// <summary>
    /// Gets the width of the graph.
    /// </summary>
    /// <value>The width of the graph.</value>
    public int GraphWidth => Width - FrameWidth * 2;

    /// <summary>
    /// Gets the height of the graph.
    /// </summary>
    /// <value>The height of the graph.</value>
    public int GraphHeight => Height - FrameWidth * 2;

    /// <summary>
    /// Gets or sets the pen used to draw the axis.
    /// </summary>
    /// <value>The axis <see cref="Pen"/>.</value>
    public Pen AxisPen
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the pen used to draw the data.
    /// </summary>
    /// <value>The data <see cref="Pen"/>.</value>
    public Pen DataPen
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the pen used to draw the grid.
    /// </summary>
    /// <value>The grid <see cref="Pen"/>.</value>
    public Pen GridPen
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the font for data.
    /// </summary>
    /// <value>The data <see cref="Font"/>.</value>
    public Font DataFont
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the font for labels.
    /// </summary>
    /// <value>The label <see cref="Font"/>.</value>
    public Font LabelFont
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the legend for the x axis.
    /// </summary>
    /// <value>The legend for axis x.</value>
    public string LegendX
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the legend for the y axis.
    /// </summary>
    /// <value>The legend for axis y.</value>
    public string LegendY
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the font for legends.
    /// </summary>
    /// <value>The <see cref="Font"/> for legends.</value>
    public Font LegendFont
    {
        get; set;
    }

    /// <summary>Get or sets whether to show a grid or not.</summary>
    public bool DrawGrid
    {
        get; set;
    }

    /// <summary>The color for the text in the legend.</summary>
    public IBrush LegendBrush
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the type of the chart.
    /// </summary>
    /// <value>The <see cref="ChartType"/>.</value>
    public ChartType Type
    {
        get; set;
    }

    public IBrush Background { get; set; }

    private new int Width => (int)Bounds.Width;

    private new int Height => (int)Bounds.Height;

    private void DrawRectangle(IPen pen, int x, int y, int width, int height)
    {
        Graphics.DrawRectangle(
                            pen.Brush,
                            pen,
                            new Rect(x, y, width, height));
    }

    private void DrawLine(IPen pen, int x1, int y1, int x2, int y2)
    {
        DrawLine(pen, new Point(x1, y1), new Point(x2, y2));
    }

    private void DrawLine(IPen pen, Point p1, Point p2)
    {
        Graphics.DrawLine(pen, p1, p2);
    }

    private void DrawString(
                    Font font, IBrush color, int x, int y, string msg,
                    bool vertical = false)
    {
        // Left to right, or viceversa
        var flow = FlowDirection.LeftToRight;

        if (CultureInfo.CurrentCulture.TextInfo.IsRightToLeft)
        {
            flow = FlowDirection.RightToLeft;
        }

        // Font
        var typeface = new Typeface(font.Family, font.Style, font.Weight);

        // Formatted text
        var text = new FormattedText(
                            msg,
                            CultureInfo.CurrentCulture,
                            flow,
                            typeface,
                            font.Size,
                            color);

        // Vertical
        if (vertical)
        {
            Graphics.PushTransform(
                            new RotateTransform(270).Value);
        }

        Graphics.DrawText(text, new Point(x, y));
    }

    private void ClearCanvas()
    {
        Graphics.DrawRectangle(
            Background,
            null,
            new Rect(0, 0, Width, Height));
    }

    private DrawingContext Graphics
    {
        get
        {
            if (_graphics is null)
            {
                throw new NoNullAllowedException("graphics (drawingcontext) is null");
            }

            return _graphics;
        }
    }

    private void InitialColors()
    {
        if (ActualThemeVariant.ToString().Equals("Dark"))
        {
            AxisPen = new Pen(Brushes.White, 4);
            DataPen = new Pen(Brushes.Yellow, 2);
            GridPen = new Pen(Brushes.Gray);
            Background = Brushes.Black;
            LegendBrush = Brushes.White;
        }
        else
        {
            AxisPen = new Pen(Brushes.Black, 4);
            DataPen = new Pen(Brushes.Red, 2);
            GridPen = new Pen(Brushes.Gray);
            Background = Brushes.White;
            LegendBrush = Brushes.Black;
        }
        isInitialized = true;
    }

    private readonly List<int> _values;
    private readonly List<string> _labels;
    private DrawingContext? _graphics;
    private int[] _normalizedData;
    public bool isInitialized = false;
}
