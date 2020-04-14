using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace CoalRBSCtrlApp
{
    public partial class MainWindow : Window
    {
        private const int UDP_RCV_PORT = 10900;
        private Thread udpRcvThread;

        private byte[] udpPckg;

        private void udpRcvThread_Action()
        {
            Int16[,] Data;
            Data = new Int16[4, 1024];

            oscilloscopeWindow.AdcData = new Int16[4, 1024];
            spectrumAnalyzerWindow.SpcData = new double[4, 512];
            UdpClient listener = new UdpClient(UDP_RCV_PORT);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, UDP_RCV_PORT);

            try
            {
                while (udpRcvThread.ThreadState == ThreadState.Running)
                {
                    udpPckg = listener.Receive(ref groupEP);

                    if (!oscilloscopeWindow.adcDataValid ||
                        !spectrumAnalyzerWindow.spcDataValid)
                        for (int j = 0; j*8 < udpPckg.Length; j++)
                            for (int k = 0; k < 4; k++)
                                Data[k, j] = (Int16)(udpPckg[j*8 + k * 2] * 0x100 + udpPckg[j*8 + k * 2 + 1]);

                    if (!oscilloscopeWindow.adcDataValid)
                    {
                        for (int j = 0; j * 8 < udpPckg.Length; j++)
                            for (int k = 0; k < 4; k++)
                                oscilloscopeWindow.AdcData[k, j] = Data[k, j];
                        oscilloscopeWindow.adcDataValid = true;
                    }

                    if (!spectrumAnalyzerWindow.spcDataValid)
                    {
                        for (int k = 0; k < 4; k++)
                            for (int j = 0; j < 512; j++)
                            {
                                double re = 0.0, im = 0.0;

                                for (int i = 0; i < 1024; i++)
                                {
                                    re += Data[k, i] * Math.Cos(-2 * Math.PI * i * j / 1024);
                                    im += Data[k, i] * Math.Sin(-2 * Math.PI * i * j / 1024);
                                }

                                spectrumAnalyzerWindow.SpcData[k, j] = 20 * Math.Log10(Math.Sqrt(re * re + im * im) / 32768.0);
                            }
                        spectrumAnalyzerWindow.spcDataValid = true;
                    }

                    /*
                    if (oscilloscopeWindow.adcDataValid) { }
                    Int16[] dsc = new Int16[4];
                    for (int i = 0, j = 0; i < udpPckg.Length; i += 8)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            dsc[k] = (Int16)(udpPckg[i + k * 2] * 0x100 + udpPckg[i + k * 2 + 1]);
                            oscilloscopeWindow.AdcData[k, j] = dsc[k];
                        }
                        j++;
                    }
                    oscilloscopeWindow.adcDataValid = true;
                    */
                }
            }
            catch
            {
                listener.Close();
            }
        }
    }
}
