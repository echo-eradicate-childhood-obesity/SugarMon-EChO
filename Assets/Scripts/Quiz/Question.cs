
public class Question {
    public string prompt{get; set;}
    public string[] answers{get; set;}
    public char correct{get; set;}

    public Question() {
        answers = new string[4];
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
