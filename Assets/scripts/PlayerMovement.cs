using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private float freeFallSpeed;
    private Animator anim;
    private CharacterController cc;

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
        float starfe = Input.GetAxis("Horizontal") * speed;
        movement += mov * transform.forward * Time.deltaTime;
        movement += starfe * transform.right * Time.deltaTime;

        cc.Move(movement);

        /* float rotationMovementX = Input.GetAxis("Horizontal") * rotationSpeed;
        transform.Rotate(new Vector3(0, rotationMovementX * Time.deltaTime, 0)); */

        /* float rotationMovementY = Input.GetAxis("Mouse Y") * rotationSpeed * (-1);
        Camera.main.transform.Rotate(new Vector3(rotationMovementY * Time.deltaTime, 0, 0)); */


    }

}
