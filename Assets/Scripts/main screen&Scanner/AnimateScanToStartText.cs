using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimateScanToStartText : MonoBehaviour
{
    //Time taken for each letter to appear (The lower it is, the faster each letter appear)
    public float finishPaused = 1.0f, letterPaused = 0.1f;
    //Message that will displays till the end that will come out letter by letter
    public string message;
    //Text for the message to display
    public Text textComp;


    // Use this for initialization
    void Start()
    {
        //Get text component
        textComp = GetComponent<Text>();
        //Message will display will be at Text
        message = textComp.text;
        //Set the text to be blank first
        textComp.text = "";
        //Call the function and expect yield to return
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        //Split each char into a char array
        foreach (char letter in message.ToCharArray())
        {
            //Add 1 letter each
            textComp.text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterPaused);
        }
        yield return new WaitForSeconds(finishPaused);
        this.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
        StartCoroutine(DeleteText());
    }
    IEnumerator DeleteText()
    {
        foreach(char letter in message.ToCharArray())
        {
            textComp.text = textComp.text.Remove(0, 1);
            yield return 0;
            yield return new WaitForSeconds(letterPaused);
        }
        yield return new WaitForSeconds(finishPaused);
        this.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
        StartCoroutine(TypeText());
    }
}