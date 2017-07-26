using System;
using System.Runtime.InteropServices;
using CH341.Native;

namespace CH341
{
    public class CH341A
    {
        private uint _Index;
        private bool _IsOpen = false;

        string MessageRequiredToOpenDevice = "It is required to open device using the method OpenDevice()";

        public enum IC_VER : uint
        {
            UNKNOWN = 0,
            CH341A = 0x20,
            CH341A3 = 0x30,
        }

        public enum EEPROM_TYPE : uint
        {
            ID_24C01 = 0,
            ID_24C02 = 1,
            ID_24C04 = 2,
            ID_24C08 = 3,
            ID_24C16 = 4,
            ID_24C32 = 5,
            ID_24C64 = 6,
            ID_24C128 = 7,
            ID_24C256 = 8,
            ID_24C512 = 9,
            ID_24C1024 = 10,
            ID_24C2048 = 11,
            ID_24C4096 = 12
        }

        public CH341A()
        { _Index = 0; }

        public CH341A(uint iIndex)
        { _Index = iIndex; }


        public bool OpenDevice()
        {
            IntPtr handler = CH341Native.CH341OpenDevice(_Index);
            if (((int)handler) == -1) { _IsOpen = false; }
            else { _IsOpen = true; }
            return _IsOpen;
        }

        public void CloseDevice()
        {
            CH341Native.CH341CloseDevice(_Index);
            _IsOpen = false;
        }

        public uint GetVersion()
        {
            return CH341Native.CH341GetVersion();
        }

