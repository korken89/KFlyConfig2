using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public static class TeleManager
    {

        private static Dictionary<KFlyCommandType, List<Delegate>> _subscribers =
            new Dictionary<KFlyCommandType,List<Delegate>>();


        /// <summary>
        /// Subscribes to a certain KflyCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="action"></param>
        /// <returns>True if successful, false if already subscriber</returns>
        public static Boolean Subscribe<T>(KFlyCommandType command, Action<T> action) where T:KFlyCommand
       // public static Boolean Subscribe(KFlyCommandType command, Action<KFlyCommand> action)
        {
            Action<KFlyCommand> act = action as Action<KFlyCommand>;
            if (_subscribers.ContainsKey(command))
            {
                if (_subscribers[command].Contains(action))
                    return false;
                _subscribers[command].Add(action);
            }
            else
            {
                _subscribers.Add(command, new List<Delegate>(){ action });
            }
            return true;
        }

        public static Boolean Unsubscribe(KFlyCommandType command, Action<KFlyCommand> action)
        {
            if (_subscribers.ContainsKey(command))
            {
                if (_subscribers[command].Contains(action))
                {
                    _subscribers[command].Remove(action);
                    return true;
                }
            }
            return false;
        }

        public static void Handle(KFlyCommand command)
        {
            if (_subscribers.ContainsKey(command.Type))
            {
                foreach (var action in _subscribers[command.Type])
                {
                    try
                    {
                        action.DynamicInvoke(command);
                    }
                    catch
                    { }
                }
            }
            if (_subscribers.ContainsKey(KFlyCommandType.All))
            {
                foreach (var action in _subscribers[KFlyCommandType.All])
                {
                    try
                    {
                        action.DynamicInvoke(command);
                    }
                    catch
                    { }
                }
            }
        }
    }

   
}
