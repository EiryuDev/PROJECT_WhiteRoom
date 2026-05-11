using System.Collections.Generic;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class PlayerHUDManager : MonoBehaviour
    {
        [Header("STAT BARS")]
        [SerializeField] private UI_StatusBar healthBar;
        [SerializeField] private UI_StatusBar staminaBar; 
        
        [Header("HUD CANVAS GROUP")]
        public List<CanvasGroup> canvasGroup = new List<CanvasGroup>();

        public void ToggleHUDWithOutPopUps(bool status)
        {
            if (status)
            {
                canvasGroup[0].alpha = 1;
            }
            else
            {
                canvasGroup[0].alpha = 0;
            }
        }

        public void ToggleHUDWithOutPopUpsBasedOnAlpha()
        {
            if (Mathf.Approximately(canvasGroup[0].alpha, 1))
            {
                canvasGroup[0].alpha = 0;
            }
            else
            {
                canvasGroup[0].alpha = 1;
            }
        }

        public void RefreshHUD() 
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);

            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }
        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(newValue);
        }

        public void SetMaxHealthValue(int maxHealth)
        {
            healthBar.SetMaxStat(maxHealth);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
    }
}