using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVehicle : MonoBehaviour
{
    //public float radius = 20f;
    public GameObject vehicle;
    GameObject currentVehicle;

    public float sphereRadius;

    public float callRadius = 5f;

  
    public SpawnPoint spawnPoint;

    private void Start() {
        currentVehicle = null;
    }

    private void Update(){

        if(Input.GetButtonDown("Fire1") && currentVehicle == null){
            GetComponent<AudioSource>().Play();
            SpawnNewVehicle();
        }
        else if(Input.GetButtonDown("Fire1") && !currentVehicle.GetComponent<VehicleBehavior>().isChasing && Vector3.Distance(transform.position, currentVehicle.transform.position)>callRadius){
            GetComponent<AudioSource>().Play();
            CallExistingVehicle();
        }

        if(currentVehicle != null){
            if(currentVehicle.GetComponent<VehicleBehavior>().isChasing &&!currentVehicle.GetComponent<VehicleBehavior>().isTrailing){
                Collider[] detectVehicleLeft = Physics.OverlapSphere(transform.position, sphereRadius);
                if(detectVehicleLeft.Length > 0){
                    int i=0;
                    while(i < detectVehicleLeft.Length){
                        if(detectVehicleLeft[i].tag == "Vehicle" && !currentVehicle.GetComponent<VehicleBehavior>().isTrailing){
                            currentVehicle.GetComponent<VehicleBehavior>().InitTrailPlayer();
                        } 
                    i++;       
                    }    
                }
            }
        }
    }
    
    private void LateUpdate() {
        if (currentVehicle != null && !currentVehicle.GetComponent<VehicleBehavior>().DestroyVehicle()){
                DestroyVehicle();
                return;
        }
    }

    void SpawnNewVehicle(){
        Vector3 vehicleOrigin = spawnPoint.ReturnRandomPointOnSemiCircle();
        vehicleOrigin.y = transform.position.y;
        currentVehicle = Instantiate(vehicle, vehicleOrigin, Quaternion.LookRotation(Vector3.forward));
        currentVehicle.GetComponent<VehicleBehavior>().isChasing = true;
    }

    void CallExistingVehicle(){
        currentVehicle.GetComponent<VehicleBehavior>().isChasing = true;
    }

    void DestroyVehicle(){
        GameObject vehicleToDestroy = currentVehicle;
        currentVehicle = null;
        Destroy(vehicleToDestroy);
    }

    //placed on camera
    //equation of semi circle concave on negative side
    //y = y0 - sqrt(r^2 - (x- x0)^2)
    // public Vector3 ReturnRandomPointOnSemiCircle(){
    //     float x = Random.Range(-radius, radius);  
    //     float z =  - Mathf.Sqrt((radius*radius)-((x )*(x)));

    //     //accounting for rotation of axis
    //     float angle = Vector3.Angle(Vector3.forward, transform.forward);     
    //     //angle = angle * Mathf.Deg2Rad;

    //     float newX = (x * Mathf.Cos(angle)) + (z * Mathf.Sin(angle)); 
    //     float newZ = (z * Mathf.Cos(angle)) - (x * Mathf.Sin(angle));  

    //     newX += transform.position.x;
    //     newZ += transform.position.z;
         

    //     Vector3 randomPointOnCircle = new Vector3(newX, 0f, newZ);
    //     return randomPointOnCircle;
    // }
}
