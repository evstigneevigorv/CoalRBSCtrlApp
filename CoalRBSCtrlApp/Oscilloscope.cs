using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Threading;

namespace CoalRBSCtrlApp
{
    public partial class OscilloscopeWindow : DockingLibrary.DockableContent
    {
        private void OscilloscopeInit()
        {
            //double height = oscilloscopeWindow.oscWindowGrid.DesiredSize.Height;
            //double width = oscilloscopeWindow.oscWindowGrid.DesiredSize.Width;
            //statusLabel.Content = contentWidth.ToString() + " x " + contentHeight.ToString();

            // Слой сетки
            oscGridCanvas.Children.Clear();
            oscGridCanvas.Height = contentHeight;
            oscGridCanvas.Width = contentWidth;

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

            oscGridCanvas.Children.Add(gridPath);


            oscDataCanvas.Children.Clear();
            oscDataCanvas.Height = contentHeight;
            oscDataCanvas.Width = contentWidth;


        }

        private void oscTimer_Action(object arg)
        {
            if (!oscTimerLock)
            {
                oscTimerLock = true;

                oscDispatcher.Invoke(
                    new Action(() =>
                    {
                        try
                        {
                            if (adcDataValid)
                            {
                                oscDataCanvas.Children.Clear();
                                int xNmb = AdcData.GetLength(1);
                                double xStep = contentWidth / (xNmb - 1);
                                for (int i = 0; i < xNmb - 1; i++)
                                {
                                    Line l = new Line();
                                    l.X1 = i * (xStep);
                                    l.X2 = (i + 1) * xStep;
                                    l.Y1 = (0.5 - (AdcData[2, i] / (double)0x8000)) * contentHeight;
                                    l.Y2 = (0.5 - (AdcData[2, i + 1] / (double)0x8000)) * contentHeight;
                                    l.StrokeThickness = 2.0;
                                    l.Stroke = Brushes.Green;
                                    oscDataCanvas.Children.Add(l);
                                }
                                adcDataValid = false;
                            }
                        }
                        catch { };
                    }));

            }
            oscTimerLock = false;
        }
    }
}
