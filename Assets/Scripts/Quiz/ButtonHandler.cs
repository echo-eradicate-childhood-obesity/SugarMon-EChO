using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
    private Question question;
    // Initialize Quiz with the first question
    void Start() {
        question = new Question();
        GameObject.Find("Current Question Text Box").GetComponentInChildren<Text>().text = question.prompt;
        GameObject.Find("Option A Button").GetComponentInChildren<Text>().text = question.answerA;
        GameObject.Find("Option B Button").GetComponentInChildren<Text>().text = question.answerB;
        GameObject.Find("Option C Button").GetComponentInChildren<Text>().text = question.answerC;
        GameObject.Find("Option D Button").GetComponentInChildren<Text>().text = question.answerD;
    }
    // Sets the text of Option A Button
    public void ButtonClicked(string ButtonLabel) {
        if(ButtonLabel[0] == question.correct) {
            Debug.Log("Correct!");
        }
        else {
            Debug.Log("Wrong");
        }
    }
}
