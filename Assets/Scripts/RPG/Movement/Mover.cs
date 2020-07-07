using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");
        private ActionScheduler _actionScheduler;
        private Health _health;


        private void Start()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination);
        }
        
        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.destination = destination;
        }
        
        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            _animator.SetFloat(ForwardSpeed, speed);
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
    }
}
