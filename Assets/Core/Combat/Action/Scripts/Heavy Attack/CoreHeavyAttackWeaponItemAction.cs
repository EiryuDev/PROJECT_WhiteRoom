using UnityEngine;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/Character Actions/Weapon Actions/Heavy Attack Action")]
    public class CoreHeavyAttackWeaponItemAction : CoreWeaponItemAction
    {
        [SerializeField] string heavy_Attack_01 = "Main_Heavy_Attack_01"; 
        [SerializeField] string heavy_Attack_02 = "Main_Heavy_Attack_02";  
        [SerializeField] string heavy_Attack_03 = "Main_Heavy_Attack_03"; 

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, ItemWeapon weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            // BELOW CODE: Check for stops
            if (playerPerformingAction.playerStatsManager.currentStamina <= 0)
                return;

            //if (!playerPerformingAction.playerLocomotionManager.isGrounded)
            //    return;
            
            playerPerformingAction.playerCombatManager.isAttacking = true;

            PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformHeavyAttack(PlayerManager playerPerformingAction, ItemWeapon weaponPerformingAction)
        {
            // BELOW CODE: If player is attacking currently, and we can combo, perform the combo attack
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                // BELOW CODE: Perform an attack based on the previous attack we just played
                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02, heavy_Attack_02, true);
                }
                else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_02)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack03, heavy_Attack_03, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavy_Attack_01, true);
                }
            }
            // BELOW CODE: Otherwise, if the player is not already attacking, just perform a regular attack
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavy_Attack_01, true);
            }
        }
    }
}
