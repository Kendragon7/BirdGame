using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Cinemachine;

public class OwnerController : NetworkBehaviour
{
	public float speedMultiplier = 10;
	private Vector2 inputAxis;
	
	void Initialize(){
		// disable components on other players...
		if(!IsOwner || IsServer) DisableUnownedComponents();
		
		// Initialize Cinemachine Camera (Main Camera)
		InitializeCamera();
	}

	// Runs whenever the network object spawns in
	public override void OnNetworkSpawn(){
		base.OnNetworkSpawn();
		Initialize();
	} 
	
	void Update()
	{
		if(inputAxis.x != 0 || inputAxis.y != 0){
			transform.position += new Vector3(inputAxis.x, 0f, inputAxis.y) * speedMultiplier * Time.deltaTime;
		}
		
	}

	public void Move(InputAction.CallbackContext context){
		Debug.Log("IsOwner? " + IsOwner + " " + gameObject.GetInstanceID());
		
		if(!IsOwner) return; // Only control your own player.

		inputAxis = context.ReadValue<Vector2>();

		Debug.Log("Attempting move... " + inputAxis);
	}

	void DisableUnownedComponents(){
		gameObject.GetComponent<PlayerInput>().enabled = IsOwner; // PlayerInput will only be enabled on the player object that you own.
		enabled = IsOwner;
	}

	void InitializeCamera(){
		GameObject camera = GameObject.FindWithTag("MainCamera");
		CinemachineFreeLook cineFreeLook = camera.GetComponent<CinemachineFreeLook>();
		cineFreeLook.Follow = transform;
		cineFreeLook.LookAt = transform;
	}
}
