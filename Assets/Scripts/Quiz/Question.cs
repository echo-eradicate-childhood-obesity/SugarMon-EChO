
public class Question {
    public string prompt{get; set;}
    public string[] answers = new string[4];
    public char correct{get; set;}

    public Question() {
        this.generateQuestion();
    }
    private void generateQuestion() {
        prompt = "Which of the following is NOT a type of sugar?";
        answers[0] = "Canes";
        answers[1] = "Concentrates";
        answers[2] = "Extracts";
        answers[3] = "OSEs";
        correct = 'C';
    }
}
