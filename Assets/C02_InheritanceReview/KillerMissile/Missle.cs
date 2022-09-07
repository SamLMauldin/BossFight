using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inheritance
{
    public class Missle : Projectile
    {
        protected override void Impact(Collision collision)
        {
            this.gameObject.SetActive(false);
        }
    }
}
