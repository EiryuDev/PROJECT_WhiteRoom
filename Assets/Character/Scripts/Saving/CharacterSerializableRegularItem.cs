using UnityEngine;

namespace WEV.WhiteRoom
{
    [System.Serializable]
    public class CharacterSerializableRegularItem : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;

        public ItemRegular GetRegularItem()
        {
            ItemRegular item = CoreItemDatabase.Instance.GetRegularItemFromSerializedData(this);
            return item;      
        }
        
        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
        
        }
    }
}
