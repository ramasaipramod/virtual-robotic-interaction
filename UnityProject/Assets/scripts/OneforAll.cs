using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class OneforAll : MonoBehaviour
{
    //private int previousNumber = -1;
    public GameObject J1, J2, J5;

    void Update()
    {
        int currentNumber = FlaskUnityConnector.TransformedNumber;
        if (!FlaskUnityConnector.used)
        {
            if (currentNumber == 1) { J1.transform.Rotate(0, 15, 0); }
            if (currentNumber == 2) { J1.transform.Rotate(0, -15, 0); }
            if (currentNumber == 3) { J2.transform.Rotate(15, 0, 0); }
            if (currentNumber == 4) { J2.transform.Rotate(-15, 0, 0); }  
            if (currentNumber == 5) { J5.transform.Rotate(15, 0, 0); }
            if (currentNumber == 6) { J5.transform.Rotate(-15, 0, 0); }

        }
        FlaskUnityConnector.used = true;
        //previousNumber = currentNumber;
    }
}
