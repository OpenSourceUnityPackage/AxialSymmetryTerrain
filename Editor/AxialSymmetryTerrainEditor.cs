using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AST
{
    [CustomEditor(typeof(AxialSymmetryTerrain))]
    public class AxialSymmetryTerrainEditor : Editor
    {
        private AxialSymmetryTerrain m_target;
        private bool isSceneLoad = false;
        private Vector3 linePoint;
        private Vector3 lineNormal;

        // Start is called before the first frame update
        void OnEnable()
        {
            m_target = target as AxialSymmetryTerrain;
        }

        public override void OnInspectorGUI()
        {
            if (m_target.scene == null)
            {
                if (GUILayout.Button("Open scene"))
                {
                    OpenScene();
                }
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                
                if (GUILayout.Button("Left flip"))
                    UpdateLeftFlip();
                
                if (GUILayout.Button("Right flip"))
                    UpdateRightFlip();
                
                if (GUILayout.Button("Top flip"))
                    UpdateTopFlip();
                
                if (GUILayout.Button("Bottom flip"))
                    UpdateBottomFlip();
                
                GUILayout.EndVertical();

                GUI.enabled = isSceneLoad;

                if (GUILayout.Button("Export"))
                {
                    Export();
                }

                GUI.enabled = true;

                if (GUILayout.Button("Close"))
                {
                    Close();
                }

                GUILayout.EndHorizontal();
            }
        }

        private void OpenScene()
        {
            string path = EditorUtility.OpenFilePanel("Open scene", Application.dataPath, "unity");
            string relativePath;
            if (path.StartsWith(Application.dataPath))
                relativePath = "Assets" + path.Substring(Application.dataPath.Length);
            else
                relativePath = path;

            m_target.scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(relativePath);

            m_target.currentScene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
        }

        void Close()
        {
            DestroyChildren();
            EditorSceneManager.CloseScene(m_target.currentScene, true);
            m_target.scene = null;
        }

        void Export()
        {
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);

            GameObject root = new GameObject("root");
            GameObject firstScene = new GameObject("first");
            GameObject secondScene = new GameObject("second");

            firstScene.transform.parent = root.transform;
            secondScene.transform.parent = root.transform;

            // first
            GameObject[] rootGameObjects = m_target.currentScene.GetRootGameObjects();
            int rootGameObjectsCount = m_target.currentScene.rootCount;

            for (int i = 0; i < rootGameObjectsCount; i++)
            {
                rootGameObjects[i].transform.parent = firstScene.transform;
            }

            // second
            int childCount = m_target.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                m_target.transform.GetChild(0).transform.parent = secondScene.transform;
            }

            EditorSceneManager.SaveScene(newScene);
            EditorSceneManager.CloseScene(newScene, true);
            EditorSceneManager.CloseScene(m_target.currentScene, true);

            isSceneLoad = false;
        }

        private void RotateGameObjects()
        {
            GameObject[] rootGameObjects = m_target.currentScene.GetRootGameObjects();
            int rootGameObjectsCount = m_target.currentScene.rootCount;

            for (int i = 0; i < rootGameObjectsCount; i++)
            {
                GameObject currentGO = rootGameObjects[i];
                GameObject cpGO = Instantiate(currentGO, m_target.transform);
                Vector3 position = cpGO.transform.position;
                Vector3 reflect = linePoint + Vector3.Reflect(linePoint - position, lineNormal);
                position = new Vector3(reflect.x, position.y, reflect.z);
                cpGO.transform.position = position;
            }
        }
        
        #region Horizontal
        private void HorizontalFlipTerrains(Terrain limite)
        {
            Terrain[] terrains = m_target.transform.GetComponentsInChildren<Terrain>();
            foreach (Terrain terrain in terrains)
            {
                terrain.transform.position -= new Vector3(0f, 0f, limite.terrainData.size.z);
                TerrainData data = Instantiate(terrain.terrainData);
                Terrain newTerrain = terrain.GetComponent<Terrain>();
                newTerrain.terrainData = data;
                newTerrain.materialTemplate = terrain.materialTemplate;
                TerrainCollider newTerrainCollider = terrain.GetComponent<TerrainCollider>();
                newTerrainCollider.terrainData = data;
                RotateScript.HorizontalFlip(terrain);
            }
        }
        
        #region Left
        private Terrain FindLimiteLeftTerrain()
        {
            Terrain[] activeTerrains = Terrain.activeTerrains;
            Terrain limiteTerrain = activeTerrains[0];
            foreach (Terrain terrain in activeTerrains)
            {
                if (limiteTerrain.GetPosition().z > terrain.GetPosition().z)
                    limiteTerrain = terrain;
            }

            linePoint = new Vector3(0f, 0f, limiteTerrain.GetPosition().z);
            return limiteTerrain;
        }
        
        void UpdateLeftFlip()
        {
            lineNormal = Vector3.right;
            DestroyChildren();

            Terrain limite = FindLimiteLeftTerrain();
            RotateGameObjects();
            HorizontalFlipTerrains(limite);

            isSceneLoad = true;
        }
        #endregion

        #region Right
        private Terrain FindLimiteRightTerrain()
        {
            Terrain[] activeTerrains = Terrain.activeTerrains;
            Terrain limiteTerrain = activeTerrains[0];
            foreach (Terrain terrain in activeTerrains)
            {
                if (limiteTerrain.GetPosition().z < terrain.GetPosition().z)
                    limiteTerrain = terrain;
            }

            linePoint = new Vector3(0f, 0f, limiteTerrain.GetPosition().z + limiteTerrain.terrainData.size.z);
            return limiteTerrain;
        }
        
        void UpdateRightFlip()
        {
            lineNormal = Vector3.right;
            DestroyChildren();

            Terrain limite = FindLimiteRightTerrain();
            RotateGameObjects();
            HorizontalFlipTerrains(limite);

            isSceneLoad = true;
        }
        #endregion
        #endregion

        #region Vertical
        private void VerticalFlipTerrains(Terrain limite)
        {
            Terrain[] terrains = m_target.transform.GetComponentsInChildren<Terrain>();
            foreach (Terrain terrain in terrains)
            {
                terrain.transform.position -= new Vector3(limite.terrainData.size.x, 0f, 0f);
                TerrainData data = Instantiate(terrain.terrainData);
                Terrain newTerrain = terrain.GetComponent<Terrain>();
                newTerrain.terrainData = data;
                newTerrain.materialTemplate = terrain.materialTemplate;
                TerrainCollider newTerrainCollider = terrain.GetComponent<TerrainCollider>();
                newTerrainCollider.terrainData = data;
                RotateScript.VerticalFlip(terrain);
            }
        }
        
        #region Top
        private Terrain FindLimiteTopTerrain()
        {
            Terrain[] activeTerrains = Terrain.activeTerrains;
            Terrain limiteTerrain = activeTerrains[0];
            foreach (Terrain terrain in activeTerrains)
            {
                if (limiteTerrain.GetPosition().x < terrain.GetPosition().x)
                    limiteTerrain = terrain;
            }

            linePoint = new Vector3(limiteTerrain.GetPosition().x + limiteTerrain.terrainData.size.x, 0f, 0f);
            return limiteTerrain;
        }
        
        void UpdateTopFlip()
        {
            lineNormal = Vector3.forward;
            DestroyChildren();

            Terrain limite = FindLimiteTopTerrain();
            RotateGameObjects();
            VerticalFlipTerrains(limite);

            isSceneLoad = true;
        }
        

        #endregion

        #region Bottom
        private Terrain FindLimiteBottomTerrain()
        {
            Terrain[] activeTerrains = Terrain.activeTerrains;
            Terrain limiteTerrain = activeTerrains[0];
            foreach (Terrain terrain in activeTerrains)
            {
                if (limiteTerrain.GetPosition().x > terrain.GetPosition().x)
                    limiteTerrain = terrain;
            }

            linePoint = new Vector3(limiteTerrain.GetPosition().x, 0f, 0f);
            return limiteTerrain;
        }
        
        void UpdateBottomFlip()
        {
            lineNormal = Vector3.forward;
            DestroyChildren();

            Terrain limite = FindLimiteBottomTerrain();
            RotateGameObjects();
            VerticalFlipTerrains(limite);

            isSceneLoad = true;
        }
        #endregion
        #endregion
        
        private void DestroyChildren()
        {
            int childCount = m_target.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(m_target.transform.GetChild(0).gameObject);
            }
        }
    }
}