using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public float gravity;

    Vector3 movementDirection = Vector3.zero;
    float inputX;
    float inputZ;
    CharacterController characterController;

    private void Start() {
        characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        GetInputs();
    }

     void GetInputs(){
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        movementDirection = Vector3.zero;

        Vector3 forwardVector = Camera.main.transform.forward;
        Vector3 rightVector = Camera.main.transform.right;

        forwardVector.y = rightVector.y = 0f;

        forwardVector.Normalize();
        rightVector.Normalize();
    

        movementDirection = forwardVector * inputZ + rightVector * inputX;
        movementDirection *= movementSpeed;
        
        if(movementDirection != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection), rotationSpeed);

        //verticals
        movementDirection.y -= gravity;

        characterController.Move(movementDirection * Time.deltaTime);
    }
}
