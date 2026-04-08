using UnityEngine;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/Character Actions/Weapon Actions/Light Attack Action")]
    public class WorldLightAttackWeaponItemAction : WorldWeaponItemAction
    {
        [Header("LIGHT ATTACKS")]
        [SerializeField] string light_Attack_01 = "Main_Light_Attack_01"; 
        [SerializeField] string light_Attack_02 = "Main_Light_Attack_02"; 
        [SerializeField] string light_Attack_03 = "Main_Light_Attack_03"; 

        [Header("RUNNING ATTACKS")]
        [SerializeField] string running_Attack_01 = "Main_Run_Attack_01"; 
        
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, ItemWeapon weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            // BELOW CODE: Check for stops
            if (playerPerformingAction.playerStatsManager.currentStamina <= 0)
                return;

            //if (!playerPerformingAction.playerLocomotionManager.isGrounded)
            //    return;
            
            playerPerformingAction.playerCombatManager.isAttacking = true;

            // BELOW CODE: If player is sprinting, play running attack
            if (playerPerformingAction.playerLocomotionManager.isSprinting)
            {
                PerformRunningAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }

            PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, ItemWeapon weaponPerformingAction)
        {
            // BELOW CODE: If player is attacking currently, and we can combo, perform the combo attack
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                // BELOW CODE: Perform an attack based on the previous attack we just played
                if(playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack02, light_Attack_02, true);
                }
                else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_02)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack03, light_Attack_03, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
                }
            }

            // BELOW CODE: Otherwise, if the player is not already attacking, just perform a regular attack
            else if(!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
            }
        }

        private void PerformRunningAttack(PlayerManager playerPerformingAction, ItemWeapon weaponPerformingAction)
        {
            // TO-DO: If player is two-handing weapon, perform a two handed running attack        
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningAttack01, running_Attack_01, true);
        }
    }
}
