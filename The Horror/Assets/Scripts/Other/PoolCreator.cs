using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCreator : MonoBehaviour {

    public int Count;
    public GameObject Obj;

	void Awake ()
    {
        for (int i = 0; i < Count; i++)
        {
            GameObject obj = Instantiate(Obj, transform);
            obj.SetActive(false);
        }
	}
	
	
}
