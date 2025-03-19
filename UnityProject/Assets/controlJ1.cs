using System;
using UnityEngine;

public class controlJ1 : MonoBehaviour
{
    private int data;
    void Update()
    {
        
        if (UDPReceive.used2 || UDPReceive.datareceived==null || UDPReceive.prevData==null) return;
        data = Int32.Parse(UDPReceive.datareceived);
        int prevData = Int32.Parse(UDPReceive.prevData);
        if (data == -1 || prevData == -1) return;
        UDPReceive.used2 = true;
        if ( prevData == 0 && data == 3 ) { transform.Rotate(0, 15, 0); }
        if ( prevData == 0 && data == 4 ) { transform.Rotate(0, -15, 0); }
    }
}
