using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 5f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwellTime = 3f;
        
        private GameObject _player;
        private Fighter _fighter;
        private Health _health;
        private Vector3 _guardLocation;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private int _currentWaypointIndex = 0;

        private void Start()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
            _player = GameObject.FindWithTag("Player");
            _guardLocation = transform.position;
        }

        private void Update()
        {
            if(_health.IsDead) return;
            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
            {
                AttackBehaviour();
            } 
            else if (_timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBBehaviour();
            }
            else
            {
                GuardBehaviour();
            }

            UpdateTimes();
        }

        private void UpdateTimes()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void GuardBehaviour()
        {
            Vector3 nextPosition = _guardLocation;

            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    _timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                _mover.StartMoveAction(nextPosition);
            }
            
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            _currentWaypointIndex = patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private bool AtWayPoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        private void SuspicionBBehaviour()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_player);
        }

        private bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position) < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}