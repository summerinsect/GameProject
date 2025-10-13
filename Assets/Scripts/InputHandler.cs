using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

	public void Init()
	{

	}

	private void Update()
	{
		HandleMouseInput();
	}

	private void HandleMouseInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			OnMouseDown();
		}

	}

	private void OnMouseDown()
	{

	}

}
