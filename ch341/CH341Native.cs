using System;
using System.Runtime.InteropServices;

namespace CH341.Native
{
    public static class CH341Native
    {
        const string DllName = "CH341DLL.DLL";

        public const uint mCH341_PACKET_LENGTH = 32;
        public const uint mCH341_PKT_LEN_SHORT = 8;

        // const uint		IOCTL_CH341_COMMAND		( FILE_DEVICE_UNKNOWN << 16 | FILE_ANY_ACCESS << 14 | 0x0f34 << 2 | METHOD_BUFFERED )
        // public static uint IOCTL_CH341_COMMAND	
        // {
        //    get { return (FILE_DEVICE_UNKNOWN << 16 | FILE_ANY_ACCESS << 14 | 0x0f34 << 2 | METHOD_BUFFERED); }
        // }

        // const uint			mWIN32_COMMAND_HEAD		mOFFSET( mWIN32_COMMAND, mBuffer )

        public const uint mCH341_MAX_NUMBER = 16;

        public const uint mMAX_BUFFER_LENGTH = 0x1000;

        // const uint			mMAX_COMMAND_LENGTH		( mWIN32_COMMAND_HEAD + mMAX_BUFFER_LENGTH )

        // const uint			mDEFAULT_BUFFER_LEN		0x0400

        // const uint			mDEFAULT_COMMAND_LEN	( mWIN32_COMMAND_HEAD + mDEFAULT_BUFFER_LEN )

        public const uint mCH341_ENDP_INTER_UP = 0x81;
        public const uint mCH341_ENDP_INTER_DOWN = 0x01;
        public const uint mCH341_ENDP_DATA_UP = 0x82;
        public const uint mCH341_ENDP_DATA_DOWN = 0x02;

        public const uint mPipeDeviceCtrl = 0x00000004;
        public const uint mPipeInterUp = 0x00000005;
        public const uint mPipeDataUp = 0x00000006;
        public const uint mPipeDataDown = 0x00000007;


        public const uint mFuncNoOperation = 0x00000000;
        public const uint mFuncGetVersion = 0x00000001;
        public const uint mFuncGetConfig = 0x00000002;
        public const uint mFuncSetTimeout = 0x00000009;
        public const uint mFuncSetExclusive = 0x0000000b;
        public const uint mFuncResetDevice = 0x0000000c;
        public const uint mFuncResetPipe = 0x0000000d;
        public const uint mFuncAbortPipe = 0x0000000e;

        public const uint mFuncSetParaMode = 0x0000000f;
        public const uint mFuncReadData0 = 0x00000010;
        public const uint mFuncReadData1 = 0x00000011;
        public const uint mFuncWriteData0 = 0x00000012;
        public const uint mFuncWriteData1 = 0x00000013;
        public const uint mFuncWriteRead = 0x00000014;
        public const uint mFuncBufferMode = 0x00000020;
        public const uint mFuncBufferModeDn = 0x00000021;

        public const uint mUSB_CLR_FEATURE = 0x01;
        public const uint mUSB_SET_FEATURE = 0x03;
        public const uint mUSB_GET_STATUS = 0x00;
        public const uint mUSB_SET_ADDRESS = 0x05;
        public const uint mUSB_GET_DESCR = 0x06;
        public const uint mUSB_SET_DESCR = 0x07;
        public const uint mUSB_GET_CONFIG = 0x08;
        public const uint mUSB_SET_CONFIG = 0x09;
        public const uint mUSB_GET_INTERF = 0x0a;
        public const uint mUSB_SET_INTERF = 0x0b;
        public const uint mUSB_SYNC_FRAME = 0x0c;

        public const uint mCH341_VENDOR_READ = 0xC0;
        public const uint mCH341_VENDOR_WRITE = 0x40;

        public const byte mCH341_PARA_INIT = 0xB1;
        public const byte mCH341_I2C_STATUS = 0x52;
        public const byte mCH341_I2C_COMMAND = 0x53;

        public const byte mCH341_PARA_CMD_R0 = 0xAC;
        public const byte mCH341_PARA_CMD_R1 = 0xAD;
        public const byte mCH341_PARA_CMD_W0 = 0xA6;
        public const byte mCH341_PARA_CMD_W1 = 0xA7;
        public const byte mCH341_PARA_CMD_STS = 0xA0;

        public const byte mCH341A_CMD_SET_OUTPUT = 0xA1;
        public const byte mCH341A_CMD_IO_ADDR = 0xA2;
        public const byte mCH341A_CMD_PRINT_OUT = 0xA3;
        public const byte mCH341A_CMD_PWM_OUT = 0xA4;
        public const byte mCH341A_CMD_SHORT_PKT = 0xA5;
        public const byte mCH341A_CMD_SPI_STREAM = 0xA8;
        // #define		mCH341A_CMD_SIO_STREAM	0xA9;
        public const byte mCH341A_CMD_I2C_STREAM = 0xAA;
        public const byte mCH341A_CMD_UIO_STREAM = 0xAB;
        public const byte mCH341A_CMD_PIO_STREAM = 0xAE;

