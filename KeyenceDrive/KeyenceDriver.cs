using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using DataService;

namespace KeyenceDrive
{ 
    public class KeyenceDriver : IPLCDriver
    {
        public KeyenceDriver(IDataServer server, short id, string name)
        {
            _id = id;
            _server = server;
            Name = name;
        }
        public int PDU => throw new NotImplementedException();
        short _id;
        public short ID { get { return _id; } }



        public string Name { get; set; }

        private Socket tcpSynCl;
        private byte[] tcpSynClBuffer = new byte[0xFF];
        public int Port { get; set; }
        public IPAddress IP { get; set; }

        public bool IsClosed
        {
            get { return tcpSynCl.Connected == false || tcpSynCl == null; }
        }

        private int _timeout;
        public int TimeOut { get { return _timeout; } set { _timeout = value; } }

        List<IGroup> _grps = new List<IGroup>(20);
        public IEnumerable<IGroup> Groups { get { return _grps; } }

        private IDataServer _server;
        public IDataServer Parent { get { return _server; } }

        public event IOErrorEventHandler OnError;

        public IGroup AddGroup(string name, short id, int updateRate, float deadBand = 0, bool active = false)
        {
            throw new NotImplementedException();
        }

        public bool Connect()
        {
            try
            {
                if (tcpSynCl != null)
                    tcpSynCl.Close();
                //IPAddress ip = IPAddress.Parse(_ip);
                // ----------------------------------------------------------------
                // Connect synchronous client
                if (_timeout <= 0) _timeout = 1000;
                tcpSynCl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tcpSynCl.SendTimeout = _timeout;
                tcpSynCl.ReceiveTimeout = _timeout;
                tcpSynCl.NoDelay = true;
                tcpSynCl.Connect(this.IP, this.Port);
                return true;
            }
            catch (SocketException error)
            {
                if (OnError != null)
                    OnError(this, new IOErrorEventArgs(error.Message));
                return false;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string GetAddress(DeviceAddress address)
        {
            throw new NotImplementedException();
        }

        public DeviceAddress GetDeviceAddress(string address)
        {
            DeviceAddress dv = DeviceAddress.Empty;
            if (address == null) return dv;
            int firstNumberIndex = FindFirstNum(address);
            string[] strs = new string[3];
            if (firstNumberIndex > 0 && firstNumberIndex<address.Length)
            {
                strs[0] = address.Substring(0, firstNumberIndex);
                string s= address.Substring(firstNumberIndex + 1, address.Length);
                string[] ss= s.Split('.');
                strs[1] = ss[0];
                strs[2] = ss[1];
            }
            else
            {
                return dv;
            }
            switch (strs[0].ToUpper())
            {
                case "DM":
                    dv.Area = 1;
                    break;
                case "EM":
                    dv.Area = 2;
                    break;
                case "R":
                    dv.Area = 3;
                    break;
                case "MR":
                    dv.Area = 4;
                    break;
                case "T":
                    dv.Area = 5;
                    break;
            }

        }

        public ItemData<bool> ReadBit(DeviceAddress address)
        {
            throw new NotImplementedException();
        }

        public ItemData<byte> ReadByte(DeviceAddress address)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadBytes(DeviceAddress address, ushort size)
        {
            throw new NotImplementedException();
        }

        public ItemData<float> ReadFloat(DeviceAddress address)
        {
            throw new NotImplementedException();
        }

        public ItemData<short> ReadInt16(DeviceAddress address)
        {
            throw new NotImplementedException();
        }

        public ItemData<int> ReadInt32(DeviceAddress address)
        {
            throw new NotImplementedException();
        }

        public ItemData<string> ReadString(DeviceAddress address, ushort size)
        {
            throw new NotImplementedException();
        }

        public ItemData<ushort> ReadUInt16(DeviceAddress address)
        {
            throw new NotImplementedException();
        }

        public ItemData<uint> ReadUInt32(DeviceAddress address)
        {
            throw new NotImplementedException();
        }

        public ItemData<object> ReadValue(DeviceAddress address)
        {
            throw new NotImplementedException();
        }

        public bool RemoveGroup(IGroup group)
        {
            throw new NotImplementedException();
        }

        public int WriteBit(DeviceAddress address, bool bit)
        {
            throw new NotImplementedException();
        }

        public int WriteBits(DeviceAddress address, byte bits)
        {
            throw new NotImplementedException();
        }

        public int WriteBytes(DeviceAddress address, byte[] bit)
        {
            throw new NotImplementedException();
        }

        public int WriteFloat(DeviceAddress address, float value)
        {
            throw new NotImplementedException();
        }

        public int WriteInt16(DeviceAddress address, short value)
        {
            throw new NotImplementedException();
        }

        public int WriteInt32(DeviceAddress address, int value)
        {
            throw new NotImplementedException();
        }

        public int WriteString(DeviceAddress address, string str)
        {
            throw new NotImplementedException();
        }

        public int WriteUInt16(DeviceAddress address, ushort value)
        {
            throw new NotImplementedException();
        }

        public int WriteUInt32(DeviceAddress address, uint value)
        {
            throw new NotImplementedException();
        }

        public int WriteValue(DeviceAddress address, object value)
        {
            throw new NotImplementedException();
        }
    }
}
