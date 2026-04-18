using System.Collections;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CoreSceneLocationManager : MonoBehaviour
    {
        public static CoreSceneLocationManager instance; 
        
        [Header("LOCATION RENDERING")]
        public List<CoreSceneLocationRendererManager> worldLocationRenderers = new List<CoreSceneLocationRendererManager>();
        
        [Header("PLAYERS IN LOCATIONS")]
        private Dictionary<CoreSceneLocationSet, List<PlayerManager>> playersInLocation = new Dictionary<CoreSceneLocationSet, List<PlayerManager>>();

        [Header("PROBE VOLUME SET")] 
        [SerializeField] private ProbeVolumeBakingSet bakeSet;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;    
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public List<string> GenerateDoNotUnloadListBasedOnPlayerLocations()
        {
            List<string> doNotUnloadLocations = new List<string>();

            // BELOW CODE: The world scene is never unloaded
            doNotUnloadLocations.Add(CoreSceneManager.instance.world);

            List<CoreSceneLocationSet> areasWithPlayersActive = new List<CoreSceneLocationSet>();

            // BELOW CODE: Search each world scene with active entries
            foreach (KeyValuePair<CoreSceneLocationSet, List<PlayerManager>> pair in playersInLocation)
            {
                // BELOW CODE: Clean up null/empty
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (pair.Value[i] == null)
                        pair.Value.RemoveAt(i);
                }

                // BELOW CODE: If a scene has at least 1 player, add that scene to the active players
                if (pair.Value.Count > 0 && !areasWithPlayersActive.Contains(pair.Key))
                    areasWithPlayersActive.Add(pair.Key);
            }

            // BELOW CODE: Go through world locations that are active, and add their required scenes to the do not unload list
            for (int i = 0; i < areasWithPlayersActive.Count; i++)
            {
                List<string> scenesRequired = areasWithPlayersActive[i].GetRequiredSceneIDsForWorldLocation();

                for (int j = 0; j < scenesRequired.Count; j++)
                {
                    doNotUnloadLocations.Add(scenesRequired[j]);
                }
            }

            return doNotUnloadLocations;
        }
        public void LoadAreasBasedOnAreaCurrentlyIn(CoreSceneLocationSet areaCurrentlyIn, PlayerManager player)
        {
            // BELOW CODE: 1. Is the player currently already in the area? If so, abort so we do not reload
            if (IsPlayerAlreadyInArea(areaCurrentlyIn, player))
                return;

            // BELOW CODE: 2. Remove the player from any previous locations
            RemovePlayerFromPreviousLocation(player);
            
            // BELOW CODE: 3. Add the player to the new location
            AddPlayerToNewLocation(areaCurrentlyIn, player);
            
            // BELOW CODE: 4. Load the new scenes around the player
            LoadAdditiveScenesAroundCurrentArea(areaCurrentlyIn);
            
            // BELOW CODE: 5. Unload any unrequired scenes
            CoreSceneManager.instance.CheckForUnrequiredScenes();
            CoreSceneManager.instance.CheckForRequiredRenderers();
        }
        private bool IsPlayerAlreadyInArea(CoreSceneLocationSet area, PlayerManager player)
        {
            bool playerInArea = false;

            if (playersInLocation.ContainsKey(area) && playersInLocation[area].Contains(player))
                playerInArea = true;

            return playerInArea;
        }
        private void RemovePlayerFromPreviousLocation(PlayerManager player)
        {
            if (player == null)
                return;

            foreach (KeyValuePair<CoreSceneLocationSet, List<PlayerManager>> pair in playersInLocation)
            {
                if (pair.Value.Contains(player))
                    pair.Value.Remove(player);

                // BELOW CODE: Clean up null/empty
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (pair.Value[i] == null)
                        pair.Value.RemoveAt(i);
                }
            }
        }
        private void AddPlayerToNewLocation(CoreSceneLocationSet area, PlayerManager player)
        {
            if (player == null)
                return;
            
            // BELOW CODE: Set the baking set
            StartCoroutine(WaitThenSetActiveScene());
            
            if(!playersInLocation.ContainsKey(area))
                playersInLocation[area] =  new List<PlayerManager>();
            
            if(!playersInLocation[area].Contains(player))
                playersInLocation[area].Add(player);
            
            player.areaCurrentlyIn = area;
            
            foreach (KeyValuePair<CoreSceneLocationSet, List<PlayerManager>> pair in playersInLocation)
            {
                // BELOW CODE: Clean up null/empty
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (pair.Value[i] == null)
                        pair.Value.RemoveAt(i);
                }
            }
        }
        private void LoadAdditiveScenesAroundCurrentArea(CoreSceneLocationSet area)
        {
            List<string> scenesToLoad = new List<string>();
            
            List<CoreSceneLocationSet> worldLocations = new List<CoreSceneLocationSet>();

            scenesToLoad = area.GetRequiredSceneIDsForWorldLocation();
            
            if(scenesToLoad.Count <= 0)
                return;
            
            CoreSceneManager.instance.LoadAdditiveScenes(scenesToLoad);
        }
        private IEnumerator WaitThenSetActiveScene()
        {
            bool hasScene = false;
            while (!hasScene)
            {
                for (int i = 0; i < CoreSceneManager.instance.loadedScenes.Count; i++)
                {
                    // BELOW CODE: "World" scene is always our active scene as it is always present
                    if (CoreSceneManager.instance.loadedScenes[i].name ==
                        CoreSceneManager.instance.world)
                    {
                        hasScene = true;
                        ProbeReferenceVolume.instance.SetActiveScene(CoreSceneManager.instance.loadedScenes[i]);
                        ProbeReferenceVolume.instance.SetActiveBakingSet(bakeSet);
                    }
                    
                    yield return null;
                    
                }
                
                yield return null;
            }
        }
        
        // Scene rendering
        public void AddLocationRenderManagerToList(CoreSceneLocationRendererManager worldLocationRendererManager)
        {
            // Check for nulls as the scenes will always be loaded/unloaded
            for (int i = 0; i < worldLocationRenderers.Count; i++)
            {
                if (worldLocationRenderers[i] == null)
                    worldLocationRenderers.RemoveAt(i);
            }

            if (!worldLocationRenderers.Contains(worldLocationRendererManager))
                worldLocationRenderers.Add(worldLocationRendererManager);
        }
        
        // Toggle game mode (disables all root objects and renderers so they can be enabled as needed during gameplay)
        public void ToggleGameMode()
        {
            CoreSceneLocationRendererManager[] rendererManagers =
                FindObjectsByType<CoreSceneLocationRendererManager>();

            for (int i = 0; i < rendererManagers.Length; i++)
            {
                if (rendererManagers[i] == null)
                    continue;

                rendererManagers[i].FindAllMeshRenderers();
                rendererManagers[i].FindAllRootObjects();
                rendererManagers[i].ToggleMeshRenderers(false);
                rendererManagers[i].ToggleRootObjects(false);
            }
        }

        // Toggle light bake mode (enables all root objects and renderers so you can world build/bake lighting)
        public void ToggleLightBakeMode()
        {
            CoreSceneLocationRendererManager[] rendererManagers =
                FindObjectsByType<CoreSceneLocationRendererManager>();

            for (int i = 0; i < rendererManagers.Length; i++)
            {
                if (rendererManagers[i] == null)
                    continue;

                rendererManagers[i].ToggleMeshRenderers(true);
                rendererManagers[i].ToggleRootObjects(true);
            }
        }
    }
}
