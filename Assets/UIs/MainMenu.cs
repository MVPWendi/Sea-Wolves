using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject Menu;
    [SerializeField]
    private GameObject JoinMenu;
    [SerializeField]
    private GameObject HostMenu;

    private MenuState State = MenuState.MainMenu;

    [SerializeField]
    private Button HostButton;
    [SerializeField]
    private Button JoinButton;
    [SerializeField]
    private Button MainButton;



    [SerializeField]
    private Button HostConfirm;

    [SerializeField]
    private Button JoinConfirm;

    [SerializeField]
    private TMP_InputField JoinIP;

    private string JoinIPText;
    void Start()
    {
        ChangeState(MenuState.MainMenu);
        HostButton.onClick.AddListener(() => OnButtonClick(MenuState.HostMenu));
        MainButton.onClick.AddListener(() => OnButtonClick(MenuState.MainMenu));
        JoinButton.onClick.AddListener(() => OnButtonClick(MenuState.JoinMenu));
    }
    private void ChangeState(MenuState state)
    {
        State = state;
        switch (State)
        {
            case MenuState.MainMenu:
                Menu.SetActive(true);
                JoinMenu.SetActive(false);
                HostMenu.SetActive(false);
                break;
            case MenuState.JoinMenu:
                JoinMenu.SetActive(true);
                HostMenu.SetActive(false);
                Menu.SetActive(false);
                break;
            case MenuState.HostMenu:
                HostMenu.SetActive(true);
                JoinMenu.SetActive(false);
                Menu.SetActive(false);
                break;
        }
    }
    
    public void OnText()
    {
        JoinIPText = JoinIP.text;
        Debug.Log($"Entered IPAdress: {JoinIPText}");
    }
    private void OnButtonClick(MenuState state)
    {
        ChangeState (state);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum MenuState
{
    MainMenu = 1,
    JoinMenu = 2,
    HostMenu = 3
}
