using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public float gravity;

    Vector3 movementDirection = Vector3.zero;
    float inputX;
    float inputZ;
    CharacterController characterController;
    Animator anim;

    [HideInInspector]public bool walking;
    [HideInInspector]public bool sprinting;
    [HideInInspector]public float speed;
    public bool mounted = false;
    SpawnVehicle spawnVehicle;

    private void Start() {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        spawnVehicle = GetComponent<SpawnVehicle>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        GetInputs();
    }
    private void LateUpdate() {
        SetAnimations();
    }

    void GetInputs(){
        if(!mounted){
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
            speed = movementDirection.magnitude;
            if(speed == 0f){
                walking = false;
                sprinting = false;
            }
            else if(speed >=4f)
            {
                walking = true;
                sprinting = true;
            }
            else
            {
                walking = true;
                sprinting = false;
            }
            
            if(movementDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection), rotationSpeed);

            //verticals
            movementDirection.y -= gravity;


            
            //transform.Rotate(0f, transform.eulerAngles.y, 0f);

            characterController.Move(movementDirection * Time.deltaTime);
        }
    }

    void SetAnimations(){
        if(!mounted){
            if(walking){
                anim.SetBool("Moving", true);
                anim.SetFloat("Velocity Z", movementDirection.magnitude);
            }
            else
            {
                anim.SetBool("Moving", false);
                anim.SetFloat("Velocity Z", 0f);
            }
        }
    }

    void ResetPlayerController(){
        walking = false;
        sprinting = false;
        movementDirection = Vector3.zero;
        
        anim.SetBool("Moving", false);
        anim.SetFloat("Velocity Z", 0f);
    }

    void MountHorse(Transform vehicleAttach){
        ResetPlayerController();
        
        transform.parent = vehicleAttach.parent;
        transform.localPosition = vehicleAttach.localPosition;
        transform.localRotation = vehicleAttach.localRotation;

        vehicleAttach.root.gameObject.GetComponent<VehicleBehavior>().Rest();
        vehicleAttach.root.gameObject.GetComponent<VehicleBehavior>().enabled = false;
        vehicleAttach.root.gameObject.GetComponent<VehicleController>().enabled = true;

        spawnVehicle.enabled = false;

        anim.SetBool("Mounted", true);
    }

    void DismountHorse(Transform vehicleAttach){
        transform.parent = null;
        Vector3 landingPosition = vehicleAttach.root.position;
        landingPosition.x += 10f;
        //transform.position = landingPosition;
        transform.position = Vector3.Lerp(transform.position, landingPosition, 1f);

        vehicleAttach.root.gameObject.GetComponent<VehicleBehavior>().Rest();
        vehicleAttach.root.gameObject.GetComponent<VehicleController>().enabled = false;
        vehicleAttach.root.gameObject.GetComponent<VehicleBehavior>().enabled = true;

        spawnVehicle.enabled = true;

        anim.SetBool("Mounted", false);
        
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag =="Saddle"){
            if(Input.GetButtonDown("Jump") && !mounted){
                mounted = true;
                MountHorse(other.gameObject.transform);
            }  
            else if (Input.GetButtonDown("Jump") && mounted)
            {
                mounted = false;
                DismountHorse(other.gameObject.transform);
            }  
        }
    }
}
