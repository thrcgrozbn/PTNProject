using UnityEngine;

namespace Cagri.Scripts.GridBuildingSystem.Buildings
{
    public class SoldierHolderAroundBuilding : MonoBehaviour
    {
        public Buildings myBuildings;
        private void Start()
        {
            BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = Vector2.one*.5f;
            gameObject.layer = 2;
        }
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Buildings bT = other.GetComponentInParent<Buildings>();
            if (bT)
            {
                if (bT==myBuildings)
                {
                    return;
                }
                myBuildings.soldierHolderAroundBuildingList.Remove(transform);
                Destroy(gameObject);
            }
        }
    }
}