using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CoreLocationRendererManager : MonoBehaviour
    {
        [Header("Scene I.D")]
        [HideInInspector] public int renderSceneID;

        [Header("Root GameObjects")]
        [SerializeField] public List<GameObject> rootGameObjects = new List<GameObject>();

        [Header("Mesh Renderers")]
        [SerializeField] public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        private Coroutine toggleAllMeshRenderersCoroutine;

        private void Awake()
        {
            // BELOW CODE: Gets the scene id of the scene this gameObject is placed in
            renderSceneID = gameObject.scene.buildIndex;
            CoreLocationManager.instance.AddLocationRenderManagerToList(this);
        }

        private void Start()
        {
            // When a scene is loaded into the world, you may optionally enable the gameObjects over time to help prevent possible stutters

            // If a loading screen is present, ignore the over time call and enable everything instantly
            if (PlayerUIManager.instance.playerUILoadingScreenManager.LoadingScreenIsActive())
            {
                // Enable all gameObjects
                ToggleRootObjects(true);
            }
            else
            {
                // Enable all gameObjects over time
                StartCoroutine(EnableRootGameObjectsOverTime());
            }
        }
        public void FindAllRootObjects()
        {
            rootGameObjects = new List<GameObject>();

            GameObject[] rootObjectsInScene = gameObject.scene.GetRootGameObjects();

            for (int i = 0; i < rootObjectsInScene.Length; i++)
            {
                // Do not add this object (the render manager) to the list
                if (rootObjectsInScene[i] == gameObject)
                    continue;

                if (rootGameObjects.Contains(rootObjectsInScene[i]))
                    continue;

                rootGameObjects.Add(rootObjectsInScene[i]);
            }
            // BELOW CODE: This code will only run in the editor, you need the #if for that check
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void ToggleRootObjects(bool status)
        {
            for (int i = 0; i < rootGameObjects.Count; i++)
            {
                if (rootGameObjects[i] == null)
                    continue;

                rootGameObjects[i].SetActive(status);
            }
            
            // BELOW CODE: This code will only run in the editor, you need the #if for that check
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
        private IEnumerator EnableRootGameObjectsOverTime()
        {
            for (int i = 0; i < rootGameObjects.Count; i++)
            {
                if (rootGameObjects[i] == null)
                    continue;

                rootGameObjects[i].SetActive(true);

                yield return new WaitForEndOfFrame();
            }
        }
        
        // Renderers
        public void FindAllMeshRenderers()
        {
            MeshRenderer[] allMeshRenderers = FindObjectsByType<MeshRenderer>(FindObjectsInactive.Include);

            meshRenderers = new List<MeshRenderer>();

            for (int i = 0; i < allMeshRenderers.Length; i++)
            {
                if (allMeshRenderers[i].gameObject.scene != gameObject.scene)
                    continue;

                if (!meshRenderers.Contains(allMeshRenderers[i]))
                    meshRenderers.Add(allMeshRenderers[i]);
            }
            
            // BELOW CODE: This code will only run in the editor, you need the #if for that check
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void ToggleMeshRenderers(bool status)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                if (meshRenderers[i] == null)
                    continue;

                meshRenderers[i].enabled = status;
            }
            
            // BELOW CODE: This code will only run in the editor, you need the #if for that check
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
        
        public void ToggleAllMeshRenderersOverTime(bool status)
        {
            if (toggleAllMeshRenderersCoroutine != null)
                StopCoroutine(toggleAllMeshRenderersCoroutine);

            toggleAllMeshRenderersCoroutine = StartCoroutine(
                ToggleAllMeshRenderersOverTimeCoroutine(status));
        }

        private IEnumerator ToggleAllMeshRenderersOverTimeCoroutine(bool status)
        {
            yield return new WaitForEndOfFrame();

            for (int i = 0; i < meshRenderers.Count; i++)
            {
                if (meshRenderers[i] == null)
                    continue;

                meshRenderers[i].enabled = status;
            }
        }
    }
}
