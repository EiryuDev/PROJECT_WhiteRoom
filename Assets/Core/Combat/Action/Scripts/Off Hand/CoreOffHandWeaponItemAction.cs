using UnityEngine;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/Character Actions/Weapon Actions/Off Hand Melee Action")]
    public class CoreOffHandWeaponItemAction : CoreWeaponItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction,
            ItemWeapon weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            // BELOW CODE: Check for power stance action (dual attack)

            // TO-DO: Check for can block
            //if (!playerPerformingAction.playerCombatManager.canBlock)
                //return;

            // BELOW CODE: Check for attack status
            if (playerPerformingAction.playerCombatManager.isAttacking)
            {
                // TO-DO: Disable blocking (when using a short/medium spear block attacking is allowed with light attacks. Handled on another action class)
                //playerPerformingAction.playerCombatManager.isBlocking = false;
                return;
            }

            // TO-DO
            //if (playerPerformingAction.playerCombatManager.isBlocking.Value)
                //return;
        }
    }
}
