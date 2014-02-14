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

        GetControllerData = 30,
        SetControllerData = 31,
        GetChannelMix = 32,
        SetChannelMix = 33,
        GetRCCalibration = 34,
        SetRCCalibration = 35,
        GetRCValues = 36,
        GetSensorData = 37,


        All = 127, //To subscribe everything
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
            tx.Add(CRC8.GenerateCRC(tx.Take(3)));
            if (data.Count > 0)
            {
                tx.AddRange(data);
                int crc16 = CRC_CCITT.GenerateCRC(tx);
                byte[] crcb = BitConverter.GetBytes(crc16);

                tx.Add(crcb[1]);
                tx.Add(crcb[0]);
            }
            return tx;
        }

        public virtual List<byte> ToTx()
        {
            return CreateTxWithHeader(null);
        }

        public virtual void ParseData(List<byte> data)
        {
        }

        public override String ToString()
        {
            return Enum.GetName(typeof(KFlyCommandType), Type);
        }

        public static KFlyCommand Parse(List<Byte> bytes)
        {
            if (bytes.Count > 3)
            {
                KFlyCommand cmd = null;
                switch ((KFlyCommandType)(bytes[1]))
                {
                    case KFlyCommandType.DebugMessage:
                        cmd = new DebugMessage();
                        break;
                    case KFlyCommandType.Ping:
                        cmd = new Ping();
                        break;
                    case KFlyCommandType.GetFirmwareVersion:
                        cmd = new GetFirmwareVersion();
                        break;
                    case KFlyCommandType.GetBootloaderVersion:
                        cmd = new GetBootLoaderVersion();
                        break;
                    default:
                        cmd = new KFlyCommand((KFlyCommandType)bytes[1]);
                        break;
                }
                if (cmd != null)
                {
                    List<byte> data = bytes.GetRange(4, bytes.Count-4);
                    cmd.ParseData(data);
                }
                return cmd;
            }
            return null;
        }

    }
}
