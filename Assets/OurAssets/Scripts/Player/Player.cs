using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Based on youtube tutorial : https://www.youtube.com/watch?v=MbWK8bCAU2w&list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz
//Written by Michael Corben
//Begun 11/11/2018


[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {


    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float timeToJumpApex = 0.4f;
    [SerializeField] private float accelerationTimeAirbourne = 0.2f;
    [SerializeField] private float accelerationTimeGrounded = 0.1f;
    [SerializeField] [Range(1, int.MaxValue)] private int numberOfJumps = 1;
    [SerializeField] private float moveSpeed = 6;

    private float gravity;
    private float jumpVelocity;
    private int currentJumpNum = 0;
    private Vector3 velocity;
    private float velocitySmoothingX;

    private Controller2D controller;

    private bool Grounded { get { return controller.collisions.below; } } 


    private void Awake() {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    private void Update() {
        if (controller.collisions.above || Grounded) { velocity.y = 0; }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && (Grounded || currentJumpNum < numberOfJumps)) {
            velocity.y = jumpVelocity;
            currentJumpNum++;
        }
        if (Grounded) { currentJumpNum = 0; }

        float targetVelocityX = input.x * moveSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocitySmoothingX,
            Grounded ? accelerationTimeGrounded : accelerationTimeAirbourne);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
