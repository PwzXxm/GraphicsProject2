using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelGenerator : MonoBehaviour
{

    public int Startinglevel = 1;
    public GameObject baseCube;
    public GameObject player;
    private GameObject curPlayer;
    private int level = 1;
    private Dictionary<int, List<Vector3>> levelmap = new Dictionary<int, List<Vector3>>();
    private Dictionary<int, Vector3[]> StartingEndingPoints = new Dictionary<int, Vector3[]>();

    // Use this for initialization
    void Start()
    {
        level = Startinglevel;
        loadData(Application.dataPath + "/data/leveldata.txt");
        loadLevel(level);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player(Clone)") && curPlayer.transform.position.y <= -20)
        {
            Destroy(this.curPlayer);
            CreatePlayer();
        }
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
        }
        CreatePlayer();
    }

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
                    if (str[0].Equals("Start"))
                    {
                        v = new Vector3(float.Parse(str[1]), float.Parse(str[2]), float.Parse(str[3]));
                        StartingEndingPoints[level][0] = v;
                    }
                    else if (str[0].Equals("End"))
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
