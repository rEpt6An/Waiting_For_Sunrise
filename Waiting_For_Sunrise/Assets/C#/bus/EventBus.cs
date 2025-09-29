using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.C_.bus
{

    /// <summary>
    /// �¼�����
    /// </summary>
    public static class EventBus
    {
        // �洢�����¼�������
        private static Dictionary<Type, List<Delegate>> _eventHandlers = new Dictionary<Type, List<Delegate>>();

        // �¼�������־���أ������ã�
        public static bool EnableLogging = false;

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="handler">�¼�������</param>
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
                Debug.Log($"[EventBus] ���� {eventType.Name} �¼�����ǰ������: {_eventHandlers[eventType].Count}");
            }
        }

        /// <summary>
        /// ȡ������
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="handler">Ҫ�Ƴ����¼�������</param>
        public static void Unsubscribe<T>(Action<T> handler) where T : class
        {
            Type eventType = typeof(T);

            if (_eventHandlers.TryGetValue(eventType, out var handlers))
            {
                handlers.Remove(handler);

                if (EnableLogging)
                {
                    Debug.Log($"[EventBus] ȡ������ {eventType.Name} �¼���ʣ�ඩ����: {handlers.Count}");
                }
            }
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="eventData">�¼�����</param>
        public static void Publish<T>(T eventData) where T : class
        {
            Type eventType = typeof(T);

            if (EnableLogging)
            {
                Debug.Log($"[EventBus] ���� {eventType.Name} �¼�");
            }

            if (_eventHandlers.TryGetValue(eventType, out var handlers))
            {
                // �������д�����������
                foreach (var handler in handlers)
                {
                    try
                    {
                        (handler as Action<T>)?.Invoke(eventData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[EventBus] �����¼� {eventType.Name} ʱ����: {e}");
                    }
                }
            }
        }

        /// <summary>
        /// ������ж��ģ������л�ʱ���ã�
        /// </summary>
        public static void ClearAllSubscriptions()
        {
            _eventHandlers.Clear();
            if (EnableLogging)
            {
                Debug.Log("[EventBus] ����������¼�����");
            }
        }
    }
}