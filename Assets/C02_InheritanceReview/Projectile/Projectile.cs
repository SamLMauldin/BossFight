using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inheritance
{
    public abstract class Projectile : MonoBehaviour
    {
        protected abstract void Impact(Collision otherCollision);

        [Header("Base Settings")]
        [SerializeField] protected float TravelSpeed = .25f;
        [SerializeField] protected Rigidbody RB;

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Projectile collision!");
            Impact(collision);
        }

        private void FixedUpdate()
        {
            Move();
        }

        protected virtual void Move()
        {
            Vector3 moveOffset = transform.forward * TravelSpeed;
            RB.MovePosition(RB.position + moveOffset);
        }
    }
}

