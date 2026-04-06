using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        [HideInInspector] public CharacterManager character; // Reference to the Character Manager script

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
    }
}