        public const byte mCH341A_BUF_CLEAR = 0xB2;
        public const byte mCH341A_I2C_CMD_X = 0x54;
        public const byte mCH341A_DELAY_MS = 0x5E;
        public const byte mCH341A_GET_VER = 0x5F;

        // const uint		mCH341_EPP_IO_MAX		( mCH341_PACKET_LENGTH - 1 )
        public static uint mCH341_EPP_IO_MAX
        {
            get { return mCH341_PACKET_LENGTH - 1; }
        }

        public const byte mCH341A_EPP_IO_MAX = 0xFF;

        public const byte mCH341A_CMD_IO_ADDR_W = 0x00;
        public const byte mCH341A_CMD_IO_ADDR_R = 0x80;

        public const byte mCH341A_CMD_I2C_STM_STA = 0x74;
        public const byte mCH341A_CMD_I2C_STM_STO = 0x75;
        public const byte mCH341A_CMD_I2C_STM_OUT = 0x80;
        public const byte mCH341A_CMD_I2C_STM_IN = 0xC0;

        // const uint		mCH341A_CMD_I2C_STM_MAX	( min( 0x3F, mCH341_PACKET_LENGTH ) )
        public static uint mCH341A_CMD_I2C_STM_MAX
        {
            get { return (0x3F < mCH341_PACKET_LENGTH) ? 0x3f : mCH341_PACKET_LENGTH; }
        }

        public const byte mCH341A_CMD_I2C_STM_SET = 0x60;
        public const byte mCH341A_CMD_I2C_STM_US = 0x40;
        public const byte mCH341A_CMD_I2C_STM_MS = 0x50;
        public const byte mCH341A_CMD_I2C_STM_DLY = 0x0F;
        public const byte mCH341A_CMD_I2C_STM_END = 0x00;

        public const uint mCH341A_CMD_UIO_STM_IN = 0x00;
        public const uint mCH341A_CMD_UIO_STM_DIR = 0x40;
        public const uint mCH341A_CMD_UIO_STM_OUT = 0x80;
        public const uint mCH341A_CMD_UIO_STM_US = 0xC0;
        public const uint mCH341A_CMD_UIO_STM_END = 0x20;

        public const uint mCH341_PARA_MODE_EPP = 0x00;
        public const uint mCH341_PARA_MODE_EPP17 = 0x00;
        public const uint mCH341_PARA_MODE_EPP19 = 0x01;
        public const uint mCH341_PARA_MODE_MEM = 0x02;
        public const uint mCH341_PARA_MODE_ECP = 0x03;

        public const uint mStateBitERR = 0x00000100;
        public const uint mStateBitPEMP = 0x00000200;
        public const uint mStateBitINT = 0x00000400;
        public const uint mStateBitSLCT = 0x00000800;
        public const uint mStateBitWAIT = 0x00002000;
        public const uint mStateBitDATAS = 0x00004000;
        public const uint mStateBitADDRS = 0x00008000;
        public const uint mStateBitRESET = 0x00010000;
        public const uint mStateBitWRITE = 0x00020000;
        public const uint mStateBitSCL = 0x00400000;
        public const uint mStateBitSDA = 0x00800000;

        public const uint MAX_DEVICE_PATH_SIZE = 128;
        public const uint MAX_DEVICE_ID_SIZE = 64;


        [DllImport(DllName)]
        public static extern IntPtr CH341OpenDevice(uint iIndex);


        [DllImport(DllName)]
        public static extern void CH341CloseDevice(uint iIndex);


        [DllImport(DllName)]
        public static extern uint CH341GetVersion();


        /*
        public static extern uint	CH341DriverCommand(
        uint		iIndex,
        mPWIN32_COMMAND	ioCommand );
         */

        [DllImport(DllName)]
        public static extern uint CH341GetDrvVersion();



        [DllImport(DllName)]
        public static extern bool CH341ResetDevice(uint iIndex);

