using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;

namespace CoalRBSCtrlApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ControlPanelWindow controlPanelWindow = new ControlPanelWindow();
        private OscilloscopeWindow oscilloscopeWindow = new OscilloscopeWindow();
        private SpectrumAnalyzerWindow spectrumAnalyzerWindow = new SpectrumAnalyzerWindow();

        private UdpClient udpClient;

        private Dispatcher mainDispatcher { set; get; }


        public MainWindow()
        {
            udpClient = new UdpClient();

            InitializeComponent();

            mainDispatcher = Dispatcher.CurrentDispatcher;



            udpRcvThread = new Thread(udpRcvThread_Action);
            udpRcvThread.Priority = ThreadPriority.AboveNormal;
            udpRcvThread.Start();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dockManager.ParentWindow = this;

            controlPanelWindow.DockManager = dockManager;
            oscilloscopeWindow.DockManager = dockManager;
            spectrumAnalyzerWindow.DockManager = dockManager;

            controlPanelWindow.Show(Dock.Right);
            oscilloscopeWindow.ShowAsDocument();
            spectrumAnalyzerWindow.ShowAsDocument();
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (udpRcvThread != null)
            {
                udpRcvThread.Abort();
                udpClient.Send(new byte[1] { 0 }, 1, "127.0.0.1", UDP_RCV_PORT);
            }
        }
    }
}
