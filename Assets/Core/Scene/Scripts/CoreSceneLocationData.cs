using UnityEngine;
using System.Collections.Generic;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(fileName = "New Location Data", menuName = "White Room/Scene/Location Data")]
    public class CoreSceneLocationData : ScriptableObject
    {
        [Tooltip("The name of the location to display on the loading screen")]
        public string locationName;
        
        [Tooltip("List of images to randomly select from for the loading screen background")]
        public List<Sprite> locationImages;
    }
}
