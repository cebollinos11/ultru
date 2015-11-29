using UnityEngine;
using System.Collections;

public class Ragdoll_Release : MonoBehaviour {

    [SerializeField] Transform[] toRelease;

	// Use this for initialization
	void Start () {
        foreach (Transform t in toRelease) {
            t.parent = null;
        }
        Destroy(gameObject);
	}

}
