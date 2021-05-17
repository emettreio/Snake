using UnityEngine;
using System.Collections;

public class ActivateOnTimer : MonoBehaviour {

    public float timeUntilStart;

    private float timer;
    private Collider2D m_collider;
	// Use this for initialization
	void Start () {
        m_collider = GetComponent<Collider2D>();
        m_collider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer > timeUntilStart)
        {
            m_collider.enabled = true;
            this.enabled = false;
        }
	}
}