        public uint GetDrvVersion()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }
            return CH341Native.CH341GetDrvVersion();
        }

        public bool ResetDevice()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }
            return CH341Native.CH341ResetDevice(_Index);
        }

        public bool GetDeviceDescr(out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)CH341Native.mMAX_BUFFER_LENGTH);

            uint ioLength = CH341Native.mMAX_BUFFER_LENGTH;

            bool opResult = CH341Native.CH341GetDeviceDescr(_Index, oBuffer, out ioLength);

            if (opResult == true)
            {
                obuf = new Byte[ioLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)ioLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        public bool GetConfigDescr(out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)CH341Native.mMAX_BUFFER_LENGTH);

            uint ioLength = CH341Native.mMAX_BUFFER_LENGTH;

            bool opResult = CH341Native.CH341GetConfigDescr(_Index, oBuffer, out ioLength);

            if (opResult == true)
            {
                obuf = new Byte[ioLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)ioLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        // bool	CH341SetIntRoutine(uint		iIndex,	mPCH341_INT_ROUTINE	iIntRoutine ){}

        public bool ReadInter(out uint iStatus)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341ReadInter(_Index, out iStatus);
        }

        public bool AbortInter()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341AbortInter(_Index);
        }

        public bool SetParaMode(uint iMode)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341SetParaMode(_Index, iMode);
        }

        public bool InitParallel(uint iMode)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341InitParallel(_Index, iMode);
        }

        public bool ReadData0(out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)CH341Native.mMAX_BUFFER_LENGTH);

            uint ioLength = CH341Native.mMAX_BUFFER_LENGTH;

            bool opResult = CH341Native.CH341ReadData0(_Index, oBuffer, out ioLength);

            if (opResult == true)
            {
                obuf = new Byte[ioLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)ioLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        public bool ReadData1(out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)CH341Native.mMAX_BUFFER_LENGTH);

            uint ioLength = CH341Native.mMAX_BUFFER_LENGTH;

            bool opResult = CH341Native.CH341ReadData1(_Index, oBuffer, out ioLength);

            if (opResult == true)
            {
                obuf = new Byte[ioLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)ioLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        public bool AbortRead()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341AbortRead(_Index);
        }

        public bool WriteData0(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341Native.CH341WriteData0(_Index, iBufferPtr, out ioLength);
        }

        public bool WriteData1(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341Native.CH341WriteData1(_Index, iBufferPtr, out ioLength);
        }

        public bool AbortWrite()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341AbortWrite(_Index);
        }

        public bool GetStatus(out uint iStatus)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341GetStatus(_Index, out iStatus);
        }

        public bool ReadI2C(byte iDevice, byte iAddr, out byte oByte)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341ReadI2C(_Index, iDevice, iAddr, out oByte);
        }

        public bool WriteI2C(byte iDevice, byte iAddr, byte iByte)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341WriteI2C(_Index, iDevice, iAddr, iByte);
        }

        public bool EppReadData(out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)CH341Native.mMAX_BUFFER_LENGTH);

            uint ioLength = CH341Native.mMAX_BUFFER_LENGTH;

            bool opResult = CH341Native.CH341EppReadData(_Index, oBuffer, out ioLength);

            if (opResult == true)
            {
                obuf = new Byte[ioLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)ioLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }


        public bool EppReadAddr(out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)CH341Native.mMAX_BUFFER_LENGTH);

            uint ioLength = CH341Native.mMAX_BUFFER_LENGTH;

            bool opResult = CH341Native.CH341EppReadAddr(_Index, oBuffer, out ioLength);

            if (opResult == true)
            {
                obuf = new Byte[ioLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)ioLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        public bool EppWriteData(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341Native.CH341EppWriteData(_Index, iBufferPtr, out ioLength);
        }

        public bool EppWriteAddr(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341Native.CH341EppWriteAddr(_Index, iBufferPtr, out ioLength);
        }

        public bool EppSetAddr(byte iAddr)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341EppSetAddr(_Index, iAddr);
        }

        public bool MemReadAddr0(out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)CH341Native.mMAX_BUFFER_LENGTH);

            uint ioLength = CH341Native.mMAX_BUFFER_LENGTH;

            bool opResult = CH341Native.CH341MemReadAddr0(_Index, oBuffer, out ioLength);

            if (opResult == true)
            {
                obuf = new Byte[ioLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)ioLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }


        public bool MemReadAddr1(out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)CH341Native.mMAX_BUFFER_LENGTH);

            uint ioLength = CH341Native.mMAX_BUFFER_LENGTH;

            bool opResult = CH341Native.CH341MemReadAddr1(_Index, oBuffer, out ioLength);

            if (opResult == true)
            {
                obuf = new Byte[ioLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)ioLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        public bool MemWriteAddr0(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341Native.CH341MemWriteAddr0(_Index, iBufferPtr, out ioLength);
        }

        public bool MemWriteAddr1(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341Native.CH341MemWriteAddr1(_Index, iBufferPtr, out ioLength);
        }

        public bool SetExclusive(uint iExclusive)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341SetExclusive(_Index, iExclusive);
        }

        public bool SetTimeout(uint iWriteTimeout, uint iReadTimeout)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341SetTimeout(_Index, iWriteTimeout, iReadTimeout);
        }

        public bool ReadData(out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)CH341Native.mMAX_BUFFER_LENGTH);

            uint ioLength = CH341Native.mMAX_BUFFER_LENGTH;

            bool opResult = CH341Native.CH341ReadData(_Index, oBuffer, out ioLength);

            if (opResult == true)
            {
                obuf = new Byte[ioLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)ioLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        public bool WriteData(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341Native.CH341WriteData(_Index, iBufferPtr, out ioLength);
        }


        public string GetDeviceName()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr devName = CH341Native.CH341GetDeviceName(_Index);

            return Marshal.PtrToStringAnsi(devName);
        }

        public IC_VER GetVerIC()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return (IC_VER)CH341Native.CH341GetVerIC(_Index);
        }

        public bool FlushBuffer()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341FlushBuffer(_Index);
        }

        public bool WriteRead(byte[] iWriteBuffer, uint iReadStep, uint iReadTimes, out byte[] oReadBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint iWriteLength = (uint)iWriteBuffer.Length;

            IntPtr iWriteBufferPtr = Marshal.AllocHGlobal((int)iWriteLength);

            Marshal.Copy(iWriteBuffer, 0, iWriteBufferPtr, (int)iWriteLength);

            // uint oReadLength = CH341Native.mMAX_BUFFER_LENGTH;
            uint oReadLength = CH341Native.mCH341_PACKET_LENGTH;

            IntPtr oReadBufferPtr = Marshal.AllocHGlobal((int)oReadLength);

            bool opResult = CH341Native.CH341WriteRead(_Index, iWriteLength, iWriteBufferPtr, iReadStep, iReadTimes, out oReadLength, oReadBufferPtr);

            if (opResult == true)
            {
                oReadBuffer = new Byte[oReadLength];
                Marshal.Copy(oReadBufferPtr, oReadBuffer, 0, (int)oReadLength);
            }
            else
            {
                oReadBuffer = null;
            }

            return opResult;
        }

        public bool SetStream(uint iMode)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341SetStream(_Index, iMode);
        }

        public bool SetDelaymS(uint iDelay)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341SetDelaymS(_Index, iDelay);
        }

        public bool StreamI2C(byte[] iWriteBuffer, uint iReadLength, out byte[] oReadBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint iWriteLength = (uint)iWriteBuffer.Length;

            IntPtr iWriteBufferPtr = Marshal.AllocHGlobal((int)iWriteLength);

            Marshal.Copy(iWriteBuffer, 0, iWriteBufferPtr, (int)iWriteLength);

            IntPtr oReadBufferPtr = Marshal.AllocHGlobal((int)iReadLength);

            bool opResult = CH341Native.CH341StreamI2C(_Index, iWriteLength, iWriteBufferPtr, iReadLength, oReadBufferPtr);

            if (opResult == true)
            {
                oReadBuffer = new Byte[iReadLength];
                Marshal.Copy(oReadBufferPtr, oReadBuffer, 0, (int)iReadLength);
            }
            else
            {
                oReadBuffer = null;
            }

            return opResult;
        }

        public bool ReadEEPROM(EEPROM_TYPE iEepromID, uint iAddr, uint iLength, out byte[] oBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBufferPtr = Marshal.AllocHGlobal((int)iLength);

            bool opResult = CH341Native.CH341ReadEEPROM(_Index, (uint)iEepromID, iAddr, iLength, oBufferPtr);

            if (opResult == true)
            {
                oBuffer = new Byte[iLength];
                Marshal.Copy(oBufferPtr, oBuffer, 0, (int)iLength);
            }
            else
            {
                oBuffer = null;
            }

            return opResult;
        }

        public bool WriteEEPROM(EEPROM_TYPE iEepromID, uint iAddr, uint iLength, byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBufferPtr = Marshal.AllocHGlobal((int)iLength);
            Marshal.Copy(iBuffer, 0, oBufferPtr, (int)iLength);

            return CH341Native.CH341WriteEEPROM(_Index, (uint)iEepromID, iAddr, iLength, oBufferPtr);
        }

        public bool GetInput(out uint iStatus)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341GetInput(_Index, out iStatus);
        }

        public bool SetOutput(uint iEnable, uint iSetDirOut, uint iSetDataOut)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341SetOutput(_Index, iEnable, iSetDirOut, iSetDataOut);
        }

        public bool Set_D5_D0(uint iSetDirOut, uint iSetDataOut)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341Set_D5_D0(_Index, iSetDirOut, iSetDataOut);
        }

        public bool StreamSPI3(uint iChipSelect, ref byte[] ioBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint iLength = (uint)ioBuffer.Length;

            IntPtr ioBufferPtr = Marshal.AllocHGlobal((int)iLength);

            Marshal.Copy(ioBuffer, 0, ioBufferPtr, (int)iLength);

            bool opResult = CH341Native.CH341StreamSPI3(_Index, iChipSelect, iLength, ioBufferPtr);

            if (opResult == true)
            {
                Marshal.Copy(ioBufferPtr, ioBuffer, 0, (int)iLength);
            }

            return opResult;
        }

        public bool StreamSPI4(uint iChipSelect, ref byte[] ioBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint iLength = (uint)ioBuffer.Length;

            IntPtr ioBufferPtr = Marshal.AllocHGlobal((int)iLength);

            Marshal.Copy(ioBuffer, 0, ioBufferPtr, (int)iLength);

            bool opResult = CH341Native.CH341StreamSPI4(_Index, iChipSelect, iLength, ioBufferPtr);

            if (opResult == true)
            {
                Marshal.Copy(ioBufferPtr, ioBuffer, 0, (int)iLength);
            }

            return opResult;
        }

        public bool StreamSPI5(uint iChipSelect, uint iLength, ref byte[] ioBuffer, ref byte[] ioBuffer2)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }
            if (iLength > ioBuffer.Length) { throw new System.ArgumentOutOfRangeException("iLength < ioBuffer.Length"); }
            if (iLength > ioBuffer2.Length) { throw new System.ArgumentOutOfRangeException("iLength < ioBuffer2.Length"); }

            IntPtr ioBufferPtr = Marshal.AllocHGlobal((int)iLength);
            Marshal.Copy(ioBuffer, 0, ioBufferPtr, (int)iLength);

            IntPtr ioBuffer2Ptr = Marshal.AllocHGlobal((int)iLength);
            Marshal.Copy(ioBuffer2, 0, ioBuffer2Ptr, (int)iLength);

            bool opResult = CH341Native.CH341StreamSPI5(_Index, iChipSelect, iLength, ioBufferPtr, ioBuffer2Ptr);

            if (opResult == true)
            {
                Marshal.Copy(ioBufferPtr, ioBuffer, 0, (int)iLength);
                Marshal.Copy(ioBuffer2Ptr, ioBuffer2, 0, (int)iLength);
            }

            return opResult;
        }

        public bool BitStreamSPI(uint iLength, ref byte[] ioBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr ioBufferPtr = Marshal.AllocHGlobal(ioBuffer.Length);

            Marshal.Copy(ioBuffer, 0, ioBufferPtr, ioBuffer.Length);

            bool opResult = CH341Native.CH341BitStreamSPI(_Index, iLength, ioBufferPtr);

            if (opResult == true)
            {
                Marshal.Copy(ioBufferPtr, ioBuffer, 0, ioBuffer.Length);
            }

            return opResult;
        }

        public bool SetBufUpload(uint iEnableOrClear)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341SetBufUpload(_Index, iEnableOrClear);
        }

        public int QueryBufUpload()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341QueryBufUpload(_Index);
        }

        public bool SetBufDownload(uint iEnableOrClear)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341SetBufDownload(_Index, iEnableOrClear);
        }

        public int QueryBufDownload()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341QueryBufDownload(_Index);
        }

        public bool ResetInter()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341ResetInter(_Index);
        }

        public bool ResetRead()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341ResetRead(_Index);
        }

        public bool ResetWrite()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Native.CH341ResetWrite(_Index);
        }


        public bool IIC_IssueStart()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            byte[] mBuffer = new byte[] { CH341Native.mCH341A_CMD_I2C_STREAM, CH341Native.mCH341A_CMD_I2C_STM_STA, CH341Native.mCH341A_CMD_I2C_STM_END };

            return (WriteData(mBuffer));
        }

        public bool IIC_IssueStop()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            byte[] mBuffer = new byte[] { CH341Native.mCH341A_CMD_I2C_STREAM, CH341Native.mCH341A_CMD_I2C_STM_STO, CH341Native.mCH341A_CMD_I2C_STM_END };

            return (WriteData(mBuffer));
        }

        public bool IIC_OutBlockSkipAck(byte[] iOutBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            byte[] mBuffer = new byte[1 + 1 + iOutBuffer.Length + 1];

            mBuffer[0] = CH341Native.mCH341A_CMD_I2C_STREAM;
            mBuffer[1] = (byte)(CH341Native.mCH341A_CMD_I2C_STM_OUT | iOutBuffer.Length);
            Array.Copy(iOutBuffer, 0, mBuffer, 2, iOutBuffer.Length);
            mBuffer[iOutBuffer.Length + 2] = CH341Native.mCH341A_CMD_I2C_STREAM;

            return (WriteData(mBuffer));
        }

        public bool IIC_OutByteCheckAck(byte iOutByte)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            byte[] mBuffer = new byte[] { CH341Native.mCH341A_CMD_I2C_STREAM, CH341Native.mCH341A_CMD_I2C_STM_OUT, iOutByte, CH341Native.mCH341A_CMD_I2C_STM_END };

            byte[] mReadBuf;

            if (WriteRead(mBuffer, CH341Native.mCH341A_CMD_I2C_STM_MAX, 1, out mReadBuf) == true)
            {
                if (mReadBuf.Length > 0 && (mReadBuf[mReadBuf.Length - 1] & 0x80) == 0) { return true; }
            }

            return false;
        }

        public bool IIC_InBlockByAck(uint iInLength, out byte[] iOutBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            if (iInLength == 0) { throw new System.ArgumentOutOfRangeException("iInLength == 0"); }

            if (iInLength > CH341Native.mCH341A_CMD_I2C_STM_MAX) { throw new System.ArgumentOutOfRangeException("iInLength > CH341Native.mCH341A_CMD_I2C_STM_MAX"); }

            byte[] mBuffer = new byte[] { CH341Native.mCH341A_CMD_I2C_STREAM, (byte)(CH341Native.mCH341A_CMD_I2C_STM_IN | iInLength), CH341Native.mCH341A_CMD_I2C_STM_END };

            if (WriteRead(mBuffer, CH341Native.mCH341A_CMD_I2C_STM_MAX, 1, out iOutBuffer) == true)
            {
                if (iOutBuffer.Length == iInLength)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IIC_InByteNoAck(out byte oInByte)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            byte[] mBuffer = new byte[] { CH341Native.mCH341A_CMD_I2C_STREAM, CH341Native.mCH341A_CMD_I2C_STM_IN, CH341Native.mCH341A_CMD_I2C_STM_END };

            byte[] mReadBuf;

            if (WriteRead(mBuffer, CH341Native.mCH341A_CMD_I2C_STM_MAX, 1, out mReadBuf) == true)
            {
                if (mReadBuf.Length > 0)
                {
                    oInByte = mReadBuf[mReadBuf.Length - 1];
                    return true;
                }
            }

            oInByte = 0;
            return false;
        }
    }
}
