using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSelector : MonoBehaviour {

    [SerializeField] private GameObject selectedMinion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectMinion(GameObject _minion)
    {
        selectedMinion = _minion;
    }
}
