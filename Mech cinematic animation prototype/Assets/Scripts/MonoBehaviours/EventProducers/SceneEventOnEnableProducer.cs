using System.Collections.Generic;
using ScriptableObjects.Events;
using UnityEngine;

namespace MonoBehaviours.EventProducers
{
    public class SceneEventOnEnableProducer : MonoBehaviour
    {
        public List<SceneEvent> sceneEvents = new List<SceneEvent>();

        public void Broadcast() => sceneEvents.ForEach(sceneEvent => sceneEvent.Broadcast());
        public void OnEnable() => Broadcast();
    }
}