using System;
using System.Runtime.InteropServices;

namespace CH341
{
    public partial class CH341A
    {
        /// <summary>
        /// Packet length supported by CH341
        /// </summary>
        public const uint mCH341_PACKET_LENGTH = 32;
        /// <summary>
        /// The length of the short packet supported by CH341
        /// </summary>
        public const uint mCH341_PKT_LEN_SHORT = 8;

        // const uint		IOCTL_CH341_COMMAND		( FILE_DEVICE_UNKNOWN << 16 | FILE_ANY_ACCESS << 14 | 0x0f34 << 2 | METHOD_BUFFERED )
        // public static uint IOCTL_CH341_COMMAND	
        // {
        //    get { return (FILE_DEVICE_UNKNOWN << 16 | FILE_ANY_ACCESS << 14 | 0x0f34 << 2 | METHOD_BUFFERED); }
        // }

        // const uint			mWIN32_COMMAND_HEAD		mOFFSET( mWIN32_COMMAND, mBuffer )

        /// <summary>
        ///  The maximum number of CH341 connected at the same time
        /// </summary>
        public const uint mCH341_MAX_NUMBER = 16;

        /// <summary>
        /// The maximum length of the data buffer is 4096
        /// </summary>
        public const uint mMAX_BUFFER_LENGTH = 0x1000;

        // const uint			mMAX_COMMAND_LENGTH		( mWIN32_COMMAND_HEAD + mMAX_BUFFER_LENGTH )

        /// <summary>
        /// The default length of the data buffer is 1024
        /// </summary>
        public const uint mDEFAULT_BUFFER_LEN = 0x0400;

        // const uint			mDEFAULT_COMMAND_LEN	( mWIN32_COMMAND_HEAD + mDEFAULT_BUFFER_LEN )

        #region CH341 endpoint address
        /// <summary>
        /// The address of the control transfer upload endpoint of CH341
        /// </summary>
        public const uint mCH341_ENDP_INTER_UP = 0x81;
        /// <summary>
        /// The interrupt data of CH341 is transmitted to the address of the endpoint
        /// </summary>
        public const uint mCH341_ENDP_INTER_DOWN = 0x01;
        /// <summary>
        /// The address of the data block upload endpoint of CH341
        /// </summary>
        public const uint mCH341_ENDP_DATA_UP = 0x82;
        /// <summary>
        /// The address of the end point of the data block of CH341
        /// </summary>
        public const uint mCH341_ENDP_DATA_DOWN = 0x02;
        #endregion

        #region Pipe operation commands provided by the device layer interface
        /// <summary>
        /// CH341's integrated control pipeline
        /// </summary>
        public const uint mPipeDeviceCtrl = 0x00000004;
        /// <summary>
        /// CH341 interrupt data upload pipeline
        /// </summary>
        public const uint mPipeInterUp = 0x00000005;
        /// <summary>
        /// The data block upload pipeline of CH341
        /// </summary>
        public const uint mPipeDataUp = 0x00000006;
        /// <summary>
        /// The data block down pipe of CH341
        /// </summary>
        public const uint mPipeDataDown = 0x00000007;
        #endregion

        #region Function code of the application layer interface
        /// <summary>
        /// No action
        /// </summary>
        public const uint mFuncNoOperation = 0x00000000;
        /// <summary>
        /// Get the driver version number
        /// </summary>
        public const uint mFuncGetVersion = 0x00000001;
        /// <summary>
        /// Get USB device configuration descriptor
        /// </summary>
        public const uint mFuncGetConfig = 0x00000002;
        /// <summary>
        /// Set USB communication timeout
        /// </summary>
        public const uint mFuncSetTimeout = 0x00000009;
        /// <summary>
        /// Set exclusive use
        /// </summary>
        public const uint mFuncSetExclusive = 0x0000000b;
        /// <summary>
        /// Reset USB device
        /// </summary>
        public const uint mFuncResetDevice = 0x0000000c;
        /// <summary>
        /// Reset USB pipe
        /// </summary>
        public const uint mFuncResetPipe = 0x0000000d;
        /// <summary>
        /// Cancel the data request of the USB pipe
        /// </summary>
        public const uint mFuncAbortPipe = 0x0000000e;
        #endregion

