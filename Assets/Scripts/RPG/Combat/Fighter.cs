using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float weaponDamage = 5f;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private Health _target;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private Animator _animator;
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;
            if (_target.IsDead) return;
            
            if (!IsInRange())
            {
                _mover.MoveTo(_target.transform.position);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event
                _animator.ResetTrigger(StopAttack);
                _animator.SetTrigger(Attack1);
                _timeSinceLastAttack = 0;
                
            }
            
        }
        
        // Animation Event
        void Hit()
        {
            if (_target == null) return;
            _target.TakeDamage(weaponDamage);
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            return !combatTarget.GetComponent<Health>().IsDead;
        }

        public void Attack(GameObject target)
        {
            _actionScheduler.StartAction(this);
            _target = target.GetComponent<Health>();
        }

        public void Cancel()
        {
            _animator.ResetTrigger(Attack1);
            _animator.SetTrigger(StopAttack);
            _target = null;
        }
        
    }
}