using UnityEngine;
using TMPro;

namespace WEV.WhiteRoom
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("MESSAGE POP UP")] 
        [SerializeField] GameObject popUpMessageGameObject; // Reference to pop up message game object
        [SerializeField] TextMeshProUGUI popUpMessageText; // Reference to the text component for the pop up message

        [Header("Animated Pop Up Settings")] 
        public GameObject popUpOrganiser;
        public GameObject animatedPopUp;

        public void SendPlayerMessagePopUp(string messageText)
        {
            PlayerUIManager.instance.isPopUpWindowOpen = true;
            popUpMessageText.text = messageText;
            popUpMessageGameObject.SetActive(true);
        }

        public void CloseAllPopUpWindows()
        {
            popUpMessageGameObject.SetActive(false);
            PlayerUIManager.instance.isPopUpWindowOpen = false;
        }
    }
}
