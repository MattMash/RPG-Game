using System;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;
        private static readonly int DieTrigger = Animator.StringToHash("die");

        public bool IsDead { get; private set; } = false;

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);

            if (Math.Abs(health) < float.Epsilon)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead) return;

            IsDead = true;
            GetComponent<Animator>().SetTrigger(DieTrigger);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}