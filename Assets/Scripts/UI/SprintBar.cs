using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class SprintBar : MonoBehaviour {

    private FirstPersonController player;
    private RectTransform rect;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();
        rect = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        rect.anchorMax = new Vector2(player.remainingStamina / 20, 1);
	}
}
