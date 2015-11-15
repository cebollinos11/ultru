using UnityEngine;
using System.Collections;

public class Raygun_Laser : MonoBehaviour {

    [SerializeField] float decayTime = 1.5f;
    [SerializeField] float width = 0.5f;

    VolumetricLines.VolumetricLineBehavior lightStrip;
    float decayTimeRemaining = 0;
    Color laserColor;

    void Start() {
        decayTimeRemaining = decayTime;
    }
	
	// Update is called once per frame
	void Update () {
        decayTimeRemaining -= Time.deltaTime;
        laserColor.a = ((1 / decayTime) * decayTimeRemaining);
        lightStrip.UpdateLineColor(laserColor); 
	}

    public void Initialize(Vector3 laserOrigin, Vector3 laserEnd, Color laserColor) {
        lightStrip = GetComponent<VolumetricLines.VolumetricLineBehavior>();
        this.laserColor = laserColor;
        lightStrip.StartPos = laserOrigin;
        lightStrip.EndPos = transform.InverseTransformPoint(laserEnd);
        lightStrip.UpdateLineWidth(width);
        lightStrip.UpdateLineColor(laserColor);
    }
}
