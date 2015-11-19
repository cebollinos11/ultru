    using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	[SerializeField] Animation doorAnim;
	public Transform zero;

    AudioSource myAudioSource;

    void Start() {
        myAudioSource = GetComponent<AudioSource>();
    }
    

	void OnTriggerEnter (Collider col) {
        GetComponentInParent<Animator>().SetBool("PlayerNearby", true);
        if (!myAudioSource.isPlaying)
            myAudioSource.Play();
	}
	
	void OnTriggerExit (Collider obj) {
        GetComponentInParent<Animator>().SetBool("PlayerNearby", false);
    }

	public void SetDoorVisibility (bool isVisible) {
		doorAnim.GetComponent<MeshRenderer>().enabled = isVisible;
        transform.GetChild(0).GetComponent<BoxCollider>().enabled = isVisible;
	}

    void Update() {
        
    }

}
