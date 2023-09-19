using UnityEditor;
using UnityEngine;

namespace BLINK.Tools
{
    public class MaterialTilingOffset : EditorWindow
    {
        
        private ScriptableObject scriptableObj;
        private SerializedObject serialObj;
        
        public GameObject[] gameObjectList;
        private float xOffset = 0.0313f;
        private float yOffset;

        private Material cachedMaterial;
        private Material newMaterial;
        
        private int sliderValue;
        
        
        [MenuItem("BLINK/Material Tiling Offset")]
        private static void OpenWindow()
        {
            var window = (MaterialTilingOffset) GetWindow(typeof(MaterialTilingOffset), false,"Blink Material Tiling Offset");
            window.minSize = new Vector2(300, 400);
            GUI.contentColor = Color.white;
            window.Show();
        }

        private void OnGUI()
        {
            DrawMainWindow();
        }

        private void OnEnable()
        {
            scriptableObj = this;
            serialObj = new SerializedObject(scriptableObj);
        }

        private void Update()
        {
            Repaint();
        }

        private void DrawMainWindow()
        {
            var serialProp = serialObj.FindProperty("gameObjectList");
            EditorGUILayout.PropertyField(serialProp, true);
            GUILayout.Space(10);
            if (GUILayout.Button("INITIALIZE", GUILayout.MinWidth(150), GUILayout.MinHeight(30),
                GUILayout.ExpandWidth(true)))
            {
                Init();
            }
            GUILayout.Space(10);
            
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Color", GUILayout.MaxWidth(50));
            sliderValue = EditorGUILayout.IntSlider(sliderValue, 0, 64);
            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                UpdateMaterial();
            }
            
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current X Offset");
            EditorGUILayout.FloatField(GetXOffset());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Y Offset");
            EditorGUILayout.FloatField(yOffset);
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10);
            if (GUILayout.Button("CREATE MATERIAL", GUILayout.MinWidth(150), GUILayout.MinHeight(30),
                GUILayout.ExpandWidth(true)))
            {
                CreateMaterial();
            }
            GUILayout.Space(10);
            if (GUILayout.Button("SAVE SELECTION", GUILayout.MinWidth(150), GUILayout.MinHeight(30),
                GUILayout.ExpandWidth(true)))
            {
                SaveSelection();
            }
            
            serialObj.ApplyModifiedProperties();
        }

        private void Init()
        {
            Renderer renderer = gameObjectList[0].GetComponent<Renderer>();
            if (renderer != null)
            {
                cachedMaterial = renderer.sharedMaterial;
                newMaterial = new Material(cachedMaterial);

                foreach (var weapon in gameObjectList)
                {
                    Renderer r = weapon.GetComponent<Renderer>();
                    if (r == null) continue;
                    r.sharedMaterial = newMaterial;
                }
                        
                UpdateMaterial();
            }
        }

        private float GetXOffset()
        {
            int offsetValue = sliderValue;
            if (sliderValue > 32)
            {
                offsetValue -= 32;
            }

            return xOffset * offsetValue;
        }

        private void UpdateMaterial()
        {
            if (gameObjectList.Length == 0) return;
            if (newMaterial == null) return;

            int offsetValue = sliderValue;
            if (sliderValue > 32)
            {
                yOffset = 0.5f;
                offsetValue -= 32;
            }
            else
            {
                yOffset = 0;
            }
            
            newMaterial.mainTextureOffset = new Vector2(xOffset * offsetValue, yOffset);
        }

        private void CreateMaterial()
        {
            string path = "Assets/Blink/Art/Weapons/LowPoly/MegaWeaponPack1/Materials_MWP1/";
            AssetDatabase.CreateAsset(newMaterial, path + AssetDatabase.GenerateUniqueAssetPath("New MWP1 Material") + ".mat");
        }

        private void SaveSelection()
        {
            foreach (var go in Selection.gameObjects)
            {
                if (!go.TryGetComponent(out global::BLINK.Tools.MaterialTilingOffset mto)) continue;
                mto.xOffset = GetXOffset();
                mto.yOffset = yOffset;
            }
        }
    }
}
