using System;
using System.Runtime.InteropServices;

namespace CH341
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 40)]
    public struct ControlTransferCommand
    {
        [FieldOffset(0)]
        public UInt32 mFunction;

        [FieldOffset(4)]
        public UInt32 mLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        [FieldOffset(8)]
        public Byte[] mData;

        public UInt32 mStatus { get { return mFunction; } }

        public ControlTransferCommand(ushort function, SetupPacket pkt)
        {
            mFunction = function;
            mLength = 32;
            mData = new byte[32];
            Array.Copy(pkt.GetBytes(), mData, 8);
        }

        public ControlTransferCommand(ushort function, byte[] databuf)
        {
            mFunction = function;
            mLength = 32;
            mData = new byte[32];

            if (databuf.Length > 32) { throw new ArgumentException("Maximum buffer lenght is 32 byte"); }
            Array.Copy(databuf, mData, databuf.Length);
        }

        public override string ToString()
        {
            string str = "mFunction: " + mFunction.ToString("X8") + Environment.NewLine;
            str += "mLength: " + mLength.ToString("X8") + Environment.NewLine;

            str += "mData: ";
            for (int i = 0; i < mLength; i++) { str += mData[i].ToString("X2") + " "; }

            return str;
        }

        public byte[] GetBytes()
        {
            byte[] buf = new byte[mLength];
            Array.Copy(mData, buf, (int)mLength);
            return buf;
        }
    }

    public struct SetupPacket
    {
        // Usb Setup Packet
        public byte mUspReqType;
        public byte mUspRequest;
        public UInt16 mUspValue;
        public UInt16 mUspIndex;
        public UInt16 mDataLength;

        public SetupPacket(byte requestType, byte request, ushort value, ushort index, ushort dataLenght)
        {
            mUspReqType = requestType;
            mUspRequest = request;
            mUspValue = value;
            mUspIndex = index;
            mDataLength = dataLenght;
        }

        public SetupPacket(byte[] databuf)
        {
            if (databuf.Length < 8) { throw new ArgumentException("Minimum data lenght for SetupPacket is 8 byte"); }

            mUspReqType = databuf[0];
            mUspRequest = databuf[1];
            mUspValue = BitConverter.ToUInt16(new byte[] { databuf[3], databuf[2] }, 0);
            mUspIndex = BitConverter.ToUInt16(new byte[] { databuf[5], databuf[4] }, 0);
            mDataLength = BitConverter.ToUInt16(new byte[] { databuf[7], databuf[6] }, 0);
        }

        public override string ToString()
        {
            string str = "mUspReqType: " + mUspReqType.ToString("X2") + Environment.NewLine;
            str += "mUspRequest: " + mUspRequest.ToString("X2") + Environment.NewLine;
            str += "mUspValue: " + mUspValue.ToString("X4") + Environment.NewLine;
            str += "mUspIndex: " + mUspIndex.ToString("X4") + Environment.NewLine;
            str += "mDataLength: " + mDataLength.ToString("X4") + Environment.NewLine;

            return str;
        }

        public byte[] GetBytes()
        {
            byte[] buf = new byte[8];

            buf[0] = mUspReqType;
            buf[1] = mUspRequest;
            buf[2] = BitConverter.GetBytes(mUspValue)[0];
            buf[3] = BitConverter.GetBytes(mUspValue)[1];
            buf[4] = BitConverter.GetBytes(mUspIndex)[0];
            buf[5] = BitConverter.GetBytes(mUspIndex)[1];
            buf[6] = BitConverter.GetBytes(mDataLength)[0];
            buf[7] = BitConverter.GetBytes(mDataLength)[1];

            return buf;
        }

    }
}
