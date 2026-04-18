using UnityEngine;

namespace WEV.WhiteRoom
{
    public class EventTriggerSceneLoad : MonoBehaviour
    {
        [Header("AREA")] 
        [SerializeField] private CoreSceneLocationSet area;

        private void OnTriggerEnter(Collider other)
        {   
            PlayerManager player = other.GetComponent<PlayerManager>();
            
            if(player == null)
                return;

            AddPlayerToArea(player);
        }

        public void AddPlayerToArea(PlayerManager player)
        {
            CoreSceneLocationManager.instance.LoadAreasBasedOnAreaCurrentlyIn(area, player);    
        }
    }
}
