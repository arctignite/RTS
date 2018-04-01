using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMinion : MonoBehaviour {

    private bool mouseOver;
    private ObjectSelector selector;

    private void Start()
    {
        selector = GameObject.FindWithTag("GameManager").GetComponent<ObjectSelector>();
    }

    // Update is called once per frame
    void Update () {
        if (mouseOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                selector.SelectMinion(gameObject);
            }
        }		
	}

    private void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }
}
