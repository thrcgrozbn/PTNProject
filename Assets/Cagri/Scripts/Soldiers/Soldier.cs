using System;
using Cagri.Scripts.GenericSystems.Pathfinding;
using Cagri.Scripts.Soldiers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CharacterController
{
    public class Soldier:MonoBehaviour
    {
        [HideInInspector]public int maxHp;
        [HideInInspector]public int damage;
        [HideInInspector]public int currentHealth;
        [HideInInspector]public Animator animator;
        [HideInInspector] public int speed;
        public Image healthBarFilled;

        [HideInInspector]public bool dead;
        public Transform target;
        public bool attack;
        private void Awake()
        {
            animator=GetComponentInChildren<Animator>();
        }
        
        public Soldier(int maxHp, int damage,int speed)
        {
            this.maxHp = maxHp;
            currentHealth = maxHp;
            this.damage = damage;
            this.speed = speed;
        }

        public void Attack(Transform target)
        {
            this.target = target;
            animator.SetTrigger("Attack");
        }
        
        public int GetCurrentHp()
        {
            return currentHealth;
        }
        
        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHp);
            HealthBarControl();
            if (currentHealth<=0)
            {
                Die();
            }
        }
        
        public void HealthBarControl()
        {
            healthBarFilled.fillAmount = Mathf.InverseLerp(0f, maxHp, currentHealth);
        }

        private void Die()
        {
            GetComponent<PathfindingMovement>().GetNode().ClearSoldier();
            dead = true;
            healthBarFilled.transform.parent.gameObject.SetActive(false);
            animator.SetTrigger("Die");
            GetComponentInChildren<Collider2D>().enabled = false;
            Destroy(gameObject,2f);
        }
    }
}