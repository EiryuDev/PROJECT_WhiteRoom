using UnityEngine;

namespace WEV.WhiteRoom
{
    public class WorldEnums : MonoBehaviour
    {
    }
    
    public enum WorldAreaLocation { 
        Gameplay_01
    }

    public enum ItemType { 
        // Regular items are used as pickup, consumables
        RegularItem, 
        // Key item are specific quest item which can't be removed
        KeyItem 
    }

    public enum ItemPickUpType { 
        World, 
        Drop 
    }

    public enum InteractableType {
     Item, 
     NPC 
    }
}
