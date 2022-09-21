using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inheritance
{
    public class Missle : Projectile
    {
        protected override void Impact(Collision collision)
        {
            Health damageableOject = collision.gameObject.GetComponent<Health>();
            if(damageableOject != null)
            {
                damageableOject.TakeDamage(50);
            }
            this.gameObject.SetActive(false);
        }
    }
}