        #region CH341 parallel port dedicated function code
        /// <summary>
        /// Set the parallel port mode
        /// </summary>
        public const uint mFuncSetParaMode = 0x0000000f;
        /// <summary>
        ///  Read data block 0 from parallel port
        /// </summary>
        public const uint mFuncReadData0 = 0x00000010;
        /// <summary>
        /// Read data block 1 from parallel port
        /// </summary>
        public const uint mFuncReadData1 = 0x00000011;
        /// <summary>
        /// Write data block 0 to the parallel port
        /// </summary>
        public const uint mFuncWriteData0 = 0x00000012;
        /// <summary>
        /// Write data block 1 to the parallel port
        /// </summary>
        public const uint mFuncWriteData1 = 0x00000013;
        /// <summary>
        /// Output first and then input
        /// </summary>
        public const uint mFuncWriteRead = 0x00000014;
        /// <summary>
        /// Set the buffer upload mode and the data length in the query buffer
        /// </summary>
        public const uint mFuncBufferMode = 0x00000020;
        /// <summary>
        /// Set the buffered downlink mode and the data length in the query buffer
        /// </summary>
        public const uint mFuncBufferModeDn = 0x00000021;
        #endregion

        #region USB device standard request code
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
        #endregion

        #region CH341 control transfer vendor-specific request type
        /// <summary>
        /// CH341 vendor-specific read operation via control transfer
        /// </summary>
        public const uint mCH341_VENDOR_READ = 0xC0;
        /// <summary>
        /// CH341 vendor-specific write operation via control transfer
        /// </summary>
        public const uint mCH341_VENDOR_WRITE = 0x40;
        #endregion

        #region Manufacturer-specific request code for CH341 control transfer
        /// <summary>
        /// Initialize the parallel port
        /// </summary>
        public const byte mCH341_PARA_INIT = 0xB1;
        /// <summary>
        /// Get the status of the I2C interface
        /// </summary>
        public const byte mCH341_I2C_STATUS = 0x52;
        /// <summary>
        ///  Command to issue I2C interface
        /// </summary>
        public const byte mCH341_I2C_COMMAND = 0x53;
        #endregion

        #region CH341 parallel port operation command code
        /// <summary> 
        /// Read data from the parallel port 0, the second byte is the length (ReadData0(), EppReadData())
        /// </summary>
        public const byte mCH341_PARA_CMD_R0 = 0xAC;
        /// <summary> 
        /// Read data from the parallel port, the second byte is the length (ReadData1()) 
        /// </summary>
        public const byte mCH341_PARA_CMD_R1 = 0xAD;
        /// <summary> 
        /// Write data 1 to the parallel port, starting with the next byte as the data stream (WriteData1())
        /// </summary>
        public const byte mCH341_PARA_CMD_W0 = 0xA6;
        /// <summary> 
        /// Write data 1 to the parallel port, starting with the next byte as the data stream (WriteData1())
        /// </summary>
        public const byte mCH341_PARA_CMD_W1 = 0xA7;
        /// <summary> 
        /// Get the parallel port status (GetInput())
        /// </summary>
        public const byte mCH341_PARA_CMD_STS = 0xA0;
        #endregion

        #region CH341A parallel port operation command code
        /// <summary>
        /// Set the parallel port output
        /// </summary>
        public const byte mCH341A_CMD_SET_OUTPUT = 0xA1;
        /// <summary>
        /// MEM with address read/write/input and output, starting from the next byte as the command stream
        /// </summary>
        public const byte mCH341A_CMD_IO_ADDR = 0xA2;
        /// <summary>
        /// PRINT compatible print mode output, starting from the next byte as the data stream
        /// </summary>
        public const byte mCH341A_CMD_PRINT_OUT = 0xA3;
        /// <summary>
        /// Command packet for PWM data output, starting from the next byte for the data stream
        /// </summary>
        public const byte mCH341A_CMD_PWM_OUT = 0xA4;
        /// <summary>
        /// Short packet, the next byte is the real length of the command packet, and the last byte and the following bytes are the original command packet
        /// </summary>
        public const byte mCH341A_CMD_SHORT_PKT = 0xA5;
        /// <summary>
        /// Command packet of SPI interface, starting from the next byte as data stream
        /// </summary>
        public const byte mCH341A_CMD_SPI_STREAM = 0xA8;

        // public const byte mCH341A_CMD_SIO_STREAM = 0xA9;  // Command packet for SIO interface, starting from the next byte for the data stream

