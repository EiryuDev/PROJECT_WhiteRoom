using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CoreCombatWeaponFlags : MonoBehaviour
    {
        // Damage Hitboxes
        public void OpenDamageHitbox()
        {
            PlayerUIManager.instance.player.playerEquipmentManager.OpenDamageHitbox();
        }
        public void CloseDamageHitbox()
        {
            PlayerUIManager.instance.player.playerEquipmentManager.CloseDamageHitbox();
        }

        // Animation Events Calls
        public void EnableCanDoCombo()
        {
            PlayerUIManager.instance.player.playerCombatManager.EnableCanDoCombo();
        }

        public void DisableCanDoCombo()
        {
            PlayerUIManager.instance.player.playerCombatManager.DisableCanDoCombo();
        }
    }
}
