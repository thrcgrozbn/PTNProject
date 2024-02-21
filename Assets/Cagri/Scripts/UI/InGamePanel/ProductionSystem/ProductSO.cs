using UnityEngine;

namespace Cagri.Scripts.UI.InGamePanel.ProductionSystem
{
    [CreateAssetMenu(fileName = "ProductionType", menuName = "Production", order = 0)]
    public class ProductSO : ScriptableObject
    {
        public int productPrice;
        public string productName;
        public Sprite productSprite;
        public Transform productPrefab;
        public int totalHp;
        public int totalDamage;
        public int speed;
    }
}