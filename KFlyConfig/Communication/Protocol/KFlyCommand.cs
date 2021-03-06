﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public enum KFlyCommandType
    {
        None = 0,
        ACK = 1,
        [SelfAck]
        Ping = 2,
        DebugMessage = 3,
        GetRunningMode = 4,
        ManageSubscriptions = 5,

        PrepareWriteFirmware = 10,   /* Bootloader specific, shall always require ACK */
        WriteFirmwarePackage = 11,	/* Bootloader specific, shall always require ACK */
        WriteLastFirmwarePackage = 12,	/* Bootloader specific, shall always require ACK */
        ReadFirmwarePackage = 13,	/* Bootloader specific, shall always require ACK */
        ReadLastFirmwarePackage = 14,   /* Bootloader specific, shall always require ACK */
        NextPackage = 15,   /* Bootloader specific, shall always require ACK */
        ExitBootloader = 16,   /* Bootloader specific, shall always require ACK */
        [SelfAck]
        GetDeviceInfo = 17,
        SetDeviceString = 18,
        SaveToFlash = 19,

        [SelfAck]
        GetArmSettings = 28,
        [AffectSettingsOnCard]
        SetArmSettings = 29,

        [SelfAck]
        GetRateControllerData = 30,
        [AffectSettingsOnCard]
        SetRateControllerData = 31,
        [SelfAck]
        GetAttitudeControllerData = 32,
        [AffectSettingsOnCard]
        SetAttitudeControllerData = 33,
        [SelfAck]
        GetVelocityControllerData = 34,
        [AffectSettingsOnCard]
        SetVelocityControllerData = 35,
        [SelfAck]
        GetPositionControllerData = 36,
        [AffectSettingsOnCard]
        SetPositionControllerData = 37,
        // 38 Excluded, will be sync when combined with ACK which is forbidden
        [SelfAck]
        GetChannelMix = 39,
        [AffectSettingsOnCard]
        SetChannelMix = 40,
        [SelfAck]
        GetRCCalibration = 41,
        [AffectSettingsOnCard]
        SetRCCalibration = 42,
        [SelfAck]
        GetRCValues = 43,
        [SelfAck]
        GetSensorData = 44,
        GetRawSensorData = 45,
        [SelfAck]
        GetSensorCalibration = 46,
        [AffectSettingsOnCard]
        SetSensorCalibration = 47,

        [SelfAck]
        GetEstimationRate    = 48,
        [SelfAck]
        GetEstimationAttitude = 49,
        [SelfAck]
        GetEstimationVelocity = 50,
        [SelfAck]
        GetEstimationPosition = 51,
        [SelfAck]
        GetEstimationAllStates = 52,
        ResetEstimation = 53,
            
        ControlDebug = 72,


        //Following is for easy access to subscribe/unsubscribe and ís not part of the communication protocol
        All = 120, //To subscribe everything
        ConnectionStatusChanged = 121, //Connection status has changed
        ConnectionStatistics = 122, //Info about the connection, sent every 2 seconds while connected
        SaveStatusChanged = 123, //The cards save status (settings) have changed
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
            var useAck = (UseAck && !SelfAck.AppliesTo(Type)); //Dont use ack on SelfAcking messages
            List<byte> tx = new List<byte>()
            {
                KFlyCommand.SYNC,
                useAck? (byte)((byte)Type | ACK_BIT): (byte)Type,
                (byte)data.Count
            };
           
            tx.Add(CRC8.GenerateCRC(tx.Take(3)));
            if (data.Count > 0)
            {
                tx.AddRange(data);
                int crc16 = CRC_CCITT.GenerateCRC(tx);
                byte[] crcb = BitConverter.GetBytes(crc16);

                tx.Add(crcb[1]);
                tx.Add(crcb[0]);
                tx = FixSyncBytes(tx);
            }
            return tx;
        }

        //
        protected List<byte> FixSyncBytes(List<byte> tx)
        {
            List<byte> fixedList = new List<byte>();
            fixedList.Add(tx[0]);
            for (var i = 1; i < tx.Count; i++)
            {
                byte b = tx[i];
                fixedList.Add(b);
                if (b == KFlyCommand.SYNC)
                    fixedList.Add(b);
            }
            return fixedList;
        }

        public virtual List<byte> ToTx()
        {
            return CreateTxWithHeader(null);
        }

        public virtual void ParseData(List<byte> data)
        {
        }

        public virtual Boolean AffectsSettingsOnCard()
        {
            return AffectSettingsOnCard.AppliesTo(Type);
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
                    case KFlyCommandType.GetDeviceInfo:
                        cmd = new GetDeviceInfo();
                        break;
                    case KFlyCommandType.GetSensorData:
                        cmd = new GetSensorData();
                        break;
                    case KFlyCommandType.GetEstimationAttitude:
                        cmd = new GetEstimationAttitude();
                        break;
                    case KFlyCommandType.GetRawSensorData:
                        cmd = new GetRawSensorData();
                        break;
                    case KFlyCommandType.GetRateControllerData:
                        cmd = new GetRateControllerData();
                        break;
                    case KFlyCommandType.GetAttitudeControllerData:
                        cmd = new GetAttitudeControllerData();
                        break;
                    case KFlyCommandType.GetSensorCalibration:
                        cmd = new GetSensorCalibration();
                        break;
                    case KFlyCommandType.GetChannelMix:
                        cmd = new GetChannelMix();
                        break;
                    case KFlyCommandType.GetRCCalibration:
                        cmd = new GetRCCalibration();
                        break;
                    case KFlyCommandType.GetRCValues:
                        cmd = new GetRCValues();
                        break;
                    case KFlyCommandType.GetArmSettings:
                        cmd = new GetArmSettings();
                        break;
                    case KFlyCommandType.ACK:
                        cmd = new Ack();
                        break;
                    default:
                        cmd = new NotImplemented((KFlyCommandType)(bytes[1]));
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
