// 
// Crate.cs
//  
// Author: team@flybackgames.com
// 
// Copyright (c) 2012 Flyback Games http://www.flybackgames.com/
// Used under a Creative Commons Attribution license: http://creativecommons.org/licenses/by/3.0/
// 
using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {
	
	public bool isGrounded = false;
	
	void Start () {
	
	}
	
	void Update () {
	}
	
	void OnCollisionEnter(Collision collisionInfo) {
		if (collisionInfo.collider.name == "Floor") {
			isGrounded = true;
		}
	}
	
	void OnCollisionExit(Collision collisionInfo) {
		if (collisionInfo.transform.name == "Floor") {
			isGrounded = false;
		}
	}
}
