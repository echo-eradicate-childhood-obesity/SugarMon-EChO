using UnityEngine;
using UnityEngine.SceneManagement;

public class backButton : MonoBehaviour {
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Main");
        }
    }

    public void GoToMain() {
        SceneManager.LoadScene("Main");

    }

    public void GoToDex() {
        SceneManager.LoadScene("SugarDisk");
    }
}
