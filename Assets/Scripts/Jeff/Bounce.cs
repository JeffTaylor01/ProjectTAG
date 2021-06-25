using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] string playerTag;
    [SerializeField] float bounceForce;



    bool onTheGround;
    Plane planeComponent;
    private void Start()
    {


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == playerTag)
        {
            Rigidbody otherRB = collision.rigidbody;
            Vector3 vl = transform.up * bounceForce;
            otherRB.velocity = vl;
        }


    }


}

