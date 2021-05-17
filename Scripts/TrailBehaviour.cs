using UnityEngine;
using System.Collections;

public class TrailBehaviour : MonoBehaviour {
    private TrailRenderer trailComponent;
    private float initialTime;
    private bool tick = false;
	// Use this for initialization
	void Start () {
        trailComponent = GetComponent<TrailRenderer>();
        initialTime = trailComponent.time;
	}
	
	// Update is called once per frame
	void Update () {
        if(tick == true)
        {
            trailComponent.time = initialTime;
            tick = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            trailComponent.time = 0;
            tick = true;
        }
	}
}
