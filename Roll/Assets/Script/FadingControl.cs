using UnityEngine;
using System.Collections;

public class FadingControl : MonoBehaviour {
    public Texture2D tex;
    public float speed = 0.5f;

    private float alpha = 1;
    private float fading_dir = -1;

    void OnGUI()
    {
        alpha += fading_dir * speed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = -1024;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);
    }

    public float BeginFade(int dir)
    {
        fading_dir = dir;
        return speed;
    }
}
