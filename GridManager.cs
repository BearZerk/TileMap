using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridManager : EditorWindow {

    GameObject activeTile;
    List<GameObject> gridObjects = new List<GameObject>();
    GameObject activeGrid;
    //NEED TO GET THIS TO LOAD THE GRID GAMEOBJECT FROM PREFABS
    GameObject gridObject;
    Grid activeGridObjectScript;

    //Create a single gameobject for the active tile being used
    Object[] spr_Tiles;
    GameObject[] obj_Tiles;
    //Create a list of Spawned tiles
    Texture spriteTexture;

    [MenuItem("Window/Grid Manager")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<GridManager>("Grid Manager");
    } 

    void OnGUI()
    {
        //Create an array that holds all the tiles in the Resources > Prefabs folder. 
        spr_Tiles = Resources.LoadAll("Sprites/ObjectSprites");
        obj_Tiles = new GameObject[spr_Tiles.Length];
        //Loop through Object Array to Create GameObject Array
        for (int i = 0; i < spr_Tiles.Length; i++)
        {
            obj_Tiles[i] = (GameObject)spr_Tiles[i];
        }

        //Create buttons in order to be able to select an active tile to be used
        int buttonsInRow = 0;
        GUILayout.BeginHorizontal();
        if (spr_Tiles != null)
        {
            for (int i = 0; i < spr_Tiles.Length; i++)
            {
                /////THIS WORKS, ALTHOUGH IT'S HIGHLY DEPENDANT UPON HOW THE SPRITE WAS IMPORTED - SINGLE (FINE), MULTIPLE (VIEW OF ENTIRE INDEX/ATLAS OF SPRITES)
                spriteTexture = obj_Tiles[i].GetComponent<SpriteRenderer>().sprite.texture;
                if (GUILayout.Button(spriteTexture, GUILayout.MaxWidth(50), GUILayout.MaxHeight(50)))
                {
                    //PASS THE SELECTED GAMEOBJECT TO THE GRIDMANAGER
                    activeTile = obj_Tiles[i];
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

    void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();
        //Get 2D position of where the mouse is currently pointing.
        Vector2 spawnPosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
        if (true)
        {
            if (Event.current.button == 0 && activeGrid != null)
            {
                Debug.Log("NEW TILE");
                activeGridObjectScript.spawnGridValue(spawnPosition.x, spawnPosition.y, activeTile);
            }
            else if (Event.current.button == 1 && activeGrid != null)
            {
                Debug.Log("OLD TILE");
                activeGridObjectScript.destroyGridValue(spawnPosition.x, spawnPosition.y);
            }
            else if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.N)
            {
                Debug.Log("NEW GRID");
                activeGrid = Instantiate(gridObject, new Vector2(0, 0), Quaternion.identity);
                activeGridObjectScript = activeGrid.GetComponent<Grid>();
                gridObjects.Add(activeGrid);
            }
        }
        Handles.EndGUI();
    }

    void SetActiveGrid()
    {
        if (Selection.activeGameObject.name == "Grid")
        {
            activeGrid = Selection.activeGameObject;
            activeGridObjectScript = activeGrid.GetComponent<Grid>();
        }
    }

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

    private void OnEnable()
    {
        Selection.selectionChanged += SetActiveGrid;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= SetActiveGrid;
    }

}
