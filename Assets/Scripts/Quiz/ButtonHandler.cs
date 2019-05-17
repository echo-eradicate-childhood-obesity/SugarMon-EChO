using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
    private Question question;
    private bool wasSkipped;
    private Text prompt;
    private Text textA;
    private Text textB;
    private Text textC;
    private Text textD;

    private Color Default = new Color(255, 255, 255, 255);
    private Color Correct = new Color(155, 255, 155, 255);
    private Color Incorrect = new Color(255, 155, 155, 255);

    // Initialize Quiz with the first question
    IEnumerator Start() {
        question = new Question();
        wasSkipped = false;
        float waitCounter;
        for(int i = 0; i <= question.prompt.Length; i++) {
            GameObject.Find("Current Question Text Box").GetComponentInChildren<Text>().text = question.prompt.Substring(0, i);
            waitCounter = 0f;
            while (waitCounter < 0.05f && wasSkipped == false) {
                if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
                yield return null;
                waitCounter += Time.deltaTime; // update timer
            }
        }
        waitCounter = 0f;
        while (waitCounter < 1f && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        textA = GameObject.Find("A Button").GetComponentInChildren<Text>();
        textA.text = question.answerA;
        StartCoroutine(FadeTextIn(2f, textA));

        waitCounter = 0f;
        while (waitCounter < 1f && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        textB = GameObject.Find("B Button").GetComponentInChildren<Text>();
        textB.text = question.answerB;
        StartCoroutine(FadeTextIn(2f, textB));

        waitCounter = 0f;
        while (waitCounter < 1f && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        textC = GameObject.Find("C Button").GetComponentInChildren<Text>();
        textC.text = question.answerC;
        StartCoroutine(FadeTextIn(2f, textC));

        waitCounter = 0f;
        while (waitCounter < 1f && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        textD = GameObject.Find("D Button").GetComponentInChildren<Text>();
        textD.text = question.answerD;
        StartCoroutine(FadeTextIn(2f, textD));
    }
    // Responds to the press of any button
    public void ButtonClicked(string ButtonLabel) {
        if(ButtonLabel[0] == question.correct) { // correct button pressed
            Debug.Log("Correct!");
            Button tempButton = GameObject.Find(ButtonLabel + " Button").GetComponentInChildren<Button>();
            ColorBlock newColors = tempButton.colors;
            newColors.normalColor = Correct;
            tempButton.colors = newColors;
        }
        else { // wrong button pressed
            Debug.Log("Wrong");
            Button tempButton = GameObject.Find(ButtonLabel + " Button").GetComponentInChildren<Button>();
            ColorBlock newColors = tempButton.colors;
            newColors.normalColor = Incorrect;
            tempButton.colors = newColors;
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
