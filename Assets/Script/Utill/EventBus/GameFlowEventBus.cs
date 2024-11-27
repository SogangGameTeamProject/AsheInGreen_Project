using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AshGreen.EventBus
{
    public enum GameFlowType
    {
        StageStart,
        StageEnd,
        GameOver,
        GameClear,
    }
    public class GameFlowEventBus : MonoBehaviour
    {
        private static readonly IDictionary<GameFlowType, UnityEvent> Events = new Dictionary<GameFlowType, UnityEvent>();

        public static void Subscribe(GameFlowType eventType, UnityAction listener)
        {
            UnityEvent thisEvent;

            if (Events.TryGetValue(eventType, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Events.Add(eventType, thisEvent);
            }
        }

        public static void Unsubscribe(GameFlowType type, UnityAction listener)
        {
            UnityEvent thisEvent;

            if (Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void Publish(GameFlowType type)
        {
            UnityEvent thisEvent;

            if (Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}