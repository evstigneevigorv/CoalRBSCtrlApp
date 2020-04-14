using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CoalRBSCtrlApp
{
    /// <summary>
    /// Interaction logic for SpectrumAnalyzerWindow.xaml
    /// </summary>
    public partial class SpectrumAnalyzerWindow : DockingLibrary.DockableContent
    {
        private double contentHeight;
        private double contentWidth;
        private bool contentIsInitialized = false;

        private Dispatcher spcDispatcher { set; get; }

        public Timer spcTimer;
        public bool spcTimerLock;

        public double[,] SpcData;
        public bool spcDataValid = false;

        public SpectrumAnalyzerWindow()
        {
            InitializeComponent();

            spcDispatcher = Dispatcher.CurrentDispatcher;

            spcTimerLock = false;
            spcTimer = new Timer(spcTimer_Action, null, 1000, 500);
        }

        private void SpcWindowGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            contentHeight = spcWindowGrid.ActualHeight;
            contentWidth = spcWindowGrid.ActualWidth;
            if (!contentIsInitialized)
            {
                SpectrumInit();
                contentIsInitialized = true;
            }
        }

        private void SpcWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (spcTimer != null) spcTimer.Dispose();
        }
    }
}
