using System;
using System.Collections;
using TMPro;
using UnityEngine;
using WEV.WhiteRoom;

namespace WEV.WhiteRoom
{
    public class UI_AnimatedPopUp : MonoBehaviour
    {
        [Header("SKILL DATA")] 
        public float destroyAfterTime = 2.5f;
        
        private AudioSource audioSource;
        private TextMeshProUGUI animatedPopUpText;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            animatedPopUpText = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        public IEnumerator ShowAnimatedPopUp(string item, int itemAmount)
        {
            audioSource.PlayOneShot(CoreSoundFXManager.instance.pickUpItemSFX);
            animatedPopUpText.text = $"{itemAmount}x " + $"{item} added to the inventory.";
            
            yield return new WaitForSeconds(destroyAfterTime);
            
            Destroy(gameObject);
        }
    }
}
