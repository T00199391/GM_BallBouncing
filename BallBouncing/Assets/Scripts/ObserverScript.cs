using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverScript : MonoBehaviour
{
    public static List<PlaneScript> planes = new List<PlaneScript>();
    public static List<SphereScript> spheres = new List<SphereScript>();

    void Update()
    {
        PlanesList();
        SphereList();
    }

    //Returns a list of PlaneScripts
    public static List<PlaneScript> PlanesList()
    {
        foreach (PlaneScript plane in FindObjectsOfType<PlaneScript>())
        {
            planes.Add(plane);
        }

        return planes;
    }

    //Returns the plane that the passed in sphere is colliding with
    public static PlaneScript CheckPlaneToSphereCollision(SphereScript sphere)
    {
        float distanceFromCenterToPlane, d2;
        PlaneScript collidingPlane = new PlaneScript();

        foreach (PlaneScript plane in planes)
        {
            distanceFromCenterToPlane = plane.DistanceTo(sphere.transform.position);
            d2 = distanceFromCenterToPlane - sphere.radius;

            if (d2 <= 0)
            {
                collidingPlane = plane;
            }
        }

        return collidingPlane;
    }

    //Return a list of SphereScripts
    public static List<SphereScript> SphereList()
    {
        foreach (SphereScript sphere in FindObjectsOfType<SphereScript>())
        {
            spheres.Add(sphere);
        }

        return spheres;
    }

    //Checks to see what spheres are colliding
    public static SphereScript CheckSphereOnSphereCollisions(SphereScript sphere1)
    {
        SphereScript sphere2;
        for (int i = 0; i < spheres.Count - 1; i++)
        {
            sphere2 = spheres[i];

            if (sphere1.transform.position != sphere2.transform.position)
            {
                if (Vector3.Distance(sphere1.transform.position, sphere2.transform.position) < (sphere2.radius + sphere1.radius))
                {
                    sphere2.ResolveSphereCollision(sphere1);
                    return sphere2;
                }
            }
        }
        return null;
    }

    //This caused freezing and frame rate issues
    /*public static void CheckSphereOnSphereCollisions()
    {
        SphereScript sphere1, sphere2;
        for(int i=0; i < spheres.Count - 1; i++)
        {
            for(int j=i+1; j<spheres.Count-1; j++)
            {
                sphere1 = spheres[i];
                sphere2 = spheres[j];

                if (sphere1.transform.position != sphere2.transform.position)
                {
                    if (Vector3.Distance(sphere1.transform.position, sphere2.transform.position) < (sphere2.radius + sphere1.radius))
                    {
                        sphere1.ResolveSphereCollision(sphere2);
                        sphere2.ResloveSphereCollision(sphere1);
                    }
                }
            }
        }
    }*/
}
