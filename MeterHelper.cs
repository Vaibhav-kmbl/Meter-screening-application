//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ZigLibrary.Commands;
//using ZigLibrary.Helper;
//using ZigLibrary.Helper.V2;

//namespace Meter_screening_application
//{
   
//    public class MeterHelper
//    {
//        MeterSerialPortHelperV1 _meterSerialPortHelperV1;
//        public byte[] responsearray = new byte[70];
//        public List<BaseCommand> _commands;
//        public MeterHelper(string portname)
//        {
            
//            List<BaseCommand> command = new List<BaseCommand>();
//            command.Add(new ZigLibrary.Commands.AuthCommand(4000, 3));
//            command.Add(new ZigLibrary.Commands.CurrentVoltageCommand(4000, 3));
//            _meterSerialPortHelperV1 = new MeterSerialPortHelperV1(portname, command);
//            _commands = command;
//        }

//        public async void Run()
//        {
//            CancellationToken cancellationToken = CancellationToken.None;
//           await _meterSerialPortHelperV1.WaitForResponse(10000, cancellationToken);
//        }
        
        
//    }
//}
