using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using KFly.Communication;

namespace KFlyConfig.Test
{
    [TestClass]
    public class KFlyCommandTest
    {
        private void SendBytes(StateMachine state, IEnumerable<byte> bytes)
        {
            foreach (byte b in bytes)
            {
                state.SerialManager(b);
            }
        }


        public static KFlyCommand LatestReceived;
        public static StateMachine State;
        private static TeleSubscription Ts;
           
        private static void HandleKFlyCommand(KFlyCommand cmd)
        {
            LatestReceived = cmd;
        }

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            State = new StateMachine();
            Ts = Telemetry.Subscribe(KFlyCommandType.All, (Action<KFlyCommand>)HandleKFlyCommand); 
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            State = null;
            Telemetry.Unsubscribe(Ts);
        }

        [TestMethod]
        public void TestDebugMessage()
        {
            State.Reset();

            DebugMessage dm = new DebugMessage();
            SendBytes(State, dm.ToTx());
            Telemetry.WaitForHandle();
            Assert.IsInstanceOfType(LatestReceived, typeof(DebugMessage));
            Assert.AreEqual("",(LatestReceived as DebugMessage).Message);

            dm = new DebugMessage("Test message");
            SendBytes(State, dm.ToTx());
            Telemetry.WaitForHandle();
            Assert.IsInstanceOfType(LatestReceived, typeof(DebugMessage));
            Assert.AreEqual("Test message", (LatestReceived as DebugMessage).Message);
        }

        [TestMethod]
        public void TestPing()
        {
            State.Reset();
            Ping dm = new Ping();
            SendBytes(State, dm.ToTx());
            Telemetry.WaitForHandle();
            Assert.IsInstanceOfType(LatestReceived, typeof(Ping));
        }

        [TestMethod]
        public void TestGetBootLoaderVersion()
        {
            State.Reset();

            KFlyCommand cmd = new GetBootLoaderVersion(){ Version = "v.1.0.3" };
            SendBytes(State, cmd.ToTx());
            Telemetry.WaitForHandle();
            Assert.IsInstanceOfType(LatestReceived, typeof(GetBootLoaderVersion));
            Assert.AreEqual("v.1.0.3",(LatestReceived as GetBootLoaderVersion).Version);
        }

        [TestMethod]
        public void TestGetFirmwareVersion()
        {
            State.Reset();

            KFlyCommand cmd = new GetFirmwareVersion(){ Version = "v.1.0.3" };
            SendBytes(State, cmd.ToTx());
            Telemetry.WaitForHandle();
            Assert.IsInstanceOfType(LatestReceived, typeof(GetFirmwareVersion));
            Assert.AreEqual("v.1.0.3",(LatestReceived as GetFirmwareVersion).Version);
        }
       
    }
}
