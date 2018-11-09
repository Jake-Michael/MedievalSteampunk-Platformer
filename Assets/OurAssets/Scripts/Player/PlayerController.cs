using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Animator anim;
	private Rigidbody2D rb2D;

	[Header("Movement")]
	[SerializeField] private float speed;
	[SerializeField] private float jumpForce;
	private float moveInput;

	private bool facingRight = true;

	[Header("Jumping")]

	private int extraJumps;
	[SerializeField] private int extraJumpsValue;

	[SerializeField] private bool isGrounded;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float checkRadius;
	[SerializeField] private LayerMask whatIsGround;

	void Start(){
		// Set the extraJumps value equal to the extraJumpsValue
		extraJumps = extraJumpsValue;
		// Find the Rigidbody2D component on the player
		rb2D = GetComponent<Rigidbody2D> ();
		// Find the Animator component on the player
		anim = GetComponent<Animator> ();
	}

	void Update(){

		// If the player IS GROUNDED - set the extraJumps value equal to the extraJumpsValue
		if (isGrounded == true) {
			extraJumps = extraJumpsValue;
		}

		// If the player presses the Spacebar AND extraJumps is GREATER THAN zero
		if (Input.GetKeyDown (KeyCode.Space) && extraJumps > 0) {
			// Set the animtion trigger "jump" to true
			anim.SetTrigger ("jump");
			// Set the player's Y velocity equal to Vector2.up multiplied by the jumpForce
			rb2D.velocity = Vector2.up * jumpForce;
			// Decrease extraJumps by 1
			extraJumps--;
		// Else if the player presses the Spacebar AND extraJumps is equal to zero AND the player IS GROUNDED
		} else if (Input.GetKeyDown (KeyCode.Space) && extraJumps == 0 && isGrounded == true) {
			// Set the animtion trigger "jump" to true
			anim.SetTrigger ("jump");
			// Set the player's Y velocity equal to Vector2.up multiplied by the jumpForce
			rb2D.velocity = Vector2.up * jumpForce;
		}

		// If the player is pressing the left OR right arrow key
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow)) {
			// the isWalking bool is equal to true
			anim.SetBool ("isWalking", true);
		// Else the isWalking bool is equal to false
		} else {
			anim.SetBool ("isWalking", false);
		}
	}

	void FixedUpdate(){
		// isGrounded is equal to the overlap circle at the groundCheck's position
		isGrounded = Physics2D.OverlapCircle (groundCheck.position, checkRadius, whatIsGround);

		// moveInput is equal to the Horizontal axis
		moveInput = Input.GetAxisRaw ("Horizontal");
		// Set the player's velocity equal to a new Vector2 valued at moveInput multiplied by speed
		rb2D.velocity = new Vector2 (moveInput * speed, rb2D.velocity.y);

		// If the player is NOT facingRight AND they're moving right, call the Flip function
		if (facingRight == false && moveInput > 0) {
			Flip ();
		// Else if the player IS facingRight AND they're moving left, call the Flip function
		} else if (facingRight == true && moveInput < 0) {
			Flip ();
		}
	}

	void Flip(){
		// facingRight bool is equal to !facingRight
		facingRight = !facingRight;
		// scaler is equal to the player's local scale
		Vector3 scaler = transform.localScale;
		// Multiply the scaler's x value and set it to -1
		scaler.x *= -1;
		// The player's transform local scale value is equal to the scaler value
		transform.localScale = scaler;

	}

}
