using System.Collections.Generic;
using Interfaces;
using ScriptableObjects.Events;
using UnityEngine;

namespace MonoBehaviours.EventProducers
{
    public class BroadcastGameEvents : MonoBehaviour, IEventBroadcaster
    {
        public List<GameEvent> gameEvents = new List<GameEvent>();

        public void Broadcast() => gameEvents.ForEach(gameEvent => gameEvent.Broadcast());
    }
}