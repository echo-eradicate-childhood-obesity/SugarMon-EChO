using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;

[CreateAssetMenu(fileName = "Questions", menuName = "ScriptableObjects/Questions")]
public class Questions : ScriptableObject {
    [System.Serializable]
    public class Question {
        public string prompt;
        public string[] answers = new string[4]; // strings of four options
        public int correct; // 0, 1, 2, 3: indexing the four options row major order
        public Question(string prompt, string[] answers, char correct) {
            prompt = prompt;
            answers = answers;
            correct = correct;
        }
    }
    public List<Question> questions;

    public Question getQuestion() {
        return questions[0];
    }
}
