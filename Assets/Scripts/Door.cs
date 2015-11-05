using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	[SerializeField] Animation doorAnim;
	public Transform zero;


	void OnTriggerEnter (Collider col) {
        GetComponentInParent<Animator>().SetBool("PlayerNearby", true);
	}
	
	void OnTriggerExit (Collider obj) {
        GetComponentInParent<Animator>().SetBool("PlayerNearby", false);
    }

	public void SetDoorVisibility (bool isVisible) {
		doorAnim.GetComponent<MeshRenderer>().enabled = isVisible;
	}

    void Update() {
        
    }

}