        /// <summary>
        /// Command packet for I2C interface, starting from the next byte for the I2C command stream
        /// </summary>
        public const byte mCH341A_CMD_I2C_STREAM = 0xAA;
        /// <summary>
        /// Command packet for UIO interface, starting from the next byte as the command stream
        /// </summary>
        public const byte mCH341A_CMD_UIO_STREAM = 0xAB;
        /// <summary>
        /// Command packet for PIO interface, starting from the next byte for data flow
        /// </summary>
        public const byte mCH341A_CMD_PIO_STREAM = 0xAE;
        #endregion

        #region Manufacturer-specific request code for CH341A control transfer
        /// <summary>
        ///  Clear unfinished data
        /// </summary>
        public const byte mCH341A_BUF_CLEAR = 0xB2;
        /// <summary>
        /// Issue the I2C interface command and execute it immediately
        /// </summary>
        public const byte mCH341A_I2C_CMD_X = 0x54;
        /// <summary>
        /// Delay the specified time in leap seconds
        /// </summary>
        public const byte mCH341A_DELAY_MS = 0x5E;
        /// <summary>
        /// Get the chip version
        /// </summary>
        public const byte mCH341A_GET_VER = 0x5F;
        #endregion

        // const uint		mCH341_EPP_IO_MAX		( mCH341_PACKET_LENGTH - 1 )

        /// <summary>
        /// The maximum length of the single read and write data block of CH341 in EPP/MEM mode
        /// </summary>
        public static uint mCH341_EPP_IO_MAX
        {
            get { return mCH341_PACKET_LENGTH - 1; }
        }

        /// <summary>
        /// The maximum length of the single read and write data block of CH341A in EPP/MEM mode
        /// </summary>
        public const byte mCH341A_EPP_IO_MAX = 0xFF;

        /// <summary>
        /// MEM command stream with address read/write/input and output: write data, bits 0 - 6 is the address, the next byte is the data to be written
        /// </summary>
        public const byte mCH341A_CMD_IO_ADDR_W = 0x00;
        /// <summary>
        /// MEM command stream with address read/write/input/output: read data, bits 0 - 6 is the address, read data back together
        /// </summary>
        public const byte mCH341A_CMD_IO_ADDR_R = 0x80;


        #region  CH341 I2C stream command code
        //  mCH341A_CMD_I2C_STREAM : mCH341A_CMD_I2C_STM...
        /// <summary>
        /// Command stream for I2C interface: generate start bit
        /// </summary>
        public const byte mCH341A_CMD_I2C_STM_STA = 0x74;
        /// <summary>
        /// Command flow for I2C interface: generate stop bit
        /// </summary>
        public const byte mCH341A_CMD_I2C_STM_STO = 0x75;
        /// <summary>
        /// Command stream of I2C interface: output data, bit 0 - bit 5 is the length, the subsequent byte is the data, 0 length only sends one byte and returns the response
        /// </summary>
        public const byte mCH341A_CMD_I2C_STM_OUT = 0x80;
        /// <summary>
        /// Command flow for I2C interface: input data, bit 0 - bit 5 is the length, 0 length receives only one byte and sends no response
        /// </summary>
        public const byte mCH341A_CMD_I2C_STM_IN = 0xC0;

        /// <summary>
        /// Command flow of I2C interface Maximum length of input and output data of a single command
        /// </summary>
        public static uint mCH341A_CMD_I2C_STM_MAX
        {
            get { return (0x3F < mCH341_PACKET_LENGTH) ? 0x3f : mCH341_PACKET_LENGTH; }
        }

        // mCH341A_CMD_I2C_STREAM:mCH341A_CMD_I2C_STM...
        /// <summary>
        /// Command flow of I2C interface: set parameter, bit 2 = number of I/O of SPI (0=single input and single output, 1=double input and double output), bit 1 bit 0=I2C speed (00= Low speed, 01=standard, 10=fast, 11=high speed)
        /// </summary>
        public const byte mCH341A_CMD_I2C_STM_SET = 0x60; // SetStream(): mCH341A_CMD_I2C_STM_SET & SPEED
        /// <summary>
        /// Command flow of I2C interface: delay in microseconds, bit 3-bit 0 is the delay value
        /// </summary>
        public const byte mCH341A_CMD_I2C_STM_US = 0x40;
        /// <summary>
        /// Command flow for I2C interface: delay in units of leap seconds, bitе 0-3 is the delay value
        /// </summary>
        public const byte mCH341A_CMD_I2C_STM_MS = 0x50; // SetDelaymS(): mCH341A_CMD_I2C_STM_MS & DELAY (0x0-0xf)
        /// <summary>
        /// The maximum delay of the command flow for the I2C interface
        /// </summary>
        public const byte mCH341A_CMD_I2C_STM_DLY = 0x0F;
        /// <summary>
        /// Command flow for I2C interface: Command packet ends early
        /// </summary>
        public const byte mCH341A_CMD_I2C_STM_END = 0x00;
        #endregion

