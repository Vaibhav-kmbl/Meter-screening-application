using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace Meter_screening_application
{
    internal class ExtraCode
    {
        //  await Task.WhenAll(tasks);
        //Parallel.ForEach(Values , i => {
        //    log.Info($"Test for 5A started for {i} meter");
        //    RunTask(i);

        //});
        //List<Task> ScreeningTask = new List<Task>();
        //ScreeningTask.Add(Task.Run(() =>

        //    master_5A.Run()
        //));
        //ScreeningTask.Add(Task.Run(() =>

        //     firstmeter_5A.Run()
        //));
        //ScreeningTask.Add(Task.Run(() =>

        //   secondmeter_5A.Run()
        //));
        //Task.WhenAll(ScreeningTask);

        //CurrentVoltageCommand currentVoltageCommandmaster = (CurrentVoltageCommand)master_5A._commands[1];

        //meter_result_data[0, 0] = currentVoltageCommandmaster.current;
        //meter_result_data[0, 1] = currentVoltageCommandmaster.voltage;
        //meter_result_data[0, 2] = currentVoltageCommandmaster.pf;
        //CurrentVoltageCommand currentVoltageCommandm1 = (CurrentVoltageCommand)firstmeter_5A._commands[1];

        //meter_result_data[1, 0] = currentVoltageCommandm1.current;
        //meter_result_data[1, 1] = currentVoltageCommandm1.voltage;
        //meter_result_data[1, 2] = currentVoltageCommandm1.pf;
        //CurrentVoltageCommand currentVoltageCommandm2 = (CurrentVoltageCommand)secondmeter_5A._commands[1];

        //meter_result_data[2, 0] = currentVoltageCommandm2.current;
        //meter_result_data[2, 1] = currentVoltageCommandm2.voltage;
        //meter_result_data[2, 2] = currentVoltageCommandm2.pf;
        //private void MeterAuthenticate()
        //{
        //    int i = -1;
        //    foreach (var ports in serial)
        //    {

        //        try
        //        {
        //            Console.WriteLine("meter started auth " + (i + 1));
        //            i++;

        //            if (ports.IsOpen && meter_authenticate[i] == 1)
        //            {


        //                Byte[] meter_auth_response = new Byte[9];
        //                try
        //                {
        //                    ports.Write(cmd.meter_auth_code, 0, cmd.meter_auth_code.Length);



        //                    Console.WriteLine("software sent the command to meter " + i);
        //                    break;
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine("retrying output......");
        //                }


        //                startTime = DateTime.Now;

        //                while (true)
        //                {

        //                    if ((DateTime.Now - startTime).TotalMilliseconds > timeout)
        //                    {

        //                        Console.WriteLine("Timeout reached. Meter not giving any auth response");
        //                        break;
        //                    }
        //                    int bytee = ports.BytesToRead;
        //                    if (bytee == 9)
        //                    {
        //                        ports.Read(meter_auth_response, 0, bytee);
        //                        PrintResponse(meter_auth_response);


        //                        if (cmd.ResponseMatchString(meter_auth_response, cmd.meter_auth_resp_string))
        //                        {
        //                            PrintResponse(meter_auth_response);
        //                            Console.WriteLine();
        //                            meter_authenticate[i] = 1;
        //                            Console.WriteLine("Response from meter " + (i) + " authenticated");
        //                            Console.WriteLine();
        //                        }
        //                        break;

        //                    }
        //                }



        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("response from meter is incorrect " + i);

        //        }
        //    }

        //     }
        //private void MeterTestResult()
        //{
        //    int i = -1;
        //    const int maxRetries = 2;

        //    foreach (var ports in serial)
        //    {
        //        bool flag = false;
        //        i++;
        //        int retryCount = 0;
        //        SingleMeterAuthenticate(i);
        //        Byte[] meter_test_response = new Byte[70];
        //        if (meter_authenticate[i] == 1)
        //        {
        //            while (retryCount <= maxRetries)
        //            {



        //                meter_test_response = new Byte[70];
        //                ports.DiscardInBuffer();
        //                ports.DiscardOutBuffer();

        //                if (ports.IsOpen && meter_authenticate[i] == 1)
        //                {
        //                    try
        //                    {
        //                        ports.Write(cmd.meter_test_command, 0, 75);
        //                        //    Thread.Sleep(100);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine("Trying to write....");
        //                    }

        //                    Console.WriteLine($"Testing from meter {i} started, Attempt {retryCount + 1}");

        //                    startTime = DateTime.Now;
        //                    int start = 0;
        //                    int time = timeout;

        //                    while (true)
        //                    {
        //                        int bytee = ports.BytesToRead;

        //                        if ((DateTime.Now - startTime).TotalMilliseconds > time)
        //                        {
        //                            Console.WriteLine("Timeout occurred. Retrying...");
        //                            retryCount++;
        //                            break;
        //                        }

        //                        if (bytee > 0)
        //                        {
        //                            ports.Read(meter_test_response, start, bytee);
        //                            start += bytee;

        //                            if (start == 70)
        //                            {

        //                                flag = true;
        //                                PrintResponse(meter_test_response);
        //                                Console.WriteLine();
        //                                break;
        //                            }
        //                        }
        //                    }

        //                }
        //                else break;
        //                if (flag) break;
        //            }
        //            if (meter_authenticate[i] == 1 && flag)
        //            {
        //                Console.WriteLine($"Meter fetched some results {i}");
        //                CheckValues(meter_test_response, i);
        //                //break;
        //            }
        //            else if (!flag)
        //            {
        //                Console.WriteLine($"Meter {i} failed after {maxRetries} retries.");
        //            }
        //        }
        //    }
        //}
      
    }
}
