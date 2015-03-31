using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Direction
{
	RIGHT,
	LEFT,
	UP,
	DOWN,
	NONE
}

public class CubeStatus : MonoBehaviour {

	public GameObject pickCube;
	public Direction path;
	public List<GameObject> CubeGroups = new List<GameObject>();
	public GameObject[] Cubes;
	public GameObject CubeCriterion;
	public GameObject CubeRotateParent;

	bool isMouseDrag = false;
	
	Vector3 mouseDown = new Vector3();
	Vector3 mouseUp = new Vector3();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
		PickCube();
		MouseButtonUp();
		
		
		if (pickCube != null)
		{
			CalculateDirection();	
			if(path!= Direction.NONE)
				FindCubePosition();
		}
	}

	void PickCube()
	{
		if (Input.GetMouseButtonDown(0))
		{
			path = Direction.NONE;
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
			 
				mouseDown = hit.collider.transform.position;
				Debug.Log("mouseDown : " + mouseDown); 
				isMouseDrag = true;
				Debug.Log(hit.collider.gameObject.name);
				pickCube =  hit.collider.gameObject;
			}
		}
	}

	void MouseButtonUp()
	{
		if(Input.GetMouseButtonUp(0))
		{
			if(isMouseDrag == true)
			{
				float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
				Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
				mouseUp = new Vector3(pos_move.x, pos_move.y, 0);

				Debug.Log("mouseUP : " + mouseUp);
				isMouseDrag = false;
			}
		}
	}

	void CalculateDirection()
	{

		Vector3 distance = mouseDown - mouseUp;
		distance.z = 0;
		int count = 0;

		if (Mathf.Abs(distance.x) < 1f)
			distance.x = 0;
		else
			count++;

		if (Mathf.Abs(distance.y) < 1f)
			distance.y = 0;
		else
			count++;

		if(count >=2)
		{
			path = Direction.NONE;
		}
		else
		{
			if (distance.x !=0)
			{
				if (distance.x > 0)
					path = Direction.LEFT;
				else
					path = Direction.RIGHT;
			}

			if (distance.y !=0)
			{
				 if (distance.y > 0)
					 path = Direction.DOWN;
				else
					 path = Direction.UP;
			}
		}
	}
	
	void CubeRotate()
	{
		Quaternion temp = CubeRotateParent.transform.rotation;
		float rotate = 90f;
		switch (path)
		{
			case Direction.RIGHT:
				{
					temp.y -= rotate;
					CubeRotateParent.transform.rotation = Quaternion.Euler(temp.x, temp.y, temp.z);
					break;
				}
			case Direction.LEFT:
				{
					temp.y += rotate;
					CubeRotateParent.transform.rotation = Quaternion.Euler(temp.x, temp.y, temp.z);
					break;
				}
			case Direction.UP:
				{
					temp.x += rotate;
					CubeRotateParent.transform.rotation = Quaternion.Euler(temp.x, temp.y, temp.z);
					break;
				}
			case Direction.DOWN:
				{
					temp.x -= rotate;
					CubeRotateParent.transform.rotation = Quaternion.Euler(temp.x, temp.y, temp.z);
					break;
				}
		}
	}

	void FindCubePosition()
	{
		switch (path)
		{
			case Direction.RIGHT:
			case Direction.LEFT:
				
					for (int i = 0; i < Cubes.Length; i++ )
					{
						if (Cubes[i].transform.position.y == pickCube.transform.position.y)
						{
							CubeGroups.Add(Cubes[i]);
						}
					}
						break;
				

			case Direction.DOWN:
			case Direction.UP:
					for (int i = 0; i < Cubes.Length; i++)
					{
						if (Cubes[i].transform.position.x == pickCube.transform.position.x)
						{
							CubeGroups.Add(Cubes[i]);
						}
					}
					break;
				
		}
		int temp = 1;
		foreach(GameObject gameobject in CubeGroups)
		{
			Debug.Log("CubeGroups" + temp + " : " + gameobject + ", " + gameobject.transform.position );
			temp++;
			gameobject.transform.parent = CubeRotateParent.transform;
		}
		CubeRotate();

		foreach (GameObject gameobject in CubeGroups)
		{
			gameobject.transform.parent = CubeCriterion.transform;
		}

		CubeRotateParent.transform.rotation = new Quaternion(0, 0, 0, 0);

		CubeGroups.Clear();
		path = Direction.NONE;
		pickCube = null;

	}
}
