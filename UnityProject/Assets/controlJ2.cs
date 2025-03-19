using System;
using UnityEngine;

public class controlJ2 : MonoBehaviour
{
    private int data;
    void Update()
    {
        if (UDPReceive.used1 || UDPReceive.datareceived == null || UDPReceive.prevData == null) return;
        data = Int32.Parse(UDPReceive.datareceived);
        int prevData = Int32.Parse(UDPReceive.prevData);
        if (data == -1 || prevData==-1) return;
        UDPReceive.used1 = true;
        if (prevData == 0 && data == 1) { transform.Rotate(15, 0, 0); }
        if (prevData == 0 && data == 2) { transform.Rotate(-15, 0, 0); }
        
    }
}
