using UniRx;
using UnityEngine;
using AlphaECS;
using AlphaECS.Unity;
using UniRx.Triggers;
using System;

namespace AlphaECS.SurvivalShooter
{
	public class MeleeAttacking : SystemBehaviour
	{
        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);

            var group = GroupFactory.Create(new Type[] { typeof(View), typeof(Melee) });
            group.OnAdd().Subscribe(entity => {                
                var viewComponent = entity.Get<View>();
                var targetTransform = viewComponent.Transforms[0];
                var attacker = entity.Get<Melee>();
                attacker.TargetInRange = new BoolReactiveProperty();
                var collider = targetTransform.GetComponent<Collider>();

                collider.OnTriggerEnterAsObservable().Subscribe(_ => {
                    var targetView = _.GetComponent<EntityBehaviour>();
                    if (targetView == null || !targetView.Entity.Has<AxisInput>() || !targetView.Entity.Has<Health>()) return;

                    attacker.Target = targetView.Entity;
                    attacker.TargetInRange.Value = true;

                }).AddTo(collider);

                collider.OnTriggerExitAsObservable().Subscribe(_ => {
                    var targetView = _.GetComponent<EntityBehaviour>();
                    if (targetView == null || !targetView.Entity.Has<AxisInput>() || !targetView.Entity.Has<Health>()) return;

                    attacker.Target = null;
                    attacker.TargetInRange.Value = false;
                }).AddTo(collider);

                attacker.TargetInRange.DistinctUntilChanged().Subscribe(targetInRange => {
                    if (targetInRange) {
                        attacker.Attack = Observable.
                            Timer(TimeSpan.FromSeconds(0f), TimeSpan.FromSeconds(1f / attacker.AttacksPerSecond)).
                            Subscribe(_ => {
                                var attackPosition = attacker.Target.Get<View>().Transforms[0].position;
                                EventSystem.Publish(new Damaged(entity, attacker.Target, attacker.Damage, attackPosition));
                            }).AddTo(attacker.Target.Get<View>().Disposer);
                    } else attacker.Attack?.Dispose();
                }).AddTo(viewComponent.Disposer);
            }).AddTo(this);
        }
			
		public void Execute(IEntity entity)
		{
//	        timer += Time.deltaTime;
//
//	        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
//	        {
//	            Attack ();
//	        }
//
//	        if(playerHealth.currentHealth <= 0)
//	        {
//	            anim.SetTrigger ("PlayerDead");
//	        }
		}

		void Attack ()
		{
//			timer = 0f;

//	        if(playerHealth.currentHealth > 0)
//	        {
//	            playerHealth.TakeDamage (attackDamage);
//	        }
		}
	}
}
