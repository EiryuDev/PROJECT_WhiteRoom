using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WEV.WhiteRoom
{
    [System.Serializable]
    public class CharacterSerializableDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<Tkey> keys = new List<Tkey>();
        [SerializeField] private List<TValue> values = new List<TValue>();  
        
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach(KeyValuePair<Tkey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value); 
            }
        }
        
        public void OnAfterDeserialize()
        {
            Clear();

            if(keys.Count != values.Count)
            {
                Debug.LogError("You key does not match the value count, something is wrong");
            }

            for(int i = 0; i < keys.Count; i++)
            {
                Add(keys[i], values[i]);
            }
        }
    }
}
