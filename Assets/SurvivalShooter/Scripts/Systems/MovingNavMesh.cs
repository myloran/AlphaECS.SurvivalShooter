using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using System;
using AlphaECS;
using UnityEngine.AI;

namespace AlphaECS.SurvivalShooter {
    public class MovingNavMesh : SystemBehaviour {
        Transform Target; //why it's here?
        Health TargetHealth;//-

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            GroupFactory.Create<Health, View, NavMeshAgent>().OnAdd((__, health, ___, navMeshAgent) => {
                Observable.EveryUpdate().Subscribe(_ => {
                    if (Target == null) {//-
                        var go = GameObject.FindGameObjectWithTag("Player"); //alternative?
                        if (go == null) return;//-
                        Target = go.transform;//-
                        if (Target == null) return;//-
                        TargetHealth = Target.GetComponent<EntityBehaviour>().Entity.Get<Health>();//not good
                        if (TargetHealth == null) return;//-
                    }
                    if (health.Current.Value > 0 && TargetHealth.Current.Value > 0) navMeshAgent.SetDestination(Target.position);
                    else navMeshAgent.enabled = false;
                }).AddTo(navMeshAgent).AddTo(health);
            }).AddTo(this);
        }
    }
}