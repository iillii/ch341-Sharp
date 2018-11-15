using System;
using System.Runtime.InteropServices;

namespace CH341
{
    public partial class CH341A
    {
        private uint _Index;
        private bool _IsOpen = false;

        string MessageRequiredToOpenDevice = "It is required to open device using the method OpenDevice()";

        #region Init and device info

        /// <summary>
        /// Implements CH341A device
        /// </summary>
        public CH341A()
        {
            _Index = 0;
        }

        /// <summary>
        /// Implements CH341A device
        /// </summary>
        /// <param name="iIndex">Specify the CH341 device serial number, 0 corresponds to the first device</param>
        public CH341A(uint iIndex)
        {
            _Index = iIndex;
        }

        /// <summary>
        /// Open the CH341 device
        /// </summary>
        /// <returns></returns>
        public bool OpenDevice()
        {
            IntPtr handler = CH341OpenDevice(_Index);
            if (((int)handler) == -1) { _IsOpen = false; }
            else { _IsOpen = true; }
            return _IsOpen;
        }
        /// <summary>
        /// Close the CH341 device
        /// </summary>
        public void CloseDevice()
        {
            CH341CloseDevice(_Index);
            _IsOpen = false;
        }

        /// <summary>
        /// Get the DLL version number
        /// </summary>
        /// <returns>return the version number</returns>
        public uint GetVersion()
        {
            return CH341GetVersion();
        }

