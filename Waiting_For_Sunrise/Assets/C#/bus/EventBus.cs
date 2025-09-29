using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.C_.bus
{

    /// <summary>
    /// 事件总线
    /// </summary>
    public static class EventBus
    {
        // 存储所有事件处理器
        private static Dictionary<Type, List<Delegate>> _eventHandlers = new Dictionary<Type, List<Delegate>>();

        // 事件总线日志开关（调试用）
        public static bool EnableLogging = false;

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="handler">事件处理器</param>
        public static void Subscribe<T>(Action<T> handler) where T : class
        {
            Type eventType = typeof(T);

            if (!_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = new List<Delegate>();
            }

            _eventHandlers[eventType].Add(handler);

            if (EnableLogging)
            {
                Debug.Log($"[EventBus] 订阅 {eventType.Name} 事件，当前订阅数: {_eventHandlers[eventType].Count}");
            }
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="handler">要移除的事件处理器</param>
        public static void Unsubscribe<T>(Action<T> handler) where T : class
        {
            Type eventType = typeof(T);

            if (_eventHandlers.TryGetValue(eventType, out var handlers))
            {
                handlers.Remove(handler);

                if (EnableLogging)
                {
                    Debug.Log($"[EventBus] 取消订阅 {eventType.Name} 事件，剩余订阅数: {handlers.Count}");
                }
            }
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventData">事件数据</param>
        public static void Publish<T>(T eventData) where T : class
        {
            Type eventType = typeof(T);

            if (EnableLogging)
            {
                Debug.Log($"[EventBus] 发布 {eventType.Name} 事件");
            }

            if (_eventHandlers.TryGetValue(eventType, out var handlers))
            {
                // 遍历所有处理器并调用
                foreach (var handler in handlers)
                {
                    try
                    {
                        (handler as Action<T>)?.Invoke(eventData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[EventBus] 处理事件 {eventType.Name} 时出错: {e}");
                    }
                }
            }
        }

        /// <summary>
        /// 清空所有订阅（场景切换时调用）
        /// </summary>
        public static void ClearAllSubscriptions()
        {
            _eventHandlers.Clear();
            if (EnableLogging)
            {
                Debug.Log("[EventBus] 已清空所有事件订阅");
            }
        }
    }
}