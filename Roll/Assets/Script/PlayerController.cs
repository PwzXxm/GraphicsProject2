using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Transform point;
    private bool isLyingDown;
    private bool isLyingLeftRight;
    private bool isMoving;
    private bool[] lyingChk;        // front, back, left, right
    private Rigidbody rb;

    void Start()
    {
        isLyingDown = false;
        isLyingLeftRight = false;
        isMoving = false;
        lyingChk = new bool[4];
        point = transform.Find("RollPoint");
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        for (int i = 0; i < lyingChk.Length; i++) lyingChk[i] = false;
        Vector3 bottomPos = transform.position - Vector3.up * 1.25f;
        RaycastHit hit;
        int contactedCount = 0;


        /* check the state of cuboid using ray casting */
        if (!isMoving)
        {
            // Checking if player lying down at front
            Ray front = new Ray(bottomPos + Vector3.forward * 0.75f, Vector3.up);
            if (Physics.Raycast(front, out hit, 5.0f))
            {
                if (hit.collider.tag == "playerTag")
                {
                    lyingChk[0] = true;
                    contactedCount++;
                }
            }

            // Checking if player lying down at back
            Ray back = new Ray(bottomPos - Vector3.forward * 0.75f, Vector3.up);
            if (Physics.Raycast(back, out hit, 5.0f))
            {
                if (hit.collider.tag == "playerTag")
                {
                    lyingChk[1] = true;
                    contactedCount++;
                }
            }

            // Checking if player lying down at back
            Ray left = new Ray(bottomPos - Vector3.right * 0.75f, Vector3.up);
            if (Physics.Raycast(left, out hit, 5.0f))
            {
                if (hit.collider.tag == "playerTag")
                {
                    lyingChk[2] = true;
                    contactedCount++;
                }
            }

            // Checking if player lying down at back
            Ray right = new Ray(bottomPos + Vector3.right * 0.75f, Vector3.up);
            if (Physics.Raycast(right, out hit, 5.0f))
            {
                if (hit.collider.tag == "playerTag")
                {
                    lyingChk[3] = true;
                    contactedCount++;
                }
            }

            if (contactedCount == 2)
            {
                isLyingDown = true;
                if (lyingChk[2] && lyingChk[3]) isLyingLeftRight = true;
                else isLyingLeftRight = false;
            }
            else
            {
                isLyingDown = false;
                isLyingLeftRight = false;
            }

            /*
            Debug.DrawRay(front.origin, front.direction, Color.blue);
            Debug.DrawRay(back.origin, back.direction, Color.blue);
            Debug.DrawRay(left.origin, left.direction, Color.blue);
            Debug.DrawRay(right.origin, right.direction, Color.blue);
            */
        }

        /* check boundary condition */
        bottomPos = transform.position - Vector3.up * 0.25f;
        bool outOfBounds = true;
        //Debug.Log(isLyingDown + " " + isLyingLeftRight);
        if (!isMoving)
        {
            if (isLyingDown)
            {
                Ray ray1, ray2;
                RaycastHit hit1, hit2;
                bool bool1, bool2;
                if (isLyingLeftRight)
                {
                    ray1 = new Ray(bottomPos - Vector3.right * 0.5f, Vector3.down);
                    ray2 = new Ray(bottomPos + Vector3.right * 0.5f, Vector3.down);
                    bool1 = Physics.Raycast(ray1, out hit1, 5.0f);
                    bool2 = Physics.Raycast(ray2, out hit2, 5.0f);
                    if (bool1 && bool2 && hit1.collider.tag == "baseCube" && hit2.collider.tag == "baseCube")
                    {
                        outOfBounds = false;
                    }
                    else
                    {
                        if (bool1)
                        {
                            rb.AddForceAtPosition(Vector3.down, ray2.origin, ForceMode.Impulse);
                        }
                        else if (bool2)
                        {
                            rb.AddForceAtPosition(Vector3.down, ray1.origin, ForceMode.Impulse);
                        }
                    }
                }
                else
                {
                    ray1 = new Ray(bottomPos - Vector3.forward * 0.5f, Vector3.down);
                    ray2 = new Ray(bottomPos + Vector3.forward * 0.5f, Vector3.down);
                    bool1 = Physics.Raycast(ray1, out hit1, 5.0f);
                    bool2 = Physics.Raycast(ray2, out hit2, 5.0f);
                    if (bool1 && bool2 && hit1.collider.tag == "baseCube" && hit2.collider.tag == "baseCube")
                        outOfBounds = false;
                    else
                    {
                        if (bool1)
                            rb.AddForceAtPosition(Vector3.down, ray2.origin, ForceMode.Impulse);
                        else if (bool2)
                            rb.AddForceAtPosition(Vector3.down, ray1.origin, ForceMode.Impulse);
                    }
                }
                //Debug.DrawRay(ray1.origin, ray1.direction, Color.red);
                //Debug.DrawRay(ray2.origin, ray2.direction, Color.red);
            }
            else
            {
                Ray ray1;
                RaycastHit hit1;
                ray1 = new Ray(bottomPos, Vector3.down);
                if (Physics.Raycast(ray1, out hit1, 5.0f))
                {
                    outOfBounds = false;
                }
                else
                {
                    rb.AddForce(Vector3.down);
                }
                //Debug.DrawRay(ray1.origin, ray1.direction, Color.red);
            }
            //Debug.Log(outOfBounds + " " + isMoving);
            if (outOfBounds)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            else
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }


        }


        float x = 0.0f, y = 0.0f, z = 0.0f;

        if (Input.GetKeyDown("left") && !isMoving && !outOfBounds)
        {
            if (isLyingDown)
            {
                x = (isLyingLeftRight ? -1.0f : -0.5f);
                y = -0.5f;
            }
            else
            {
                x = -0.5f;
                y = -1.0f;
            }
            //Debug.Log(isLyingDown + " " + isLyingLeftRight);
            //Debug.Log(point.position);
            //Debug.Log(x + " " + y + " " + z);
            point.Translate(x, y, z, Space.World);
            //Debug.Log(point.position);
            StartCoroutine(Roll(point.position, Vector3.forward, 90.0f, speed));
            isMoving = true;
        }
        else if (Input.GetKeyDown("right") && !isMoving && !outOfBounds)
        {
            if (isLyingDown)
            {
                x = (isLyingLeftRight ? 1.0f : 0.5f);
                y = -0.5f;
            }
            else
            {
                x = 0.5f;
                y = -1.0f;
            }
            point.Translate(x, y, z, Space.World);
            StartCoroutine(Roll(point.position, -Vector3.forward, 90.0f, speed));
            isMoving = true;
        }
        else if (Input.GetKeyDown("up") && !isMoving && !outOfBounds)
        {
            if (isLyingDown)
            {
                z = (isLyingLeftRight ? 0.5f : 1.0f);
                y = -0.5f;
            }
            else
            {
                z = 0.5f;
                y = -1.0f;
            }
            point.Translate(x, y, z, Space.World);
            StartCoroutine(Roll(point.position, Vector3.right, 90.0f, speed));
            isMoving = true;
        }
        else if (Input.GetKeyDown("down") && !isMoving && !outOfBounds)
        {
            if (isLyingDown)
            {
                z = (isLyingLeftRight ? -0.5f : -1.0f);
                y = -0.5f;
            }
            else
            {
                z = -0.5f;
                y = -1.0f;
            }
            point.Translate(x, y, z, Space.World);
            StartCoroutine(Roll(point.position, -Vector3.right, 90.0f, speed));
            isMoving = true;
        }

    }

    /* Rolling the player */
    IEnumerator Roll(Vector3 pos, Vector3 ax, float angle, float speed)
    {
        int steps = Mathf.CeilToInt(speed * 25.0f);
        float eachAngle = angle / steps;

        for (int i = 0; i < steps; i++)
        {
            transform.RotateAround(pos, ax, eachAngle);
            yield return new WaitForSeconds(0.03333f);
        }

        //snaping
        Vector3 vec = transform.eulerAngles;
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;
        transform.eulerAngles = vec;

        vec = transform.position;
        vec.x = Mathf.Round(vec.x / 0.5f) * 0.5f;
        vec.y = Mathf.Round(vec.y / 0.5f) * 0.5f;
        vec.z = Mathf.Round(vec.z / 0.5f) * 0.5f;
        transform.position = vec;

        point.position = transform.position;
        isMoving = false;
    }

    public bool isPlayerLyingDown()
    {
        return this.isLyingDown;
    }
}
