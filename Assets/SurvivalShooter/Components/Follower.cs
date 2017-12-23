using UnityEngine;
using AlphaECS.Unity;

namespace AlphaECS.SurvivalShooter {
    public class Follower : ComponentBehaviour {
        public Transform Target;
        public Vector3 Offset;
        public float Smoothing = 5f;
    }
}