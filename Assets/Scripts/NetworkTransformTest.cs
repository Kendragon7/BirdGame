using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class NetworkTransformTest : NetworkBehaviour
{
	public float speedMultiplier;
	private Vector2 inputAxis;
	
	void Initialize(){
		// disable components on other players..
		gameObject.GetComponent<PlayerInput>().enabled = IsOwner; // PlayerInput will only be enabled on the player object that you own.
		this.enabled = IsOwner;
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
}
