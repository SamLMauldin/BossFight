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

        [SerializeField] ParticleSystem _collectParticles;
        [SerializeField] AudioClip _collectSound;

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Projectile collision!");
            Impact(collision);
            Feedback();
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

        private void Feedback()
        {
            //particles 
            if (_collectParticles != null)
            {
                _collectParticles = Instantiate(_collectParticles, transform.position, Quaternion.identity);
            }
            //audio. TODO - consider Object Pooling for performance
            if (_collectSound != null)
            {
                AudioHelper.PlayClip2D(_collectSound, 1f);
            }
        }
    }
}

