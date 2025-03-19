using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Transform parentObject;
    public Transform[] allChildren;
    void Start()
    {
        allChildren = new Transform[parentObject.transform.childCount];
        for (int i = 0; i < allChildren.Length; i++)
        {
            allChildren[i] = parentObject.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
