using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterWeaponManager : MonoBehaviour
    {
        
        [Header("TRAIL")] 
        [SerializeField] private ParticleSystem trail;
        
        public CharacterMeleeWeaponDamageHitbox meleeDamageHitbox; // Reference to the Character Melee Damage Hitbox script

        private void Awake()
        {
            meleeDamageHitbox = GetComponentInChildren<CharacterMeleeWeaponDamageHitbox>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, ItemWeapon weapon)
        {
            if(meleeDamageHitbox == null)
                return; 
            
            // TO-DO: States Scailing Multipliers
            meleeDamageHitbox.characterCausingDamage = characterWieldingWeapon;
        }

        public void ToggleWeaponTrail(bool status)
        {
            if(trail == null)
                return;
            
            if(status)
                trail.Play();
            
            if(!status)
                trail.Stop();
        }
    }
}
