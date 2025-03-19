using UnityEngine;
using UnityEngine.UI;

public class TextFieldExample : MonoBehaviour
{
    public Text myTextField;
    public GameObject joint1, joint2, joint5;

    void Update()
    {
        myTextField.text = "J1: X = " + joint1.transform.eulerAngles.x.ToString("F2") +
                           ", Y = " + joint1.transform.eulerAngles.y.ToString("F2") +
                           ", Z = " + joint1.transform.eulerAngles.z.ToString("F2") + "\n" +
                           "J2: X = " + joint2.transform.eulerAngles.x.ToString("F2") +
                           ", Y = " + joint2.transform.eulerAngles.y.ToString("F2") +
                           ", Z = " + joint2.transform.eulerAngles.z.ToString("F2") + "\n" +
                           "J5: X = " + joint5.transform.eulerAngles.x.ToString("F2") +
                           ", Y = " + joint2.transform.eulerAngles.y.ToString("F2") +
                           ", Z = " + joint2.transform.eulerAngles.z.ToString("F2");
    }

    public void UpdateText(string newText)
    {
        myTextField.text = newText;
    }
}
