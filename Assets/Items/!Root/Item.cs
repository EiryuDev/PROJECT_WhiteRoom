using UnityEngine;

namespace WEV.WhiteRoom
{
    public class Item : ScriptableObject
    {
        [Header("INFO DATA")]
        [Tooltip("Name of the item")]
        public new string name;
        [Tooltip("Icon sprite of the item")]
        public Sprite icon;
        [Tooltip("Icon lore of the item")]
        [TextArea]public string lore;
    }
}
