using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WEV.WhiteRoom
{
    public class PlayerUILoadingScreenManager : MonoBehaviour
    {
        [Header("LOADING SCREEN DATA")]
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private CanvasGroup canvasGroup;
        private Coroutine fadeLoadingScreenCoroutine;
        
        [Header("LOADING SCREEN UI COMPONENTS")]
        [Tooltip("The Image component that displays the loading screen background")]
        [SerializeField] private Image locationImage;
        [Tooltip("The TextMeshProUGUI component that displays the location name")]
        [SerializeField] private TextMeshProUGUI locationName;

        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        private void OnSceneChanged(Scene argo0, Scene argo1)
        {
            DeactivateLoadingScreen();
        }
        public void ActivateLoadingScreen()
        {
            // BELOW CODE: If loading screen is already active, then return
            if(loadingScreen.activeSelf)
                return;
            
            // BELOW CODE: If the loading screen is in process of deactivating, cancel it
            
            canvasGroup.alpha = 1;
            loadingScreen.SetActive(true);
        }
        public void ActivateLoadingScreenUsingData(WorldLocationData locationData)
        {
            // BELOW CODE: If loading screen is already active, then return
            if(loadingScreen.activeSelf)
                return;
            
            // Update loading screen UI with location data
            if (locationData != null)
            {
                // Update the location name text
                if (locationName != null)
                {
                    locationName.text = locationData.locationName;
                }
                
                // Choose a random image from the location images list
                if (locationImage != null && locationData.locationImages != null && locationData.locationImages.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, locationData.locationImages.Count);
                    locationImage.sprite = locationData.locationImages[randomIndex];
                }
            }
            
            canvasGroup.alpha = 1;
            loadingScreen.SetActive(true);
        }

        public void DeactivateLoadingScreen(float delay = 1)
        {
            if(loadingScreen == null)
                return;
            
            if(this == null)
                return;
            
            // BELOW CODE: If loading screen is not active, then return
            if(!loadingScreen.activeSelf)
                return;
            
            // BELOW CODE: If we are already fading away, then the loading screen returns
            if(fadeLoadingScreenCoroutine != null)
                return;

            fadeLoadingScreenCoroutine = StartCoroutine(FadeLoadingScreen(1, delay));
            
            //loadingScreen.SetActive(false);
        }
        private IEnumerator FadeLoadingScreen(float duration, float delay)
        {
            //while (WRLD_AI_MANAGER.instance.isPerformingLoadingOperation)
            //{
                //yield return null;
            //}
            
            loadingScreen.SetActive(true);
            
            // BELOW CODE: Wait for all queued additive scenes to finish loading
            while (WorldSceneManager.instance != null && 
                   (WorldSceneManager.instance.quedScenesToLoad > 0 || WorldSceneManager.instance.sceneIsLoading))
            {
                yield return null;
            }
            
            if (duration > 0)
            {
                while (delay > 0)
                {
                    delay -= Time.deltaTime;
                    yield return null; 
                }
                
                canvasGroup.alpha = 1;
                float elapsedTime = 0;

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
                    yield return null;
                }
            }
            
            canvasGroup.alpha = 0;
            loadingScreen.SetActive(false); 
            fadeLoadingScreenCoroutine = null;
            yield return null;
        }
        public bool LoadingScreenIsActive()
        {
            return loadingScreen.activeSelf;
        }
    }
}
