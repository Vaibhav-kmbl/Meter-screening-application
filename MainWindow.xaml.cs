using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Xamarin.Forms;
using System.Collections.Generic;
using static Meter_screening_application.MainWindow;
using System.Collections.ObjectModel;
using System.Threading;
using System.Security.Cryptography;
using System.Diagnostics.Eventing.Reader;
using System.Configuration;
//using ZigLibrary.Helper;
//using ZigLibrary.Commands;
//using ZigLibrary.Commands.V2;
//using BaseCommand = ZigLibrary.Commands.BaseCommand;
//using static ZigLibrary.Helper.SerialPortHelperV2;
using System.IO;
using System.ComponentModel.Design;
using log4net;
using System.Reflection;
using System.Collections;
using System.DirectoryServices.ActiveDirectory;
//using Newtonsoft.Json.Linq;

namespace Meter_screening_application
{

    public partial class MainWindow : Window
    {
        public SerialPort jig;
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
        public SerialPort[] serial;
        public int MAXRETRY = 2;
        public int stt = 1;
        public ObservableCollection<data> dataList = new ObservableCollection<data>();
        public ObservableCollection<data> dataList2 = new ObservableCollection<data>();
        public int[] meter_authenticate = [0, 0, 0];
        public int[] Meter_firmware = [0, 0, 0];
        string[] RTCvalues = new string[3];
        // public int[] meter_authenticate = [1, 1, 1];

        public double[,] meter_result_data = new double[3, 10];
        public double[,] meter_result_data2 = new double[3, 6];
        public double[,] tolerance = new double[2, 10];
        public int retry = 1, delay = 2000;
        public int timeout = 5000;
        public bool[] RTCOutput = { false, false, false , false , false , false,false };
        public bool[] CoverOpen = { false, false, false, false, false, false, false };

        public bool[] MagnetTemper = { false, false, false, false, false, false, false };

        Commands cmd;
        ConfigCommands Configcmd = new ConfigCommands();
        TemperCoverOpen TemperCoverOpen = new TemperCoverOpen();    
        public DateTime startTime;
        public bool jig_res_to_app = false, jig_response = false, master_meter_resp = false, meter_resp1 = false, meter_resp2 = false;
        public bool five_ampere = false, mili_ampere = false, app_to_be_started = false;
        public string filepath = "C:\\Users\\VaibhavSinghal\\OneDrive - Sinhal Udyog pvt ltd\\Documents\\meter.txt";

