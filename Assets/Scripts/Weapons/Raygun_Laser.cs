using UnityEngine;
using System.Collections;

public class Raygun_Laser : MonoBehaviour {

    [SerializeField] float decaySpeed = 1.5f;
    [SerializeField] float width = 0.5f;

    VolumetricLines.VolumetricLineBehavior lightStrip;
    MeshRenderer meshRenderer;

    void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {

        meshRenderer.material.SetFloat("_LineWidth", meshRenderer.material.GetFloat("_LineWidth") - Time.deltaTime * decaySpeed);

        if (meshRenderer.material.GetFloat("_LineWidth") <= 0.1f) {
            Destroy(gameObject);
        }
	}

    public void Initialize(Vector3 laserOrigin, Vector3 laserEnd, Color laserColor) {
        lightStrip = GetComponent<VolumetricLines.VolumetricLineBehavior>();
        lightStrip.StartPos = laserOrigin;
        lightStrip.EndPos = transform.InverseTransformPoint(laserEnd);
        lightStrip.UpdateLineWidth(width);
        lightStrip.UpdateLineColor(laserColor);
    }
}