        #region  CH341 UIO stream command code
        // mCH341A_CMD_UIO_STREAM : mCH341A_CMD_UIO_STM...
        /// <summary>
        /// Command stream for UIO interface: input data D7-D0
        /// </summary>
        public const uint mCH341A_CMD_UIO_STM_IN = 0x00;
        /// <summary>
        /// Command flow for UIO interface: Set I/O direction D5-D0, bits 0 - 5 is direction data
        /// </summary>
        public const uint mCH341A_CMD_UIO_STM_DIR = 0x40;
        /// <summary>
        /// Command stream of UIO interface: output data D5-D0, bits 0 - 5 is data
        /// </summary>
        public const uint mCH341A_CMD_UIO_STM_OUT = 0x80;
        /// <summary>
        /// Command flow of UIO interface: delay in microseconds, bits 0 - 5 is the delay value
        /// </summary>
        public const uint mCH341A_CMD_UIO_STM_US = 0xC0;
        /// <summary>
        /// Command stream for UIO interface: Command packet ends early
        /// </summary>
        public const uint mCH341A_CMD_UIO_STM_END = 0x20;
        #endregion

        #region CH341 parallel port working mode
        //  mCH341A_CMD_PIO_STREAM : mCH341_PARA_MODE...
        /// <summary>
        /// EPP mode
        /// </summary>
        public const uint mCH341_PARA_MODE_EPP = 0x00;
        /// <summary>
        /// EPP mode V1.7
        /// </summary>
        public const uint mCH341_PARA_MODE_EPP17 = 0x00;
        /// <summary>
        /// EPP mode V1.9
        /// </summary>
        public const uint mCH341_PARA_MODE_EPP19 = 0x01;
        /// <summary>
        ///  MEM mode
        /// </summary>
        public const uint mCH341_PARA_MODE_MEM = 0x02;
        /// <summary>
        /// ECP mode
        /// </summary>
        public const uint mCH341_PARA_MODE_ECP = 0x03;
        #endregion

        #region I/O direction setting bit definition, bit definition of direct input status signal, direct output bit data definition
        /// <summary>
        /// ERR# pin input state, 1: high level, 0: low level
        /// </summary>
        public const uint mStateBitERR = 0x00000100;
        /// <summary>
        /// PEMP pin input state, 1: high level, 0: low level
        /// </summary>
        public const uint mStateBitPEMP = 0x00000200;
        /// <summary>
        /// INT# pin input state, 1: high level, 0: low level
        /// </summary>
        public const uint mStateBitINT = 0x00000400;
        /// <summary>
        /// SLCT pin input state, 1: high level, 0: low level
        /// </summary>
        public const uint mStateBitSLCT = 0x00000800;
        /// <summary>
        /// WAIT# pin input state, 1: high level, 0: low level
        /// </summary>
        public const uint mStateBitWAIT = 0x00002000;
        /// <summary>
        /// DATAS#/READ# pin input state, 1: high level, 0: low level
        /// </summary>
        public const uint mStateBitDATAS = 0x00004000;
        /// <summary>
        /// ADDRS#/ADDR/ALE pin input state, 1: high level, 0: low level
        /// </summary>
        public const uint mStateBitADDRS = 0x00008000;
        /// <summary>
        /// RESET# pin input state, 1: high level, 0: low level
        /// </summary>
        public const uint mStateBitRESET = 0x00010000;
        /// <summary>
        /// WRITE# pin input state, 1: high level, 0: low level
        /// </summary>
        public const uint mStateBitWRITE = 0x00020000;
        /// <summary>
        /// SCL pin input state, 1: high level, 0: low level
        /// </summary>
        public const uint mStateBitSCL = 0x00400000;
        /// <summary>
        /// SDA pin input state, 1: high level, 0: low level
        /// </summary>
        public const uint mStateBitSDA = 0x00800000;
        #endregion

        public const uint MAX_DEVICE_PATH_SIZE = 128;
        public const uint MAX_DEVICE_ID_SIZE = 64;

        // public const uint IC_VER_CH341A = 0x20
        // public const uint IC_VER_CH341A3 = 0x30

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
    }
}
