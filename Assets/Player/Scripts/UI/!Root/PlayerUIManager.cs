using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WEV.WhiteRoom
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;
        private AudioSource audioSource;

        [HideInInspector] public PlayerManager player;
        [HideInInspector] public CanvasGroup canvasGroup;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;
        [HideInInspector] public PlayerHUDManager playerHUDManager;
        [HideInInspector] public PlayerUILoadingScreenManager playerUILoadingScreenManager; 
        
        [Header("UI FLAGS")] 
        public bool isMenuWindowOpen = false; 
        public bool isPopUpWindowOpen = false;

        [Header("MENU DATA")]
        [SerializeField] private GameObject escapeMenu;
        [SerializeField] private Button returnButton; 

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
            playerHUDManager = GetComponentInChildren<PlayerHUDManager>();
            playerUILoadingScreenManager = GetComponentInChildren<PlayerUILoadingScreenManager>();
        }

        public void UsePauseMenu()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                bool isActive = escapeMenu.activeSelf;

                escapeMenu.SetActive(!isActive);

                if (!isActive)
                {
                    // Menu just opened
                    returnButton.Select();

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }

        public void ReturnToTitleScene()
        {
            escapeMenu.SetActive(false);
            SceneManager.LoadScene(0);
        }

        public void CloseAllMenuWindows()
        {
            escapeMenu.SetActive(false);
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

        // UI SFX
        public void PlayUnableToContinueSFX()
        {
            if (CoreSoundFXManager.instance.unableToContinueUISFX == null)
                return;

            audioSource.PlayOneShot(CoreSoundFXManager.instance.unableToContinueUISFX);
        }

        public void PlayHoverSFX()
        {
            if (CoreSoundFXManager.instance.hoverUISFX == null)
                return;

            audioSource.PlayOneShot(CoreSoundFXManager.instance.hoverUISFX);
        }

        public void PlayConfirmSFX()
        {
            if (CoreSoundFXManager.instance.confirmUISFX == null)
                return;

            audioSource.PlayOneShot(CoreSoundFXManager.instance.confirmUISFX);
        }

        public void PlayCancelSFX()
        {
            if (CoreSoundFXManager.instance.cancelSFX == null)
                return;

            audioSource.PlayOneShot(CoreSoundFXManager.instance.cancelSFX);
        }
    }
}
