using System.Collections.Generic;
using Interfaces;
using ScriptableObjects.Events;
using UnityEngine;

namespace MonoBehaviours.EventProducers
{
    public class SceneEventOnDisableProducer : MonoBehaviour, IEventBroadcaster
    {
        public List<SceneEvent> sceneEvents = new List<SceneEvent>();

        public void Broadcast() => sceneEvents.ForEach(sceneEvent => sceneEvent.Broadcast());
        public void OnDisable() => Broadcast();
    }
}