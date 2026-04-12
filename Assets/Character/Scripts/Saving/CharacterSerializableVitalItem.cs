using UnityEngine;

namespace WEV.WhiteRoom
{
    [System.Serializable]
    public class CharacterSerializableVitalItem : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;

        public ItemVital GetVitalItem()
        {
            ItemVital item = CoreItemDatabase.Instance.GetVitalItemFromSerializedData(this);
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
