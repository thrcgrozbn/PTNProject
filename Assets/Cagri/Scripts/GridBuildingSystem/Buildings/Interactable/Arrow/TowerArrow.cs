using System.Collections;
using Cagri.Scripts.GridBuildingSystem.Buildings.Type;
using CharacterController;
using UnityEngine;

namespace Cagri.Scripts.GridBuildingSystem.Buildings.Interactable.Arrow
{
    public class TowerArrow : MonoBehaviour
    {
        
        private void Start()
        {
            StartCoroutine(DestroyObject());
        }

        IEnumerator DestroyObject()
        {
            yield return new WaitForSeconds(1.5f);
            float timer = 0f;
            while (true)
            {
                timer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer);
                
                if (timer>=1f)
                {
                    Destroy(gameObject);
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Soldier soldier = other.GetComponentInParent<Soldier>();
            if (soldier) 
            {
                soldier.TakeDamage(2);
                Destroy(gameObject);
            }

            if (!soldier)
            {
                Buildings buildings = other.GetComponentInParent<Buildings>();
                if (buildings)
                {
                    EnemyTower eT = buildings.GetComponentInParent<EnemyTower>();
                    if (!eT)
                    {
                        buildings.TakeDamage(2);
                        Destroy(gameObject);
                    }

                }
            }
        }
    }
}