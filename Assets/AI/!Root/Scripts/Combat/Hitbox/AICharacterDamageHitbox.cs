using UnityEngine;

namespace WEV.WhiteRoom
{
    public class AICharacterDamageHitbox : CoreHitboxDamage
    {
        [SerializeField] AICharacterManager aiCharacter; // Reference to the AI Character Manager script
        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponentInParent<AICharacterManager>();
            damageHitbox = GetComponent<Collider>();
        }
        protected override void DamageTarget(CharacterManager damageTarget)
        {
            // BELOW CODE: Not damaging the same target more than once in a single attack, So add them to list that checks before applying damage
            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);

            /* TO-DO: Add damage 
            CharacterTakeDamageState damageState = Instantiate(WRLD_CHARACTER_STATES_MANAGER.instance.takeDamageState);
            damageState.physicalDamage = physicalDamage;
            damageState.magicDamage = magicDamage;
            damageState.fireDamage = fireDamage;
            damageState.waterDamage = waterDamage;
            damageState.iceDamage = iceDamage;
            damageState.lightningDamage = lightningDamage;
            damageState.holyDamage = holyDamage;
            damageState.contactPoint = contactPoint;
            damageState.angleHitFrom = Vector3.SignedAngle(aiCharacter.transform.forward, damageTarget.transform.forward, Vector3.up);
            */
        }
    }
}
