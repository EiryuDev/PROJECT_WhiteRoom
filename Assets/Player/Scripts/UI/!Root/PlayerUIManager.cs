using UnityEngine;
using UnityEngine.SceneManagement;

namespace WEV.WhiteRoom
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance; 
        [HideInInspector] public PlayerManager player;
        [HideInInspector] public CanvasGroup canvasGroup;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;
        [HideInInspector] public PlayerUILoadingScreenManager playerUILoadingScreenManager; 
        
        [Header("UI FLAGS")] 
        public bool isMenuWindowOpen = false; 
        public bool isPopUpWindowOpen = false; 

        private void Awake()
        {
            // BELOW CODE: There can only be one instance of this script, if not and more then destory the gameobject
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            player = FindAnyObjectByType<PlayerManager>();
            canvasGroup = GetComponent<CanvasGroup>();

            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
            playerUILoadingScreenManager = GetComponentInChildren<PlayerUILoadingScreenManager>();
        }

        public void CheckForTheMenuScene()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                canvasGroup.alpha = 0;
            }
            else
            {
                canvasGroup.alpha = 1;
            }
        }
    }
}
