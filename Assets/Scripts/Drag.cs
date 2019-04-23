using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : ConsumeObj,IDragable {
    
    float distance;
    public float Distance
    {
        get { return distance; }

    }

    
    [SerializeField]
    private Vector3 pos;
    public Vector3 Pos { get { return pos; }  }


    public void Start()
    {
        distance = 10f;

        //pos = new Vector3(-1.9225f, 0.84155f, -4.64f);
    }
    public void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objpostion = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objpostion;  
	}

	public void OnMouseUp()
	{
        transform.position = pos;
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        Consume();
    }
}
