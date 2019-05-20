using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
    private const float TimeBetweenLetters = 0.02f; // time between each letter for typewriter text effect
    private const float TimeBetweenWords = 0.7f; // time between each word fade appears



    private Question question; // current quiz question object, including prompt, answers, correct answer values

    private bool wasSkipped; // was the mouse pressed to skip all into animations?
    private Text prompt; // question/prompt at the top of the screen
    private Text textA; // top-left
    private Text textB; // top-right
    private Text textC; // bottom-left
    private Text textD; // bottom-right
    private char correct; // Correct answer

    private GameObject result;

    // private Color Default = new Color(255, 255, 255, 255);
     private Color CorrectColor = new Color(155, 255, 155, 255);
     private Color IncorrectColor = new Color(255, 165, 165, 255);

    // Initialize Quiz with the first question
    IEnumerator Start() {
        result = GameObject.Find("Result Box");
        result.SetActive(false);
        //result.color = new Color(result.color.r, result.color.g, result.color.b, 0);
        question = new Question();
        wasSkipped = false;
        string choiceA = question.answers[0];
        //if(correctString == choiceA) correct = 'A';
        string choiceB = question.answers[1];
        //if(correctString == choiceB) correct = 'B';
        string choiceC = question.answers[2];
        //if(correctString == choiceC) correct = 'C';
        string choiceD = question.answers[3];
        //if(correctString == choiceD) correct = 'D';
        correct = question.correct;
        float waitCounter;
        for(int i = 0; i <= question.prompt.Length; i++) {
            GameObject.Find("Current Question Text Box").GetComponentInChildren<Text>().text = question.prompt.Substring(0, i);
            waitCounter = 0f;
            while (waitCounter < TimeBetweenLetters && wasSkipped == false) {
                if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
                yield return null;
                waitCounter += Time.deltaTime; // update timer
            }
        }
        waitCounter = 0f;
        while (waitCounter < TimeBetweenWords && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        textA = GameObject.Find("A Button").GetComponentInChildren<Text>();
        textA.text = choiceA;
        StartCoroutine(FadeTextIn(2f, textA));

        waitCounter = 0f;
        while (waitCounter < TimeBetweenWords && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        textB = GameObject.Find("B Button").GetComponentInChildren<Text>();
        textB.text = choiceB;
        StartCoroutine(FadeTextIn(2f, textB));

        waitCounter = 0f;
        while (waitCounter < TimeBetweenWords && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        textC = GameObject.Find("C Button").GetComponentInChildren<Text>();
        textC.text = choiceC;
        StartCoroutine(FadeTextIn(2f, textC));

        waitCounter = 0f;
        while (waitCounter < TimeBetweenWords && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        textD = GameObject.Find("D Button").GetComponentInChildren<Text>();
        textD.text = choiceD;
        StartCoroutine(FadeTextIn(2f, textD));
    }
    // Responds to the press of any button
    public void ButtonClicked(string ButtonLabel) {
        if(ButtonLabel[0] == correct) { // correct button pressed
            Debug.Log("Correct!");
            // Button tempButton = GameObject.Find(ButtonLabel + " Button").GetComponentInChildren<Button>();
            // ColorBlock newColors = tempButton.colors;
            // newColors.normalColor = Correct;
            // tempButton.colors = newColors;
        }
        else { // wrong button pressed
            Debug.Log("Wrong");
            // Button tempButton = GameObject.Find(ButtonLabel + " Button").GetComponentInChildren<Button>();
            // ColorBlock newColors = tempButton.colors;
            // newColors.normalColor = Incorrect;
            // tempButton.colors = newColors;
        }
    }

    public IEnumerator FadeTextIn(float timer, Text text) {
        float waitCounter = 0f;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (waitCounter < timer) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (waitCounter / timer));
            yield return null;
            waitCounter += Time.deltaTime;
        }
    }
}
