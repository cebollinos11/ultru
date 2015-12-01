using UnityEngine;
using System.Collections;

public class Interactable_weapon_pickup : Interactable {

    


    public override void DoHighlight()
    {
        base.DoHighlight();

        
        GUIManager.Instance.interactableLabelText.text = "Press E to pick up";        

    }
    public override void DoDeHighlight()
    {
        base.DoDeHighlight();
        GUIManager.Instance.interactableLabelText.text = "";
    }
    public override void DoInteractButtonDown()
    {
        base.DoInteractButtonDown();
        GetComponent<Weapon_Pickup>().PickUp();

    }


    public override void DoInteractButtonUp()
    {
        base.DoInteractButtonUp();
    }


}
