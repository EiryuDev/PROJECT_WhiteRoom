using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractableDoor : MonoBehaviour
    {
        [Header("Status Settings")]
        public bool isOpen = false;
        private string doorID;

        [Header("Key Settings")]
        [SerializeField] private bool requiresItem = false;
        [SerializeField] private Item itemRequiredToOpen;

        [Header("Animation Settings")]
        [SerializeField] private Animator animator;
        [SerializeField] private string openAnimationAnimation;
        [SerializeField] private string openedAnimationAnimation;

        [Header("SFX Settings")]
        [SerializeField] private AudioClip doorOpeningSFX;

        private void Start()
        {
            doorID = gameObject.scene.buildIndex + " " + gameObject.name;

            
        }
    }
}
