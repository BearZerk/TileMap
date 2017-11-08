using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileManagerWindow : EditorWindow {

    //Create an array for all prefabs we'll be using as tiles
    GameObject[] prefabs;
    //Create a single gameobject for the active tile being used
    GameObject selectedPrefab;
    GameObject selectedGameObject;
    //Create a list of Spawned tiles
    ////CONSIDER USING A 2D ARRAY PERHAPS FOR CLARITY? OR WOULD A LIST SUIT? WOULD THE LIST HOLD EMPTY OBJECTS AND WRAP AT THE POINT OF LINE-BREAK?
    List<GameObject> spawnedGO = new List<GameObject>();
    Texture prefabTexture;

    private float tileDimension;
    private float displacement;
    private Vector3 clickPos;
    private int roundedWorldSpace;

    [MenuItem("Window/Tile Manager")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<TileManagerWindow>("Tile Manager");
    }

    void OnGUI()
    {
        //Create an array that holds all the tiles in the Resources > Prefabs folder. 
        ////CAN BE CHANGED IN MY OWN VERSION TO PULL ITEMS FROM A DIFFERENT SECTION, WOULD STILL NEED TO BE RESOURCES > [FOLDERNAME]
        Object[] obj = Resources.LoadAll("Prefabs", typeof(GameObject));
        //Set Prefab array to the length of the number of tiles loaded in 
        prefabs = new GameObject[obj.Length];
        //For each object we find, set the prefab array value to the GameObject loaded into the obj array from the Prefab Folder
        for (int i = 0; i < obj.Length; i++)
        {
            prefabs[i] = (GameObject)obj[i];
        }

        //Create buttons in order to be able to select an active tile to be used
        //If our array of prefabs isn't empty
        int buttonsInRow = 0;
        GUILayout.BeginHorizontal();
        if (prefabs != null)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                /////THIS WORKS, ALTHOUGH IT'S HIGHLY DEPENDANT UPON HOW THE SPRITE WAS IMPORTED - SINGLE (FINE), MULTIPLE (VIEW OF ENTIRE INDEX/ATLAS OF SPRITES)
                prefabTexture = prefabs[i].GetComponent<SpriteRenderer>().sprite.texture;
                if (GUILayout.Button(prefabTexture, GUILayout.MaxWidth(50), GUILayout.MaxHeight(50)))
                {
                    selectedPrefab = prefabs[i];
                    tileDimension = prefabs[i].GetComponent<SpriteRenderer>().bounds.size.x;
                    ////THIS DOESN'T WORK. NEED IT TO FOCUS ON THE INSPECTOR AS THE "MAP SCRIPT" COMPONENT HAS FURTHER TILES ===> Problem not here, this is at button not at create point
                    //EditorWindow.FocusWindowIfItsOpen<TileManagerWindow>();
                }
                buttonsInRow++;
                if (buttonsInRow == 3)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    buttonsInRow = 0;
                }
            }
        }
        GUILayout.EndHorizontal();
    }

    // Window has been selected
    void OnFocus()
    {
        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    void OnDestroy()
    {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        // Do your drawing here using Handles.
        Handles.BeginGUI();
        //Get 2D position of where the mouse is currently pointing.
        Vector2 spawnPosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
        //Event on user input, is it 
        if (Event.current.type == EventType.MouseDown)
        {
            //run spawn procedure
            Spawn(spawnPosition);
        }

        Handles.EndGUI();
    }

    //Create an object
    void Spawn(Vector2 _spawnPosition)
    {
        Vector2 clickPos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
        Debug.Log("X co-ordinate: " + clickPos.x);
        Debug.Log("Y co-ordinate: " + clickPos.y);
        //Object being created is instantiated, it's the selected prefab, and it's at the x & y coordinates of where was last clicked in Scene
        GameObject go = (GameObject)Instantiate(selectedPrefab, new Vector2(RoundTile(clickPos.x), RoundTile(clickPos.y)), selectedPrefab.transform.rotation);
        selectedGameObject = go;
        //Rename object
        go.name = selectedPrefab.name;
        //Add it to our list
        spawnedGO.Add(go);
    }

    float RoundTile(float toRound)
    {
        displacement = toRound % tileDimension;
        roundedWorldSpace = (int)(toRound / tileDimension);
        if (displacement > tileDimension / 2)
            return (roundedWorldSpace*tileDimension+tileDimension);
        return (roundedWorldSpace*tileDimension);
    }
}