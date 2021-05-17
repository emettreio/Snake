using UnityEngine;
using System.Collections;

public class spawnBehaviour : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player") {
            Destroy(this.gameObject);
        }
    }
}
