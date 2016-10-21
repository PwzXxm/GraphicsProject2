using UnityEngine;
using System.Collections;

public class ShadowLight : MonoBehaviour {

    public Shader shader;
    public Texture2D tex;

	void Start () {
        Material mat = this.GetComponent<MeshRenderer>().material;
        mat.shader = shader;
        mat.SetTexture("_MainTex", tex);
	}
}
