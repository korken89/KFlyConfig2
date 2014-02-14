using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFly.Communication
{
    public static class TeleManager
    {
        /// <summary>
        /// All posts in the dictionary is created in the initializer and therefore no need for a threadsafe
        /// dictionary. The ConcurrentStack part is a thread safe alternative to List. 
        /// </summary>
        private static Dictionary<KFlyCommandType, ConcurrentDictionary<dynamic, Delegate>> _subscribers;
           

        static TeleManager()
        {
            _subscribers = new Dictionary<KFlyCommandType, ConcurrentDictionary<dynamic, Delegate>>();
            foreach (KFlyCommandType type in Enum.GetValues(typeof(KFlyCommandType)))
            {
                _subscribers.Add(type, new ConcurrentDictionary<dynamic, Delegate>());
            }
        }

        /// <summary>
        /// Subscribes to a certain KflyCommand
        /// </summary>
        /// <param name="command">CommandType to subscribe</param>
        /// <param name="id">used to id this subscription (for unsubscription)</param>
        /// <param name="action">delegate to run when message is recevied</param>
        /// <returns>True if successful, false if already subscriber</returns>
        public static Boolean Subscribe<T>(KFlyCommandType command, dynamic id, Action<T> action) where T : KFlyCommand
        {
            return _subscribers[command].TryAdd(id, action);
        }


        /// <summary>
        /// Subscribes to a certain KflyCommand
        /// id will be the same as action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="action"></param>
        /// <returns>True if successful, false if already subscriber</returns>
        public static Boolean Subscribe<T>(KFlyCommandType command, Action<T> action) where T:KFlyCommand
        {
            return Subscribe<T>(command, action, action);
        }

        public static Boolean Unsubscribe(KFlyCommandType command, dynamic id)
        {
            Delegate dummy;
            return _subscribers[command].TryRemove(id, out dummy);
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

        /// <summary>
        /// This is more for the testclasses
        /// </summary>
        public static void WaitForHandle()
        {
            if (_latestHandleTask != null)
                _latestHandleTask.Wait(1000);
        }
    }

   
}
