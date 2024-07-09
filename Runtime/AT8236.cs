using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using Codice.Client.Common;
using PsychoUnity.Manager;

namespace Pump
{
    public class AT8236
    {
        private const string DeviceName = "PumpAT8236";
        private const int DataBits = 8;
        private const StopBits StopBit = StopBits.None;
        private const Parity ParityBits = Parity.None;
        private const Handshake FlowCtrl = Handshake.None;

        private readonly byte[] _header = { 0xAB, 0xCD };

        private readonly byte[] _cmdStart = { 0x00 };
        private readonly byte[] _cmdStop = { 0x01 };
        private readonly byte[] _cmdReverse = { 0x02 };
        private readonly byte[] _cmdSetSpeed = { 0x03 };
        private readonly byte[] _cmdGetSpeed = { 0x04 };

        public AT8236(string portName, int baudRate)
        {
            var config = new SerialComManager.SerialPortConfig(DeviceName, portName, baudRate, DataBits, StopBit,
                ParityBits, FlowCtrl);
            SerialComManager.Instance.AddSerialCom(DeviceName);
            SerialComManager.Instance.SetSerialCom(config);
            SerialComManager.Instance.EnableDtr(DeviceName, true);
            SerialComManager.Instance.Open(DeviceName);
        }

        private byte[] BuildMsg(byte[] cmd, byte[] data = null)
        {
            if (data != null)
            {
                var rv = _header.Concat(cmd).Concat(data);
                return rv.ToArray();
            }
            else
            {
                var rv = _header.Concat(cmd);
                return rv.ToArray();
            }
        }

        public void GiveReward()
        {
            var msg = BuildMsg(_cmdStart);
            SerialComManager.Instance.Write(DeviceName, ref msg, msg.Length);
        }

        public void GiveRewardToMilliSeconds(int duration)
        {
            var data = BitConverter.GetBytes(duration);
            var msg = BuildMsg(_cmdStart, data);
            SerialComManager.Instance.Write(DeviceName, ref msg, msg.Length);
        }

        public void StopReward()
        {
            var msg = BuildMsg(_cmdStop);
            SerialComManager.Instance.Write(DeviceName, ref msg, msg.Length);
        }

        public void Reverse()
        {
            var msg = BuildMsg(_cmdReverse);
            SerialComManager.Instance.Write(DeviceName, ref msg, msg.Length);
        }

        public void SetDirection()
        {
        }

        public void SetSpeed(int speed)
        {
            var data = BitConverter.GetBytes(speed);
            var msg = BuildMsg(_cmdSetSpeed, data);
            SerialComManager.Instance.Write(DeviceName, ref msg, msg.Length);
        }

        public float GetSpeed()
        {
            var msg = BuildMsg(_cmdGetSpeed);
            SerialComManager.Instance.Write(DeviceName, ref msg, msg.Length);

            var buf = new byte[4];
            SerialComManager.Instance.Read(DeviceName, ref buf, buf.Length);

            return BitConverter.ToSingle(buf);
        }
    }
}