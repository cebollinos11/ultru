    using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	[SerializeField] Animation doorAnim;
	public Transform zero;

    AudioSource[] myAudioSource;
    int peopleOnCollider = 0;

    void Awake() {
        myAudioSource = GetComponents<AudioSource>();
    }
    

	void OnTriggerEnter (Collider col) {
        if (col.isTrigger) return;
        peopleOnCollider++;

        if (peopleOnCollider == 1)
        {
            GetComponentInParent<Animator>().SetBool("PlayerNearby", true);
            if (!myAudioSource[0].isPlaying)
                myAudioSource[0].Play();
        }

        
	}
	
	void OnTriggerExit (Collider col) {
        if (col.isTrigger) return;
        peopleOnCollider--;
        if (peopleOnCollider == 0)
        {
            GetComponentInParent<Animator>().SetBool("PlayerNearby", false);
            if (!myAudioSource[1].isPlaying)
                myAudioSource[1].Play();
        }
        
    }

	public void SetDoorVisibility (bool isVisible) {
		doorAnim.GetComponent<MeshRenderer>().enabled = isVisible;
        transform.GetChild(0).GetComponent<BoxCollider>().enabled = isVisible;
	}

    void Update() {
        
    }

}
