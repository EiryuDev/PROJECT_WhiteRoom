using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace WEV.WhiteRoom
{
    public class CoreSceneManager : MonoBehaviour
    {
        public static CoreSceneManager instance;

        // Loaded scenes
        public List<Scene> loadedScenes = new List<Scene>();

        // Do not unload
        public List<string> doNotUnloadList = new List<string>();

        // Qued Scenes
        private List<string> quedSceneIDs = new List<string>();
        private List<string> quedUnloadSceneIDs = new List<string>();
        private int quedScenesToUnload = 0;
        public int quedScenesToLoad = 0;
        private Coroutine loadingAdditiveScenesCoroutine;
        private Coroutine unloadAdditiveScenesCoroutine;

        // Loading status
        public bool sceneIsLoading = false;
        private bool sceneIsUnloading = false;

        // Scene Renderers
        private Coroutine requiredRenderersCoroutine;

        [Header("SCENE I.Ds")] 
        public string world = "World_01";
        public CoreLocationData currentLocation;

        public bool IsLoadingComplete => quedScenesToLoad <= 0 && !sceneIsLoading && !sceneIsUnloading;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            for (int i = 0; i < loadedScenes.Count; i++)
            {
                if (loadedScenes[i].IsValid())
                {
                    SceneManager.UnloadSceneAsync(loadedScenes[i]);
                }
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            loadedScenes.Add(scene);

            // Clean invalid scenes
            for (int i = loadedScenes.Count - 1; i >= 0; i--)
            {
                if (!loadedScenes[i].isLoaded)
                    loadedScenes.RemoveAt(i);
            }

            sceneIsLoading = false;
            sceneIsUnloading = false;

            CheckForRequiredRenderers();
        }

        // Scene Loading

        public void LoadWorldScene(int buildIndex)
        {
            PlayerUIManager.instance.playerUILoadingScreenManager
                .ActivateLoadingScreenUsingData(currentLocation);

            string worldScene = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            StartCoroutine(LoadWorldSceneCoroutine(worldScene));
        }

        private IEnumerator LoadWorldSceneCoroutine(string worldScene)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(worldScene, LoadSceneMode.Single);
            while (!op.isDone)
                yield return null;

            // Scene is fully loaded — now find the fresh player instance and apply data
            PlayerManager playerInScene = FindAnyObjectByType<PlayerManager>();
            if (playerInScene != null)
            {
                playerInScene.LoadGameDataToCurrentCharacterData(
                    ref CoreSaveGameManager.instance.currentCharacterData);
            }
        }

        public void LoadAdditiveScene(string sceneName)
        {
            for (int i = 0; i < loadedScenes.Count; i++)
            {
                if (loadedScenes[i] == null)
                    continue;

                if (loadedScenes[i].name.Contains(sceneName) && loadedScenes[i].isLoaded)
                    return;
            }

            sceneIsLoading = true;

            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public void LoadAdditiveScenes(List<string> scenesToLoad)
        {

            for (int i = 0; i < scenesToLoad.Count; i++)
            {
                quedSceneIDs.Add(scenesToLoad[i]);
            }

            quedScenesToLoad = quedSceneIDs.Count;

            if (loadingAdditiveScenesCoroutine != null)
                StopCoroutine(loadingAdditiveScenesCoroutine);

            loadingAdditiveScenesCoroutine = StartCoroutine(LoadAdditiveScenesCoroutine());
        }

        private IEnumerator LoadAdditiveScenesCoroutine()
        {
            float waitTime = 0.1f;

            for (int i = 0; i < quedSceneIDs.Count; i++)
            {
                if (PlayerUIManager.instance.playerUILoadingScreenManager.LoadingScreenIsActive())
                    waitTime = 0;

                while (sceneIsLoading || sceneIsUnloading)
                    yield return new WaitForSeconds(waitTime);

                if (quedSceneIDs[i] == null)
                {
                    quedScenesToLoad--;
                    continue;
                }

                LoadAdditiveScene(quedSceneIDs[i]);

                while (sceneIsLoading || sceneIsUnloading)
                    yield return new WaitForSeconds(waitTime);

                quedScenesToLoad--;

                if (quedScenesToLoad <= 0)
                    quedSceneIDs.Clear();

                yield return new WaitForFixedUpdate();
            }

            loadingAdditiveScenesCoroutine = null;
        }

        // Scene unloading

        private void UnloadAdditiveScene(string sceneName)
        {
            for (int i = 0; i < doNotUnloadList.Count; i++)
            {
                if (sceneName == doNotUnloadList[i])
                    return;
            }

            for (int i = 0; i < loadedScenes.Count; i++)
            {
                if (loadedScenes[i] == null)
                    continue;

                if (loadedScenes[i].name.Contains(sceneName) && loadedScenes[i].isLoaded)
                {
                    StartCoroutine(UnloadSceneRoutine(loadedScenes[i]));
                    break;
                }
            }
        }

        private IEnumerator UnloadSceneRoutine(Scene scene)
        {
            sceneIsUnloading = true;

            AsyncOperation op = SceneManager.UnloadSceneAsync(scene);

            while (op != null && !op.isDone)
                yield return null;

            for (int i = loadedScenes.Count - 1; i >= 0; i--)
            {
                if (!loadedScenes[i].isLoaded)
                    loadedScenes.RemoveAt(i);
            }

            sceneIsUnloading = false;
        }

        public void UnloadAdditiveScenes(List<string> sceneList)
        {
            for (int i = 0; i < sceneList.Count; i++)
            {
                quedUnloadSceneIDs.Add(sceneList[i]);
            }

            quedScenesToUnload = quedUnloadSceneIDs.Count;

            if (unloadAdditiveScenesCoroutine != null)
                StopCoroutine(unloadAdditiveScenesCoroutine);

            unloadAdditiveScenesCoroutine = StartCoroutine(UnloadAdditiveScenesCoroutine());
        }

        private IEnumerator UnloadAdditiveScenesCoroutine()
        {
            float waitTime = 1.0f;

            for (int i = 0; i < quedUnloadSceneIDs.Count; i++)
            {
                if (PlayerUIManager.instance.playerUILoadingScreenManager.LoadingScreenIsActive())
                    waitTime = 0;

                while (sceneIsLoading || sceneIsUnloading)
                    yield return new WaitForSeconds(waitTime);

                while (quedScenesToLoad > 0)
                    yield return new WaitForSeconds(waitTime);

                if (quedUnloadSceneIDs[i] == null)
                {
                    quedScenesToUnload--;
                    continue;
                }

                UnloadAdditiveScene(quedUnloadSceneIDs[i]);

                while (sceneIsLoading || sceneIsUnloading)
                    yield return new WaitForSeconds(waitTime);

                quedScenesToUnload--;

                if (quedScenesToUnload <= 0)
                    quedUnloadSceneIDs.Clear();
            }

            unloadAdditiveScenesCoroutine = null;
        }

        private IEnumerator UnloadAllAdditiveScenesNonNetwork()
        {
            for (int i = 0; i < loadedScenes.Count; i++)
            {
                if (loadedScenes[i] == null)
                    continue;

                if (!loadedScenes[i].IsValid())
                    continue;

                var op = SceneManager.UnloadSceneAsync(loadedScenes[i]);

                while (op != null && !op.isDone)
                    yield return null;
            }
        }

        public void CheckForUnrequiredScenes()
        {
            List<string> scenesToUnload = new List<string>();

            for (int i = 0; i < loadedScenes.Count; i++)
            {
                scenesToUnload.Add(loadedScenes[i].name);
            }

            doNotUnloadList = CoreLocationManager.instance.GenerateDoNotUnloadListBasedOnPlayerLocations();

            for (int i = scenesToUnload.Count - 1; i >= 0; i--)
            {
                if (doNotUnloadList.Contains(scenesToUnload[i]))
                    scenesToUnload.RemoveAt(i);
            }

            UnloadAdditiveScenes(scenesToUnload);
        }

        public void CheckForRequiredRenderers()
        {
            if (CoreLocationManager.instance == null)
                return;

            if (requiredRenderersCoroutine != null)
                StopCoroutine(requiredRenderersCoroutine);

            CoreLocationSceneSet location = PlayerUIManager.instance.player.areaCurrentlyIn;

            if (location != null)
                requiredRenderersCoroutine = StartCoroutine(
                    CheckForRequiredSceneRenderersCoroutine(location));
        }

        private IEnumerator CheckForRequiredSceneRenderersCoroutine(CoreLocationSceneSet location)
        {
            while (sceneIsLoading)
                yield return null;

            List<string> scenesRelevantToLocationCurrentlyIn =
                location.GetRequiredSceneIDsForWorldLocation();

            List<int> sceneBuildIndexes = new List<int>();

            if (scenesRelevantToLocationCurrentlyIn != null)
            {
                for (int i = 0; i < scenesRelevantToLocationCurrentlyIn.Count; i++)
                {
                    sceneBuildIndexes.Add(
                        GetBuildIndexFromSceneID(scenesRelevantToLocationCurrentlyIn[i]));
                }
            }

            for (int i = 0; i < CoreLocationManager.instance.worldLocationRenderers.Count; i++)
            {
                if (CoreLocationManager.instance.worldLocationRenderers[i] == null)
                    continue;

                if (sceneBuildIndexes.Contains(
                        CoreLocationManager.instance.worldLocationRenderers[i].renderSceneID))
                {
                    CoreLocationManager.instance.worldLocationRenderers[i]
                        .ToggleAllMeshRenderersOverTime(true);
                }
                else
                {
                    CoreLocationManager.instance.worldLocationRenderers[i]
                        .ToggleAllMeshRenderersOverTime(false);
                }
            }
        }

        public int GetBuildIndexFromSceneID(string sceneID)
        {
            return SceneUtility.GetBuildIndexByScenePath(sceneID);
        }
    }
}