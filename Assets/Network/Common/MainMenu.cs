using TMPro;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private MenuState State = MenuState.HostMenu;

    [SerializeField]
    private Button HostButton;
    [SerializeField]
    private Button JoinButton;
    [SerializeField]
    private Button MainButton;
    [SerializeField]
    public Button ExitButton;


    [SerializeField]
    public Button HostConfirm;

    [SerializeField]
    public Button JoinConfirm;

    [SerializeField]
    public TMP_InputField JoinIP;

    private string JoinIPText;
    [SerializeField]
    private Scene subScene; // Добавьте ссылку на вашу подсцену
    private bool IsHidden = false;
    void Start()
    {
        ChangeState(MenuState.HostMenu);
        JoinButton.onClick.AddListener(() => OnButtonClick(MenuState.JoinMenu));
        HostButton.onClick.AddListener(() => OnButtonClick(MenuState.HostMenu));
        HostConfirm.onClick.AddListener(() => OnHostConfirm());
        JoinConfirm.onClick.AddListener(() => OnJoinConfirm());
    }

    private void OnHostConfirm()
    {
        
        StartHost();
    }
    private void Awake()
    {
        Application.runInBackground = true;
    }
    private void StartHost()
    {
        var server = ClientServerBootstrap.CreateServerWorld("ServerWorld");
        var client = ClientServerBootstrap.CreateClientWorld("ClientWorld");
        DestroyDefaultWorld();

        //Destroy the local simulation world to avoid the game scene to be loaded into it
        //This prevent rendering (rendering from multiple world with presentation is not greatly supported)
        //and other issues.
        if (World.DefaultGameObjectInjectionWorld == null)
            World.DefaultGameObjectInjectionWorld = server;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);


        NetworkEndpoint ep = NetworkEndpoint.AnyIpv4.WithPort(7777);
        {
            using var drvQuery = server.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
            drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(ep);
        }

        ep = NetworkEndpoint.Parse("26.106.161.104", 7777);
        {
            using var drvQuery = client.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
            drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(client.EntityManager, ep);
        }

    }
    private void StartClient()
    {
        var client = ClientServerBootstrap.CreateClientWorld("ClientWorld");

        if (World.DefaultGameObjectInjectionWorld == null)
            World.DefaultGameObjectInjectionWorld = client;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        var ep = NetworkEndpoint.Parse("26.106.161.104", 7777);
        {
            using var drvQuery = client.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
            drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(client.EntityManager, ep);
        }

    }
    private void DestroyDefaultWorld()
    {
        foreach (var world in World.All)
        {
            if (world.Flags == WorldFlags.Game)
            {
                world.Dispose();
                break;
            }
        }
    }

    private void OnJoinConfirm()
    {
        DestroyDefaultWorld();
        StartClient();
    }

    private void ChangeState(MenuState state)
    {
        State = state;
        switch (State)
        {
            case MenuState.JoinMenu:
                JoinMenu.SetActive(true);
                HostMenu.SetActive(false);
                break;
            case MenuState.HostMenu:
                HostMenu.SetActive(true);
                JoinMenu.SetActive(false);
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {         
            IsHidden = !IsHidden;
            Menu.SetActive(IsHidden);
        }
    }
}
public enum MenuState
{
    JoinMenu = 2,
    HostMenu = 3
}
