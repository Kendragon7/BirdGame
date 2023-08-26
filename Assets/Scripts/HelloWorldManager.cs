using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class HelloWorldManager : MonoBehaviour
{
	public GameObject testNetworkObjectPrefab;
	string ipPort = "127.0.0.1:7777"; // will need to be separated by a colon
	string ip = "127.0.0.1";
	ushort port = 7777;
	void OnGUI() {
		GUILayout.BeginArea(new Rect(10, 10, 300, 300));
		if(!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer){
			StartButtons();
		}
		else{
			StatusLabels();
			SpawnTestObjectButton();
		}
		
		GUILayout.EndArea();
	}
	
	void Start(){

	}

	void StartButtons(){
		ipPort = GUILayout.TextField(ipPort);
		
		if (GUILayout.Button("Host")){
			SetConnectionData();
			NetworkManager.Singleton.StartHost();
		}
        if (GUILayout.Button("Client")){
			SetConnectionData();
			NetworkManager.Singleton.StartClient();
		}
        if (GUILayout.Button("Server")){
			SetConnectionData();
			NetworkManager.Singleton.StartServer();
		}

		
		
	}

	void StatusLabels(){
		var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
		GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
		GUILayout.Label("Mode: " + mode);
		GUILayout.Label("IP: " + NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address);
	}

	void SpawnTestObjectButton(){
		if (GUILayout.Button("Spawn Object")){
            if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient ) {
                //foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds) NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
				// Spawn the object on the server...
				GameObject newObject = Instantiate(testNetworkObjectPrefab, new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-5f, 5f)), Quaternion.identity);
				// spawns the new object on all clients
				newObject.GetComponent<NetworkObject>().Spawn();
            }
        }
	}
	
	void SetConnectionData(){
		string[] splitIpPort = ipPort.Split(':');
		ip = splitIpPort[0];
		if(splitIpPort.Length > 1) port = ushort.Parse(splitIpPort[1]);
		
		NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
    		ip,  // The IP address is a string
    		port, // The port number is an unsigned short
    		"0.0.0.0" // The server listen address is a string.
		);
	}

}
