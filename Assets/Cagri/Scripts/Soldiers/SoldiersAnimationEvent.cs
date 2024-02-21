using Cagri.Scripts.GridBuildingSystem.Buildings;
using CharacterController;
using UnityEngine;

namespace Cagri.Scripts.Soldiers
{
    public class SoldiersAnimationEvent : MonoBehaviour
    {
        private Soldier soldier;
        private void Awake()
        {
            soldier=GetComponentInParent<Soldier>();
        }

        public void Attack()
        {
            if (!soldier.target)
            {
                if (soldier.attack)
                {
                    soldier.attack = false;
                    soldier.animator.SetTrigger("Idle");
                }
                return;
            }

            Buildings targetBuilding = soldier.target.GetComponent<Buildings>();
            Soldier targetSoldier = soldier.target.GetComponent<Soldier>();
            if (targetBuilding)
            {
                targetBuilding.TakeDamage(soldier.damage);
                if (targetBuilding.GetCurrentHp()<=0)
                {
                    soldier.attack = false;
                    soldier.animator.SetTrigger("Idle");
                }
            }


            if (targetSoldier)
            {
                targetSoldier.TakeDamage(soldier.damage);
                if (targetSoldier.GetCurrentHp()<=0)
                {
                    soldier.attack = false;
                    soldier.animator.SetTrigger("Idle");
                }
            }
        }
    }
}