        [DllImport(DllName)]
        public static extern bool CH341GetDeviceDescr(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341GetConfigDescr(uint iIndex, IntPtr oBuffer, out uint ioLength);

        /*
        public static extern bool	CH341SetIntRoutine(
        uint		iIndex,
        mPCH341_INT_ROUTINE	iIntRoutine );
         */

        [DllImport(DllName)]
        public static extern bool CH341ReadInter(uint iIndex, out uint iStatus);

        [DllImport(DllName)]
        public static extern bool CH341AbortInter(uint iIndex);

        [DllImport(DllName)]
        public static extern bool CH341SetParaMode(uint iIndex, uint iMode);

        [DllImport(DllName)]
        public static extern bool CH341InitParallel(uint iIndex, uint iMode);

        [DllImport(DllName)]
        public static extern bool CH341ReadData0(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341ReadData1(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341AbortRead(uint iIndex);

        [DllImport(DllName)]
        public static extern bool CH341WriteData0(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341WriteData1(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341AbortWrite(uint iIndex);

        [DllImport(DllName)]
        public static extern bool CH341GetStatus(uint iIndex, out uint iStatus);

        [DllImport(DllName)]
        public static extern bool CH341ReadI2C(uint iIndex, byte iDevice, byte iAddr, out byte oByte);

        [DllImport(DllName)]
        public static extern bool CH341WriteI2C(uint iIndex, byte iDevice, byte iAddr, byte iByte);

        [DllImport(DllName)]
        public static extern bool CH341EppReadData(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341EppReadAddr(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341EppWriteData(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341EppWriteAddr(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341EppSetAddr(uint iIndex, byte iAddr);

        [DllImport(DllName)]
        public static extern bool CH341MemReadAddr0(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341MemReadAddr1(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341MemWriteAddr0(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341MemWriteAddr1(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341SetExclusive(uint iIndex, uint iExclusive);

        [DllImport(DllName)]
        public static extern bool CH341SetTimeout(uint iIndex, uint iWriteTimeout, uint iReadTimeout);

        [DllImport(DllName)]
        public static extern bool CH341ReadData(uint iIndex, IntPtr oBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern bool CH341WriteData(uint iIndex, IntPtr iBuffer, out uint ioLength);

        [DllImport(DllName)]
        public static extern IntPtr CH341GetDeviceName(uint iIndex);

        [DllImport(DllName)]
        public static extern uint CH341GetVerIC(uint iIndex);

        // #define		IC_VER_CH341A		0x20
        // #define		IC_VER_CH341A3		0x30

        [DllImport(DllName)]
        public static extern bool CH341FlushBuffer(uint iIndex);

        [DllImport(DllName)]
        public static extern bool CH341WriteRead(uint iIndex,
                                                 uint iWriteLength,
                                                 IntPtr iWriteBuffer,
                                                 uint iReadStep,
                                                 uint iReadTimes,
                                                 out uint oReadLength,
                                                 IntPtr oReadBuffer);

        [DllImport(DllName)]
        public static extern bool CH341SetStream(uint iIndex, uint iMode);

        [DllImport(DllName)]
        public static extern bool CH341SetDelaymS(uint iIndex, uint iDelay);

        [DllImport(DllName)]
        public static extern bool CH341StreamI2C(uint iIndex,
                                                 uint iWriteLength,
                                                 IntPtr iWriteBuffer,
                                                 uint iReadLength,
                                                 IntPtr oReadBuffer);


        [DllImport(DllName)]
        public static extern bool CH341ReadEEPROM(uint iIndex,
                                                  uint iEepromID,
                                                  uint iAddr,
                                                  uint iLength,
                                                  IntPtr oBuffer);

        [DllImport(DllName)]
        public static extern bool CH341WriteEEPROM(uint iIndex,
                                                   uint iEepromID,
                                                   uint iAddr,
                                                   uint iLength,
                                                   IntPtr iBuffer);


        [DllImport(DllName)]
        public static extern bool CH341GetInput(uint iIndex, out uint iStatus);

        [DllImport(DllName)]
        public static extern bool CH341SetOutput(uint iIndex,
                                                 uint iEnable,
                                                 uint iSetDirOut,
                                                 uint iSetDataOut);

        [DllImport(DllName)]
        public static extern bool CH341Set_D5_D0(uint iIndex,
                                                 uint iSetDirOut,
                                                 uint iSetDataOut);

        [DllImport(DllName)]
        public static extern bool CH341StreamSPI3(uint iIndex,
                                                  uint iChipSelect,
                                                  uint iLength,
                                                  IntPtr ioBuffer);

        [DllImport(DllName)]
        public static extern bool CH341StreamSPI4(uint iIndex,
                                                  uint iChipSelect,
                                                  uint iLength,
                                                  IntPtr ioBuffer);

        [DllImport(DllName)]
        public static extern bool CH341StreamSPI5(uint iIndex,
                                                  uint iChipSelect,
                                                  uint iLength,
                                                  IntPtr ioBuffer,
                                                  IntPtr ioBuffer2);

        [DllImport(DllName)]
        public static extern bool CH341BitStreamSPI(uint iIndex, uint iLength, IntPtr ioBuffer);

        [DllImport(DllName)]
        public static extern bool CH341SetBufUpload(uint iIndex, uint iEnableOrClear);

        [DllImport(DllName)]
        public static extern int CH341QueryBufUpload(uint iIndex);

        [DllImport(DllName)]
        public static extern bool CH341SetBufDownload(uint iIndex, uint iEnableOrClear);

        [DllImport(DllName)]
        public static extern int CH341QueryBufDownload(uint iIndex);

        [DllImport(DllName)]
        public static extern bool CH341ResetInter(uint iIndex);

        [DllImport(DllName)]
        public static extern bool CH341ResetRead(uint iIndex);

        [DllImport(DllName)]
        public static extern bool CH341ResetWrite(uint iIndex);

    }
}
