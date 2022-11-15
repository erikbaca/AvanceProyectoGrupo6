using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalMove;
    public float verticalMove;
    private Vector3 playerInput;

    public CharacterController player;

    //Variable para controlar la velocidad - Movimiento - Gravedad
    public float playerSpeed;
    private Vector3 movePlayer;
    public float gravity = 9.8f;
    public float fallVelocity;
    public float jumpForce;

    // Camara
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    //
    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideVelocity;
    public float slopeForceDown;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        // Posicion de la camara
        camDirection();

        // El player se mueva siempre mirando a la camara
        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        movePlayer = movePlayer * playerSpeed;

        // Player mire hacia donde se mueva
        player.transform.LookAt(player.transform.position + movePlayer);

        //Llamamos la Funcion Gravedad
        SetGravity();


        PlayerSkills();

        // Definimos los movimientos del player
        player.Move(movePlayer * Time.deltaTime);

    }

    // FUNCION PARA DETERMINAR LA DIRECCION A LA QUE MIRA LA CAMARA
    public void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    // FUNCION PARA HABILIDADES DEL PLAYER
    public void PlayerSkills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;

        }
    }

    // FUNCION DE GRAVEDAD
    public void SetGravity()
    {
        // Validacion si el player toca el suelo
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
           fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }

        SlideDown();
    }


    // FUNCION DE VALIDACION SI EL PLAYER ESTA O NO EN UNA RAMPA
    public void SlideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;

        if (isOnSlope)
        {
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;

            movePlayer.y += slopeForceDown;
        }
    }


    // FUNCION DE DETECTAR CUANDO NUESTRO PLAYER COLISIONA CON OTRO OBJETO
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;

    }

}
