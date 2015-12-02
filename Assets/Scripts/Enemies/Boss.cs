using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

    [SerializeField] Transform innerSphere;
    [SerializeField] Transform outerSphere;
    [SerializeField] float innerSphereRotationSpeed;
    [SerializeField] float outerSphereRotationSpeed;
    [SerializeField] Transform tinyOrb;
    [SerializeField] Transform[] teleportLocations;
    [SerializeField] GameObject giantAssExplosion;
    [SerializeField] float teleportTime = 15;
    [SerializeField] float scaleTime = 0.5f;

    float teleportTimeRemaining;
    bool allIsDead = false;
    bool isTeleporting = false;
    bool scalingDown = false;
    FinalBossStageController controller;
    // Use this for initialization
    void Start () {
        teleportTimeRemaining = teleportTime;
    }
	
	// Update is called once per frame
	void Update () {
        innerSphere.Rotate(new Vector3(0.5f, 0.8f, 0.2f), innerSphereRotationSpeed * Time.deltaTime, Space.World);
        outerSphere.Rotate(new Vector3(0.1f, 1f, 0.5f), outerSphereRotationSpeed * Time.deltaTime, Space.World);

        if (outerSphere.childCount == 0) {
            AllOrbsDead();
        }

        if (teleportTimeRemaining <= 0) {
            scalingDown = true;
            //StartCoroutine(ScaleOverTime(scaleTime, new Vector3(1, 0, 1)));
            Teleport();
            teleportTimeRemaining = teleportTime;
        }
        else
            teleportTimeRemaining -= Time.deltaTime;
    }

    public void Init(Transform[] locations, FinalBossStageController controller) {
        teleportLocations = locations;
        this.controller = controller;
    }

    void AllOrbsDead() {
        if (allIsDead) return;
        innerSphere.GetComponent<MeshRenderer>().enabled = false;
        innerSphere.GetComponent<SphereCollider>().enabled = false;

        if (tinyOrb == null) {
            Boom();
        }
    }

    void Boom() {
        allIsDead = true;
        Instantiate(giantAssExplosion, transform.position, Quaternion.identity);
        controller.BigBossKilled();
        Destroy(gameObject);
    }

    void Teleport() {
        transform.position = teleportLocations[Random.Range(0, teleportLocations.Length)].position;
        //StartCoroutine(ScaleOverTime(scaleTime, new Vector3(1, 1, 1)));
    }

    IEnumerator ScaleOverTime(float time, Vector3 scaleDestination) {
        if (isTeleporting) yield break;
        isTeleporting = true;
        Vector3 originalScale = transform.localScale;

        float currentTime = 0.0f;

        do {
            transform.localScale = Vector3.Lerp(originalScale, scaleDestination, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
        isTeleporting = false;
        if (scalingDown) {
            scalingDown = false;
            Teleport();
        }
        
    }


}
