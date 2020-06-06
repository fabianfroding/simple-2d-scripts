using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleScript : MonoBehaviour
{
    Rigidbody2D rb2d;
    private Plane plane;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        plane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseDir = mousePos - gameObject.transform.position;
        mouseDir.z = 0.0f;
        mouseDir = mouseDir.normalized;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb2d.AddForce(mouseDir * 667);
        }
    }
}
