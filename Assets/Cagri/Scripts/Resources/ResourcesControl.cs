using System;
using Cagri.Scripts.UI;
using UnityEngine;

namespace Cagri.Scripts.Resources
{
    public class ResourcesControl : MonoBehaviour
    {
        public static ResourcesControl instance;
        public int energyAmount;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            EnergyUpdate(10,false);
        }

        public void EnergyUpdate(int energyValue,bool scaleAnimation=true)
        {
            if (energyValue<0)
            {
                scaleAnimation = false;
            }
            int oldEnergyAmount = energyAmount;
            energyAmount += energyValue;
            UIManager.instance.resourcesPanel.EnergyUpdate(oldEnergyAmount,energyAmount,scaleAnimation);
        }
        
        public int GetEnergyAmount()
        {
            return energyAmount;
        }
    }
}