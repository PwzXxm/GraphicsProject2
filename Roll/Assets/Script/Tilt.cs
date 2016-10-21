using UnityEngine;
using System.Collections;

public class Tilt : MonoBehaviour
{
    public float tiltRatio;
    public float offSet;

    private float diffX;
    private float diffY;


    void Start()
    {
        diffX = 0;
        diffY = 0;
    }

    void Update()
    {
        float inX = Input.acceleration.x;
        float inY = Input.acceleration.y;

        if (Mathf.Abs(diffX + inX) < offSet)
        {
            this.transform.Translate(Input.acceleration.x, 0, 0);
            diffX += inX;
        }
        if (Mathf.Abs(diffY + inY) < offSet)
        {
            this.transform.Translate(0, Input.acceleration.y, 0);
            diffY += inY;
        }
    }
}
