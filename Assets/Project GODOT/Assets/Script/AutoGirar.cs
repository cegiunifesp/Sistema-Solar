﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGirar : MonoBehaviour {

    //Game settings
    public float velRot; //

	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0,0,velRot * Time.deltaTime));
	}


}
