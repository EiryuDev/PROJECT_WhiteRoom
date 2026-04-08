using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CoreSoundFXManager : MonoBehaviour
    {
        public static CoreSoundFXManager instance; // Static instance for this game object
        
        [Header("DAMAGE SOUNDS")]
        public AudioClip[] physicalDamageSFX; // Sound effect for the physical damage

        [Header("ACTION SOUNDS")]
        public AudioClip pickUpItemSFX; // Sound effect for picking up the item

        [Header("UI SOUNDS")] 
        public AudioClip hoverUISFX;
        public AudioClip confirmUISFX;
        public AudioClip cancelSFX;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);
            return array[index];
        }
        
        public AudioClip ChooseRandomFootStepSoundBasedOnGround(GameObject steppedOnObject, CharacterManager character)
        {
            if (steppedOnObject.tag == "Dirt")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footstepDirt);
            }
            else if (steppedOnObject.tag == "Sand")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footstepSand);
            }
            else if (steppedOnObject.tag == "Wood")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footstepWood);
            }
            else if (steppedOnObject.tag == "Tile")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footstepTile);
            }
            else if (steppedOnObject.tag == "Water")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footstepWater);
            }
            return null;    
        }
    }
}
