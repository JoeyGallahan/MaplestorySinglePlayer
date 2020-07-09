/* * Event Handler heavily inspired/stolen from
 * https://stackoverflow.com/questions/42034245/unity-eventmanager-with-delegate-instead-of-unityevent */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EventManager : MonoBehaviour
{
    private static EventManager eventManager;

    private static readonly object padlock = new object();

    private Dictionary<string, Action<EventParam>> eventDictionary;

    EventManager() { }

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    private void Init()
    {
        eventDictionary = new Dictionary<string, Action<EventParam>>();
    }

    public static void AddListener(string eventType, Action<EventParam> listener)
    {
        if (instance.eventDictionary.ContainsKey(eventType))
        {
            instance.eventDictionary[eventType] += listener;
        }
        else
        {
            instance.eventDictionary.Add(eventType, listener);
        }
    }

    public static void TriggerEvent(string eventName, EventParam param)
    {
        Action<EventParam> thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
        }
    }
}
