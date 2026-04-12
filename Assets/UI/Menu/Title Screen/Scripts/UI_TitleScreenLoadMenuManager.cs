using UnityEngine;

namespace WEV.WhiteRoom
{
    public class UI_TitleScreenLoadMenuManager : MonoBehaviour
    {
        PlayerControls playerControls; // Reference to the Player Controls script

        [Header("TITLE SCREEN INPUTS")]
        [SerializeField] bool deleteCharacterSlot = false; // Use to delete the character slot

        private void Update()
        {
            if (deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                UI_TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerUI.X.performed += i => deleteCharacterSlot = true;
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}
