using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using static Cinemachine.DocumentationSortingAttribute;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _uiGroup;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Sprite E_Sprite;
    [SerializeField] private Sprite F_Sprite;
    [SerializeField] private Image GrabVisual;
    [SerializeField] private GameObject GameOverUI;
    private bool _hasDied;
    private bool _uiSwitch;
    public bool GetUISwitch()
    {
        return _uiSwitch;
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        SetGrabVisual(false);
        SetPauseVisual(false);
    }

    public void HasDied()
    {
        _hasDied = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameOverUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasDied)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton10) || Input.GetKeyDown(KeyCode.Escape))
            {
                _uiSwitch = !_uiSwitch;
                if (_uiSwitch)
                {
                    SetPauseVisual(true);
                }
                else
                {
                    SetPauseVisual(false);
                }

            }
        }
    }

    public void SetText(string input)
    {
        _text.text = input;
    }

    public void EorF(bool set)
    {
        if (set)
        {
            GrabVisual.sprite = E_Sprite;
        }
        else
        {
            GrabVisual.sprite = F_Sprite;
        }
    }

    public void SetGrabVisual(bool value)
    {    
         _uiGroup.SetActive(value); 
    }

    public void SetPauseVisual(bool value)      
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = value;
        _pauseMenuUI.SetActive(value);
    }

    public void ResumeButton()
    {
        SetPauseVisual(false);
    }

    public void ExitButton()
    {
        StartCoroutine(DoDisconnect());
    }
    IEnumerator DoDisconnect()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene("Lobby");
    }
}
