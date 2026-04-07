using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterWeaponHolderSlot : MonoBehaviour
    {
        public WeaponModelSlot weaponSlot; // Check what slot is this? (left, right, or hips or back)
        public GameObject currentWeaponModel; 

        public void UnloadWeapon()
        {
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }
        
        public void LoadWeapon(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }
    }
}
