using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.Netcode;

// Chat will act as server authoritative instead of client, so users will know when chat messages fail to send
public class ChatController : NetworkBehaviour {


    public TMP_InputField ChatInputField;

    public TMP_Text ChatDisplayOutput;

	public RectTransform TextToResize;

	//OnEnable moved to OnNetworkSpawn

    void OnDisable()
    {
        if(IsServer)	ChatInputField.onSubmit.RemoveListener(SendChatMessageFromServer);
		else 			ChatInputField.onSubmit.RemoveListener(SendChatMessageFromClient);
    }

	public override void OnNetworkSpawn(){
		if(IsServer){ // have server simulate chat messages
			StartCoroutine(FakeChatCoroutine());
		}

		if(IsServer)	ChatInputField.onSubmit.AddListener(SendChatMessageFromServer);
		else 			ChatInputField.onSubmit.AddListener(SendChatMessageFromClient);
	}

	void SendChatMessageFromClient(string newText){
		//Debug.Log("SendMessageToChat(): Is this even running?");
		// Clear Input Field
        ChatInputField.text = string.Empty;

        var timeNow = System.DateTime.Now;

        string formattedInput = "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + newText;

		// send to the chat
		SendChatMessageToServerRpc(formattedInput);

		// Keep Chat input field active
        ChatInputField.ActivateInputField();
	}

	// To be called by the server when it receives an incoming chat message
	[ClientRpc] // Server tells Clients to run this 
    void AddToChatOutputClientRpc(string message)
    {
		//Debug.Log("AddToChatOutputClientRpc(): Is this even running?");
        if (ChatDisplayOutput != null)
        {
            // No special formatting for first entry
            // Add line feed before each subsequent entries
            if (ChatDisplayOutput.text == string.Empty)
                ChatDisplayOutput.text = message;

            else
                ChatDisplayOutput.text += "\n" + message;
        }

    }

	// to be called by a client when sending a message
	[ServerRpc(RequireOwnership = false)]
	void SendChatMessageToServerRpc(string message){
		//Debug.Log("SendChatMessageToServerRpc(): Is this even running?");
		AddToChatOutputLocal(message); // put in server chat
		AddToChatOutputClientRpc(message); // put in clients' chats
	}

	void AddToChatOutputLocal(string message){
		if (ChatDisplayOutput != null)
        {
            // No special formatting for first entry
            // Add line feed before each subsequent entries
            if (ChatDisplayOutput.text == string.Empty)
                ChatDisplayOutput.text = message;

            else
                ChatDisplayOutput.text += "\n" + message;
        }
	}

	void SendChatMessageFromServer(string newText){
		if(!IsServer) return;

		//Debug.Log("SendChatMessageFromServer(): Is this even running?");

		// Clear Input Field
        ChatInputField.text = string.Empty;

        var timeNow = System.DateTime.Now;

        string formattedInput = "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + newText;

		AddToChatOutputLocal(formattedInput); // put in server chat
		AddToChatOutputClientRpc(formattedInput); // put in clients' chats

		// Keep Chat input field active
        ChatInputField.ActivateInputField();
	}

	IEnumerator FakeChatCoroutine(){
		if(!IsOwner) yield break;
		while(true){
			AddToChatOutputLocal("String :) " + Time.frameCount);
			AddToChatOutputClientRpc("String :) " + Time.frameCount);
			yield return new WaitForSecondsRealtime(1f);
		}
	}

}
