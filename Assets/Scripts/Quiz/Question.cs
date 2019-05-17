
public class Question {
    public string prompt{get; set;}
    public string answerA{get; set;}
    public string answerB{get; set;}
    public string answerC{get; set;}
    public string answerD{get; set;}
    public char correct{get; set;}

    public Question() {
        generateQuestion();
    }
    private void generateQuestion() {
        prompt = "Which of the following is NOT a type of sugar?";
        answerA = "Canes";
        answerB = "Concentrates";
        answerC = "Extracts";
        answerD = "OSEs";
        correct = 'C';
    }
}
