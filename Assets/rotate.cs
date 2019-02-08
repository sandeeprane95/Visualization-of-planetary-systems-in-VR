﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

	public float rotateSpeed = -5.0f;
    float scaleSpeed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.RotateAround (Vector3.up, rotateSpeed * Time.deltaTime * scaleSpeed);

	}

    public void SetNewSpeedScale(float scale) {
        scaleSpeed = scale;
    }
}
