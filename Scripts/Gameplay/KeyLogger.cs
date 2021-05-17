using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyLogger : MonoBehaviour {
    private List<float> Timestamps;
    private float counter;
	// Use this for initialization
	void Start () {
        Timestamps = new List<float>();
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if(Input.GetButtonDown("Right") || Input.GetButtonDown("Left"))
        {
            Timestamps.Add(counter);
        }
	}

    void OnDestroy()
    {
        for(int i = 1; i < Timestamps.Count; i++)
        {
            Debug.Log((Timestamps[i] - Timestamps[i - 1]));
        }
    }
}
