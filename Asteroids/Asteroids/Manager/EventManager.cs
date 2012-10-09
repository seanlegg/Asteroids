using System;
using System.Collections.Generic;

namespace Asteroids
{
    class EventManager : Manager
    {
        private static EventManager instance;

        private Queue<Event> eventQueue;
        private Dictionary<EventType, EventSubscribers> eventSubscribers;

        private EventManager()
        {
            eventQueue = new Queue<Event>();
            eventSubscribers = new Dictionary<EventType, EventSubscribers>();
        }

        public void Publish(Event e)
        {
            eventQueue.Enqueue(e);
        }

        public void Subscribe(EventType type, Base subscriber)
        {
            if (eventSubscribers.ContainsKey(type))
            {
                EventSubscribers subscribers = eventSubscribers[type];

                // Add the subscriber
                subscribers.entries.Add(subscriber);
            } else {
                EventSubscribers subscribers = new EventSubscribers();

                // Add the subscriber
                subscribers.entries.Add(subscriber);

                // Register the EventSubscriber object
                eventSubscribers.Add(type, subscribers);
            }
        }

        public void Notify()
        {
            if (eventQueue.Count == 0 || eventSubscribers.Count == 0) return;

            while (eventQueue.Count > 0)
            {
                Event e = eventQueue.Dequeue();

                if (eventSubscribers.ContainsKey(e.EventType))
                {
                    EventSubscribers subscribers = eventSubscribers[e.EventType];

                    foreach (Base b in subscribers.entries)
                    {
                        b.OnEvent(e);
                    }
                }
            }
        }

        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventManager();
                }
                return instance;
            }
        }
    }
}
