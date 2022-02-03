using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : EditorWindow
{
    /// <summary>
    /// I should add color field for circle and also create circle on the childPosition of any tube the will affect 
    /// ordinarily and preciously like a ball in the real world
    /// On the other hand, I should make it possible for tube work very clearly and very accurately thank to myself I should do everything very easily.
    /// </summary>
    private GameMode gameMode;
    private GameObject tubePrefab;
    private GameObject circlePrefab;
    private int numberOfTube;
    private GameObject[] tubeArray;
    private List<Color> colorsforCircle = new List<Color>();
    private Camera mainCamera;
    private bool generator = false;

    [MenuItem("Level Generating/ Generator")]
    public static void OpenWindow()
    {
        GetWindow<LevelGenerator>();
    }

    
    private void OnGUI()
    {
        tubePrefab = EditorGUILayout.ObjectField("Tube Prefab", tubePrefab, typeof(GameObject), true) as GameObject;
        circlePrefab = EditorGUILayout.ObjectField("Circle Prefab", circlePrefab, typeof(GameObject), true) as GameObject;
        numberOfTube = EditorGUILayout.IntField("Number Of Tube", numberOfTube); //should input even number for creating in 2 row.
        generator = EditorGUILayout.Toggle("Create before start", generator);

        if (GUILayout.Button("Generate"))
        {
            Generate();
        }
    }
    
    public void Generate()
    {
        Debug.Log("Grid generator Called!");

        GameObject obj = GameObject.FindGameObjectWithTag("GameMode");
        if (!obj)
        {
            gameMode = new GameObject("GameMode").AddComponent<GameMode>();
            gameMode.tag = "GameMode";
        }
        else
        {
            gameMode = obj.GetComponent<GameMode>();
        }

        gameMode.SetupGrid(tubePrefab ,circlePrefab, numberOfTube, generator);
    }


   /* public void Generate()
    {
        tubeArray = new GameObject[numberOfTube];
        Debug.Log("It has generated");
        Vector3 pos = new Vector3();
        for (int i = 0; i < numberOfTube; i++)
        {
            if (i == (numberOfTube / 2) + 1)
            {
                pos.x = 0 + .5f;
                pos.y -= 2.5f;
            }
            GameObject temp = Instantiate(tubePrefab, pos, Quaternion.identity);
            tubeArray[i] = temp;
            pos.x += 1;
            /*for (int j = 0; j < 4; j++)
            {
                GameObject tempcircle = Instantiate(circlePrefab, pos, Quaternion.identity, temp.transform);
                tempcircle.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        
    }*/

    
}
