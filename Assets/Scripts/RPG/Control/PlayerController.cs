using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Fighter _fighter;
        private Health _health;

        // Start is called before the first frame update
        void Start()
        {
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            if(_health.IsDead) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            print("Nothing todo");
            
        }

        private bool InteractWithCombat()
        {
            foreach (var raycastHit in Physics.RaycastAll(GetMouseRay()))
            {
                CombatTarget target = raycastHit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                
                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    _fighter.Attack(target.gameObject);
                }

                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            bool hasHit = Physics.Raycast(GetMouseRay(), out var hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hit.point);
                }

                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}