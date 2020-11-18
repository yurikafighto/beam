#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System;
using System.Linq;

public class ScenesManager : EditorWindow
{
    private List<SceneData> scenesList = new List<SceneData>();
    private Vector2 scrollPosition;

    // create scenes manager menu
    [MenuItem("Tools/Scenes Manager")]
    private static void ShowWindow ()
    {
        ScenesManager window = (ScenesManager)EditorWindow.GetWindow(typeof(ScenesManager), false, "Scenes Manager");
    }

    // create adding scene to build menu
    [MenuItem("Assets/Scene/Add to build", false)]
    private static void AddToBuild()
    {
        SceneAsset sceneToAdd = (SceneAsset)Selection.activeObject;

        // check if scene to add is already in the build setting scenes list
        if (EditorBuildSettings.scenes.Any(scene => scene.path == AssetDatabase.GetAssetPath(sceneToAdd)) ) // POINT BONUS pour avoir fait en une ligne ???? :D
        {

            // warn the user
            EditorUtility.DisplayDialog("Adding scene error", "Scene has already been added.", "OK");
        }
        else
        {
            // convert EditorBuildSettings.scenes to List<>
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();
            // add the new scene
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(sceneToAdd), true) );
            // convert back to an Array and update build settings scene list
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
    }

    // menu only active for scene object
    [MenuItem("Assets/Scene/Add to build", true)]
    private static bool AddToBuildValidate()
    {
        UnityEngine.Object selectedObject = Selection.activeObject;

        if(selectedObject != null)
        {
            return selectedObject.GetType() == typeof(SceneAsset);
        }
        return false;
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();


        GUILayout.Label("Always check all the required scenes are in build.");



        // check build settings scenes

        if (GUILayout.Button("Check build"))
        {
            EditorWindow.GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
        }

        GUILayout.Label(""); 

        GUILayout.Label("Select scene to load: ");

        // refresh the scenes
        if (GUILayout.Button("Refresh Scenes"))
        {
            RefreshContent();
        }


        // start scroll view
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(100), GUILayout.Height(100));
        foreach (SceneData scene in scenesList)
        {
            // update the desired state
            scene.m_isNextActive = GUILayout.Toggle(scene.m_isActive, scene.m_sceneName);

            // take action only if actual state is different than desired state
            if ( scene.m_isActive != scene.m_isNextActive )
            {
                // open scene in additive mode
                if (scene.m_isNextActive)
                {
                    EditorSceneManager.OpenScene(scene.m_scenePath, OpenSceneMode.Additive);
                }
                // close scene
                else
                {
                    // can close scene only if there are more than 1 active scenes
                    if (EditorSceneManager.sceneCount <= 1)
                    {
                        // warn the user
                        EditorUtility.DisplayDialog("Closing scene error", "1 scene must be at least open.", "OK");
                        // update state to macth actual state
                        scene.m_isNextActive = true;
                    }
                    else
                    {
                        // close scene
                        EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByName(scene.m_sceneName), true);
                    }
                }

            }
            // update the future state as the desired state
            scene.m_isActive = scene.m_isNextActive;
            
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private void RefreshContent()
    {
        string name;
        string path;

        // cleare list first
        scenesList.Clear();
        // get all build settings scenes
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            path = scene.path;
            name = System.IO.Path.GetFileNameWithoutExtension(path);
            
            scenesList.Add(new SceneData( name, path, false, false ));
        }

        // get active scene and update status as active
        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            int index = EditorSceneManager.GetSceneAt(i).buildIndex;
            scenesList[index].m_isActive = true;
            scenesList[index].m_isNextActive = true;
        }
    }
}

public class SceneData
{
    public string m_sceneName;
    public string m_scenePath;
    public bool m_isActive, m_isNextActive;

    public SceneData(string name, string path, bool active, bool nextActive)
    {
        m_sceneName = name;
        m_scenePath = path;
        m_isActive = active;
        m_isNextActive = nextActive;
    }
}
#endif
