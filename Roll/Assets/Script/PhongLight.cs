using UnityEngine;
using System.Collections;

public class PhongLight : MonoBehaviour {

    public Shader shader;
    public Texture2D tex;
    private PointLight pointLight;

	// Use this for initialization
	void Start () {
        pointLight = GameObject.Find("Light").GetComponent<PointLight>();
        Material mat = this.GetComponent<MeshRenderer>().material;
        mat.shader = shader;
        mat.SetTexture("_MainTex", tex);
	}
	
	// Update is called once per frame
	void Update () {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }
}
