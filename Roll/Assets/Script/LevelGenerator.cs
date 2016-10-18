using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelGenerator : MonoBehaviour
{

    public int Startinglevel = 1;
    public GameObject baseCube;
    public GameObject player;
    public GameObject FinishCube;

    private GameObject curPlayer;
    private int level = 1;
    private Dictionary<int, List<Vector3>> levelmap;
    private Dictionary<int, Vector3[]> StartingEndingPoints;
    private List<GameObject> curObj;

    // Use this for initialization
    void Start()
    {
        levelmap = new Dictionary<int, List<Vector3>>();
        StartingEndingPoints = new Dictionary<int, Vector3[]>();
        curObj = new List<GameObject>();
        loadData("Assets/Data/leveldata.txt");

        level = Startinglevel;
        loadLevel(level);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("playerTag"))
        {
            if (curPlayer.transform.position.y <= -20)
            {
                Destroy(this.curPlayer);
                CreatePlayer();
            }
            Vector3 endPos = StartingEndingPoints[level][1];
            PlayerController pc = curPlayer.gameObject.GetComponent<PlayerController>();
            if (curPlayer.transform.position.Equals(new Vector3(endPos.x, endPos.y + 1.5f, endPos.z)) && !pc.isPlayerLyingDown()) {
                curPlayer.transform.position = new Vector3(endPos.x, endPos.y + 5.0f, endPos.z);
                StartCoroutine(nextLevel());
            }
        }
    }

    IEnumerator nextLevel()
    {
        yield return new WaitForSeconds(0.6f);

        float fadingTime = GameObject.Find("Fading").GetComponent<FadingControl>().BeginFade(1);
        yield return new WaitForSeconds(fadingTime);
        foreach (GameObject gm in curObj)
        {
            Destroy(gm);
        }
        curObj.Clear();

        level++;

        fadingTime = GameObject.Find("Fading").GetComponent<FadingControl>().BeginFade(-1);
        yield return new WaitForSeconds(fadingTime);
        loadLevel(level);
    }

    void CreatePlayer()
    {
        curPlayer = GameObject.Instantiate<GameObject>(player);
        curPlayer.transform.parent = this.transform;
        curPlayer.transform.position = StartingEndingPoints[level][0];
    }

    private void loadLevel(int level)
    {
        foreach (Vector3 v in levelmap[level])
        {
            GameObject cube = GameObject.Instantiate<GameObject>(baseCube);
            cube.transform.parent = this.transform;
            cube.transform.position = v;
            curObj.Add(cube);
        }

        GameObject endCube = GameObject.Instantiate<GameObject>(FinishCube);
        endCube.transform.parent = this.transform;
        endCube.transform.position = StartingEndingPoints[level][1];
        curObj.Add(endCube);

        CreatePlayer();
        curObj.Add(curPlayer);
    }


    /* trying to load data from file but this cause having error while building
    */
    private void loadData(string filename)
    {
        string line;
        Vector3 v;
        try
        {
            for (StreamReader reader = new StreamReader(filename); ;)
            {
                line = reader.ReadLine();
                if (line == null) break;
                string[] str = line.Split(',');
                if (str.Length == 1)
                {
                    level = int.Parse(str[0]);
                    levelmap.Add(level, new List<Vector3>());
                    StartingEndingPoints.Add(level, new Vector3[2]);
                }
                else if (str.Length == 3)
                {
                    v = new Vector3(float.Parse(str[0]), float.Parse(str[1]), float.Parse(str[2]));
                    levelmap[level].Add(v);
                }
                else if (str.Length == 4)
                {
                    if (str[0].Equals("S"))
                    {
                        v = new Vector3(float.Parse(str[1]), float.Parse(str[2]), float.Parse(str[3]));
                        StartingEndingPoints[level][0] = v;
                    }
                    else if (str[0].Equals("E"))
                    {
                        v = new Vector3(float.Parse(str[1]), float.Parse(str[2]), float.Parse(str[3]));
                        StartingEndingPoints[level][1] = v;
                    }
                    else
                    {
                        throw new IOException("loading Starting/Ending points fails\n");
                    }
                }
                else
                {
                    throw new IOException("loading file fails\n");
                }
            }
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
        }
    }

    public int getCurrentLevel()
    {
        return level;
    }
}