        /// <summary>
        /// Get the driver version number
        /// </summary>
        /// <returns>return the version number</returns>
        public uint GetDrvVersion()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }
            return CH341GetDrvVersion();
        }

        /// <summary>
        /// Get device name
        /// </summary>
        /// <returns>returns a CH341 device name</returns>
        public string GetDeviceName()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr devName = CH341GetDeviceName(_Index);

            return Marshal.PtrToStringAnsi(devName);
        }

        /// <summary>
        /// Get the version of the CH341 chip
        /// </summary>
        /// <returns>0 = device is invalid, 0x10=CH341, 0x20=CH341A</returns>
        public IC_VER GetVerIC()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return (IC_VER)CH341GetVerIC(_Index);
        }

        #endregion

        // TODO: bool	CH341SetIntRoutine(uint		iIndex,	mPCH341_INT_ROUTINE	iIntRoutine ){}

        /// <summary>
        /// Set to use the current CH341 device exclusively
        /// </summary>
        /// <param name="iExclusive">0 is the device can be shared, non-zero is exclusive use</param>
        /// <returns></returns>
        public bool SetExclusive(uint iExclusive)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341SetExclusive(_Index, iExclusive);
        }

        // Bit 1 - Bit 0: I2C interface speed / SCL frequency
        //  00 = low speed / 20KHz
        //  01 = standard / 100KHz (default)
        //  10 = fast / 400KHz
        //  11 = high speed / 750KHz
        // Bit 2: SPI I/O count/IO pin, 
        //  0=Single input and single output (D3 clock/D5 out/D7 input) (default), 
        //  1=Double in and double out (D3 clock/D5 out D4) Out / D7 into D6 into
        // Bit 7: The bit order in the SPI byte, 
        //  0 = low bit first, 
        //  1 = high bit first
        // other reservations must be 0
        /// <summary>
        /// Set serial stream mode
        /// </summary>
        /// <param name="iMode"> Specify the mode</param>
        /// <returns></returns>
        public bool SetStream(uint iMode)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341SetStream(_Index, iMode);
        }

        /// <summary>
        /// Set hardware asynchronous delay, return quickly after the call, and delay the specified number of milliseconds before the next stream operation
        /// </summary>
        /// <param name="iDelay">Specify the number of milliseconds to delay</param>
        /// <returns></returns>
        public bool SetDelaymS(uint iDelay)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341SetDelaymS(_Index, iDelay);
        }

        #region USB layer call

        // The program returns the data length after the call, and still returns the command structure. If it is a read operation, the data is returned in the command structure.
        // The returned data length is 0 when the operation fails. When the operation succeeds, it is the length of the entire command structure. For example, if one byte is read, mWIN32_COMMAND_HEAD+1 is returned.
        // The command structure is provided before the call: pipe number or command function code, length of access data (optional), data (optional)
        // After the command structure is called, it returns: the operation status code, the length of the subsequent data (optional),
        // The operation status code is the code defined by WINDOWS, you can refer to NTSTATUS.H,
        // The length of the subsequent data refers to the length of the data returned by the read operation. The data is stored in the subsequent buffer, which is generally 0 for write operations.
        /// <summary>
        /// Directly pass the command to the driver (USB Control Transfer)
        /// </summary>
        /// <param name="ioCommand">Command structure</param>
        /// <returns>return 0 if error occurs, otherwise return the data length</returns>
        public uint CH341DriverCommand(ref ControlTransferCommand ioCommand)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr iocmd = Marshal.AllocHGlobal(Marshal.SizeOf(ioCommand));

            Marshal.StructureToPtr(ioCommand, iocmd, false);

            uint opResult = CH341DriverCommand(_Index, iocmd);

            ioCommand = (ControlTransferCommand)Marshal.PtrToStructure(iocmd, typeof(ControlTransferCommand));

            return opResult;
        }

        /// <summary>
        /// Set the timeout for USB data read and write
        /// </summary>
        /// <param name="iWriteTimeout">Specifies the timeout period for the USB to write out the data block, in milliseconds mS, 0xFFFFFFFF specifies no timeout (default)</param>
        /// <param name="iReadTimeout">Specify the timeout period for USB to read the data block, in milliseconds mS, 0xFFFFFFFF specifies no timeout (default)</param>
        /// <returns></returns>
        public bool SetTimeout(uint iWriteTimeout, uint iReadTimeout)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341SetTimeout(_Index, iWriteTimeout, iReadTimeout);
        }

        /// <summary>
        /// Directly read data block from CH341 USB endpoint (USB Bulk Transfer)
        /// </summary>
        /// <param name="oBuffer">Returns an array with bytes read</param>
        /// <returns></returns>
        public bool ReadData(out byte[] oBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr ptrBuffer = Marshal.AllocHGlobal((int)mMAX_BUFFER_LENGTH);

            uint ioLength = mMAX_BUFFER_LENGTH;

            bool opResult = CH341ReadData(_Index, ptrBuffer, out ioLength);

            if (opResult == true)
            {
                oBuffer = new Byte[ioLength];
                Marshal.Copy(ptrBuffer, oBuffer, 0, (int)ioLength);
            }
            else
            {
                oBuffer = null;
            }

            return opResult;
        }

        /// <summary>
        /// Directly write data block to CH341 USB endpoint (USB Bulk Transfer)
        /// </summary>
        /// <param name="iBuffer">An array of bytes to be written</param>
        /// <returns></returns>
        public bool WriteData(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341WriteData(_Index, iBufferPtr, out ioLength);
        }

        /// <summary>
        /// Execute the data stream command, first output and then input (USB Bulk Transfer)
        /// </summary>
        /// <param name="iWriteBuffer">An array of bytes to be written</param>
        /// <param name="iReadStep">The length of the single block to be read, the total length to be read is (iReadStep*iReadTimes)</param>
        /// <param name="iReadTimes">Number of times to prepare for reading</param>
        /// <param name="oReadBuffer">Returns an array with bytes read</param>
        /// <returns></returns>
        public bool WriteRead(byte[] iWriteBuffer, uint iReadStep, uint iReadTimes, out byte[] oReadBuffer)
        {
            // iReadStep должен быть больше или равен принимаемым данным
            // iReadTimes=1

            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint iWriteLength = (uint)iWriteBuffer.Length;

            IntPtr iWriteBufferPtr = Marshal.AllocHGlobal((int)iWriteLength);

            Marshal.Copy(iWriteBuffer, 0, iWriteBufferPtr, (int)iWriteLength);

            // uint oReadLength = mMAX_BUFFER_LENGTH;
            // uint oReadLength = mCH341_PACKET_LENGTH;
            uint oReadLength = mDEFAULT_BUFFER_LEN;

            IntPtr oReadBufferPtr = Marshal.AllocHGlobal((int)oReadLength);

            bool opResult = CH341WriteRead(_Index, iWriteLength, iWriteBufferPtr, iReadStep, iReadTimes, out oReadLength, oReadBufferPtr);

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

        /// <summary>
        /// Clear internal CH341 buffer
        /// </summary>
        /// <returns></returns>
        public bool FlushBuffer()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341FlushBuffer(_Index);
        }

        /// <summary>
        /// Reset USB device
        /// </summary>
        /// <returns></returns>
        public bool ResetDevice()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }
            return CH341ResetDevice(_Index);
        }

        /// <summary>
        /// Reset data block read operation (USB Bulk Transfer)
        /// </summary>
        /// <returns></returns>
        public bool ResetRead()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341ResetRead(_Index);
        }

        /// <summary>
        ///  Reset data block write operation (USB Bulk Transfer)
        /// </summary>
        /// <returns></returns>
        public bool ResetWrite()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341ResetWrite(_Index);
        }

        /// <summary>
        /// Read USB device descriptor
        /// </summary>
        /// <param name="oBuffer">returns an array of bytes containing the USB device descriptor</param>
        /// <returns></returns>
        public bool GetDeviceDescr(out byte[] oBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr ptrBuffer = Marshal.AllocHGlobal((int)mMAX_BUFFER_LENGTH);

            uint ioLength = mMAX_BUFFER_LENGTH;

            bool opResult = CH341GetDeviceDescr(_Index, ptrBuffer, out ioLength);

            if (opResult == true)
            {
                oBuffer = new Byte[ioLength];
                Marshal.Copy(ptrBuffer, oBuffer, 0, (int)ioLength);
            }
            else
            {
                oBuffer = null;
            }

            return opResult;
        }

        /// <summary>
        /// Read USB configuration descriptor
        /// </summary>
        /// <param name="obuf">Returns an array of bytes containing the USB configuration descriptor</param>
        /// <returns></returns>
        public bool GetConfigDescr(out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)mMAX_BUFFER_LENGTH);

            uint ioLength = mMAX_BUFFER_LENGTH;

            bool opResult = CH341GetConfigDescr(_Index, oBuffer, out ioLength);

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

        /* 
         * If the internal buffer upload mode is enabled, the CH341 driver 
         * creation thread automatically receives the USB upload data to 
         * the internal buffer and clears the existing data in the buffer. 
         * When the application calls ReadData(), it will immediately
         * return to the existing buffer data
         * 
         */
        /// <summary>
        /// Set internal buffer upload mode
        /// </summary>
        /// <param name="iEnableOrClear">Set 0 to disable internal buffer upload mode, use direct upload, non-zero to enable internal buffer upload mode and clear existing data in buffer</param>
        /// <returns></returns>
        public bool SetBufUpload(uint iEnableOrClear)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341SetBufUpload(_Index, iEnableOrClear);
        }

        /// <summary>
        /// Query the number of existing data packets in the internal upload buffer
        /// </summary>
        /// <returns>successfully return the number of data packets, error returns -1</returns>
        public int QueryBufUpload()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341QueryBufUpload(_Index);
        }

        /*
         * If the internal buffer downlink mode is enabled, then when 
         * the application calls CH341WriteData will simply put the USB 
         * downlink data into the internal buffer and return immediately,
         * and the thread created by the CH341 driver is automatically sent 
         * until the completion
         */
        /// <summary>
        /// Set internal buffer downlink mode
        /// </summary>
        /// <param name="iEnableOrClear">Set 0 to disable internal buffering mode, use direct downlink, non-zero to enable internal buffering mode and clear existing data in buffer</param>
        /// <returns></returns>
        public bool SetBufDownload(uint iEnableOrClear)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341SetBufDownload(_Index, iEnableOrClear);
        }

        /// <summary>
        /// Query the number of remaining packets in the internal downlink buffer (not yet sent)
        /// </summary>
        /// <returns>Successfully return the number of packets, error returns -1</returns>
        public int QueryBufDownload()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341QueryBufDownload(_Index);
        }

        #endregion

        #region Interrupt control

        /*
         * TODO: Set interrupt service routine
         * public bool CH341SetIntRoutine() { }
         */

        // Bit 7-bit 0 corresponds to the D7-D0 pin of CH341
        // Bit 8 corresponds to the ERR# pin of CH341, 
        // bit 9 corresponds to the PEMP pin of CH341, 
        // bit 10 corresponds to the INT# pin of CH341, // 
        // bit 11 corresponds to the SLCT pin of CH341
        /// <summary>
        /// Read interrupt data
        /// </summary>
        /// <param name="iStatus">return interrupt status data</param>
        /// <returns></returns>
        public bool ReadInter(out uint iStatus)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341ReadInter(_Index, out iStatus);
        }

        /// <summary>
        /// Abandon interrupt data read operation
        /// </summary>
        /// <returns></returns>
        public bool AbortInter()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341AbortInter(_Index);
        }

        /// <summary>
        /// Reset interrupt data read operation
        /// </summary>
        /// <returns></returns>
        public bool ResetInter()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341ResetInter(_Index);
        }

        #endregion

        #region Parallel/EPP transfer

        /// <summary>
        /// Reset and initialize the parallel port, RST# output low level pulse
        /// </summary>
        /// <param name="iMode">Specify parallel mode: 0 for EPP mode/EPP mode V1.7, 1 for EPP mode V1.9, 2 for MEM mode, >= 0x00000100 Keep current mode</param>
        /// <returns></returns>
        public bool InitParallel(uint iMode)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341InitParallel(_Index, iMode);
        }

        /// <summary>
        /// Set the parallel port mode
        /// </summary>
        /// <param name="iMode">Specify the parallel port mode: 0 is EPP mode / EPP mode V1.7, 1 is EPP mode V1.9, 2 is MEM mode</param>
        /// <returns></returns>
        public bool SetParaMode(uint iMode)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341SetParaMode(_Index, iMode);
        }

        // Bit 7-bit 0 corresponds to the D7-D0 pin of CH341
        // Bit 8 corresponds to the ERR# pin of CH341, 
        // bit 9 corresponds to the PEMP pin of CH341, 
        // bit 10 corresponds to the INT# pin of CH341, 
        // bit 11 corresponds to the SLCT pin of CH341, 
        // bit 23 corresponds to the SDA pin of CH341
        // Bit 13 corresponds to the BUDY/WAIT# pin of CH341, 
        // bit 14 corresponds to the AUTOFD#/DATAS# pin of CH341, 
        // bit 15 corresponds to the SLCTIN#/ADDRS# pin of CH341
        /// <summary>
        /// Directly enter data and status via CH341
        /// </summary>
        /// <param name="iStatus">Pins status</param>
        /// <returns></returns>
        public bool GetStatus(out uint iStatus)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341GetStatus(_Index, out iStatus);
        }

        /// <summary>
        /// // Abandon data block read operation
        /// </summary>
        /// <returns></returns>
        public bool AbortRead()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341AbortRead(_Index);
        }

        /// <summary>
        /// // Abandon data block write operation
        /// </summary>
        /// <returns></returns>
        public bool AbortWrite()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341AbortWrite(_Index);
        }

        /// <summary>
        /// Read data block from #0 port
        /// </summary>
        /// <param name="ioLength">The length to be read</param>
        /// <param name="obuf">Returns an array with bytes read</param>
        /// <returns></returns>
        public bool ReadData0(UInt32 ioLength, out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)ioLength);

            uint tLength = ioLength;

            bool opResult = CH341ReadData0(_Index, oBuffer, ref tLength);

            if (opResult == true)
            {
                obuf = new Byte[tLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)tLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        /// <summary>
        /// Read data block from #1 port
        /// </summary>
        /// <param name="ioLength">The length to be read</param>
        /// <param name="obuf">Returns an array with bytes read</param>
        /// <returns></returns>
        public bool ReadData1(UInt32 ioLength, out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)ioLength);

            uint tLength = ioLength;

            bool opResult = CH341ReadData1(_Index, oBuffer, ref tLength);

            if (opResult == true)
            {
                obuf = new Byte[tLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)tLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        /// <summary>
        /// Write data block to port #0
        /// </summary>
        /// <param name="iBuffer">An array of bytes to be written</param>
        /// <returns></returns>
        public bool WriteData0(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341WriteData0(_Index, iBufferPtr, out ioLength);
        }

        /// <summary>
        /// Write data block to port #1
        /// </summary>
        /// <param name="iBuffer">An array of bytes to be written</param>
        /// <returns></returns>
        public bool WriteData1(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341WriteData1(_Index, iBufferPtr, out ioLength);
        }

        /// <summary>
        /// EPP mode read data: WR#=1, DS#=0, AS#=1, D0-D7=input
        /// </summary>
        /// <param name="ioLength">The length to be read</param>
        /// <param name="obuf">Returns an array with bytes read</param>
        /// <returns></returns>
        public bool EppReadData(UInt32 ioLength, out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)ioLength);

            uint tLength = ioLength;

            bool opResult = CH341EppReadData(_Index, oBuffer, out tLength);

            if (opResult == true)
            {
                obuf = new Byte[tLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)tLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        /// <summary>
        /// EPP mode read address: WR#=1, DS#=1, AS#=0, D0-D7=input
        /// </summary>
        /// <param name="ioLength">The length to be read</param>
        /// <param name="obuf">Returns an array with bytes read</param>
        /// <returns></returns>
        public bool EppReadAddr(UInt32 ioLength, out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)mMAX_BUFFER_LENGTH);

            uint tLength = mMAX_BUFFER_LENGTH;

            bool opResult = CH341EppReadAddr(_Index, oBuffer, out tLength);

            if (opResult == true)
            {
                obuf = new Byte[tLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)tLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        /// <summary>
        /// EPP mode write data: WR#=0, DS#=0, AS#=1, D0-D7=output
        /// </summary>
        /// <param name="iBuffer">An array of bytes to be written</param>
        /// <returns></returns>
        public bool EppWriteData(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341EppWriteData(_Index, iBufferPtr, out ioLength);
        }

        /// <summary>
        /// EPP mode write address: WR#=0, DS#=1, AS#=0, D0-D7=output
        /// </summary>
        /// <param name="iBuffer">An array of bytes to be written</param>
        /// <returns></returns>
        public bool EppWriteAddr(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341EppWriteAddr(_Index, iBufferPtr, out ioLength);
        }

        /// <summary>
        /// EPP mode setting address: WR#=0, DS#=1, AS#=0, D0-D7=output
        /// </summary>
        /// <param name="iAddr">Specify EPP address</param>
        /// <returns></returns>
        public bool EppSetAddr(byte iAddr)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341EppSetAddr(_Index, iAddr);
        }

        /// <summary>
        /// MEM mode read address 0: WR#=1, DS#/RD#=0, AS#/ADDR=0, D0-D7=input 
        /// </summary>
        /// <param name="ioLength">The length to be read</param>
        /// <param name="obuf">Returns an array with bytes read</param>
        /// <returns></returns>
        public bool MemReadAddr0(UInt32 ioLength, out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)ioLength);

            uint tLength = ioLength;

            bool opResult = CH341MemReadAddr0(_Index, oBuffer, out tLength);

            if (opResult == true)
            {
                obuf = new Byte[tLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)tLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        /// <summary>
        /// MEM mode read address 1: WR#=1, DS#/RD#=0, AS#/ADDR=1, D0-D7=input
        /// </summary>
        /// <param name="ioLength">The length to be read</param>
        /// <param name="obuf">Returns an array with bytes read</param>
        /// <returns></returns>
        public bool MemReadAddr1(UInt32 ioLength, out byte[] obuf)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBuffer = Marshal.AllocHGlobal((int)ioLength);

            uint tLength = ioLength;

            bool opResult = CH341MemReadAddr1(_Index, oBuffer, out tLength);

            if (opResult == true)
            {
                obuf = new Byte[tLength];
                Marshal.Copy(oBuffer, obuf, 0, (int)tLength);
            }
            else
            {
                obuf = null;
            }

            return opResult;
        }

        /// <summary>
        /// MEM mode write address 0: WR#=0, DS#/RD#=1, AS#/ADDR=0, D0-D7=output
        /// </summary>
        /// <param name="iBuffer">An array of bytes to be written</param>
        /// <returns></returns>
        public bool MemWriteAddr0(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341MemWriteAddr0(_Index, iBufferPtr, out ioLength);
        }

        /// <summary>
        /// MEM mode write address 1: WR#=0, DS#/RD#=1, AS#/ADDR=1, D0-D7=output
        /// </summary>
        /// <param name="iBuffer">An array of bytes to be written</param>
        /// <returns></returns>
        public bool MemWriteAddr1(byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint ioLength = (uint)iBuffer.Length;
            IntPtr iBufferPtr = Marshal.AllocHGlobal((int)ioLength);
            Marshal.Copy(iBuffer, 0, iBufferPtr, (int)ioLength);

            return CH341MemWriteAddr1(_Index, iBufferPtr, out ioLength);
        }

        #endregion

        #region Direct pin control

        // Bit 7-bit 0 corresponds to the D7-D0 pin of CH341
        // Bit 8 corresponds to the ERR# pin of CH341, 
        // bit 9 corresponds to the PEMP pin of CH341, 
        // bit 10 corresponds to the INT# pin of CH341, 
        // bit 11 corresponds to the SLCT pin of CH341, 
        // bit 23 corresponds to the SDA pin of CH341
        // Bit 13 corresponds to the BUDY/WAIT# pin of CH341, 
        // bit 14 corresponds to the AUTOFD#/DATAS# pin of CH341, 
        // bit 15 corresponds to the SLCTIN#/ADDRS# pin of CH341
        /// <summary>
        /// Direct input of data and status via CH341, efficiency is higher than GetStatus()
        /// </summary>
        /// <param name="iStatus">Pin state</param>
        /// <returns></returns>
        public bool GetInput(out uint iStatus)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341GetInput(_Index, out iStatus);
        }

        // Use this API cautiously to prevent the I/O direction from being 
        // changed so that the input pin becomes an output pin, causing a short
        // circuit between the other output pins and damaging the chip.

        // Data valid flag:
        // Bit 0 is 1 indicating that bit 15-bit 8 of iSetDataOut is valid, otherwise it is ignored.
        // Bit 1 is 1 indicating that bit 15-bit 8 of iSetDirOut is valid, otherwise it is ignored
        // Bit 2 is 1 to indicate that 7-bit 0 of iSetDataOut is valid, otherwise it is ignored.
        // Bit 3 is 1 to indicate that bit 7-bit 0 of iSetDirOut is valid, otherwise ignore
        // Bit 4 is 1 indicating that bit 23-bit 16 of iSetDataOut is valid, otherwise it is ignored.

        // iSetDirOut and iSetDataOut
        // Bit 7-bit 0 corresponds to the D7-D0 pin of CH341
        // Bit 8 corresponds to the ERR# pin of CH341, 
        // bit 9 corresponds to the PEMP pin of CH341, 
        // bit 10 corresponds to the INT# pin of CH341, 
        // bit 11 corresponds to the SLCT pin of CH341
        // Bit 13 corresponds to the WAIT# pin of CH341, 
        // bit 14 corresponds to the DATAS#/READ# pin of CH341, 
        // bit 15 corresponds to the ADDRS#/ADDR/ALE pin of CH341

        // The following pins can only be output, regardless of I/O direction: 
        // Bit 16 corresponds to CH341 RESET# pin, 
        // Bit 17 corresponds to CH341 WRITE# pin, 
        // Bit 18 corresponds to CH341 SCL pin, 
        // Bit 29 corresponds to CH341 SDA pin

        /// <summary>
        /// Set the I/O direction of CH341 and output data directly through CH341
        /// </summary>
        /// <param name="iEnable">Data valid flag</param>
        /// <param name="iSetDirOut">Set the I/O direction. When a bit is cleared to 0, the corresponding pin is the input. If a bit is set to 1, the corresponding pin is the output. In the parallel mode, the default value is 0x000FC000</param>
        /// <param name="iSetDataOut">Output data, if the I/O direction is output, then the corresponding pin outputs a low level when a bit is cleared to 0. When a bit is set to 1, the corresponding pin outputs a high level</param>
        /// <returns></returns>
        public bool SetOutput(uint iEnable, uint iSetDirOut, uint iSetDataOut)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341SetOutput(_Index, iEnable, iSetDirOut, iSetDataOut);
        }

        // Use this API cautiously to prevent the I/O direction from being 
        // changed so that the input pin becomes an output pin, causing a short
        // circuit between the other output pins and damaging the chip.

        // Bit 0 - bit 5 corresponds to D0-D5 pin of CH341

        /// <summary>
        /// Set the I/O direction of the D5-D0 pin of CH341, and directly output data through the D5-D0 pin of CH341, the efficiency is higher than SetOutput()
        /// </summary>
        /// <param name="iSetDirOut">Set the I/O direction of each pin of D5-D0. When a bit is cleared to 0, the corresponding pin is input. If a bit is set to 1, the corresponding pin is output. In the parallel mode, the default value is 0x00</param>
        /// <param name="iSetDataOut">Set the output data of each pin of D5-D0. If the I/O direction is output, then the corresponding pin outputs a low level when a bit is cleared to 0, and the corresponding pin outputs a high level when a bit is set to 1</param>
        /// <returns></returns>
        public bool Set_D5_D0(uint iSetDirOut, uint iSetDataOut)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341Set_D5_D0(_Index, iSetDirOut, iSetDataOut);
        }

        #endregion

        #region SPI transfer

        /// <summary>
        /// The API has expired, please do not use
        /// </summary>
        /// <param name="iChipSelect"></param>
        /// <param name="ioBuffer"></param>
        /// <returns></returns>
        public bool StreamSPI3(uint iChipSelect, ref byte[] ioBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint iLength = (uint)ioBuffer.Length;

            IntPtr ioBufferPtr = Marshal.AllocHGlobal((int)iLength);

            Marshal.Copy(ioBuffer, 0, ioBufferPtr, (int)iLength);

            bool opResult = CH341StreamSPI3(_Index, iChipSelect, iLength, ioBufferPtr);

            if (opResult == true)
            {
                Marshal.Copy(ioBufferPtr, ioBuffer, 0, (int)iLength);
            }

            return opResult;
        }

        /*
        * SPI Timing: The DCK/D3 pin is the clock output.The default is low.
        * The DOUT/D5 pin is output during the low period before the rising 
        * edge of the clock.The DIN/D7 pin is high during the falling edge 
        * of the clock
        */
        /// <summary>
        /// Process SPI data stream, 4-wire interface, clock line is DCK/D3 pin, output data line is DOUT/D5 pin, input data line is DIN/D7 pin, chip select line is D0/ D1/D2, speed about 68K bytes
        /// </summary>
        /// <param name="iChipSelect">Chip Select Control, Bit 7 is 0 to ignore Chip Select Control, Bit 7 is 1 to enable parameter: Bit 1 Bit 0 is 00/01/10 Select D0/D1/D2 pin as low level respectively Effective chip selection</param>
        /// <param name="ioBuffer">An array of bytes to be written from DOUT, and return the data read from DIN</param>
        /// <returns></returns>
        public bool StreamSPI4(uint iChipSelect, ref byte[] ioBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint iLength = (uint)ioBuffer.Length;

            IntPtr ioBufferPtr = Marshal.AllocHGlobal((int)iLength);

            Marshal.Copy(ioBuffer, 0, ioBufferPtr, (int)iLength);

            bool opResult = CH341StreamSPI4(_Index, iChipSelect, iLength, ioBufferPtr);

            if (opResult == true)
            {
                Marshal.Copy(ioBufferPtr, ioBuffer, 0, (int)iLength);
            }

            return opResult;
        }

        /* 
         * SPI Timing: The DCK/D3 pin is the clock output. The default is low. 
         * The DOUT/D5 and DOUT2/D4 pins are output during the low period before 
         * the rising edge of the clock. DIN/D7 and DIN2/D6 pins. Enter
         * during the high level before the falling edge of the clock 
         */

        /// <summary>
        /// Process SPI data stream, 5-wire interface, clock line is DCK/D3 pin, output data line is DOUT/D5 and DOUT2/D4 pin, input data line is DIN/D7 and DIN2/D6 Foot, chip selection line is D0/D1/D2, speed is about 30K bytes*2
        /// </summary>
        /// <param name="iChipSelect">Chip Select Control, Bit 7 is 0 to ignore Chip Select Control, Bit 7 is 1 to enable parameter: Bit 1 Bit 0 is 00/01/10 Select D0/D1/D2 pin as low level respectively Effective chip selection</param>
        /// <param name="iLength">The number of data bytes to be transferred</param>
        /// <param name="ioBuffer">The first array of bytes to be written from DOUT, and returns the data read from DIN</param>
        /// <param name="ioBuffer2">The second array of bytes to be written from DOUT2, and return the data read from DIN2</param>
        /// <returns></returns>
        public bool StreamSPI5(uint iChipSelect, uint iLength, ref byte[] ioBuffer, ref byte[] ioBuffer2)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }
            if (iLength > ioBuffer.Length) { throw new System.ArgumentOutOfRangeException("iLength < ioBuffer.Length"); }
            if (iLength > ioBuffer2.Length) { throw new System.ArgumentOutOfRangeException("iLength < ioBuffer2.Length"); }

            IntPtr ioBufferPtr = Marshal.AllocHGlobal((int)iLength);
            Marshal.Copy(ioBuffer, 0, ioBufferPtr, (int)iLength);

            IntPtr ioBuffer2Ptr = Marshal.AllocHGlobal((int)iLength);
            Marshal.Copy(ioBuffer2, 0, ioBuffer2Ptr, (int)iLength);

            bool opResult = CH341StreamSPI5(_Index, iChipSelect, iLength, ioBufferPtr, ioBuffer2Ptr);

            if (opResult == true)
            {
                Marshal.Copy(ioBufferPtr, ioBuffer, 0, (int)iLength);
                Marshal.Copy(ioBuffer2Ptr, ioBuffer2, 0, (int)iLength);
            }

            return opResult;
        }

        /* 
        * SPI Timing: The DCK/D3 pin is the clock output. The default is low. The DOUT/D5 and DOUT2/D4 pins are output during the low period before the rising edge of the clock. DIN/D7 and DIN2/D6 pins. 
        * Enter during the high level before the falling edge of the clock
        * One byte in ioBuffer corresponds to D7-D0 pin, bit 5 is output to DOUT, bit 4 is output to DOUT2, bit 2 bit 0 is output to D2-D0, bit 7 is input from DIN, bit 6 Input from DIN2, bit 3 data is ignored
        * Before calling this API, you should first call CH341Set_D5_D0 to set the I/O direction of the D5-D0 pin of CH341 and set the default level of the pin 
        */
        /// <summary>
        /// Process SPI bit data stream, 4-wire/5-line interface, clock line is DCK/D3 pin, output data line is DOUT/DOUT2 pin, input data line is DIN/DIN2 pin, chip select The line is D0/D1/D2 and the speed is about 8K*2
        /// </summary>
        /// <param name="iLength">The number of data bits to be transmitted, up to 896 at a time, no more than 256 recommended</param>
        /// <param name="ioBuffer">An array of bytes to be written from DOUT/DOUT2/D2-D0, and return the data read from DIN/DIN2</param>
        /// <returns></returns>
        public bool BitStreamSPI(uint iLength, ref byte[] ioBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr ioBufferPtr = Marshal.AllocHGlobal(ioBuffer.Length);

            Marshal.Copy(ioBuffer, 0, ioBufferPtr, ioBuffer.Length);

            bool opResult = CH341BitStreamSPI(_Index, iLength, ioBufferPtr);

            if (opResult == true)
            {
                Marshal.Copy(ioBufferPtr, ioBuffer, 0, ioBuffer.Length);
            }

            return opResult;
        }

        #endregion

        #region EEPROM read/write

        /// <summary>
        /// Read data block from EEPROM at a speed of approximately 56K bytes
        /// </summary>
        /// <param name="iEepromID">specify EEPROM model</param>
        /// <param name="iAddr">Specify the address of the data unit</param>
        /// <param name="iLength">The number of bytes of data to be read</param>
        /// <param name="oBuffer">Returns an array with bytes read</param>
        /// <returns></returns>
        public bool ReadEEPROM(EEPROM_TYPE iEepromID, uint iAddr, uint iLength, out byte[] oBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBufferPtr = Marshal.AllocHGlobal((int)iLength);

            bool opResult = CH341ReadEEPROM(_Index, (uint)iEepromID, iAddr, iLength, oBufferPtr);

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

        /// <summary>
        /// Write data block to EEPROM
        /// </summary>
        /// <param name="iEepromID">Specify EEPROM model</param>
        /// <param name="iAddr">Specify the address of the data unit</param>
        /// <param name="iLength">The number of bytes of data to be written out</param>
        /// <param name="iBuffer">An array of bytes to be written</param>
        /// <returns></returns>
        public bool WriteEEPROM(EEPROM_TYPE iEepromID, uint iAddr, uint iLength, byte[] iBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            IntPtr oBufferPtr = Marshal.AllocHGlobal((int)iLength);
            Marshal.Copy(iBuffer, 0, oBufferPtr, (int)iLength);

            return CH341WriteEEPROM(_Index, (uint)iEepromID, iAddr, iLength, oBufferPtr);
        }

        #endregion

        #region I2C transfer

        /// <summary>
        /// Read a byte of data from the I2C interface
        /// </summary>
        /// <param name="iDevice">lower 7 bits specify the I2C device address</param>
        /// <param name="iAddr">specify the address of the data unit</param>
        /// <param name="oByte">Returns byte read</param>
        /// <returns></returns>
        public bool ReadI2C(byte iDevice, byte iAddr, out byte oByte)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341ReadI2C(_Index, iDevice, iAddr, out oByte);
        }

        /// <summary>
        /// Write a byte of data to the I2C interface
        /// </summary>
        /// <param name="iDevice">Lower 7 bits specify the I2C device address</param>
        /// <param name="iAddr">Specify the address of the data unit</param>
        /// <param name="iByte">byte data to be written</param>
        /// <returns></returns>
        public bool WriteI2C(byte iDevice, byte iAddr, byte iByte)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341WriteI2C(_Index, iDevice, iAddr, iByte);
        }

        /// <summary>
        /// Handle I2C data stream, 2-wire interface, clock line is SCL pin, data line is SDA pin (quasi-bidirectional I/O), speed is about 56K bytes
        /// </summary>
        /// <param name="iWriteBuffer">An array of bytes to be written</param>
        /// <param name="iReadLength">The number of bytes of data to be read</param>
        /// <param name="oReadBuffer">Returns an array with bytes read</param>
        /// <returns></returns>
        public bool StreamI2C(byte[] iWriteBuffer, uint iReadLength, out byte[] oReadBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            uint iWriteLength = (uint)iWriteBuffer.Length;

            IntPtr iWriteBufferPtr = Marshal.AllocHGlobal((int)iWriteLength);

            Marshal.Copy(iWriteBuffer, 0, iWriteBufferPtr, (int)iWriteLength);

            IntPtr oReadBufferPtr = Marshal.AllocHGlobal((int)iReadLength);

            bool opResult = CH341StreamI2C(_Index, iWriteLength, iWriteBufferPtr, iReadLength, oReadBufferPtr);

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

        public bool IIC_IssueStart()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            byte[] mBuffer = new byte[] { mCH341A_CMD_I2C_STREAM, mCH341A_CMD_I2C_STM_STA, mCH341A_CMD_I2C_STM_END };

            return (WriteData(mBuffer));
        }

        public bool IIC_IssueStop()
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            byte[] mBuffer = new byte[] { mCH341A_CMD_I2C_STREAM, mCH341A_CMD_I2C_STM_STO, mCH341A_CMD_I2C_STM_END };

            return (WriteData(mBuffer));
        }

        /// <summary>
        /// Output data block, do not check the response
        /// </summary>
        /// <param name="iOutBuffer">An array of bytes to be written, which must be less than 29 bytes</param>
        /// <returns></returns>
        public bool IIC_OutBlockSkipAck(byte[] iOutBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            byte[] mBuffer = new byte[1 + 1 + iOutBuffer.Length + 1];

            mBuffer[0] = mCH341A_CMD_I2C_STREAM;
            mBuffer[1] = (byte)(mCH341A_CMD_I2C_STM_OUT | iOutBuffer.Length);
            Array.Copy(iOutBuffer, 0, mBuffer, 2, iOutBuffer.Length);
            mBuffer[iOutBuffer.Length + 2] = mCH341A_CMD_I2C_STREAM;

            return (WriteData(mBuffer));
        }

        /// <summary>
        ///  Output one byte of data and check if the response is valid
        /// </summary>
        /// <param name="iOutByte">Byte to be written</param>
        /// <returns></returns>
        public bool IIC_OutByteCheckAck(byte iOutByte)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            byte[] mBuffer = new byte[] { mCH341A_CMD_I2C_STREAM, mCH341A_CMD_I2C_STM_OUT, iOutByte, mCH341A_CMD_I2C_STM_END };

            byte[] mReadBuf;

            if (WriteRead(mBuffer, mCH341A_CMD_I2C_STM_MAX, 1, out mReadBuf) == true)
            {
                if (mReadBuf.Length > 0 && (mReadBuf[mReadBuf.Length - 1] & 0x80) == 0) { return true; }
            }

            return false;
        }

        /// <summary>
        ///  Input data block, valid response generated every input byte
        /// </summary>
        /// <param name="iInLength">The number of bytes of data to be read, which must be less than 32 bytes in a single time</param>
        /// <param name="iOutBuffer">Returns an array with bytes read</param>
        /// <returns></returns>
        public bool IIC_InBlockByAck(uint iInLength, out byte[] iOutBuffer)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            if (iInLength == 0) { throw new System.ArgumentOutOfRangeException("iInLength == 0"); }

            if (iInLength > mCH341A_CMD_I2C_STM_MAX) { throw new System.ArgumentOutOfRangeException("iInLength > mCH341A_CMD_I2C_STM_MAX"); }

            byte[] mBuffer = new byte[] { mCH341A_CMD_I2C_STREAM, (byte)(mCH341A_CMD_I2C_STM_IN | iInLength), mCH341A_CMD_I2C_STM_END };

            if (WriteRead(mBuffer, mCH341A_CMD_I2C_STM_MAX, 1, out iOutBuffer) == true)
            {
                if (iOutBuffer.Length == iInLength)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Read one byte of data, but does not generate a response
        /// </summary>
        /// <param name="oInByte">Returns one byte read</param>
        /// <returns></returns>
        public bool IIC_InByteNoAck(out byte oInByte)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            byte[] mBuffer = new byte[] { mCH341A_CMD_I2C_STREAM, mCH341A_CMD_I2C_STM_IN, mCH341A_CMD_I2C_STM_END };

            byte[] mReadBuf;

            if (WriteRead(mBuffer, mCH341A_CMD_I2C_STM_MAX, 1, out mReadBuf) == true)
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

        #endregion

        /// <summary>
        /// Set the serial port mode of CH341. This API can only be used for CH341 chip working in serial port mode.
        /// </summary>
        /// <param name="iParityMode">Specify the data check mode of the CH341 serial port: NOPARITY/ODDPARITY/EVENPARITY/MARKPARITY/SPACEPARITY</param>
        /// <param name="iBaudRate"> Specify the communication baud rate value of the CH341 serial port, which can be any value between 50 and 3000000</param>
        /// <returns></returns>
        public bool SetupSerial(uint iParityMode, uint iBaudRate)
        {
            if (_IsOpen == false) { throw new System.InvalidOperationException(MessageRequiredToOpenDevice); }

            return CH341SetupSerial(_Index, iParityMode, iBaudRate);
        }
    }
}
