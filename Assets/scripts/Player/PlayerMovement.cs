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
    public void initialize() {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    public void update() {
        if (cc.isGrounded) {
            if (Input.GetButton("Jump") || onTrampoline) {
                jump();
            }
            else {
                freeFallSpeed = 0;
            }
        }

        doMovement();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(hit.transform.position.y < transform.position.y - 1 && hit.gameObject.layer == LayerMask.NameToLayer("Piece")) {
            hit.gameObject.transform.parent.SendMessage("steppedOn");
        }
    }

    public void doMovement() {
        // gestión de gravedad con CC sin RB
        Vector3 movement = Vector3.zero;
        freeFallSpeed += gravity * Time.deltaTime;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        movement += vertical * transform.forward;
        movement += horizontal * transform.right;
        movement = movement.normalized * movement.magnitude * speed * Time.deltaTime;
        movement += freeFallSpeed * Vector3.down;

        cc.Move(movement);
    }

    public void jump() {
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

    public void isOnTrampoline() {
        onTrampoline = true;
    }

    public void Teleport(Vector3 highestPortal) {
        CharacterController cc = GetComponent<CharacterController>();
        float height = cc.height / 2;
        Vector3 heightCompensation = Vector3.up * height;

        Debug.Log("Teleporting to: " + highestPortal);
        transform.position = highestPortal + heightCompensation;
    }

}
