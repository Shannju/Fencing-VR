using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardActions : MonoBehaviour
{
    private CharacterController characterController;
    public float speedRun = 7f;
    public float speedWalk = 4f;
    private float speed = 4f;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
    
        Vector3 move = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
        characterController.Move(move*Time.deltaTime*speed);
        transform.rotation = Quaternion.LookRotation(move);

    }


}
