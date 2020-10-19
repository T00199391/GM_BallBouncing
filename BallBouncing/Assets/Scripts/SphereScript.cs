using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour
{
    PlaneScript plane;

    Vector3 velocity = new Vector3(0, 0, 0);
    Vector3 accelleration = new Vector3(0, -9.8f, 0);
    Vector3 point1 = new Vector3(0, 0, 0), point2 = new Vector3(0, 0, 0);

    float radiusOfSphere;
    public float massOfSphere;
    public float radius { get { return radiusOfSphere; } }
    public float mass { get { return massOfSphere; } }
    private float coefficientOfRestitution = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        radiusOfSphere = transform.localScale.x / 2;
        //massOfSphere = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        plane = ObserverScript.CheckPlaneToSphereCollision(this);

        ResolvePlaneCollision();

        SphereScript sphere2 = ObserverScript.CheckSphereOnSphereCollisions(this);
    }

    private Vector3 PerpComp(Vector3 vel, Vector3 normal)
    {
        return vel - ParComp(vel, normal);
    }

    private Vector3 ParComp(Vector3 vel, Vector3 normal)
    {
        return Vector3.Dot(vel, normal) * normal;
    }

    private Vector3 Momentum(SphereScript s2,Vector3 par, Vector3 nor)
    {
        return ((mass - s2.mass) / (mass + s2.mass)) * par + ((2 * s2.mass) / (mass + s2.mass)) * s2.ParComp(s2.velocity, nor);
    }

    private void ResolvePlaneCollision()
    {
        float d1 = plane.DistanceTo(transform.position) - radiusOfSphere;
        velocity += accelleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

        if (plane != null)
        {
            float distanceFromCenterToPlane = plane.DistanceTo(transform.position);
            float d2 = distanceFromCenterToPlane - radiusOfSphere;

            //determine the distance between 
            Vector3 pra = ParComp(velocity, plane.normal);
            Vector3 perp = PerpComp(velocity, plane.normal);

            float timeOfImpact = Time.deltaTime * (d1 / (d1 - d2));

            transform.position -= velocity * (Time.deltaTime - timeOfImpact);
            
            velocity = perp - coefficientOfRestitution * pra;

            transform.position += velocity * (Time.deltaTime - timeOfImpact);
        }
    }

    private Vector3 FindNormal(SphereScript sphere2)
    {
        Vector3 normal = new Vector3(0, 0, 0);

        float d = 0;

        d = (float)Math.Sqrt(Math.Pow(point1.x - point2.x, 2) + Math.Pow(point1.y - point2.y, 2) + Math.Pow(point1.z - point2.z, 2));

        if (d <= radius + sphere2.radius)
        {
            if(mass > sphere2.mass)
                normal = (point2 - point1).normalized;
            else
                normal = -(point2 - point1).normalized;
        }

        return normal;
    }

    private Vector3 FindPointOfImpact(SphereScript sphere2)
    {
        Vector3 pointOfImpact = new Vector3(0, 0, 0);

        float d = 0;

        d = (float)Math.Sqrt(Math.Pow(point1.x - point2.x, 2) + Math.Pow(point1.y - point2.y, 2) + Math.Pow(point1.z - point2.z, 2));

        if (d <= radius + sphere2.radius)
        {
            pointOfImpact = point1 - (radius * FindNormal(sphere2));
        }

        return pointOfImpact;
    }

    public void ResolveSphereCollision(SphereScript sphere2)
    {
        Vector3 normal = FindNormal(sphere2);
        float d1 = Vector3.Dot((transform.position - sphere2.transform.position), normal) - (radius + sphere2.radius);
        point1 = transform.position;
        point2 = sphere2.transform.position;
        
        if (sphere2 != null)
        {
            float d2 = Vector3.Dot((transform.position - sphere2.transform.position), normal) - (radius + sphere2.radius);

            Vector3 par = ParComp(velocity, normal);
            Vector3 perp = PerpComp(velocity, normal);
            Vector3 momParComp = Momentum(sphere2,par, normal);
            float timeOfImpact = Time.deltaTime * (d1 / (d1 - d2));

            transform.position -= velocity * (Time.deltaTime - timeOfImpact);
           
            velocity = perp - coefficientOfRestitution * momParComp;
            
            transform.position += velocity * (Time.deltaTime - timeOfImpact);
        }
    }
}
