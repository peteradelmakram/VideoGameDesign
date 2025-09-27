#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public static class CreateEasyStartScene
{
    [MenuItem("Tools/Create EasyStart Third Person Scene")]
    public static void BuildScene()
    {
        // New empty scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "NightScene_EasyStart";

        // --- Atmosphere ---
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.025f, 0.035f, 0.07f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogColor = new Color(0.02f, 0.03f, 0.06f);
        RenderSettings.fogDensity = 0.0125f;

        // --- Moon light ---
        var moon = new GameObject("Moon Light");
        var dl = moon.AddComponent<Light>();
        dl.type = LightType.Directional;
        dl.color = new Color(0.65f, 0.75f, 1f);
        dl.intensity = 0.35f;
        dl.shadows = LightShadows.Soft;
        moon.transform.rotation = Quaternion.Euler(25f, -45f, 0f);

        // --- Ground plane ---
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(5f, 1f, 5f);
        var mat = new Material(Shader.Find("Standard")) { color = new Color(0.06f, 0.06f, 0.07f) };
        ground.GetComponent<MeshRenderer>().sharedMaterial = mat;

        // --- Place ThirdPersonController prefab ---
        var playerPrefab = FindPrefab("ThirdPersonController");
        if (playerPrefab == null)
        {
            Debug.LogError("⚠ Could not find ThirdPersonController prefab. Make sure it's in Assets/EasyStart Third Person Controller/Prefabs/");
            return;
        }
        var player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
        player.transform.position = new Vector3(0, 1.1f, 0);
        player.tag = "Player"; // required by CameraController

        // --- Place CameraController prefab ---
        var camPrefab = FindPrefab("CameraController");
        if (camPrefab == null)
        {
            Debug.LogError("⚠ Could not find CameraController prefab. Make sure it's in Assets/EasyStart Third Person Controller/Prefabs/");
            return;
        }
        var camController = (GameObject)PrefabUtility.InstantiatePrefab(camPrefab);
        camController.transform.position = new Vector3(0, 2f, -4f);

        // --- Clean extra cameras (keep only the one under CameraController) ---
        var allCams = Object.FindObjectsOfType<Camera>();
        foreach (var c in allCams)
        {
            if (!c.transform.IsChildOf(camController.transform))
            {
                Object.DestroyImmediate(c.gameObject);
            }
        }

        // --- Save scene ---
        const string folder = "Assets/Scenes";
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        string scenePath = $"{folder}/NightScene_EasyStart.unity";
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), scenePath);
        Debug.Log($"✅ Scene created at {scenePath}. Hit Play to test movement.");
    }

    private static GameObject FindPrefab(string name)
    {
        string[] guids = AssetDatabase.FindAssets($"t:Prefab {name}");
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (go != null && go.name == name)
                return go;
        }
        return null;
    }
}
#endif

