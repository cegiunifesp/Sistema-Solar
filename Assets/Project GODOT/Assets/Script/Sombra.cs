using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sombra : MonoBehaviour {

    public GameObject Sol;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        transform.right = Sol.transform.position - transform.position;

	}
}
