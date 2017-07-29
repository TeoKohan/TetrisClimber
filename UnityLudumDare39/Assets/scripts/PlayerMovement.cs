using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private float freeFallSpeed;
    private Animator anim;
    private CharacterController cc;
    private float savedGravity;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float jumpSpeed;


    // Use this for initialization
    void Awake()
    {
        cc = GetComponent<CharacterController>();
        savedGravity = gravity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (cc.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                freeFallSpeed = jumpSpeed * (-1);
            }
            else
            {
                freeFallSpeed = 0;
            }
        }

        // gestión de gravedad con CC sin RB
        freeFallSpeed += gravity * Time.deltaTime;
        Vector3 movement = freeFallSpeed * Vector3.down;

        float mov = Input.GetAxis("Vertical") * speed;
        movement += mov * transform.forward * Time.deltaTime;

        cc.Move(movement);
        
        float rotationMovement = Input.GetAxis("Horizontal") * rotationSpeed;
        //cc.angularVelocity =new Vector3(0, rotationMovement, 0);
        transform.Rotate(new Vector3(0, rotationMovement * Time.deltaTime, 0));


    }

}
