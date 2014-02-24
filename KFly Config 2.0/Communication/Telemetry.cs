using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace KFly.Communication
{
    /// <summary>
    /// Handles all Telemetry 
    /// </summary>
    public static class Telemetry
    {
        private static ConcurrentQueue<KFlyCommand> _sendBuffer = new ConcurrentQueue<KFlyCommand>();
        private static ConcurrentQueue<KFlyCommand> _receiveBuffer = new ConcurrentQueue<KFlyCommand>();

       
        private static Boolean _waitingForAck;
        private static KFlyCommand _lastSentCmd;

        private static TelemetryLink _link = new TelemetryLink();

        public static SendResult SendAsync(KFlyCommand cmd)
        {
            if (_link.Connected)
            {
                _sendBuffer.Enqueue(cmd);
                Task.Run(() =>
                {
                    SendNextMessage();
                });
                return SendResult.OK;
            }
            else
                return SendResult.NOT_CONNECTED;
        }

        public static TelemetryLink Link
        {
            get 
            {
                return _link;
            }
        }

        public static SendResult SendWithAck(KFlyCommand cmd, int timeout = 1000)
        {
            _sendBuffer.Enqueue(cmd);
            return SendResult.OK;
        }

        private static void SendNextMessage()
        {
            while ((!(_waitingForAck)) && (!_sendBuffer.IsEmpty))
            {
                _sendBuffer.TryDequeue(out _lastSentCmd);
                if (_lastSentCmd != null)
                {
                    _link.SendData(_lastSentCmd);
                }
            }
        }

        #region subscription
        /// <summary>
        /// All posts in the dictionary is created in the initializer and therefore no need for a threadsafe
        /// dictionary. The ConcurrentStack part is a thread safe alternative to List. 
        /// </summary>
        private static Dictionary<KFlyCommandType, ConcurrentDictionary<TeleSubscription, Delegate>> _subscribers;
           

        static Telemetry()
        {
            _subscribers = new Dictionary<KFlyCommandType, ConcurrentDictionary<TeleSubscription, Delegate>>();
            foreach (KFlyCommandType type in Enum.GetValues(typeof(KFlyCommandType)))
            {
                _subscribers.Add(type, new ConcurrentDictionary<TeleSubscription, Delegate>());
            }
        }

        /// <summary>
        /// Subscribes to a certain KflyCommand
        /// id will be the same as action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="action"></param>
        /// <returns>True if successful, false if already subscriber</returns>
        public static TeleSubscription Subscribe<T>(KFlyCommandType command,  Action<T> action) where T:KFlyCommand
        {
            var ts = new TeleSubscription();
            _subscribers[command].TryAdd(ts, action);
            return ts;
        }

        public static Boolean Unsubscribe(TeleSubscription ts)
        {
            Delegate dummy;
            foreach (var subscriptionList in _subscribers.Values)
            {
                if (subscriptionList.TryRemove(ts, out dummy)) //Can only exist one so if found return
                    return true;
            }
            return false;
        }


        public static Boolean IsSubscriber(TeleSubscription ts)
        {
            foreach (var subscriptionList in _subscribers.Values)
            {
                if (subscriptionList.ContainsKey(ts))
                    return true;
            }
            return false;
        }

        private static Task _latestHandleTask;

        public static void Handle(KFlyCommand command)
        {
            _latestHandleTask = Task.Factory.StartNew(() =>
                {
                    foreach (var action in _subscribers[command.Type].Values)
                    {
                        try
                        {
                            action.DynamicInvoke(command);
                        }
                        catch
                        { }
                    }
                    foreach (var action in _subscribers[KFlyCommandType.All].Values)
                    {
                        try
                        {
                            action.DynamicInvoke(command);
                        }
                        catch
                        { }
                    }
                });
        }
        #endregion

        #region ForTest
        /// <summary>
        /// This is more for the testclasses
        /// </summary>
        public static void WaitForHandle()
        {
            if (_latestHandleTask != null)
                _latestHandleTask.Wait(1000);
        }

        public static int TotalSubscriptionCount()
        {
            int count = 0;
            foreach (var key in _subscribers.Values)
            {
                foreach (var sb in key)
                {
                    count++;
                }
            }
            return count;
        }

        public static int SubscriptionCount(KFlyCommandType cmd)
        {
            int count = 0;
            foreach (var key in _subscribers[cmd])
            {
                count++;
            }
            return count;
        }
        #endregion
    }

    public class TeleSubscription
    {
    }

    public enum SendResult
    {
        OK,
        NOT_CONNECTED,
        TIME_OUT
    }

   
}
