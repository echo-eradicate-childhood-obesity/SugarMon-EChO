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
        wasSkipped = false;

        question = new Question();
        correct = question.correct;

        for(int i = 0; i <= question.prompt.Length; i++) {
            GameObject.Find("Current Question Text Box").GetComponentInChildren<Text>().text = question.prompt.Substring(0, i);
            if(wasSkipped == false) yield return StartCoroutine(TimerWithSkip(TimeBetweenLetters));
        }

        textA = GameObject.Find("A Button").GetComponentInChildren<Text>();
        textA.text = question.answers[0];
        yield return StartCoroutine(FadeTextIn(TimeBetweenWords, textA));

        textB = GameObject.Find("B Button").GetComponentInChildren<Text>();
        textB.text = question.answers[1];
        yield return StartCoroutine(FadeTextIn(TimeBetweenWords, textB));

        textC = GameObject.Find("C Button").GetComponentInChildren<Text>();
        textC.text = question.answers[2];
        yield return StartCoroutine(FadeTextIn(TimeBetweenWords, textC));

        textD = GameObject.Find("D Button").GetComponentInChildren<Text>();
        textD.text = question.answers[3];
        yield return StartCoroutine(FadeTextIn(TimeBetweenWords, textD));
    }
    // Responds to the press of any button
    public void ButtonClicked(string ButtonLabel) {
        if(ButtonLabel[0] == correct) { // correct button pressed
            Debug.Log("Correct!");

        }
        else { // wrong button pressed
            Debug.Log("Wrong");

        }
        Debug.Log(correct);
        Debug.Log(ButtonLabel);
    }
    public IEnumerator TimerWithSkip(float timer) {
        float waitCounter = 0f;
        while (waitCounter < timer && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
    }
    public IEnumerator FadeTextIn(float timer, Text text) {
        float alpha = text.color.a;
        float waitCounter = 0f;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (waitCounter < timer && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (waitCounter / timer));
            yield return null;
            waitCounter += Time.deltaTime;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }
}
