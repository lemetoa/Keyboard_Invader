using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {

    [SerializeField]
    float destroyTime;
    private void OnEnable()
    {
        Destroy(this, destroyTime);
    }
    void Update ()
	{

		// if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C))
		//    Destroy(transform.gameObject);
	
	}
}
