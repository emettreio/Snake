using UnityEngine;
using System.Collections;

public class ThemeBehaviour : MonoBehaviour {
    public string themeName;
    public GameObject selectionObject;
    private ShopMenuBehaviour selectionHandler;
	// Use this for initialization
	void Start () {
        selectionHandler = selectionObject.GetComponent<ShopMenuBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.name == "Center")
        {
            Debug.Log("AHHHH I HAVE BEEN HIT");
            selectionHandler.ChangeSelectedTheme(themeName);
        }
    }
}
