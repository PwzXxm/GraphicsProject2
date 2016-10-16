using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    public Shader shader;

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<MeshRenderer>().material.shader = shader;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
