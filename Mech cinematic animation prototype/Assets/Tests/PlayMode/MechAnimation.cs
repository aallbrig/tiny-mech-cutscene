using System.Collections;
using System.Collections.Generic;
using Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.TestTools;
using UnityEngine.Timeline;

namespace Tests.PlayMode
{
    public class MechAnimation
    {
        private List<GameObject> GetGameObjectsWithTag(Transform parent, string tag)
        {
            var gameObjects = new List<GameObject>();
            foreach (Transform child in parent)
            {
                if (child.CompareTag(tag)) gameObjects.Add(child.gameObject);
            }
            return gameObjects;
        }

        [Test]
        public void MechAnimation_MechAnimationPrefab_APrefabCanBeLoadedFromAssetDatabase()
        {
            var mechAnimationPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mech Animation.prefab");

            Assert.NotNull(mechAnimationPrefab);
        }

        [Test]
        public void MechAnimation_CanCreateInstanceFromPrefab()
        {
            var mechAnimationPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mech Animation.prefab");

            var mechAnimation = Object.Instantiate(mechAnimationPrefab, Vector3.zero, Quaternion.identity);

            Assert.NotNull(mechAnimation);
        }

        [Test]
        public void MechAnimation_ThreeMechsAreInPrefab()
        {
            var mechAnimationPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mech Animation.prefab");

            var mechAnimation = Object.Instantiate(mechAnimationPrefab, Vector3.zero, Quaternion.identity);

            Assert.AreEqual(3, GetGameObjectsWithTag(mechAnimation.transform, "Mech").Count);
        }

        [UnityTest]
        public IEnumerator MechAnimation_ThreeMechsMoveForward()
        {
            var mechAnimationPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mech Animation.prefab");
            var mechAnimation = Object.Instantiate(mechAnimationPrefab, Vector3.zero, Quaternion.identity);
            var mechs = GetGameObjectsWithTag(mechAnimation.transform, "Mech");
            var mech1 = mechs[0];
            var mech2 = mechs[1];
            var mech3 = mechs[2];
            var mech1Position = mech1.transform.position;
            var mech2Position = mech2.transform.position;
            var mech3Position = mech3.transform.position;
            var director = mechAnimation.GetComponentInChildren<PlayableDirector>();
            director.time = 1.1f;

            director.Play();
            yield return null;

            Assert.Greater(mech1.transform.position.z, mech1Position.z);
            Assert.Greater(mech2.transform.position.z, mech2Position.z);
            Assert.Greater(mech3.transform.position.z, mech3Position.z);
        }

        [Test]
        public void MechAnimation_MechAnimationCompleteSignalExists()
        {
            var mechAnimationCompleteSignal =
                AssetDatabase.LoadAssetAtPath<SignalAsset>("Assets/Animation/Signals/Mech Animation Complete.signal");

            Assert.NotNull(mechAnimationCompleteSignal);
        }

        [UnityTest]
        public IEnumerator MechAnimation_AnimationEnd_AnEventIsBroadcast()
        {
            var signalHasReceivedCall = false;

            var mechAnimationCompleteSignal =
                AssetDatabase.LoadAssetAtPath<SignalAsset>("Assets/Animation/Signals/Mech Animation Complete.signal");
            var unityEvent = new UnityEvent();
            unityEvent.AddListener(() => signalHasReceivedCall = true);

            var mechAnimationPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mech Animation.prefab");
            var mechAnimation = Object.Instantiate(mechAnimationPrefab, Vector3.zero, Quaternion.identity);
            var director = mechAnimation.GetComponentInChildren<PlayableDirector>();

            var signalReceiver = director.gameObject.AddComponent<SignalReceiver>();
            signalReceiver.AddReaction(mechAnimationCompleteSignal, unityEvent);
            // I wish I could input one frame before so I could yield return null (let a frame go by)
            // Maybe calculate, knowing 1 second ~= 60 frames?
            var timeOffset = 0.1f;
            director.time = 13 - timeOffset;

            director.Play();
            while (director.state == PlayState.Playing) yield return null;

            Assert.True(signalHasReceivedCall);
        }
    }
}