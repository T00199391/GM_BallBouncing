using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{
    Vector3 pointOnPlane, normalToPlane;
    public Vector3 normal { get { return normalToPlane; } }

    // Start is called before the first frame update
    void Start()
    {
        FindPlaneDetails();
    }

    //Gets the position and normal of the plane the script is attached to
    private void FindPlaneDetails()
    {
        pointOnPlane = transform.position;
        normalToPlane = transform.up;
    }

    //Gets the distance from the plane to the sphere
    public float DistanceTo(Vector3 point)
    {
        //i.e. sphere.tranform.position - plane.tranform.position
        Vector3 pointOnPlaneToPoint = point - pointOnPlane;

        return Vector3.Dot(pointOnPlaneToPoint, normalToPlane);
    }
}
