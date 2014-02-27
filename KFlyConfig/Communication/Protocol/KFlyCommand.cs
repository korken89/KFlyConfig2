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

        GetRateControllerData = 30,
        SetRateControllerData = 31,
        GetAttitudeControllerData = 32,
  	    SetAttitudeControllerData = 33,
  	    GetVelocityControllerData = 34,
	    SetVelocityControllerData = 35,
        GetPositionControllerData = 36,
        SetPositionControllerData = 37,
        // 38 Excluded, will be sync when combined with ACK which is forbidden
        GetChannelMix = 39,
        SetChannelMix = 40,
        GetRCCalibration = 41,
        SetRCCalibration = 42,
        GetRCValues = 43,
        GetSensorData = 44,
        GetRawSensorData = 45,
        GetSensorCalibration = 46,
        SetSensorCalibration = 47,


        //Following is for easy access to subscribe/unsubscribe and ís not part of the communication protocol
        All = 120, //To subscribe everything
        ConnectionStatusChanged = 121, //Connection status has changed
        ConnectionStatistics = 122, //Info about the connection, sent every 2 seconds while connected
    };

    public abstract class KFlyCommand
    {
        public static byte SYNC = (byte)0xa6;
	    public static byte ACK_BIT = (byte)0x80;
	    public static byte ACK_MASK = (byte)((int)~0x80 & 0xff);

        public KFlyCommandType Type;

        public Boolean UseAck;
        public int TimeOut;
        public Action<SendResult> ActionAfterAck;

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
                UseAck? (byte)((byte)Type | ACK_BIT): (byte)Type,
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
                    case KFlyCommandType.GetSensorData:
                        cmd = new GetSensorData();
                        break;
                    case KFlyCommandType.GetRawSensorData:
                        cmd = new GetRawSensorData();
                        break;
                    case KFlyCommandType.GetSensorCalibration:
                        cmd = new GetSensorCalibration();
                        break;
                    case KFlyCommandType.ACK:
                        cmd = new Ack();
                        break;
                    default:
                        cmd = null;
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
