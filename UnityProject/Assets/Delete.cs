using UnityEngine;
using UnityEngine.UI;

public class Delete : MonoBehaviour
{
    public Text Text;
    private int previousNumber = -1;

    void Update()
    {
        int currentNumber = FlaskUnityConnector.TransformedNumber;

        // Only update if number has changed
        if (currentNumber != previousNumber)
        {
            Debug.Log("Number changed to: " + currentNumber);
            if (currentNumber == 1) { Text.text = "Recognised gesture : Flexion"; }
            else if (currentNumber == 2) { Text.text = "Recognised gesture : Thumbs Up"; }
            else if (currentNumber == 3) { Text.text = "Recognised gesture : Wave Out"; }
            else if (currentNumber == 4) { Text.text = "Recognised gesture : Wave in"; }
            else if (currentNumber == 5) { Text.text = "Recognised gesture : Fist"; }
            else if (currentNumber == 6) { Text.text = "Recognised gesture : Pointing"; }
            else { Text.text = "No gesture Recognised"; }

            previousNumber = currentNumber;
        }
    }
}