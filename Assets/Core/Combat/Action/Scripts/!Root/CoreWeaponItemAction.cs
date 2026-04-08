using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/Character Actions/Weapon Actions/Test Action")]
    public class CoreWeaponItemAction : ScriptableObject
    {
        [Tooltip("ID number for the particular action.")]
        public int actionID; // ID number for the particular action

        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, ItemWeapon weaponPerformingAction)
        {
            playerPerformingAction.playerCombatManager.currentWeaponBeingUsed = weaponPerformingAction;
        }
    }
}
