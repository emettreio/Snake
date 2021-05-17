using UnityEngine;
using System.Collections;

public class PowerUpHandler : MonoBehaviour {
    public Transform followTarget;
    public Vector3 targetScale;
    private Vector3 initialScale;

    private bool follow = true;

    private bool grow = false;
    private bool shrink = false;
	// Use this for initialization
	void Start () {
        initialScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        if (grow)
            transform.localScale = Vector3.Lerp(transform.localScale, initialScale, Time.deltaTime);
        if (shrink)
            Debug.Log("Shrinking");
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime);
        if (follow)
            transform.position = followTarget.position;
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            follow = true;
            grow = true;
            shrink = false;
        }
    }

    public void Detach()
    {
        follow = false;
        shrink = true;
        grow = false;
    }
}
