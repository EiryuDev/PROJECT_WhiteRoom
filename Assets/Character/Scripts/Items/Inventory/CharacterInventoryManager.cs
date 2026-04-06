using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        [HideInInspector] public CharacterManager character; 

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
    }
}
