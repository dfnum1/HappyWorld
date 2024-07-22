using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TopGame.UI
{
    public static class TopGameUIExecuteEvents
    {
        public delegate bool OnEventExcudeCallback(IEventSystemHandler hander, BaseEventData pData);
        public static OnEventExcudeCallback OnEventExcude = null;
        public static T ValidateEventData<T>(BaseEventData data) where T : class
        {
            if ((data as T) == null)
                throw new ArgumentException(String.Format("Invalid type: {0} passed to event expecting {1}", data.GetType(), typeof(T)));
            return data as T;
        }

        private static void GetEventChain(GameObject root, IList<Transform> eventChain)
        {
            eventChain.Clear();
            if (root == null)
                return;

            var t = root.transform;
            while (t != null)
            {
                eventChain.Add(t);
                t = t.parent;
            }
        }

        private static readonly ObjectPool<List<IEventSystemHandler>> s_HandlerListPool = new ObjectPool<List<IEventSystemHandler>>(null, l => l.Clear());

        public static bool Execute<T>(GameObject target, BaseEventData eventData, ExecuteEvents.EventFunction<T> functor) where T : IEventSystemHandler
        {
            var internalHandlers = s_HandlerListPool.Get();
            GetEventList<T>(target, internalHandlers);
            //  if (s_InternalHandlers.Count > 0)
            //      Debug.Log("Executinng " + typeof (T) + " on " + target);

            for (var i = 0; i < internalHandlers.Count; i++)
            {
                T arg;
                try
                {
                    arg = (T)internalHandlers[i];
                }
                catch (Exception e)
                {
                    var temp = internalHandlers[i];
                    Debug.LogException(new Exception(string.Format("Type {0} expected {1} received.", typeof(T).Name, temp.GetType().Name), e));
                    continue;
                }

                try
                {
                    if(OnEventExcude== null || !OnEventExcude(arg, eventData))
                        functor(arg, eventData);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            var handlerCount = internalHandlers.Count;
            s_HandlerListPool.Release(internalHandlers);
            return handlerCount > 0;
        }

        /// <summary>
        /// Execute the specified event on the first game object underneath the current touch.
        /// </summary>
        private static readonly List<Transform> s_InternalTransformList = new List<Transform>(30);

        public static GameObject ExecuteHierarchy<T>(GameObject root, BaseEventData eventData, ExecuteEvents.EventFunction<T> callbackFunction) where T : IEventSystemHandler
        {
            GetEventChain(root, s_InternalTransformList);

            for (var i = 0; i < s_InternalTransformList.Count; i++)
            {
                var transform = s_InternalTransformList[i];
                if (Execute(transform.gameObject, eventData, callbackFunction))
                    return transform.gameObject;
            }
            return null;
        }

        private static bool ShouldSendToComponent<T>(Component component) where T : IEventSystemHandler
        {
            var valid = component is T;
            if (!valid)
                return false;

            var behaviour = component as Behaviour;
            if (behaviour != null)
                return behaviour.enabled && behaviour.isActiveAndEnabled;
            return true;
        }

        /// <summary>
        /// Get the specified object's event event.
        /// </summary>
        private static void GetEventList<T>(GameObject go, IList<IEventSystemHandler> results) where T : IEventSystemHandler
        {
            // Debug.LogWarning("GetEventList<" + typeof(T).Name + ">");
            if (results == null)
                throw new ArgumentException("Results array is null", "results");

            if (go == null || !go.activeInHierarchy)
                return;

            var components = ComponentListPool.Get();
            go.GetComponents(components);
            for (var i = 0; i < components.Count; i++)
            {
                if (!ShouldSendToComponent<T>(components[i]))
                    continue;

                // Framework.Plugin.Logger.Info(string.Format("{2} found! On {0}.{1}", go, s_GetComponentsScratch[i].GetType(), typeof(T)));
                results.Add(components[i] as IEventSystemHandler);
            }
            ComponentListPool.Release(components);
            // Debug.LogWarning("end GetEventList<" + typeof(T).Name + ">");
        }

        /// <summary>
        /// Whether the specified game object will be able to handle the specified event.
        /// </summary>
        public static bool CanHandleEvent<T>(GameObject go) where T : IEventSystemHandler
        {
            var internalHandlers = s_HandlerListPool.Get();
            GetEventList<T>(go, internalHandlers);
            var handlerCount = internalHandlers.Count;
            s_HandlerListPool.Release(internalHandlers);
            return handlerCount != 0;
        }

        /// <summary>
        /// Bubble the specified event on the game object, figuring out which object will actually receive the event.
        /// </summary>
        public static GameObject GetEventHandler<T>(GameObject root) where T : IEventSystemHandler
        {
            if (root == null)
                return null;

            Transform t = root.transform;
            while (t != null)
            {
                if (CanHandleEvent<T>(t.gameObject))
                    return t.gameObject;
                t = t.parent;
            }
            return null;
        }

        class ObjectPool<T> where T : new()
        {
            private readonly Stack<T> m_Stack = new Stack<T>();
            private readonly UnityAction<T> m_ActionOnGet;
            private readonly UnityAction<T> m_ActionOnRelease;

            public int countAll { get; private set; }
            public int countActive { get { return countAll - countInactive; } }
            public int countInactive { get { return m_Stack.Count; } }

            public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
            {
                m_ActionOnGet = actionOnGet;
                m_ActionOnRelease = actionOnRelease;
            }

            public T Get()
            {
                T element;
                if (m_Stack.Count == 0)
                {
                    element = new T();
                    countAll++;
                }
                else
                {
                    element = m_Stack.Pop();
                }
                if (m_ActionOnGet != null)
                    m_ActionOnGet(element);
                return element;
            }

            public void Release(T element)
            {
                if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
                    Framework.Plugin.Logger.Error("Internal error. Trying to destroy object that is already released to pool.");
                if (m_ActionOnRelease != null)
                    m_ActionOnRelease(element);
                m_Stack.Push(element);
            }
        }
        static class ComponentListPool
        {
            // Object pool to avoid allocations.
            private static readonly ObjectPool<List<Component>> s_ComponentListPool = new ObjectPool<List<Component>>(null, l => l.Clear());

            public static List<Component> Get()
            {
                return s_ComponentListPool.Get();
            }

            public static void Release(List<Component> toRelease)
            {
                s_ComponentListPool.Release(toRelease);
            }
        }
    }
}
