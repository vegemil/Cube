using UnityEngine;
using System.Collections;

public enum Status
{
	CENTER,
	DIAGONAL,
	EDGE,
}

public class CubeStatus : MonoBehaviour {

	public Status status;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		PickCube();
	}

	void PickCube()
	{
		if (Input.GetMouseButtonDown(0))
		{ 
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{    
				Debug.Log(hit.transform.parent);
			}
		}
	}


}
