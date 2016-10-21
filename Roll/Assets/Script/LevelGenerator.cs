using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{

    public int Startinglevel = 1;
    public GameObject baseCube;
    public GameObject player;
    public GameObject FinishCube;

    private GameObject curPlayer;
    private int level = 1;
    private int maxLevel;
    private Dictionary<int, List<Vector3>> levelmap;
    private Dictionary<int, Vector3[]> StartingEndingPoints;
    private List<GameObject> curObj;

    // Use this for initialization
    void Start()
    {
        curObj = new List<GameObject>();
        loadData();

        maxLevel = levelmap.Count;
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
            if (curPlayer.transform.position.Equals(new Vector3(endPos.x, endPos.y + 1.5f, endPos.z)) && !pc.isPlayerLyingDown())
            {
                if (level == maxLevel)
                    SceneManager.LoadScene("Ending");

                curPlayer.transform.position = new Vector3(endPos.x, endPos.y + 2.5f, endPos.z);
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

        if (GameObject.FindWithTag("playerTag"))
        {
            Destroy(curPlayer);
        }

        if (level < maxLevel)
        {
            level++;
        }

        yield return new WaitForSeconds(0.6f);

        loadLevel(level);

        fadingTime = GameObject.Find("Fading").GetComponent<FadingControl>().BeginFade(-1);
        yield return new WaitForSeconds(fadingTime);
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
    }


    private void loadData()
    {
        levelmap = new Dictionary<int, List<Vector3>>()
        {
            {1, new List<Vector3>() {
                new Vector3(-3,0,-1),
                new Vector3(-2,0,-1),
                new Vector3(-1,0,-1),
                new Vector3(0,0,-1),
                new Vector3(0,0,0),
                new Vector3(0,0,1),
                new Vector3(0,0,2),
                new Vector3(1,0,2),
                new Vector3(2,0,2)
            }
            },
            {2, new List<Vector3>() {
                new Vector3(0,0,0),
                new Vector3(-1,0,0),
                new Vector3(-1,0,1),
                new Vector3(-1,0,2),
                new Vector3(-2,0,1),
                new Vector3(-2,0,2),
                new Vector3(-3,0,1),
                new Vector3(-3,0,2),
                new Vector3(-4,0,1),
                new Vector3(-4,0,2),
                new Vector3(1,0,0),
                new Vector3(2,0,0),
                new Vector3(2,0,-1),
                new Vector3(2,0,-2),
                new Vector3(3,0,0),
                new Vector3(3,0,-1),
                new Vector3(3,0,-2)
            }
            }
        };
        StartingEndingPoints = new Dictionary<int, Vector3[]>()
        {
            {1, new Vector3[2]
            {
                new Vector3(-3,1.5f, -1),
                new Vector3(3,0,2)
            }
            },
            {2, new Vector3[2]
            {
                new Vector3(0,1.5f,0),
                new Vector3(-4,0,0)
            } }
        };
    }

    public int getCurrentLevel()
    {
        return level;
    }
}
