using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
    private Question question;
    private bool wasSkipped;
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
        while (waitCounter < 1.5f && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        GameObject.Find("Option A Button").GetComponentInChildren<Text>().text = question.answerA;

        waitCounter = 0f;
        while (waitCounter < 1.5f && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        GameObject.Find("Option B Button").GetComponentInChildren<Text>().text = question.answerB;

        waitCounter = 0f;
        while (waitCounter < 1.5f && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        GameObject.Find("Option C Button").GetComponentInChildren<Text>().text = question.answerC;

        waitCounter = 0f;
        while (waitCounter < 1.5f && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
        GameObject.Find("Option D Button").GetComponentInChildren<Text>().text = question.answerD;
    }
    // Responds to the press of any button
    public void ButtonClicked(string ButtonLabel) {
        if(ButtonLabel[0] == question.correct) { // correct button pressed
            Debug.Log("Correct!");
        }
        else { // wrong button pressed
            Debug.Log("Wrong");
        }
    }
    // DOES NOT WORK
    IEnumerator waitWithSkip(float seconds) {
        float waitCounter = 0f;
        while (waitCounter < seconds && wasSkipped == false) {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) wasSkipped = true; // if screen tapped, end timer
            yield return null;
            waitCounter += Time.deltaTime; // update timer
        }
    }
}
