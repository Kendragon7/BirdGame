using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatScrollbar : MonoBehaviour
{
	[SerializeField]
	private bool atBottom = true;
	
	[SerializeField]
	private Scrollbar scrollbar;

	private bool isDragging;

	private bool attemptingMoveToBottom = false;

	void Start(){
		scrollbar = GameObject.Find("Chat Scrollbar").GetComponent<Scrollbar>();
	}

	public void OnScrollbarMove(){
		//Debug.Log("ScrollRect has been moved...");
		// if was atBottom when the scrollbar started to move, move it down to account for new chat messages moving the scrollbar up
		if(atBottom && !isDragging && !attemptingMoveToBottom){
			MoveScrollBarToBottom();
			//Debug.Log("normal move down");
		}

		// update variables
		atBottom = scrollbar.value <= 0.01f && scrollbar.value >= -0.01f;
	}

	
	public void MoveScrollBarToBottom(){
		// bool prevents recursive event calls
		attemptingMoveToBottom = true;
		scrollbar.value = 0;//(-0.01f * scrollbar.size);
		attemptingMoveToBottom = false;
	}

	public void BeginDrag(){
		isDragging = true;
	}
	public void EndDrag(){
		isDragging = false;
	}

	
}
