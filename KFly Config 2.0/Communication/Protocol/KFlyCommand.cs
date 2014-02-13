using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public enum KFlyCommandType
    {
        None = 0,
        ACK = 1,
        Ping = 2,
        DebugMessage = 3,
        GetRunningMode = 4,

        PrepareWriteFirmware = 10,   /* Bootloader specific, shall always require ACK */
        WriteFirmwarePackage = 11,	/* Bootloader specific, shall always require ACK */
        WriteLastFirmwarePackage = 12,	/* Bootloader specific, shall always require ACK */
        ReadFirmwarePackage = 13,	/* Bootloader specific, shall always require ACK */
        ReadLastFirmwarePackage = 14,   /* Bootloader specific, shall always require ACK */
        NextPackage = 15,   /* Bootloader specific, shall always require ACK */
        ExitBootloader = 16,   /* Bootloader specific, shall always require ACK */
        GetBootloaderVersion = 17,
        GetFirmwareVersion = 18,
        SaveToFlash = 19,

        GetRegulatorData = 30,
        SetRegulatorData = 31,
        GetChannelMix = 32,
        SetChannelMix = 33,
        StartRCCalibration = 34,
        StopRCCalibration = 35,
        CalibrateRCCenters = 36,
        GetRCCalibration = 37,
        SetRCCalibration = 38,
        GetRCValues = 39,
        GetSensorData = 40
    };

    public class KFlyCommand
    {
        public static byte SYNC = (byte)0xa6;
	    public static byte ACK_BIT = (byte)0x40;
	    public static byte ACK_MASK = (byte)((int)~0x40 & 0xff);

        public KFlyCommandType Type;

        public KFlyCommand()
        {
        }
        public KFlyCommand(KFlyCommandType type)
        {
            Type = type;
        }

        /// <summary>
        /// Creates a data byte array and fills the header part
        /// only data part left
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        protected List<byte> CreateTxWithHeader(List<byte> data)
        {
            data = (data != null) ? data : new List<byte>();
            List<byte> tx = new List<byte>()
            {
                KFlyCommand.SYNC,
                (byte)Type,
                (byte)data.Count,
            };
            tx.Add(CRC8.GenerateCRC(data.Take(3)));
            if (data.Count > 0)
            {
                tx.AddRange(data);
                int crc16 = CRC_CCITT.GenerateCRC(data);
                tx.Add((byte)(crc16 >> 8));
                tx.Add((byte)(crc16));
            }
            return data;
        }

        public virtual List<byte> ToTx()
        {
            return CreateTxWithHeader(null);
        }

        public virtual void ParseRx(List<byte> data)
        {
        }

        public static KFlyCommand Parse(List<Byte> bytes)
        {
            if (bytes.Count > 3)
            {
                KFlyCommand cmd = null;
                switch ((KFlyCommandType)(bytes[0]))
                {
                    case KFlyCommandType.DebugMessage:
                        return new DebugMessage();
                }
                if (cmd != null)
                {
                    List<byte> data = bytes.GetRange(3, bytes.Count-3);
                    cmd.ParseRx(data);
                }
                return cmd;
            }
            return null;
        }

    }
}
