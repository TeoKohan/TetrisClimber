using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private float freeFallSpeed;
    private Animator anim;
    private CharacterController cc;
    private bool onTrampoline = false;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float superJumpMultiplier = 2;


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
            if (Input.GetButton("Jump") || onTrampoline)
            {
                Debug.Log("Entering Jump");
                Jump();
            }
            else
            {
                freeFallSpeed = 0;
            }
        }

        doMovement();


    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.position.y < transform.position.y - 1 && hit.gameObject.layer == LayerMask.NameToLayer("Piece"))
        {
            hit.gameObject.SendMessage("steppedOn");
        }
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
        float jumpMultiplier = 1;
        if (onTrampoline)
        {
            jumpMultiplier = superJumpMultiplier;
            onTrampoline = false;
        }
        if(cc.isGrounded)
        {
            freeFallSpeed = -jumpSpeed * jumpMultiplier;
        }
    }

    public void IsOnTrampoline()
    {
        onTrampoline = true;
    }

    public void Teleport()
    {
        //Preguntar por el portal mas alto de la torre
        //getHighest(PieceTypes pieceType)
        Vector3 highestPortal = Vector3.one;
        transform.Translate(highestPortal);
    }

}
