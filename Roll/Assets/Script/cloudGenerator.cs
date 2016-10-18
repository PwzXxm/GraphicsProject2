using UnityEngine;
using System.Collections;

public class cloudGenerator : MonoBehaviour {
    public Shader shader;
    public Texture2D tex;
    public Material fogMaterial;

    // Use this for initialization
    void Start () {
	}

    void Update()
    {
        fogMaterial.SetFloat("_Scale", 0.8f);
        fogMaterial.SetFloat("_Intensity", 1.0f);
        fogMaterial.SetFloat("_Alpha", 1.0f);
        fogMaterial.SetFloat("_Pow", 0.15f);
        fogMaterial.SetColor("_Color", new Color(1f, 1f, 1f));
    }
}
