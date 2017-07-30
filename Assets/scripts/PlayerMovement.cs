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
    [SerializeField]
    private float superJumpMultiplier = 1.5f;


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
                Jump();
            }
            else
            {
                freeFallSpeed = 0;
            }
        }

        doMovement();


    }

    public void doMovement()
    {
        // gestión de gravedad con CC sin RB
        freeFallSpeed += gravity * Time.deltaTime;
        Vector3 movement = freeFallSpeed * Vector3.down;

        float mov = Input.GetAxis("Vertical") * speed;
        float starfe = Input.GetAxis("Horizontal") * speed;
        movement += mov * transform.forward * Time.deltaTime;
        movement += starfe * transform.right * Time.deltaTime;

        cc.Move(movement);
    }

    public void Jump()
    {
        freeFallSpeed = -jumpSpeed;
    }

    public void SpecialJump()
    {
        freeFallSpeed = -jumpSpeed * superJumpMultiplier;
    }


    public void Teleport()
    {
        //Preguntar por el portal mas alto de la torre
        Vector3 highestPortal = Vector3.one;
        transform.Translate(highestPortal);
    }

}
