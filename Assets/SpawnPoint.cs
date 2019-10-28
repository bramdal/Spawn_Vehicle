using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float radius;

    //equation of semi circle concave on negative side
    //y = y0 - sqrt(r^2 - (x- x0)^2)
    public Vector3 ReturnRandomPointOnSemiCircle(){
        float x = Random.Range(-radius, radius);  
        float z =  - Mathf.Sqrt((radius*radius)-((x )*(x)));

        //accounting for rotation of axis
        float angle = Vector3.Angle(Vector3.forward, transform.forward);     
        //angle = angle * Mathf.Deg2Rad;

        float newX = (x * Mathf.Cos(angle)) + (z * Mathf.Sin(angle)); 
        float newZ = (z * Mathf.Cos(angle)) - (x * Mathf.Sin(angle));  

        newX += transform.position.x;
        newZ += transform.position.z;
         

        Vector3 randomPointOnCircle = new Vector3(newX, 0f, newZ);
        return randomPointOnCircle;
    }
}
