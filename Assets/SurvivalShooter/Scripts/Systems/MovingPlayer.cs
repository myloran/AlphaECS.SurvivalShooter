using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using System;

namespace AlphaECS.SurvivalShooter {
    public class MovingPlayer : SystemBehaviour {
        public readonly float MovementSpeed = 2.0f; //to scriptable settings
        private int FloorMask;//-

        public override void Initialize() {
            FloorMask = LayerMask.GetMask("Floor");
            Group<View, AxisInput, Rigidbody> group = GroupFactory.Create<View, AxisInput, Rigidbody>();
            Observable.EveryFixedUpdate().Subscribe(_ => {
                group.ForEach((__, ___, input, rigidbody) => {//rigidbody wtf?
                    input.Horizontal.Value = Input.GetAxisRaw("Horizontal");//extract to input system
                    input.Vertical.Value = Input.GetAxisRaw("Vertical");//-

                    var movement = new Vector3(input.Horizontal.Value, 0f, input.Vertical.Value);
                    var speed = 6f;//to settings
                    movement = movement.normalized * speed * Time.deltaTime;
                    rigidbody.MovePosition(rigidbody.transform.position + movement);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//separate system for turning?
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 1000f, FloorMask)) {
                        Vector3 rotation = hit.point - rigidbody.transform.position;
                        rotation.y = 0f;
                        rigidbody.MoveRotation(Quaternion.LookRotation(rotation));}});
            }).AddTo(this);
        }
    }
}