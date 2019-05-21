using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour {
    private const float TimeBetweenLetters = 0.02f; // time between each letter for typewriter text effect
    private const float TimeBetweenWords = 0.7f; // time between each word fade appears
    private bool wasSkipped; // this is true when one animation is skipped so that all other animations are skipped as well
    [SerializeField]

    public Questions questions; // current quiz question object list, including prompt, answers, correct answer values
    public Questions.Question question; // current question

    private Text prompt; // question/prompt at the top of the screen
    private Text textA; // top-left
    private Text textB; // top-right
    private Text textC; // bottom-left
    private Text textD; // bottom-right
    private int correct; // Correct answer
    private Image resultBox; // Correct/incorret result card that appears after selection
    private Text resultText;

    // private Color Default = new Color(255, 255, 255, 255);
    private Color32 CorrectColor = new Color32(155, 255, 155, 255);
    private Color32 IncorrectColor = new Color32(255, 165, 165, 255);

    // Initialize Quiz with the first question
    IEnumerator Start() {
        // initializaitons
        resultBox = GameObject.Find("Result Box").GetComponentInChildren<Image>();
        resultText = GameObject.Find("Result Text").GetComponentInChildren<Text>();
        resultBox.enabled = false;
        resultText.enabled = false;
        wasSkipped = false;


        question = questions.questions[0];
        correct = question.correct;

        // "typewriter" text appearing one letter at a time effect
        for(int i = 0; i <= question.prompt.Length; i++) {
            GameObject.Find("Current Question Text Box").GetComponentInChildren<Text>().text = question.prompt.Substring(0, i);
            if(wasSkipped == false) yield return StartCoroutine(TimerWithSkip(TimeBetweenLetters));
            Debug.Log("AA");
        }

        // button initializations and fade in effects
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
        if(int.Parse(ButtonLabel) == correct) { // character "correct" matches the button letter pressed (A, B, C, D)
            Debug.Log("Correct!");
            resultBox.color = CorrectColor;
            resultText.text = "Correct!";
        }
        else { // wrong button pressed
            resultBox.color = IncorrectColor;
            resultText.text = "Not Quite!";
            Debug.Log("Wrong");

        }
        resultBox.enabled = true;
        resultText.enabled = true;

        StartCoroutine(TimerWithSkip(2.0f));
        SceneManager.LoadScene("Main");
    }

    // Waits for timer amount of time unless the mouse is pressed or screen is tapped
    public IEnumerator TimerWithSkip(float timer) {
        float waitCounter = 0f;
        while (waitCounter < timer && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
    }

    // Text fade in effect for timer time with tap or press to skip
    public IEnumerator FadeTextIn(float timer, Text text) {
        float waitCounter = 0f;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (waitCounter < timer && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (waitCounter / timer));
            yield return null;
            waitCounter += Time.deltaTime;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 255);
    }
}
