using UnityEngine;
using System.Collections;

public class Interactable_HealthTerminal : Interactable {

    LifeManager_Player lifePlayer;
    bool used;

	// Use this for initialization
	void Start () {
        lifePlayer = GameObject.Find("Player").GetComponent<LifeManager_Player>();
	}

    public override void DoHighlight()
    {
        base.DoHighlight();
        if (used) return;
        GUIManager.Instance.interactableLabelText.text = "Press E to heal";
        
    }
    public override void DoDeHighlight()
    {
        base.DoDeHighlight();
        GUIManager.Instance.interactableLabelText.text = "";
        
    }
    public override void DoInteractButtonDown()
    {

        if (used) return;
        base.DoInteractButtonDown();
        lifePlayer.Heal(50);
        used = true;
        GetComponentInChildren<TextureTiler>().speed = 0.1f;

    }
    public override void DoInteractButtonUp()
    {
        base.DoInteractButtonUp();
        
    }

}
