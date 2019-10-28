using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class VehicleBehavior : MonoBehaviour
{
    GameObject player;
    Transform playerTransform;
    public GameObject leftSide;
    public GameObject rightSide;

    CharacterController vehicleController;

    //state bools
    public bool isChasing = true;
    public bool isTrailing = false;
    public float trailMinDistance = 3f;
    bool isOnLeftSide;
    //public bool isInVicinity = false;

    public float maxDestroyDistance = 15f;

    Vector3 moveDirection = Vector3.zero;
    public float movementSpeed;
    float chaseSpeed;

    private void Start() {
        vehicleController = GetComponent<CharacterController>();
        player = GameObject.FindWithTag("Player");
        leftSide = GameObject.FindWithTag("LeftCheckpoint");
        rightSide = GameObject.FindWithTag("RightCheckpoint");
        playerTransform = player.transform;

        chaseSpeed = movementSpeed;
    }
    private void Update() {
        if(isChasing && !isTrailing){
            ChasePlayer();
        }    
        else if(isChasing && isTrailing){
            TrailAlongPlayer();
        }  
    }

    void ChasePlayer(){
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        moveDirection = playerTransform.position - transform.position;
        moveDirection.Normalize();
        moveDirection *= movementSpeed;
        vehicleController.SimpleMove(moveDirection);
    }

    void TrailAlongPlayer(){
        if(!player.GetComponent<PlayerController>().walking){
            Rest();
        }

        if(Vector3.Distance(transform.position, playerTransform.position)> trailMinDistance){
            Rest();
        }

        if(isOnLeftSide){
            transform.LookAt(new Vector3(leftSide.transform.position.x, transform.position.y, leftSide.transform.position.z));
            moveDirection = leftSide.transform.position - transform.position;
            moveDirection.Normalize();
            moveDirection *= movementSpeed;
            vehicleController.SimpleMove(moveDirection);
        }
        else
        {
            transform.LookAt(new Vector3(rightSide.transform.position.x, transform.position.y, rightSide.transform.position.z));
            moveDirection = rightSide.transform.position - transform.position;
            moveDirection.Normalize();
            moveDirection *= movementSpeed;
            vehicleController.SimpleMove(moveDirection);
        }
    }
    
    public void InitTrailPlayer(){
        isTrailing = true;
        if(Vector3.Distance(transform.position, leftSide.transform.position)<Vector3.Distance(transform.position, rightSide.transform.position)){
            isOnLeftSide = true;
        }
        else
        {
            isOnLeftSide = false;
        }
        if(player.GetComponent<PlayerController>().sprinting)
            movementSpeed = player.GetComponent<PlayerController>().movementSpeed;
        else
            Rest();
    }

    public void Rest(){
        isChasing = false;
        isTrailing = false;
        movementSpeed = chaseSpeed;
    }

    //uncomment if want to destroy based on visibility
    // public bool DestroyVehicle(){
    //     Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
    //     if(screenPosition.x > 0 && screenPosition.x < 1 && screenPosition.y > 0 && screenPosition.y < 1 && screenPosition.z > 0){
    //         print("visible");
    //         return true;
    //     }
    //     else
    //     {
    //         print("invisible");
    //         return false;
    //     }
    // }

    public bool DestroyVehicle(){
        if(Vector3.Distance(playerTransform.position, transform.position) < maxDestroyDistance)
            return true;
        else
            return false;
        
       
                
    }
}
