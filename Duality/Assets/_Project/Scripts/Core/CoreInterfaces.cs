using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace Duality.Core
{
    public interface IDamageable
    {
        public System.Action DamagedEvent { get; set; }
        public System.Action DeathEvent { get; set; }
        public void TakeDamage(float damage);
    }
}
