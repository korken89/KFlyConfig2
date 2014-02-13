using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using KFly.Communication;

namespace KFlyConfig.Test
{
    [TestClass]
    public class StateMachineTest
    {
        private void SendBytes(StateMachine state, byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                state.SerialManager(b);
            }
        }

        [TestMethod]
        public void TestSuccessfulStates()
        {
            StateMachine state = new StateMachine();
            
            //New machine should be in waitforsync state:
            Assert.AreEqual(StateMachine.State.WaitingForSYNC, state.CurrentState);

            //Send sync
            SendBytes(state, new byte[] { KFlyCommand.SYNC });

            //Should be in receive command state
            Assert.AreEqual(StateMachine.State.ReceivingCommand, state.CurrentState);

            //Send command
            SendBytes(state, new byte[] { (byte)KFlyCommandType.DebugMessage });

            //Should be in receive size
            Assert.AreEqual(StateMachine.State.ReceivingSize, state.CurrentState);

            //Send size ()
            SendBytes(state, new byte[] { 0x05});

            //Should be in receive CRC8 state
            Assert.AreEqual(StateMachine.State.ReceivingCRC8, state.CurrentState);

            //Send crc8
            byte crc8 = CRC8.GenerateCRC(new List<byte>(new byte[] { 
                KFlyCommand.SYNC, (byte)KFlyCommandType.DebugMessage, 0x05 }));
            SendBytes(state, new byte[] { crc8 });

            //Should be in receive data state
            Assert.AreEqual(StateMachine.State.ReceivingData, state.CurrentState);

            //Send 5 "random" bytes
            SendBytes(state, new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 });
            
            //Should be in receive crc16 state
            Assert.AreEqual(StateMachine.State.ReceivingCRC16, state.CurrentState);

            //Send crc16
            int crc16 = CRC_CCITT.GenerateCRC(new List<byte>(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }));
            SendBytes(state, new byte[] { (byte)(crc16>>8), (byte)crc16 });

            //Should be in receive sync state again
            Assert.AreEqual(StateMachine.State.WaitingForSYNC, state.CurrentState);
        }

        [TestMethod]
        public void TestAck()
        {
            StateMachine state = new StateMachine();
            SendBytes(state, new byte[] { KFlyCommand.SYNC });
            Assert.AreEqual(StateMachine.State.ReceivingCommand, state.CurrentState);
            Assert.AreEqual(false, state.Ack);
            SendBytes(state, new byte[] { (byte)((byte)KFlyCommandType.Ping | KFlyCommand.ACK_BIT) });
            Assert.AreEqual(StateMachine.State.ReceivingSize, state.CurrentState);
            Assert.AreEqual(true, state.Ack, "Ack should be set to true now");
        }

        [TestMethod]
        public void TestReSync()
        {
            StateMachine state = new StateMachine();
            SendBytes(state, new byte[] { KFlyCommand.SYNC });
            SendBytes(state, new byte[] { (byte)KFlyCommandType.DebugMessage });
            SendBytes(state, new byte[] { 0x05 });
            byte crc8 = CRC8.GenerateCRC(new List<byte>(new byte[] { 
                KFlyCommand.SYNC, (byte)KFlyCommandType.DebugMessage, 0x05 }));
            SendBytes(state, new byte[] { crc8 });

            Assert.AreEqual(StateMachine.State.ReceivingData, state.CurrentState);
            //Send 3 "random" bytes
            SendBytes(state, new byte[] { 0x01, 0x02, 0x03 });

            //Here something goes wrong and we resync
            SendBytes(state, new byte[] { KFlyCommand.SYNC });
            SendBytes(state, new byte[] { (byte)KFlyCommandType.DebugMessage });
            SendBytes(state, new byte[] { 0x05 });
            SendBytes(state, new byte[] { crc8 });
            //Should be in receive data state again
            Assert.AreEqual(StateMachine.State.ReceivingData, state.CurrentState);
        }

        [TestMethod]
        public void TestSyncLikeData()
        {
            StateMachine state = new StateMachine();
            SendBytes(state, new byte[] { KFlyCommand.SYNC });
            SendBytes(state, new byte[] { (byte)KFlyCommandType.DebugMessage });
            SendBytes(state, new byte[] { 0x05 });
            byte crc8 = CRC8.GenerateCRC(new List<byte>(new byte[] { 
                KFlyCommand.SYNC, (byte)KFlyCommandType.DebugMessage, 0x05 }));
            SendBytes(state, new byte[] { crc8 });

            Assert.AreEqual(StateMachine.State.ReceivingData, state.CurrentState);
            //Send "4" bytes where the 4:th byte is the same as sync so its sent double
            SendBytes(state, new byte[] { 0x01, 0x02, 0x03, KFlyCommand.SYNC, KFlyCommand.SYNC});
            //Should still wait for the 5:th byte
            Assert.AreEqual(StateMachine.State.ReceivingData, state.CurrentState);
            SendBytes(state, new byte[] { 0x05 });
            
            Assert.AreEqual(StateMachine.State.ReceivingCRC16, state.CurrentState);
        }

        [TestMethod]
        public void TestSyncLikeSize()
        {
            StateMachine state = new StateMachine();
            SendBytes(state, new byte[] { KFlyCommand.SYNC });
            SendBytes(state, new byte[] { (byte)KFlyCommandType.DebugMessage });
            SendBytes(state, new byte[] { KFlyCommand.SYNC, KFlyCommand.SYNC });
            byte crc8 = CRC8.GenerateCRC(new List<byte>(new byte[] { 
                KFlyCommand.SYNC, (byte)KFlyCommandType.DebugMessage, KFlyCommand.SYNC }));
            SendBytes(state, new byte[] { crc8 });

            Assert.AreEqual(StateMachine.State.ReceivingData, state.CurrentState);
        }

        //would like tests on synclike crc8 and crc16 as well but have no idea how to make
    }
}
