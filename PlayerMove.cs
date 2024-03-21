using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float Gravity = -9.81f;
	public float minGroundDistance = 0.4f;
	public float jumpHeight = 1;
	public bool isGroundLevel;

    public float PlayerSpeed = 15f;
    public float PlayerRunSpeed = 10;
    public static bool IsRunning = false;
	public Transform groundCheck;
	public LayerMask groundMask;
    public CharacterController controller;
	//public GunController gunController;
    Vector3 velocity;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {	
		isGroundLevel = Physics.CheckSphere(groundCheck.position, minGroundDistance, groundMask);

		if (isGroundLevel && velocity.y < 0) {
			velocity.y = -2f;
		}

        bool flag1 = Input.GetKey("left shift");
		
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
        float runSpeed = Convert.ToInt32(flag1) * PlayerRunSpeed;

		Vector3 move = transform.right * x + transform.forward * z;
        move *= (PlayerSpeed + runSpeed);
		move.y = 0;
        move += velocity;

		velocity.y += Gravity * Time.deltaTime;

		controller.Move(move * Time.deltaTime);

		if (Input.GetButtonDown("Jump") && isGroundLevel) {
			Jump();
		} 
    }

	void Jump() {
		velocity.y = Mathf.Sqrt(jumpHeight * -2f * Gravity);
	}
}