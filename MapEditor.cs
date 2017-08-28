using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Map))]

public class MapEditor : Editor {
    //Create an array for all prefabs we'll be using as tiles
    GameObject[] prefabs;
    //Create a single gameobject for the active tile being used
    GameObject selectedPrefab;
    //Create a list of Spawned tiles
    ////CONSIDER USING A 2D ARRAY PERHAPS FOR CLARITY? OR WOULD A LIST SUIT? WOULD THE LIST HOLD EMPTY OBJECTS AND WRAP AT THE POINT OF LINE-BREAK?
    List<GameObject> spawnedGO = new List<GameObject>();

    Texture prefabTexture;

    //As this is custom inspector behaviour need to update the GUI within Unity
    public override void OnInspectorGUI()
    {
        //Initially draw the normal inspector view
        DrawDefaultInspector();
        //Create an array that holds all the tiles in the Resources > Prefabs folder. 
        ////CAN BE CHANGED IN MY OWN VERSION TO PULL ITEMS FROM A DIFFERENT SECTION, WOULD STILL NEED TO BE RESOURCES > [FOLDERNAME]
        Object[] obj = Resources.LoadAll("Prefabs", typeof(GameObject));
        //Set Prefab array to the length of the number of tiles loaded in 
        prefabs = new GameObject[obj.Length];
        //For each object we find, set the prefab array value to the GameObject loaded into the obj array from the Prefab Folder
        for(int i =0; i < obj.Length; i++)
        {
            prefabs[i] = (GameObject)obj[i];
        }

        //Create buttons in order to be able to select an active tile to be used
        GUILayout.BeginHorizontal(); 
        //If our array of prefabs isn't empty
        if(prefabs != null)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                /////THIS WORKS, ALTHOUGH IT'S HIGHLY DEPENDANT UPON HOW THE SPRITE WAS IMPORTED - SINGLE (FINE), MULTIPLE (VIEW OF ENTIRE INDEX/ATLAS OF SPRITES)
                prefabTexture = prefabs[i].GetComponent<SpriteRenderer>().sprite.texture;
                if(GUILayout.Button(prefabTexture, GUILayout.MaxWidth(50), GUILayout.MaxHeight(50)))
                {
                    selectedPrefab = prefabs[i];
                    EditorWindow.FocusWindowIfItsOpen(typeof(Editor));
                }
            }   
        }
        GUILayout.EndHorizontal();
    }
}
