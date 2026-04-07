using UnityEngine;

namespace WEV.WhiteRoom
{
    public class PlayerSoundFXManager : CharacterSoundFXManager
    {
        PlayerManager player; 

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }
    }
}
