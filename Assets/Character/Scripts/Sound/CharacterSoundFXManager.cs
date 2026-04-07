using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        [HideInInspector] public AudioSource audioSource; // Reference to the audio source component

        [Header("FOOTSTEPS")]
        public AudioClip[] footstepSand; // List of all the footsteps audio clip
        public AudioClip[] footstepDirt; // List of all the footsteps audio clip
        public AudioClip[] footstepWood; // List of all the footsteps audio clip
        public AudioClip[] footstepTile; 
        public AudioClip[] footstepWater;
        
        [Header("DAMAGE GRUNTS")]
        [SerializeField] protected AudioClip[] damageGrunts; // List of all the damage grunts audio clip

        [Header("ATTACK GRUNTS")]
        [SerializeField] protected AudioClip[] attackGrunts; // List of all the attack grunts audio clip

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        protected virtual void Start()
        {
            
        }

        public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            audioSource.PlayOneShot(soundFX, volume);
            // BELOW CODE: Resets Pitch
            audioSource.pitch = 1;

            if(randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }

        public virtual void PlayDamageGruntSFX()
        {
            if(damageGrunts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts));
        }

        public virtual void PlayAttackGruntSFX()
        {
            if(attackGrunts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
        } 

        public virtual void PlayBlockSoundFX()
        {

        }
    }
}
