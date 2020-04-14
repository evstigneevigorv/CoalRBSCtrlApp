using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
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
    /// Interaction logic for OscilloscopeWindow.xaml
    /// </summary>
    public partial class OscilloscopeWindow : DockingLibrary.DockableContent
    {
        private double contentHeight;
        private double contentWidth;
        private bool contentIsInitialized = false;

        private Dispatcher oscDispatcher { set; get; }

        public Timer oscTimer;
        public bool oscTimerLock;

        public Int16[,] AdcData;
        public bool adcDataValid = false;

        public OscilloscopeWindow()
        {
            InitializeComponent();

            oscDispatcher = Dispatcher.CurrentDispatcher;

            oscTimerLock = false;
            oscTimer = new Timer(oscTimer_Action, null, 0, 100);
        }

        private void OscWindowGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            contentHeight = oscWindowGrid.ActualHeight;
            contentWidth = oscWindowGrid.ActualWidth;
            if (!contentIsInitialized)
            {
                OscilloscopeInit();
                contentIsInitialized = true;
            }
        }

        private void oscWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (oscTimer != null) oscTimer.Dispose();
        }
    }
}
