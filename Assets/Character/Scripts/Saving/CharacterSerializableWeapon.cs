using UnityEngine;

namespace WEV.WhiteRoom
{
    [System.Serializable]
    public class CharacterSerializableWeapon : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;

        public ItemWeapon GetWeapon()
        {
            ItemWeapon weapon = CoreItemDatabase.Instance.GetWeaponFromSerializedData(this);
            return weapon;      
        }
        
        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
        
        }
    }
}