        public MainWindow()
        {
            InitializeComponent();
            cmd = new Commands();
            serial = new SerialPort[3];
        
          

        }
        private void click_me(Object ob, EventArgs e)
        {

            string[] str = new string[4];
             str = [Configcmd.MASTERMETER, Configcmd.METER1, Configcmd.METER2, Configcmd.JIG];
            //  str = ["COM13", "COM15", "COM14", "COM16"];
            // str = ["COM22", "COM24", "COM26", "COM21"]; 
             // str = ["COM5", "COM7", "COM9", "COM11"];
         //   str = ["COM29", "COM28", "COM27", "COM30"];

            Initial(str);
        }
        private async void MonitorAndProcess()
        {
            while (true)
            {
                log.Info("-<---------------Process of testing being started------------->");
                // await Task.Delay(1000);
                DataIncoming();
                //  app_to_be_started = true;
             //   meter_authenticate[0] = 0;
                if (app_to_be_started && jig.IsOpen)
                {
                    int[] Values = { 0, 1, 2 };
                    Jig(retry, cmd.jig_command_5A, cmd.jig_response_5A);
                    jig_response = jig_res_to_app;
                    await Task.Delay(Configcmd.delay);
                     //   jig_res_to_app = true;
                    if (jig_res_to_app)
                    {
                        log.Info($"Current switched to 5A {jig_res_to_app}");
                        jig_res_to_app = false;                     
                        five_ampere = true;                                       
                        List<Task> tasks = new();
                        foreach (int i in Values)
                        {
                            tasks.Add(Task.Run(() => RunTaskAsync(i)));
                        }
                     
                        await Task.WhenAll(tasks);                     
                        if (meter_result_data[0, 0] > 0 && Math.Abs(meter_result_data[0, 3]) <= 1 && CoverOpen[0] && RTCOutput[0] && MagnetTemper[0])
                        {
                            data masterr = new data("Master", Math.Round(meter_result_data[0, 0], 3).ToString(), "-",
                                Math.Round(meter_result_data[0, 1], 3).ToString() + "(N)", Math.Round(meter_result_data[0, 2], 3).ToString() + "(P)",
                                "-","-"
                               , Math.Round(meter_result_data[0, 3], 3).ToString() + "(N)", Math.Round(meter_result_data[0, 4], 3).ToString() + "(P)",
                                "-","-", RTCOutput[0].ToString(), RTCvalues[0],
                                CoverOpen[0].ToString() + "(C)", MagnetTemper[0].ToString() + "(M)",
                                "PASSED"
                                );
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                dataList.Add(masterr);
                            });
                        }
                        else
                        {
                            data masterr = new data("Master", Math.Round(meter_result_data[0, 0], 3).ToString(), "-",
                                                          Math.Round(meter_result_data[0, 1], 3).ToString() + "(N)", Math.Round(meter_result_data[0, 2], 3).ToString() + "(P)",
                                                          "-", "-"
                                                         , Math.Round(meter_result_data[0, 3], 3).ToString() + "(N)", Math.Round(meter_result_data[0, 4], 3).ToString() + "(P)",
                                                          "-", "-", RTCOutput[0].ToString() , RTCvalues[0],
                                                           CoverOpen[0].ToString() + "(C)", MagnetTemper[0].ToString() + "(M)",

                                                          "FAILED"
                                                          );                                                    
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                dataList.Add(masterr);
                            });                        
                        }
                        if (Meter_firmware[1] == 1)
                        {
                            log.Info($"meter 1 has old firmware");
                            PopulateDataOldFirmwareHighCurrent(1);
                        }
                        else
                        {
                            log.Info($"meter 1 has new firmware");
                            PopulateDataNewFirmwareHighCurrent(1);
                        }
                        if (Meter_firmware[2] == 1)
                        {
                            log.Info($"meter 2has old firmware");
                            PopulateDataOldFirmwareHighCurrent(2);
                        }
                        else
                        {
                            log.Info($"meter 2 has new firmware");
                            PopulateDataNewFirmwareHighCurrent(2);
                        }
                        log.Info($"RTC Values are {RTCvalues[0]} , {RTCvalues[1]} , {RTCvalues[2]}");
                        RTCvalues = new string[3];                     
                        log.Info("          ");
                    }
                    log.Info($"master meter --> {meter_authenticate[0]}   1st meter --> {meter_authenticate[1]}  2nd meter --> {meter_authenticate[2]}");
                    mili_ampere = true;
                    five_ampere = false;
                    retry = 1;
                    await Task.Run(() => Jig(retry, cmd.jig_command_500mA, cmd.jig_response_500mA));
                    jig_response = jig_res_to_app;
                  //  jig_res_to_app = true;
                    await Task.Delay(Configcmd.delay);
                    Console.WriteLine();
                    if (jig_res_to_app)
                    {
                        log.Info("<------------------testing for 500mA started-------------->");
                        List<Task> tasks = new();
                        foreach (int i in Values)
                        {
                            tasks.Add(Task.Run(() => RunTaskAsync(i)));
                        }
                        await Task.WhenAll(tasks);
                        if (meter_result_data[0, 5] > 0 && Math.Abs(meter_result_data[0, 8]) <= 1&& CoverOpen[3] && RTCOutput[3] && MagnetTemper[3])
                        {
                            data masterr = new data("Master", Math.Round(meter_result_data[0, 5], 3).ToString(), "-",
                                Math.Round(meter_result_data[0, 6], 3).ToString() + "(N)", Math.Round(meter_result_data[0, 7], 3).ToString() + "(P)",
                                "-", "-",
                                Math.Round(meter_result_data[0, 8], 3).ToString() + "(N)", Math.Round(meter_result_data[0, 9], 3).ToString() + "(P)",
                                "-", "-", RTCOutput[3].ToString(), RTCvalues[0],
                                 CoverOpen[3].ToString() + "(C)", MagnetTemper[3].ToString() + "(M)",
                                "PASSED"
                                );
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                dataList2.Add(masterr);
                            });
                        }
                        else
                        {
                            data masterr = new data("Master", Math.Round(meter_result_data[0, 5], 3).ToString(),
                                "-", Math.Round(meter_result_data[0, 5], 3).ToString() + "(N)", Math.Round(meter_result_data[0, 7], 3).ToString() + "(P)", "-", "-",
                                Math.Round(meter_result_data[0, 8], 3).ToString() + "(N)", Math.Round(meter_result_data[0, 9], 3).ToString() + "(P)",
                                "-", "-", RTCOutput[0].ToString(),RTCvalues[0],
                                 CoverOpen[0].ToString() + "(C)", MagnetTemper[0].ToString() + "(M)",
                                "FAILED"
                                                    );

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                dataList2.Add(masterr);
                            });
                        }
                       
                        if (Meter_firmware[1] == 1)
                        {
                            log.Info($"meter 1 has old firmware");
                            PopulateDataOldFrmwareLowCurrent(1);
                        }
                        else
                        {
                            log.Info($"meter 1 has new firmware");
                            PopulateDataNewFirmwareLowCurrent(1);
                        }
                        if (Meter_firmware[1] == 1)
                        {
                            log.Info($"meter 2 has old firmware");
                            PopulateDataOldFrmwareLowCurrent(2);
                        }
                        else
                        {
                            log.Info($"meter 2 has new firmware");
                            PopulateDataNewFirmwareLowCurrent(2);
                        }
                        log.Info($"RTC Values are {RTCvalues[0]} , {RTCvalues[1]} , {RTCvalues[2]}");

                    }

                    if (dataList2.Count == 3)
                        Dispatcher.Invoke(() =>
                        {
                            if (dataList2[1].Result == "FAILED")
                            {
                                Meter_first.Content = "Failed";
                                Meter_first.Background = new SolidColorBrush(Colors.Red);
                            }
                            else
                            {
                                Meter_first.Content = "Passed";
                                Meter_first.Background = new SolidColorBrush(Colors.Green);
                            }
                            if (dataList2[2].Result == "FAILED")
                            {
                                Meter_second.Content = "Failed";
                                Meter_second.Background = new SolidColorBrush(Colors.Red);
                            }
                            else
                            {
                                Meter_second.Content = "Passed";
                                Meter_second.Background = new SolidColorBrush(Colors.Green);
                            }
                        });
                    CloseJig(1);
                    log.Info($"jig closed");
                    await Task.Delay(Configcmd.DELAYAFTERCLOSINGJIG);
                    jig.DiscardInBuffer();
                    jig.DiscardOutBuffer();
                    AppendDataGridToTextFile(DataGrid1, dataList, Configcmd.filepath);
                    AppendDataGridToTextFile(DataGrid2, dataList2, Configcmd.filepath);
                    app_to_be_started = false;
                    log.Info("---------------------------Test Completed---------------------------");
                }
            }
        }
        private void PopulateDataOldFirmwareHighCurrent(int i)
        {
            if (meter_authenticate[i] == 0 || meter_authenticate[0]==0)
            {
                log.Info($"meter {i} not working/failed the test/incorrect output");
                data daa;
                daa = new data(i + "st Meter-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-","-","-","-", "-", "FAILED");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dataList.Add(daa);

                });

            }
            else
            {
                for (int j = 0; j <= 2; j++)
                {
                    tolerance[i - 1, j] = (meter_result_data2[i, j] - meter_result_data[0, j]) / meter_result_data[0, j];
                    tolerance[i - 1, j] = tolerance[i - 1, j] * 100;

                }
                if (Math.Abs(tolerance[i - 1, 0]) > Configcmd.VOLTAGETOLERANCE || double.IsNaN(tolerance[i - 1, 0]))
                {
                    log.Error($"voltage tolerance level for meter {i} is out of bound for highcurrent");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 1]) > Configcmd.CURRENTTOLERANCENEUTRAL5A || double.IsNaN(tolerance[i - 1, 1]))
                {
                    log.Error($"current neutral tolerance level for meter {i} is out of bound for high current");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 2]) > Configcmd.CURRENTTOLERANCEPHASE5A || double.IsNaN(tolerance[i - 1, 2]))
                {
                    log.Error($"current phase tolerance level for meter {i} is out of bound for high current");
                    meter_authenticate[i] = 0;
                }
                if (!RTCOutput[i])
                {
                    log.Error($"RTC check fail for meter {i} for high current");
                    meter_authenticate[i] = 0;
                }
                string strr = "tolerance levels of meter " + i + " when 5A current is passed " + "are " + tolerance[i - 1, 0] + " " + tolerance[i - 1, 1] + " " + tolerance[i - 1, 2];
                Console.WriteLine(strr);
                data m1 = new data(i + "st Meter",
                    Math.Round(meter_result_data2[i, 0], 3).ToString(),
                    Math.Round(tolerance[i - 1, 0], 3).ToString(),
                    Math.Round(meter_result_data2[i, 1], 3).ToString() + "(N)", Math.Round(meter_result_data2[i, 2], 3).ToString() + "(P)",
                    Math.Round(tolerance[i - 1, 1], 3).ToString() + "(N)", Math.Round(tolerance[i - 1, 2], 3).ToString() + "(P)",
                    "-", "-"
                    , "-", "-", RTCOutput[i].ToString(), RTCvalues[i],
                    "-" , "-",
                    ""
                );

                Application.Current.Dispatcher.Invoke(() =>
                {
                    dataList.Add(m1);
                });

            }
            if (meter_authenticate[i] == 1) dataList[i].Result = "PASSED";
            else dataList[i].Result = "FAILED";
        }
        private void PopulateDataNewFirmwareLowCurrent(int i)
        {
            if (meter_authenticate[i] == 0 || dataList[i].Result == "FAILED" || meter_authenticate[0] == 0)
            {
                log.Info($"meter {i} not working/failed the test/incorrect output");
                data daa;
                //  daa = new data(i + "st Meter", "-", "-", "-", "-", "-", "-", "FAILED");
                daa = new data(i + "st Meter", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-" , "-", "-" ,"-" , "-" , "FAILED");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dataList2.Add(daa);
                });

            }
            else
            {
                for (int j = 0; j <= 4; j++)
                {
                    if (meter_authenticate[i] == 0) break;
                    else
                    {
                        tolerance[i - 1, j + 5] = (meter_result_data[i, j + 5] - meter_result_data[0, j + 5]) / meter_result_data[0, j + 5];
                        tolerance[i - 1, j + 5] = tolerance[i - 1, j + 5] * 100;
                    }
                }
                string strr = "tolerance levels of meter " + i + " when 5A current is passed " + "are " + tolerance[i - 1, 0] + " " + tolerance[i - 1, 1] + " " + tolerance[i - 1, 2];
                Console.WriteLine(strr);

                data m1 = new data(i + "st Meter", Math.Round(
                    meter_result_data[i, 5], 3).ToString(), Math.Round(tolerance[i - 1, 5], 3).ToString(),
                    Math.Round(meter_result_data[i, 6], 3).ToString() + "(N)", Math.Round(meter_result_data[i, 7], 3).ToString() + "(P)",
                    Math.Round(tolerance[i - 1, 6], 3).ToString() + "(N)", Math.Round(tolerance[i - 1, 7], 3).ToString() + "(P)",
                    Math.Round(meter_result_data[i, 8], 3).ToString() + "(N)", Math.Round(meter_result_data[i, 9], 3).ToString() + "(P)",
                    Math.Round(tolerance[i - 1, 8], 3).ToString() + "(N)", Math.Round(tolerance[i - 1, 9], 3).ToString() + "(P)"
                    , RTCOutput[i+3].ToString(), RTCvalues[i],
                     CoverOpen[i+3].ToString() + "(C)", MagnetTemper[i+3].ToString() + "(M)", ""
    );
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dataList2.Add(m1);
                });
                if (Math.Abs(tolerance[i - 1, 5]) > Configcmd.VOLTAGETOLERANCE || double.IsNaN(tolerance[i - 1, 5]))
                {
                    log.Error($"voltage tolerance level for meter {i} is out of bound");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 6]) > Configcmd.CURRENTTOLERANCENEUTRAL500mA || double.IsNaN(tolerance[i - 1, 6]))
                {
                    log.Error($" neutral current tolerance level for meter {i} is out of bound");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 7]) > Configcmd.CURRENTTOLERANCEPHASE500mA || double.IsNaN(tolerance[i - 1, 7]))
                {
                    log.Error($"phase current tolerance level for meter {i} is out of bound value {tolerance[i - 1, 7]}");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 8]) > Configcmd.PFTOLERANCENEUTRAL500mA || double.IsNaN(tolerance[i - 1, 8]))
                {
                    log.Error($"pf neutral tolerance level for meter {i} is out of bound value tolerance[i-1,8]");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 9]) > Configcmd.PFTOLERANCEPHASE500mA || double.IsNaN(tolerance[i - 1, 9]))
                {
                    log.Error($"pf phase tolerance level for meter {i} is out of bound value {tolerance[i - 1, 9]}");
                    meter_authenticate[i] = 0;
                }
                if (!RTCOutput[i + 3])
                {
                    log.Error($"RTC check fail for meter {i} for low current");
                    meter_authenticate[i] = 0;
                }
               
                if (!MagnetTemper[i+3])
                {
                    log.Error($"Magnet Tamper fail for meter {i} for high current");
                    meter_authenticate[i] = 0;
                }
                if (!CoverOpen[i+3])
                {
                    log.Error($"Cver open fail for meter {i} for high current");
                    meter_authenticate[i] = 0;
                }
                if (meter_authenticate[i] == 0) dataList2[i].Result = "FAILED";
                if (meter_authenticate[i] == 1) dataList2[i].Result = "PASSED";
            }
        }
        private void PopulateDataOldFrmwareLowCurrent(int i)
        {
            if (meter_authenticate[i] == 0 || dataList[i].Result == "FAILED" || meter_authenticate[0] == 0)
            {
                log.Info($"meter {i} not working/failed the test/incorrect output");
                data daa;
                //  daa = new data(i + "st Meter", "-", "-", "-", "-", "-", "-", "FAILED");
                daa = new data(i + "st Meter", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-" , "-" , "-","-", "FAILED");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dataList2.Add(daa);
                });

            }
            else
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (meter_authenticate[i] == 0) break;
                    else
                    {
                        tolerance[i - 1, j + 5] = (meter_result_data2[i, j + 5] - meter_result_data[0, j + 5]) / meter_result_data[0, j + 5];
                        tolerance[i - 1, j + 5] = tolerance[i - 1, j + 5] * 100;
                    }
                }
                string strr = "tolerance levels of meter " + i + " when 500mA current is passed " + "are " + tolerance[i - 1, 0] + " " + tolerance[i - 1, 1] + " " + tolerance[i - 1, 2];
                Console.WriteLine(strr);

                data m1 = new data(i + "st Meter", Math.Round(
                    meter_result_data2[i, 5], 3).ToString(), Math.Round(tolerance[i - 1, 5], 3).ToString(),
                    Math.Round(meter_result_data2[i, 6], 3).ToString() + "(N)", Math.Round(meter_result_data2[i, 7], 3).ToString() + "(P)",
                    Math.Round(tolerance[i - 1, 6], 3).ToString() + "(N)", Math.Round(tolerance[i - 1, 7], 3).ToString() + "(P)",
                  "-", "-"
                  , "-", "-"               
                  , RTCOutput[i+3].ToString(),RTCvalues[i],
                  "-" , "-" , ""

    );
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dataList2.Add(m1);
                });
                if (Math.Abs(tolerance[i - 1, 5]) > Configcmd.VOLTAGETOLERANCE || double.IsNaN(tolerance[i - 1, 5]))
                {
                    log.Error($"voltage tolerance level for meter {i} is out of bound");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 6]) > Configcmd.CURRENTTOLERANCENEUTRAL500mA || double.IsNaN(tolerance[i - 1, 6]))
                {
                    log.Error($" neutral current tolerance level for meter {i} is out of bound");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 7]) > Configcmd.CURRENTTOLERANCEPHASE500mA || double.IsNaN(tolerance[i - 1, 7]))
                {
                    log.Error($"phase current tolerance level for meter {i} is out of bound value {tolerance[i - 1, 7]}");
                    meter_authenticate[i] = 0;
                }
                if (!RTCOutput[i + 3])
                {
                    log.Error($"RTC check fail for meter {i} for low current");
                    meter_authenticate[i] = 0;
                }

                if (meter_authenticate[i] == 0) dataList2[i].Result = "FAILED";
                if (meter_authenticate[i] == 1) dataList2[i].Result = "PASSED";

            }
        }
        private void PopulateDataNewFirmwareHighCurrent(int i)
        {
            if (meter_authenticate[i] == 0 || meter_authenticate[0] == 0 )
            {
                log.Info($"meter {i} not working/failed the test/incorrect output");
                data daa;
                daa = new data(i + "st Meter-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-","-" , "-" , "-", "FAILED");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dataList.Add(daa);

                });



            }
            else
            {
                for (int j = 0; j <= 4; j++)
                {
                    tolerance[i - 1, j] = (meter_result_data[i, j] - meter_result_data[0, j]) / meter_result_data[0, j];
                    tolerance[i - 1, j] = tolerance[i - 1, j] * 100;

                }
                if (Math.Abs(tolerance[i - 1, 0]) > Configcmd.VOLTAGETOLERANCE || double.IsNaN(tolerance[i - 1, 0]))
                {
                    log.Error($"voltage tolerance level for meter {i} is out of bound for highcurrent");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 1]) > Configcmd.CURRENTTOLERANCENEUTRAL5A || double.IsNaN(tolerance[i - 1, 1]))
                {
                    log.Error($"current neutral tolerance level for meter {i} is out of bound for high current");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 2]) > Configcmd.CURRENTTOLERANCEPHASE5A || double.IsNaN(tolerance[i - 1, 2]))
                {
                    log.Error($"current phase tolerance level for meter {i} is out of bound for high current");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 3]) > Configcmd.PFTOLERANCENEUTRAL5A || double.IsNaN(tolerance[i - 1, 3]))
                {
                    log.Error($"pf neutral tolerance level for meter {i} is out of bound for high current");
                    meter_authenticate[i] = 0;
                }
                if (Math.Abs(tolerance[i - 1, 4]) > Configcmd.PFTOLERANCEPHASE5A || double.IsNaN(tolerance[i - 1, 4]))
                {
                    log.Error($"pf phase tolerance level for meter {i} is out of bound for high current");
                    meter_authenticate[i] = 0;
                }
                if (!RTCOutput[i])
                {
                    log.Error($"RTC check fail for meter {i} for high current");
                    meter_authenticate[i] = 0;
                }
                if (!MagnetTemper[i])
                {
                    log.Error($"Magnet Tamper fail for meter {i} for high current");
                    meter_authenticate[i] = 0;
                }
                if (!CoverOpen[i])
                {
                    log.Error($"Cver open fail for meter {i} for high current");
                    meter_authenticate[i] = 0;
                }
                string strr = "tolerance levels of meter " + i + " when 5A current is passed " + "are " + tolerance[i - 1, 0] + " " + tolerance[i - 1, 1] + " " + tolerance[i - 1, 2];
                Console.WriteLine(strr);
                data m1 = new data(i + "st Meter",
                    Math.Round(meter_result_data[i, 0], 3).ToString(),
                    Math.Round(tolerance[i - 1, 0], 3).ToString(),
                    Math.Round(meter_result_data[i, 1], 3).ToString() + "(N)", Math.Round(meter_result_data[i, 2], 3).ToString() + "(P)",
                    Math.Round(tolerance[i - 1, 1], 3).ToString() + "(N)", Math.Round(tolerance[i - 1, 2], 3).ToString() + "(P)",
                    Math.Round(meter_result_data[i, 3], 3).ToString() + "(N)", Math.Round(meter_result_data[i, 4], 3).ToString() + "(P)",
                    Math.Round(tolerance[i - 1, 3], 3).ToString() + "(N)", Math.Round(tolerance[i - 1, 4], 3).ToString() + "(P)",
                     RTCOutput[i].ToString(), RTCvalues[i],
                      CoverOpen[i].ToString() + "(C)", MagnetTemper[i].ToString() + "(M)",
                    ""
                );
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dataList.Add(m1);
                });
            }
            if (meter_authenticate[i] == 1) dataList[i].Result = "PASSED";
            else dataList[i].Result = "FAILED";

        }
        private async Task RunTaskAsync(int i)
        {
            bool flag = false;
            const int maxRetries = 2;
            int retryCount = 0;
           
            try
            {
                SingleMeterAuthenticate(i);

                byte[] meter_test_response = new byte[70];
                if (meter_authenticate[i] == 1)
                {
                    log.Info($"{i}th meter authenticated");
                    log.Info($"Test started on {i}th meter");

                    while (retryCount <= maxRetries)
                    {
                        meter_test_response = new byte[70];
                        serial[i].DiscardInBuffer();
                        serial[i].DiscardOutBuffer();

                        if (serial[i].IsOpen && meter_authenticate[i] == 1)
                        {
                            try
                            {
                                serial[i].Write(cmd.meter_test_command, 0, 75);
                                log.Info($"Command written to meter {i}, attempt {retryCount + 1}");
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex + $"Failed writing command on meter {i}");
                                retryCount++;
                                continue;
                            }
                            DateTime startTime = DateTime.Now;
                            int start = 0;
                            int timeout = Configcmd.TIMEOUTCURRENTVOLTAGECOMMAND;
                            while (true)
                            {
                                int bytesToRead = serial[i].BytesToRead;
                                if (bytesToRead > 0)
                                {
                                    int bytesRead = Math.Min(bytesToRead, 70 - start);
                                    serial[i].Read(meter_test_response, start, bytesRead);
                                    start += bytesRead;

                                    log.Info($"Bytes read by meter {i}: {bytesRead}, Response: {cmd.PrintResponseString(meter_test_response)}");

                                    if (start >= 70)
                                    {
                                        flag = true;
                                        cmd.PrintResponse(meter_test_response);
                                        log.Info($"Response complete for meter {i}: {cmd.PrintResponseString(meter_test_response)}");
                                        break;
                                    }
                                    if (start == 9 && cmd.ResponseMatchString(meter_test_response.Take(9).ToArray(), cmd.MeterNotGivingResponse)) 
                                    {
                                        Meter_firmware[i] =1;
                                        log.Info($"meter {i} has old firmware response match with -->> {cmd.MeterNotGivingResponse} has response -->> {cmd.PrintResponseString(meter_test_response)}");
                                        break;
                                    }
                                }

                                if ((DateTime.Now - startTime).TotalMilliseconds > timeout)
                                {
                                    log.Warn($"Timeout for meter {i}, total bytes received: {start}");
                                    retryCount++;
                                    break;
                                }
                            }
                               
                        }
                       
                        else
                        {
                            log.Warn($"Meter {i} is not open or not authenticated");
                            break;
                        }
                        if (Meter_firmware[i] == 1)
                        {
                            break;
                        }
                        if (flag) break;
                    }

                    if (meter_authenticate[i] == 1 && flag)
                    {
                        log.Info($"Meter {i} fetched results successfully");
                        CheckValues(meter_test_response, i);
                    }
                    else if (Meter_firmware[i] ==1)
                    {
                        log.Info($"meter {i} has older firmware");
                        MeterTestResultFirmwareChange(i);
                    }
                    else if (!flag)
                    {
                        log.Warn($"Meter {i} failed after {maxRetries} retries");
                    }
                  if(five_ampere)  RTCOutput[i] = RTCCheckTest(i);
                    if (mili_ampere ) RTCOutput[3+i] = RTCCheckTest(i);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex + $"Failed to run task for meter {i}");
            }
        }

        private void MeterTestResultFirmwareChange(int i)
        {
            if (meter_authenticate[i] == 1 && serial[i].IsOpen)
            {
                if (Meter_firmware[i] ==1)
                {
                    byte[] buffer = new byte[21];
                    ValuesCalculationSecondFirmware v = new ValuesCalculationSecondFirmware();
                    int retries = 0;
                    bool flag = false;
                   while(retries <= Configcmd.RETRIESOLDFIRMWARE)
                    {
                        serial[i].DiscardInBuffer();
                        serial[i].DiscardOutBuffer();


                        try
                        {
                            serial[i].Write(cmd.meter_command_for_vi_check_second_firmware, 0, cmd.meter_command_for_vi_check_second_firmware.Length);
                            log.Info($"second firmware command written successfully on meter {i}");
                        }
                        catch (Exception ex)
                        {
                            log.Info($"second firmware command written unsuccessfully on meter {i}");
                        }
                        buffer = new byte[21];
                        int start = 0;
                        DateTime startTime = DateTime.Now;
                        while(true){
                            int bytee = serial[i].BytesToRead;
                            if (bytee > 0)
                            {
                               serial[i].Read(buffer, start, Math.Min(bytee, 21 - start));
                                start += Math.Min(bytee, 21 - start);
                                log.Info($"total bytes readed from meter {i} of length -->> {start} with response {cmd.PrintResponseString(buffer)}");
                                if(start >= 21)
                                {
                                    flag = true;
                                    retries++;
                                    break;
                                }
                            }
                            if ((DateTime.Now - startTime).TotalMilliseconds > Configcmd.TIMEOUTOLDFIRMWARE)
                            {
                                log.Info($"Timeout for meter {i} with retry number {retries} and total bytes recieved {start}");
                                retries++;
                                break;
                            }                         
                        }
                        if (flag)
                        {                           
                            if (v.check(buffer)) break;
                        }

                       
                    }
                   if(flag && v.check(buffer))
                    {
                        v.ValueCalculation(buffer, five_ampere, mili_ampere , meter_result_data2 , i);
                    }
                    else
                    {
                        log.Info($"result is not given by meter {i} ");
                    }


                }
                else
                {
                    log.Info($"meter {i} firmaware output is false");
                }
            }
            else
            {
                log.Info($"meter result for second command cannot be fetched meter {i} authentication -->> {meter_authenticate[i]} or port open -->> {serial[i].IsOpen}");
            }
        }

        private void SingleMeterAuthenticate(int i)
        {
            int maxRetries = Configcmd.RETRYMETERAUTH;
            int attempt = 0;
            bool success = false;
            while (attempt <= maxRetries && !success)
            {
                serial[i].DiscardInBuffer();
                serial[i].DiscardOutBuffer();
                attempt++;
                startTime = DateTime.Now;
                try
                {
                    serial[i].Write(cmd.meter_auth_code, 0, cmd.meter_auth_code.Length);
                    log.Info($"Meter auth command for meter {i} written successfully");
                }
                catch (Exception e)
                {
                    log.Error($"Error during command write: {e}");
                    Thread.Sleep(100);
                    continue;
                }
                Byte[] buffer = new byte[cmd.meter_auth_resp.Length];
                startTime = DateTime.Now;
                int byteeDone = 0;
                while (true)
                {
                    int bytee = serial[i].BytesToRead;
                    if (bytee > 0)
                    {
                        serial[i].Read(buffer, byteeDone, Math.Min(bytee, 9 - byteeDone));
                        byteeDone += Math.Min(bytee, 9 - byteeDone);
                        log.Info($"respnse fetched from {i} meter for auth is -->> {cmd.PrintResponseString(buffer)}");
                        if (byteeDone == 9)
                        {
                            log.Info($"{i}th meter fetched 9 bytes command");
                            break;
                        }
                    }

                    if ((DateTime.Now - startTime).TotalMilliseconds > timeout)
                    {
                        if (i == 0)
                            log.Error($"Timeout: Unable to read auth response from master meter where recieved bytes are {bytee} and response string is -->> {cmd.PrintResponseString(buffer)}");

                        else log.Error($"Timeout: Unable to read auth response from meter {i} where recieved bytes are {bytee} and response string is -->> {cmd.PrintResponseString(buffer)}");
                        break;
                    }



                }
                if (cmd.ResponseMatchString(buffer, cmd.meter_auth_resp_string))
                {
                    meter_authenticate[i] = 1;
                    log.Info($"meter {i} authenticated");
                    success = true;
                    break;
                }
                else
                {
                    meter_authenticate[i] = 0;
                    log.Error($"Authentication failed for meter {i} on attempt {attempt}.");
                }
                if (!success && attempt <= maxRetries)
                {
                    log.Error($"Retrying authentication for meter {i}...");
                }
            }
            if (!success)
            {
                log.Error($"Authentication failed for meter {i} after {maxRetries + 1} attempts.");
                meter_authenticate[i] = 0;
            }
            else
            {
                log.Info($"Authentication succeeded for meter {i}.");
            }
        }

        private bool RTCCheckTest(int i)
        {
            byte[] buffer = new byte[18];
            int retries = 0;
            RTCCheckCalculation RTCCheckCalculation = new RTCCheckCalculation(); 
            try
            {

                while (retries < Configcmd.RTCCHECKMAXRETRIES)
                {
                    serial[i].DiscardInBuffer();
                    serial[i].DiscardOutBuffer();
                    try
                    {
                        serial[i].Write(cmd.RTCCheckCommand, 0, cmd.RTCCheckCommand.Length);
                        log.Info($"RTC check command written successfully on merter {i}");

                    }
                    catch (Exception e)
                    {
                        log.Error($"RTC check command written FAILED on merter {i}");

                    }
                    buffer = new byte[18];
                    int start = 0;
                    bool flag = false;
                    DateTime startTime = DateTime.Now;
                    while (true)
                    {
                        int bytee = serial[i].BytesToRead;
                        if (bytee > 0)
                        {
                            serial[i].Read(buffer, start, Math.Min(bytee, 18 - start));
                            start += bytee;
                            log.Info($"bytes recieved for RTCCheck on meter {i} are -->> {start} where string is --->>> {cmd.PrintResponseString(buffer)}");
                            if (start >= 18)
                            {
                                flag = true;
                                log.Info($"response recieved for RTCCheck on meter {i} are -->> {start} where string is --->>> {cmd.PrintResponseString(buffer)}");
                                break;
                            }
                        }
                        if ((DateTime.Now - startTime).TotalMilliseconds > Configcmd.RTCCHECKTIMOUT)
                        {
                            log.Info($"timeout on RTC Check for meter {i} where total bytes recieved are {start} and response string is -->> {cmd.PrintResponseString(buffer)} on retry number -->> {retries}");
                            retries++;
                            break;
                        }
                    }
                    if (flag && RTCCheckCalculation.Check(buffer))
                    {
                        log.Info($"Calculating RTC values for meter {i}");
                        return RTCCheckCalculation.FinalResult(buffer , RTCvalues,i);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Info(ex + "$EXCEPTION CAUSED IN RTC TESTING FOR METER {i}");
            }
            return false;
        }

        private async void Initial(string[] str)
        {
            try
            {
                await Task.Run(() => InitializeSerialPorts(str));
                await Task.Run(() => MonitorAndProcess());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void InitializeSerialPorts(string[] str)
        {
            for (int i = 0; i <= 2; i++)
            {
                const int maxtry = 2;
                int rety = 0;
                while (maxtry >= rety)
                {


                    serial[i] = new SerialPort(str[i], 9600, Parity.None, 8);
                    try
                    {
                        serial[i].Open();
                        log.Info($"serial port {i} opened with port number {str[i]}");

                        break;
                    }
                    catch (Exception ex)
                    {
                        log.Error($"serial port {i} failed to open with port number {str[i]}");
                        rety++;
                    }
                }

            }
            jig = new SerialPort(str[3], 9600, Parity.None, 8);
            const int maxtry2 = 2;
            int rety2 = 0;
            while (maxtry2 >= rety2)
            {
                try
                {
                    jig.Open();
                    log.Info($"jig opened with port number {str[3]}");
                    break;
                }
                catch (Exception ex)
                {
                    log.Error($"jig failed to open with port number {str[3]}");

                    rety2++;
                }
            }
            if (rety2 == 3) log.Error("failed to open jig port");
        }

        private void ReloadView()
        {
            Dispatcher.Invoke(() =>
            {
                DataGrid1.ItemsSource = null;
                DataGrid2.ItemsSource = null;
                dataList2?.Clear();
                dataList?.Clear();

            });
            Application.Current.Dispatcher.Invoke(() =>
            {
                //dataList = null;
                //dataList2 = null;
               
                DataGrid1.ItemsSource = dataList;
                DataGrid2.ItemsSource = dataList2;
                Meter_second.Background = new SolidColorBrush(Colors.Orange);
                Meter_first.Background = new SolidColorBrush(Colors.Orange);
                Meter_first.Content = "Inprogress";
                Meter_second.Content = "Inprogress";
                meter_authenticate[0] = 0;
                meter_authenticate[1] = 0;
                meter_authenticate[2] = 0;
                meter_result_data = new double[3, 10];
                meter_result_data2 = new double[3, 10];
                tolerance = new double[3, 10];
                five_ampere = false;
                mili_ampere = false;
                Meter_firmware = new int[3];
                MagnetTemper = new bool[6];
                CoverOpen = new bool[6];
                RTCOutput = new bool[6];
            });

            var parent = this.Parent as ContentControl;
            if (parent != null)
            {
                parent.Content = null;
                parent.Content = new MainWindow();
            }
        }
        private void Jig(int retry, Byte[] jig_command, Byte[] jig_response)

        {
            if (jig.IsOpen)
            {
                jig.DiscardInBuffer();
                jig.DiscardOutBuffer();

                Byte[] jig_res = new Byte[12];

                try
                {
                    jig.Write(jig_command, 0, jig_command.Length);
                }
                catch
                {
                    log.Info($"jig failed to write {cmd.PrintResponseString(jig_command)}");

                }

                startTime = DateTime.Now;
                while (true)
                {

                    if ((DateTime.Now - startTime).TotalMilliseconds > timeout)
                    {

                        log.Info($"jig failed to read for command {cmd.PrintResponseString(jig_command)}");


                        if (retry == 1)
                        {
                            retry = 0;
                            Jig(retry, jig_command, jig_response);
                        }
                        jig_res_to_app = false;
                        break;
                    }
                    int bytee = jig.BytesToRead;
                    if (bytee == 12)
                    {
                        jig.Read(jig_res, 0, 12);
                        log.Info($" Response read by jig for command {cmd.PrintResponseString(jig_command)} is {cmd.PrintResponseString(jig_res)}");
                        cmd.PrintResponse(jig_res);

                        break;

                    }
                }


                if (cmd.ResponseMatch(jig_res, jig_response))
                {


                    log.Info("Jig fetched successful response");

                    cmd.PrintResponse(jig_response);

                    jig_res_to_app = true;
                    return;
                }
                else
                {
                    if (retry == 1)
                    {
                        retry = 0;
                        log.Info($"Jig retrying for command {cmd.PrintResponseString(jig_command)}");
                        Jig(retry, jig_command, jig_response);
                    }
                    else
                    {
                        jig_res_to_app = false;
                        log.Error("resp doesnt match with value after retry " + jig_res_to_app);
                        return;
                    }
                }

            }



        }
        private void DataIncoming()
        {
           
            while (true)
            {
                byte[] buffer = new byte[11];             
                DateTime starttime = DateTime.Now;
                int start = 0;
              
                while (true)
                {
                    int bytee = jig.BytesToRead;
                    if (bytee > 11)
                    {
                        jig.DiscardInBuffer();
                        jig.DiscardOutBuffer();
                    }
                  
                    if (bytee > 0)
                    {
                        jig.Read(buffer, start, Math.Min(bytee, 11 - start));
                        start += Math.Min(bytee, 11 - start);

                        if (start == 11)
                        {

                           

                            if (cmd.ResponseMatchString(buffer, cmd.jig_start_resp_string))
                            {
                                ReloadView();
                                log.Info("response match for starting the test");
                                app_to_be_started = true;
                                Task.Delay(Configcmd.JIGONDELAY);
                                log.Info("starting the test");
                                break;
                            }
                        }


                    }
                    if ((DateTime.Now - starttime).TotalMilliseconds > timeout)
                    {
                        log.Info($"do this again as byte recieved on start is {start}");
                        break;
                    }
                }
                if (app_to_be_started == true) break;
            }
        }
        private void CloseJig(int retry)
        {
            jig.DiscardOutBuffer();
            jig.DiscardInBuffer();
            try
            {
                jig.Write(cmd.jig_off_command, 0, cmd.jig_off_command.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine();
            }
            startTime = DateTime.Now;
            byte[] buffer = new byte[12];
            int start = 0;
            while (true)
            {

                if ((DateTime.Now - startTime).TotalMilliseconds > timeout)
                {
                    break;
                }
                int bytee = jig.BytesToRead;
                if (bytee > 0)
                {
                    jig.Write(buffer, start, Math.Min(bytee, 12 - start));
                    start += bytee;
                    if (start >= 12) break;
                }
            }
            return;

        }
      


        private double GetFloat(byte[] buffer, int index, double defaultValue = float.MinValue)
        {
            try
            {
                double value = BitConverter.ToSingle(buffer, index);
                if (!double.IsNaN(value)) return value;
            }
            catch (Exception ex)
            {
            }
            return defaultValue;
        }   
        private void CheckValues(Byte[] arr, int i)
        {

            int n = arr.Length;
            if (arr[0] != 02 && arr[n - 1] != 03 && arr.Length != 70)
            {

                meter_authenticate[i] = 0;
                log.Error("METER " + (i) + " FAILED IN TESTING BECAUSE OF INCORRECT PACKET WITH SIZE" + arr.Length + " STARTING BYTE " + arr[0]
                    );

                return;
            }
            if (five_ampere)
            {              
                stt--;
                meter_result_data[i, 0] = GetFloat(arr, 6);
                meter_result_data[i, 2] = GetFloat(arr, 10);
                meter_result_data[i, 1] = GetFloat(arr, 14);
                meter_result_data[i, 4] = GetFloat(arr, 50);
                meter_result_data[i, 3] = GetFloat(arr, 54);
                CoverOpen[i] = TemperCoverOpen.CoverOpen(arr[66] ,i);
                MagnetTemper[i] = TemperCoverOpen.Magnet(arr[66] ,i);
                log.Info("Voltage " +i +"th meter is " +GetFloat(arr, 6));
                log.Info("Current " +i+"th meter is " + GetFloat(arr, 10) + "   "+ GetFloat(arr,14));
                log.Info("Power factor "+i+ "th meter is " + GetFloat(arr, 50) + "   "+ GetFloat(arr,54));
                log.Info ("####################################################");             
            }
            else if (mili_ampere)
            {
                meter_result_data[i, 5] = GetFloat(arr, 6);
                meter_result_data[i, 7] = GetFloat(arr, 10);
                meter_result_data[i, 6] = GetFloat(arr, 14);
                meter_result_data[i, 9] = GetFloat(arr, 50);
                meter_result_data[i, 8] = GetFloat(arr, 54);
                CoverOpen[i+3] = TemperCoverOpen.CoverOpen(arr[66] ,i);
                MagnetTemper[i+3] = TemperCoverOpen.Magnet(arr[66] ,i);
                log.Info("Voltage " + i + "th meter is " + GetFloat(arr, 6));
                log.Info("Current " + i + "th meter is " + GetFloat(arr, 10) + "    " + GetFloat(arr,14));
                log.Info("Power factor " + i + "th meter is " + GetFloat(arr, 50) + "    " + GetFloat(arr,54));
                log.Info("####################################################");
            }         
        }
       
      
        private void AppendDataGridToTextFile<T>(DataGrid dataGrid, ObservableCollection<T> dataCollection, string filePath)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
                    {
                        writer.WriteLine($"--- Test Results ({DateTime.Now}) ---");
                        writer.WriteLine($"DataGrid: {dataGrid.Name}");


                        foreach (var column in dataGrid.Columns)
                        {
                            writer.Write(column.Header + "\t");
                        }
                        writer.WriteLine();


                        foreach (var item in dataCollection)
                        {
                            foreach (var column in dataGrid.Columns)
                            {
                                var binding = (column as DataGridBoundColumn)?.Binding as Binding;
                                if (binding != null)
                                {
                                    var propertyName = binding.Path.Path;
                                    var propertyValue = item.GetType().GetProperty(propertyName)?.GetValue(item);
                                    writer.Write(propertyValue + "\t");
                                }
                            }
                            writer.WriteLine();
                        }
                        writer.WriteLine();
                    }



                });
            }
            catch (Exception ex)
            {

            }
        }

    }

}




























