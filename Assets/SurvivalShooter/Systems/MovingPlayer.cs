using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using Zenject;
using System;
using System.Collections;
using AlphaECS;

namespace AlphaECS.SurvivalShooter
{
	public class MovingPlayer : SystemBehaviour
	{
		public readonly float MovementSpeed = 2.0f;
		private int FloorMask;

		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Initialize (eventSystem, poolManager, groupFactory);

			FloorMask = LayerMask.GetMask("Floor");

			var group = GroupFactory.Create(new Type[] { typeof(View), typeof(AxisInput), typeof(Rigidbody) });

			Observable.EveryFixedUpdate ().Subscribe (_ =>
			{
				foreach(var entity in group.Entities)
				{
					var input = entity.Get<AxisInput> ();
					input.Horizontal.Value = Input.GetAxisRaw("Horizontal");
					input.Vertical.Value = Input.GetAxisRaw("Vertical");

					var movement = Vector3.zero;
					movement.Set(input.Horizontal.Value, 0f, input.Vertical.Value);
					var speed = 6f;
					movement = movement.normalized * speed * Time.deltaTime;
					var rb = entity.Get<Rigidbody>();
					rb.MovePosition(rb.transform.position + movement);

					// execute turning
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;

					if (Physics.Raycast(ray, out hit, 1000f, FloorMask))
					{
						Vector3 playerToMouse = hit.point - rb.transform.position;
						playerToMouse.y = 0f;

						Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
						rb.MoveRotation(newRotation);
					}
				}
			}).AddTo (this);
		}
	}
}
