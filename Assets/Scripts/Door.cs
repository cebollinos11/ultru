using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	[SerializeField] Animation doorAnim;
	public Transform zero;

	void OnTriggerEnter (Collider col) {
		doorAnim.Play("open");
	}
	
	void OnTriggerExit (Collider obj) {
		doorAnim.Play("close");
	}

	void SetDoorVisibility (bool isVisible) {
		doorAnim.GetComponent<MeshRenderer>().enabled = isVisible;
	}

}
