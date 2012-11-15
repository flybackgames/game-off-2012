// 
// Player.cs
//  
// Author: team@flybackgames.com
// 
// Copyright (c) 2012 Flyback Games http://www.flybackgames.com/
// Used under a Creative Commons Attribution license: http://creativecommons.org/licenses/by/3.0/
// 
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public enum BlockDir {
		NONE,
		LEFT,
		RIGHT
	}

	private BlockDir Blocked = BlockDir.NONE;
	
	private GameObject StickedObject = null;
	private bool GrabStickedObject = false;
	
	public float WalkSpeed;
	
	void Start () {
		if (WalkSpeed == 0)
			Debug.LogWarning("Player walk speed is 0, it will not move");
	}
	
	void Update () {
		float translation = Input.GetAxis("Horizontal");
		if ( (translation < 0 && (Blocked != BlockDir.LEFT || GrabStickedObject))
			||
			(translation > 0 && (Blocked != BlockDir.RIGHT || GrabStickedObject)) ) {
			// half speed while pulling/pushing
			float speed = (GrabStickedObject ? WalkSpeed / 2f : WalkSpeed);
			float amountToMove = translation * Time.deltaTime * speed;
			transform.Translate(amountToMove, 0, 0);
		}
		if (Input.GetKey(KeyCode.Space)) {
			if (StickedObject != null && !GrabStickedObject) {
				// pull / push object
				GrabStickedObject = true;
				GrabCrate(StickedObject);
			}
		}
		else if (GrabStickedObject){
			GrabStickedObject = false;
			if (StickedObject != null) {
				ReleaseCrate(StickedObject);
			}
		}
	}
	
	/**
	 * Sets a Fixed Joint with this crate so they move along as if the player
	 * really grabbed it
	 */
	void GrabCrate(GameObject crate) {
		CorrectPlayerPosition(crate);
		if (crate != null) {
			if (crate.GetComponent<FixedJoint>() == null) {
				FixedJoint joint = crate.AddComponent("FixedJoint") as FixedJoint;
				joint.connectedBody = transform.rigidbody;
			}
		}
	}
	
	/**
	 * Sets player position to absolute based on item.
	 * Prevents clipping inside grabbed object, for example
	 * 
	 * TODO Calculate more accurately, best wait for art
	 */
	void CorrectPlayerPosition(GameObject item) {
		if (item != null) {
			float amount = item.renderer.bounds.size.x / 2 + renderer.bounds.size.x / 2;
			Debug.Log (amount);
			float direction = 0f;
			if (Blocked == BlockDir.LEFT) {
				direction = 1f;
			}
			else {
				direction = -1f;
			}
			transform.position = new Vector3(
				transform.position.x + (amount * direction),
				transform.position.y,
				0f // Z plane always 0
			);
		}
	}
	
	void ReleaseCrate(GameObject crate) {
		if (crate != null) {
			FixedJoint joint = crate.GetComponent("FixedJoint") as FixedJoint;
			if (joint != null) {
				Debug.Log (joint);
				Destroy(joint);
			}
		}
	}
	
	void OnGUI() {
		GUI.Label (new Rect(10f, 10f, 100f, 20f), "Blocked: "+Blocked);
		GUI.Label (new Rect(10f, 30f, 100f, 20f), "Grabbed: "+GrabStickedObject);
		
		GUI.Label (new Rect(10f, 80f, 500f, 20f), "ARROWS TO MOVE, PRESS SPACE TO GRAB WHEN NEXT TO CRATE");
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Crate")) {
			if (other.transform.position.x < transform.position.x) {
				Blocked = BlockDir.LEFT;
			}
			else {
				Blocked = BlockDir.RIGHT;
			}
			if (StickedObject == null) {
				StickedObject = other.gameObject;
			}
		}
	}
	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Crate")) {
			Blocked = BlockDir.NONE;
			if (!GrabStickedObject) {
				StickedObject = null;
			}
		}
	}
}
