using UnityEngine;

namespace WEV.WhiteRoom
{
    public class PlayerInventoryManager : MonoBehaviour
    {
        [HideInInspector] public PlayerManager player;

        [Header("PLAYER DATA")]
        public ItemPlayer currentPlayerDataBeingUsed;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }
    }
}
