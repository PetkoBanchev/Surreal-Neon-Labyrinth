using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject InstructionsPanel;
    private void Start()
    {
        HideInstructions();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void PlayGame2()
    {
        SceneManager.LoadScene(2);
    }
    public void PlayGame3()
    {
        SceneManager.LoadScene(3);
    }
    public void PlayGame4()
    {
        SceneManager.LoadScene(4);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void HideInstructions()
    {
        InstructionsPanel.SetActive(false);
    }
    public void ShowInstructions()
    {
        InstructionsPanel.SetActive(true);
    }
}
