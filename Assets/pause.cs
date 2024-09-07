using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pause : MonoBehaviour
{
    [SerializeField] Sprite playBtn;
    [SerializeField] Sprite pauses;
    [SerializeField] GameObject pauseBtn;
    [SerializeField] GameObject gameInfo;
    private bool isPlayng;

    private void Start()
    {
        isPlayng = true;

    }

    public void pauseGame()
    {
        if (isPlayng) {
            Time.timeScale = 0;
            isPlayng = false;
            if (pauseBtn != null && playBtn != null)
            {
                pauseBtn.GetComponent<Image>().sprite = playBtn;
            }
            
        }
        else
        {
            Time.timeScale = 1.0f;
            isPlayng = true;
            if (pauseBtn != null)
            {

                pauseBtn.GetComponent<Image>().sprite = pauses;
            }
        }
    }

    public void showInfo()
    {
        Time.timeScale = 0;
        gameInfo.SetActive(true);

    }

    public void closeInfo()
    {
        Time.timeScale = 1.0f;
        gameInfo.SetActive(false);
    }
}
