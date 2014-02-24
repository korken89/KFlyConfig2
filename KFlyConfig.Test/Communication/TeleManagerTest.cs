using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using KFly.Communication;

namespace KFlyConfig.Test
{
    [TestClass]
    public class TeleManagerTest
    {

        private static KFlyCommand LastMessage;
        [TestMethod]
        public void TestSubscription()
        {
            var ts = Telemetry.Subscribe(KFlyCommandType.All, (KFlyCommand cmd) =>
                {
                    LastMessage = cmd;
                });

            Assert.IsTrue(Telemetry.IsSubscriber(ts));
            Assert.AreEqual(1, Telemetry.TotalSubscriptionCount());
            Assert.AreEqual(1, Telemetry.SubscriptionCount(KFlyCommandType.All));

            Telemetry.Handle(new DebugMessage());
            Telemetry.WaitForHandle();

            Assert.IsInstanceOfType(LastMessage, typeof(DebugMessage));

            Telemetry.Unsubscribe(ts);

            Assert.IsFalse(Telemetry.IsSubscriber(ts));
            Assert.AreEqual(0, Telemetry.TotalSubscriptionCount());
            Assert.AreEqual(0, Telemetry.SubscriptionCount(KFlyCommandType.All));

            
        }

       
       
    }
}
