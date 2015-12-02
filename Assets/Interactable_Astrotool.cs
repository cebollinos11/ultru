using UnityEngine;
using System.Collections;

public class Interactable_Astrotool : Interactable {

    float astroStep = 0.1f;
    bool used;

	// Use this for initialization
	void Start () {
	
	}

    public override void DoHighlight()
    {
        base.DoHighlight();        
            GUIManager.Instance.interactableLabelText.text = "Press E to use strange mechanism";
            if (used) {
                GUIManager.Instance.interactableLabelText.text = "";
            }

    }
    public override void DoDeHighlight()
    {
        base.DoDeHighlight();
        GUIManager.Instance.interactableLabelText.text = "";
    }
    public override void DoInteractButtonDown()
    {
        base.DoInteractButtonDown();
        if (used) return;

        used = true;
        StartCoroutine(ScaleOverTime(1));
        
    }


    public override void DoInteractButtonUp()
    {
        base.DoInteractButtonUp();
    }


    IEnumerator ScaleOverTime(float time)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 destinationScale = new Vector3(0.0f, 0.0f, 0.0f);

        float currentTime = 0.0f;

        do
        {
            transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);

        GameObject.Find("FinalBossStageController").GetComponent<FinalBossStageController>().SpawnBoss();          


        Destroy(gameObject);
    }



}
