using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Threading;

namespace CoalRBSCtrlApp
{
    public partial class SpectrumAnalyzerWindow : DockingLibrary.DockableContent
    {
        private void SpectrumInit()
        {
            //double height = oscilloscopeWindow.oscWindowGrid.DesiredSize.Height;
            //double width = oscilloscopeWindow.oscWindowGrid.DesiredSize.Width;
            //statusLabel.Content = contentWidth.ToString() + " x " + contentHeight.ToString();

            // Слой сетки
            spcGridCanvas.Children.Clear();
            spcGridCanvas.Height = contentHeight;
            spcGridCanvas.Width = contentWidth;

            Path gridPath = new Path();
            GeometryGroup gridGroup = new GeometryGroup();
            for (int i = 0; i < 10; i++)
            {
                LineGeometry horLine = new LineGeometry(new Point(0, i * contentHeight / 10),
                                                        new Point(contentWidth, i * contentHeight / 10));
                gridGroup.Children.Add(horLine);
            }
            for (int i = 0; i < 20; i++)
            {
                LineGeometry vertLine = new LineGeometry(new Point(i * contentWidth / 20, 0),
                                                         new Point(i * contentWidth / 20, contentHeight));
                gridGroup.Children.Add(vertLine);
            }
            gridPath.Data = gridGroup;
            gridPath.Stroke = Brushes.Gray;
            gridPath.StrokeThickness = 1;
            // gridPath.RenderTransform = new TranslateTransform(200, 100);

            spcGridCanvas.Children.Add(gridPath);


            spcDataCanvas.Children.Clear();
            spcDataCanvas.Height = contentHeight;
            spcDataCanvas.Width = contentWidth;
        }

        private void spcTimer_Action(object arg)
        {
            if (!spcTimerLock)
            {
                spcTimerLock = true;

                spcDispatcher.Invoke(
                    new Action(() =>
                    {
                        try
                        {
                            if (spcDataValid)
                            {
                                spcDataCanvas.Children.Clear();
                                int xNmb = SpcData.GetLength(1);
                                double xStep = contentWidth / (xNmb - 1);
                                for (int i = 0; i < xNmb - 1; i++)
                                {
                                    Line l = new Line();
                                    l.X1 = i * (xStep);
                                    l.X2 = (i + 1) * xStep;
                                    l.Y1 = (0.5 - (SpcData[2, i] / 100.0)) * contentHeight;
                                    l.Y2 = (0.5 - (SpcData[2, i + 1] / 100.0)) * contentHeight;
                                    l.StrokeThickness = 2.0;
                                    l.Stroke = Brushes.Green;
                                    spcDataCanvas.Children.Add(l);
                                }
                                spcDataValid = false;
                            }
                        }
                        catch { };
                    }));
            }
            spcTimerLock = false;
        }
    }
}
