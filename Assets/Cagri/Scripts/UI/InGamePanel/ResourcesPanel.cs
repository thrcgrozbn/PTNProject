using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Cagri.Scripts.UI
{
    public class ResourcesPanel : MonoBehaviour
    {
        public TextMeshProUGUI energyCountTMP;

        public AnimationCurve energyUpdateAnimationCurve;
        public RectTransform energyRectTransform;
        private float currentEnergy;
        private int newEnergy;
        public void EnergyUpdate(int oldEnergy,int newEnergy,bool scaleAnimation=true)
        {
            currentEnergy = oldEnergy;
            this.newEnergy = newEnergy;
            if (!scaleAnimation)
            {
                energyCountTMP.text = newEnergy.ToString();
            }
            else
            {
                StartCoroutine(EnergyUpdateAnimation());
            }
        }

        private void Update()
        {
            currentEnergy = Mathf.Lerp(currentEnergy, newEnergy, Time.deltaTime*5f);

            energyCountTMP.text = currentEnergy.ToString("0.");
        }

        IEnumerator EnergyUpdateAnimation()
        {
            float timer = 0f;
           
            while (true)
            {
                timer += Time.deltaTime;
                energyRectTransform.localScale =Vector3.one* energyUpdateAnimationCurve.Evaluate(timer);
                yield return new WaitForEndOfFrame();
                if (timer>=1f)
                {
                    break;
                }
            }
        }

        

    }
}