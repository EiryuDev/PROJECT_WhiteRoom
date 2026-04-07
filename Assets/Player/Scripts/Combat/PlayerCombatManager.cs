using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace WEV.WhiteRoom
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player; 
        
        public ItemWeapon currentWeaponBeingUsed; 

        [Header("FLAGS")]
        public bool canComboWithMainHandWeapon = false; 
        public bool isUsingRightHand = false;
        public bool isUsingLeftHand = false;
        //public bool canComboWithOffHandWeapon = false; 

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            //lockOnTransform = GetComponentInChildren<WRLD_UTILITY_LOCK_ON_TRANSFORM>().transform;
        }

        public void SetPlayerActionHand(bool rightHandedAction)
        {
            if(rightHandedAction)
            {
                isUsingLeftHand = false;
                isUsingRightHand = true;
            }
            else
            {
                isUsingRightHand = false;
                isUsingLeftHand = true;
            }
        }

        public void PerformWeaponBasedAction(WorldWeaponItemAction weaponAction, ItemWeapon weaponPerformingAction)
        {
            // BELOW CODE: Perform the action
            if(weaponPerformingAction != null)
                weaponAction.AttemptToPerformAction(player, weaponPerformingAction);
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            if (currentWeaponBeingUsed == null)
                return;

            float staminaDeducted = 0; 

            switch(currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightAttack03:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack03:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargeAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargeAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack03:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargeAttackStaminaCostMultiplier;
                    break;
                case AttackType.RunningAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.runningAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }

            Debug.Log("STAMINA DEDUCTED: " + staminaDeducted);
            player.playerStatsManager.currentStamina -= Mathf.RoundToInt(staminaDeducted);
        }
        
        // Animation Events Calls
        public override void EnableCanDoCombo()
        {
            if (isUsingRightHand)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = true;
            }
            else
            {
                // Enable off hand combo
            }
        }
        public override void DisableCanDoCombo()
        {
            player.playerCombatManager.canComboWithMainHandWeapon = false;
            //player.playerCombatManager.canComboWithOffHandWeapon = false;
        }
    }
}
