using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    private int numberOfTube;
    [SerializeField]
    private GameObject circlePrefab;
    [SerializeField]
    private GameObject tubePrefab;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text levelCounterTxt;
    [SerializeField]
    private bool doGenrate = false;
    [SerializeField]
    private GameObject pnlGameOver;

    private float staroPos = 0.0f;
    private List<Color> useableColor = new List<Color>();
    private GameObject[] tubes;
    public static int Score { get; private set; }
    public static int numberoffullTube;
    public static int LevelCounter { get; set; } 
    [SerializeField]
    private Color[] savedColors = new Color[14];/*{ Color.black, Color.blue, Color.yellow, Color.grey, Color.red, Color.green, Color.cyan, Color.white }; */

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Level") != 1 && PlayerPrefs.GetInt("Level") > 1)
        {
            LevelCounter = PlayerPrefs.GetInt("Level");
        }
        else /*if (PlayerPrefs.GetInt("Level") < 1)*/
        {
            LevelCounter = 1;
        }
        pnlGameOver.SetActive(false);
        Score = PlayerPrefs.GetInt("Score");
        //numberoffullTube = numberOfTube - 2;
        if (doGenrate)
        {
            GenerateTube(LevelCounter);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        levelCounterTxt.text = string.Format("LeveL: {0}", LevelCounter);
        scoreText.text = string.Format("Score: {0}" , Score);
    }
    public void SetupGrid(GameObject tubeObj, GameObject circleObj, int num, bool shouldGenerate)
    {
        this.numberOfTube = num;
        this.tubePrefab = tubeObj;
        this.circlePrefab = circleObj;
        this.doGenrate = shouldGenerate;
        //numberoffullTube = num - 2;
        if (!shouldGenerate)
        {
            GenerateTube(numberOfTube);
            //numberoffullTube = num - 2;
        }

    }


    private void GenerateTube(int lvl)
    {
        PlayerPrefs.SetInt("Level", LevelCounter);
        tubes = new GameObject[LevelIdentifier(lvl)];
        numberOfTube = tubes.Length;

        //for 3 and 4 tubes it's like manually
        if (numberOfTube == 3)
        {
            numberoffullTube = numberOfTube - 1;
        }
        else if (numberOfTube == 4)
        {
            numberoffullTube = numberOfTube - 1;
        }
        else
        {
            numberoffullTube = numberOfTube - 2;
        }

        //for startpos to be in the center of the camera.
        if (numberOfTube % 2 == 0)
        {
            Debug.Log("the number is odd.");
            staroPos = (float)((float)numberOfTube / 2) / 2 - .5f;
        }
        else if (numberOfTube % 2 == 1)
        {
            Debug.Log("The number is even.");
            staroPos = (float)(((float)numberOfTube / 2) / 2) - .25f;
        }
        Vector3 pos = new Vector3(-staroPos, 1);

        for (int i = 0; i < tubes.Length; i++)
        {

            if (numberOfTube % 2 == 1 && i == (numberOfTube / 2) + 1)
            {
                pos.x = -staroPos + .5f;
                pos.y -= 2.5f;
            }
            else if (numberOfTube %2 == 0 && i == numberOfTube / 2)
            {
                pos.x = -staroPos ;
                pos.y -= 2.5f;
            }
            GameObject temp = Instantiate(tubePrefab, pos, Quaternion.identity);
            tubes[i] = temp;
            pos.x += 1.0f;
        }

        int index = 0;
        List<Color> selectedColors = new List<Color>();

        for (int i = 0; i < savedColors.Length; i++)
        {
            useableColor.Add(savedColors[i]);
        }
        //Debug.Log("useable colors length is: " + useableColor.Count);
        for (int i = 0; i < numberoffullTube; i++)
        {
            index = Random.Range(0, useableColor.Count);
            if (!selectedColors.Contains(useableColor[index]))
            {
                //counter++;
                for (int j = 0; j < 4; j++)
                {
                    selectedColors.Add(useableColor[index]);
                }

                useableColor.RemoveAt(index);
            }
        }

        //Debug.Log("Selected colors Count is: " + selectedColors.Count);
        for (int i = 0; i < numberoffullTube; i++)
        {
            Vector3 position = tubes[i].transform.GetChild(0).position;
            float yTemp = 1.65f;
            for (int j = 0; j < 4; j++)
            {
                int n = Random.Range(0, selectedColors.Count);
                GameObject temp = Instantiate(circlePrefab, new Vector3(position.x, position.y - yTemp), Quaternion.identity);
                temp.GetComponent<Ball>().SetColor(selectedColors[n]);
                selectedColors.RemoveAt(n);
                yTemp -= .32f;
            }
        }
        useableColor.RemoveRange(0, useableColor.Count);
        //mainCamera.orthographicSize = 7;
        
    }

    private int LevelIdentifier(int lvl)
    {
        if (lvl == 0)
        {
            return 3;
        }
        else if (lvl > 30)
        {
            return 12;
        }
        if (lvl % 3 == 0)
        {
            return lvl / 3 + 2;
        }
        else if (lvl % 3 != 0)
        {
            return lvl / 3 + 3; 
        }
        return 0;
    }

    public void NextLevel()
    {
        LevelCounter++;
        PlayerPrefs.SetInt("Level", LevelCounter);
        DeleteSceneObj();
        GenerateTube(LevelCounter);
        pnlGameOver.SetActive(false);

    }

    public void DeleteSceneObj()
    {
        Ball[] tempBall = FindObjectsOfType<Ball>();
        Tube[] tubeTemp = FindObjectsOfType<Tube>();
        for (int i = 0; i < tempBall.Length; i++)
        {
            Destroy(tempBall[i].gameObject);
        }
        for (int j = 0; j < tubeTemp.Length; j++)
        {
            Destroy(tubeTemp[j].gameObject);
        }
        //Debug.Log("Ball in the Scene is: " + tempBall.Length);
        //Debug.Log("Tube in the Scene is: " + tubeTemp.Length);
    }

    public void AddScore(int score)
    {
        Score += score;
        PlayerPrefs.SetInt("Score", Score);
    }

    public void Restart()
    {
        DeleteSceneObj();
        GenerateTube(LevelCounter);
    }

    public void GameOver()
    {
        //Debug.Log("Before decreasing: " + numberoffullTube);
        numberoffullTube -= 1;
        //Debug.Log("after decreasing: " + numberoffullTube);

        if (numberoffullTube <= 0)
        {
            pnlGameOver.SetActive(true);
            //Debug.Log("We should call game over Panel or load next scene to play.");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }


}
