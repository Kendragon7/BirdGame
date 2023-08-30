using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestMovement : NetworkBehaviour
{
	public Vector3 velocity;
	public float speedMultiplier;
    void Initialize(){
		velocity = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
	}

	// Runs whenever the network object spawns in
	public override void OnNetworkSpawn(){
		base.OnNetworkSpawn();
		Initialize();
	} 

    // Update is called once per frame
    void Update()
    {
		transform.position += velocity * speedMultiplier * Time.deltaTime;
		if(transform.position.x > 5f || transform.position.x < -5f ){
			velocity = new Vector3(-velocity.x, 0f, velocity.z);
		}
		if(transform.position.z > 5f || transform.position.z < -5f ){
			velocity = new Vector3(velocity.x, 0f, -velocity.z);
		}
    }

}
