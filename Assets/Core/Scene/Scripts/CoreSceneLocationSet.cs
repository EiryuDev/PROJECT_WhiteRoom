using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/Scene/World Locations")]
    public class CoreSceneLocationSet : ScriptableObject
    {
        // The scenes that this location will require to be loaded, we use a list of individual scenes
        // to make loading/unloading perform without a stutter
        [Header("Scenes Required")] 
        public List<string> scenesRequiredForThisLocation = new List<string>();

        // Other locations that need to be loaded in when this location has been loaded
        // (anywhere you can see from this location)
        [Header("Other Required Locations")] 
        [SerializeField] CoreSceneLocationSet[] requiredLocations;

        // Optional add ons (try it for fun)
        // 1. Send a pop up message when entering a new area
        // 2. Change the post processing fx when entering a new area
        // 3. Change the ambient music when entering a new area

        public List<string> GetRequiredSceneIDsForWorldLocation()
        {
            List<string> totalSceneIDsRequiredForAllLocations = new List<string>();

            // BELOW CODE: Add the scenes needed from this location
            for (int i = 0; i < scenesRequiredForThisLocation.Count; i++)
            {
                totalSceneIDsRequiredForAllLocations.Add(scenesRequiredForThisLocation[i]);
            }

            // BELOW CODE: Add the scenes needed for each location that is attached to this one (other required locations)
            for (int i = 0; i < requiredLocations.Length; i++)
            {
                List<string> sceneIDsRequiredForLocation = new List<string>();

                for (int j = 0; j < requiredLocations[i].scenesRequiredForThisLocation.Count; j++)
                {
                    if (!sceneIDsRequiredForLocation.Contains(requiredLocations[i].scenesRequiredForThisLocation[j]))
                    {
                        sceneIDsRequiredForLocation.Add(requiredLocations[i].scenesRequiredForThisLocation[j]);
                    }
                }

                for (int j = 0; j < sceneIDsRequiredForLocation.Count; j++)
                {
                    if (!totalSceneIDsRequiredForAllLocations.Contains(sceneIDsRequiredForLocation[j]))
                    {
                        totalSceneIDsRequiredForAllLocations.Add(sceneIDsRequiredForLocation[j]);
                    }
                }
            }

            return totalSceneIDsRequiredForAllLocations;
        }

        public List<string> GetDoNotUnloadListForWorldLocation()
        {
            return scenesRequiredForThisLocation;
        }
    }
}
