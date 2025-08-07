// *******************************************************
// Copyright 2013 Daikon Forge, all rights reserved under 
// US Copyright Law and international treaties
// *******************************************************
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class OnUnityLoad
{
    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            AssetDatabase.SaveAssets();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                Debug.Log("Auto-Saving scene before entering Play mode: " + UnityEngine.SceneManagement.SceneManager.GetSceneAt(i));
                EditorSceneManager.SaveOpenScenes();
            }
        }
    }

    static OnUnityLoad()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
}
