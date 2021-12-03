using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;
    public TextMeshProUGUI dialogueText;
    public RubyController player;

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
    }

    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        if (player.getLevel() == 1 && player.robotsToFix == 0)
        {
            dialogueText.text = "Good job. Think you can fix some more?";
            StartCoroutine(LoadNextLevel(3, "Level2"));
        }
    }

    IEnumerator LoadNextLevel(float time, string scene)
    {
        yield return new WaitForSeconds(time);
        player.setLevel(2);
        SceneManager.LoadScene(scene);
    }
}
