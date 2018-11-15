using System;
using System.Runtime.InteropServices;

namespace CH341
{
    public partial class CH341A
    {
        const string DllName = "CH341DLL.DLL";

        [DllImport(DllName)]
        private static extern IntPtr CH341OpenDevice(uint iIndex);

        [DllImport(DllName)]
        private static extern void CH341CloseDevice(uint iIndex);

        [DllImport(DllName)]
        private static extern uint CH341GetVersion();

        [DllImport(DllName)]
        private static extern uint CH341DriverCommand(uint iIndex, IntPtr ioCommand);

        [DllImport(DllName)]
        private static extern uint CH341GetDrvVersion();

        [DllImport(DllName)]
        private static extern bool CH341ResetDevice(uint iIndex);

        [DllImport(DllName)]
        private static extern bool CH341GetDeviceDescr(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341GetConfigDescr(uint iIndex, IntPtr oBuffer, out uint ioLength);

        /*
        private static extern bool	CH341SetIntRoutine(
        uint		iIndex,
        mPCH341_INT_ROUTINE	iIntRoutine );
         */

        [DllImport(DllName)]
        private static extern bool CH341ReadInter(uint iIndex, out uint iStatus);

        [DllImport(DllName)]
        private static extern bool CH341AbortInter(uint iIndex);

        [DllImport(DllName)]
        private static extern bool CH341SetParaMode(uint iIndex, uint iMode);

        [DllImport(DllName)]
        private static extern bool CH341InitParallel(uint iIndex, uint iMode);

        [DllImport(DllName)]
        private static extern bool CH341ReadData0(uint iIndex, IntPtr oBuffer, ref uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341ReadData1(uint iIndex, IntPtr oBuffer, ref uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341AbortRead(uint iIndex);

        [DllImport(DllName)]
        private static extern bool CH341WriteData0(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341WriteData1(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341AbortWrite(uint iIndex);

        [DllImport(DllName)]
        private static extern bool CH341GetStatus(uint iIndex, out uint iStatus);

        [DllImport(DllName)]
        private static extern bool CH341ReadI2C(uint iIndex, byte iDevice, byte iAddr, out byte oByte);

        [DllImport(DllName)]
        private static extern bool CH341WriteI2C(uint iIndex, byte iDevice, byte iAddr, byte iByte);

        [DllImport(DllName)]
        private static extern bool CH341EppReadData(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341EppReadAddr(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341EppWriteData(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341EppWriteAddr(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341EppSetAddr(uint iIndex, byte iAddr);

        [DllImport(DllName)]
        private static extern bool CH341MemReadAddr0(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341MemReadAddr1(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341MemWriteAddr0(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341MemWriteAddr1(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341SetExclusive(uint iIndex, uint iExclusive);

        [DllImport(DllName)]
        private static extern bool CH341SetTimeout(uint iIndex, uint iWriteTimeout, uint iReadTimeout);

        [DllImport(DllName)]
        private static extern bool CH341ReadData(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern bool CH341WriteData(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        private static extern IntPtr CH341GetDeviceName(uint iIndex);

        [DllImport(DllName)]
        private static extern uint CH341GetVerIC(uint iIndex);

        [DllImport(DllName)]
        private static extern bool CH341FlushBuffer(uint iIndex);

        [DllImport(DllName)]
        private static extern bool CH341WriteRead(uint iIndex,
                                                 uint iWriteLength,
                                                 IntPtr iWriteBuffer,
                                                 uint iReadStep,
                                                 uint iReadTimes,
                                                 out uint oReadLength,
                                                 IntPtr oReadBuffer);

        [DllImport(DllName)]
        private static extern bool CH341SetStream(uint iIndex, uint iMode);

        [DllImport(DllName)]
        private static extern bool CH341SetDelaymS(uint iIndex, uint iDelay);

        [DllImport(DllName)]
        private static extern bool CH341StreamI2C(uint iIndex,
                                                 uint iWriteLength,
                                                 IntPtr iWriteBuffer,
                                                 uint iReadLength,
                                                 IntPtr oReadBuffer);


        [DllImport(DllName)]
        private static extern bool CH341ReadEEPROM(uint iIndex,
                                                  uint iEepromID,
                                                  uint iAddr,
                                                  uint iLength,
                                                  IntPtr oBuffer);

        [DllImport(DllName)]
        private static extern bool CH341WriteEEPROM(uint iIndex,
                                                   uint iEepromID,
                                                   uint iAddr,
                                                   uint iLength,
                                                   IntPtr iBuffer);


        [DllImport(DllName)]
        private static extern bool CH341GetInput(uint iIndex, out uint iStatus);

        [DllImport(DllName)]
        private static extern bool CH341SetOutput(uint iIndex,
                                                 uint iEnable,
                                                 uint iSetDirOut,
                                                 uint iSetDataOut);

        [DllImport(DllName)]
        private static extern bool CH341Set_D5_D0(uint iIndex,
                                                 uint iSetDirOut,
                                                 uint iSetDataOut);

        [DllImport(DllName)]
        private static extern bool CH341StreamSPI3(uint iIndex,
                                                  uint iChipSelect,
                                                  uint iLength,
                                                  IntPtr ioBuffer);

        [DllImport(DllName)]
        private static extern bool CH341StreamSPI4(uint iIndex,
                                                  uint iChipSelect,
                                                  uint iLength,
                                                  IntPtr ioBuffer);

        [DllImport(DllName)]
        private static extern bool CH341StreamSPI5(uint iIndex,
                                                  uint iChipSelect,
                                                  uint iLength,
                                                  IntPtr ioBuffer,
                                                  IntPtr ioBuffer2);

        [DllImport(DllName)]
        private static extern bool CH341BitStreamSPI(uint iIndex, uint iLength, IntPtr ioBuffer);

        [DllImport(DllName)]
        private static extern bool CH341SetBufUpload(uint iIndex, uint iEnableOrClear);

        [DllImport(DllName)]
        private static extern int CH341QueryBufUpload(uint iIndex);

        [DllImport(DllName)]
        private static extern bool CH341SetBufDownload(uint iIndex, uint iEnableOrClear);

        [DllImport(DllName)]
        private static extern int CH341QueryBufDownload(uint iIndex);

        [DllImport(DllName)]
        private static extern bool CH341ResetInter(uint iIndex);

        [DllImport(DllName)]
        private static extern bool CH341ResetRead(uint iIndex);

        [DllImport(DllName)]
        private static extern bool CH341ResetWrite(uint iIndex);

        [DllImport(DllName)]
        private static extern bool CH341SetupSerial(uint iIndex, uint iParityMode, uint iBaudRate);
    }
}

