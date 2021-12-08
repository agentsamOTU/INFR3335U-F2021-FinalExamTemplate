using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkSetup : MonoBehaviourPunCallbacks
{
    public GameObject lobbyPanel;
    private InputField createField;
    private InputField joinField;
    private InputField nameField;
    public GameObject roomPanel;
    private Text roomName;
    private Text playerList;
    private Text masterNotice;
    private VerticalLayoutGroup content;
    private Button startButton;
    private List<GameObject> playerNames= new List<GameObject>();
    public GameObject namePrefab;

    // Start is called before the first frame update
    void Start()
    {
        lobbyPanel.SetActive(true);
        createField = lobbyPanel.GetComponentsInChildren<InputField>()[0];
        joinField = lobbyPanel.GetComponentsInChildren<InputField>()[1];
        nameField = lobbyPanel.GetComponentsInChildren<InputField>()[2];
        roomPanel.SetActive(false);
        roomName = roomPanel.GetComponentsInChildren<Text>()[0];
        masterNotice = roomPanel.GetComponentsInChildren<Text>()[1];
        startButton = roomPanel.GetComponentInChildren<Button>();
        content = roomPanel.GetComponentInChildren<VerticalLayoutGroup>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(nameField.text))
            PhotonNetwork.NickName = nameField.text;
        if (string.IsNullOrEmpty(createField.text))
            return;
        PhotonNetwork.CreateRoom(createField.text);
    }
    public void JoinRoom()
    {
        if (!string.IsNullOrEmpty(nameField.text))
            PhotonNetwork.NickName = nameField.text;
        PhotonNetwork.JoinRoom(joinField.text);
    }

    public override void OnJoinedRoom()
    {
        PanelToggle();
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        MasterCheck();
        UpdateList();
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateList();
        MasterCheck();
        base.OnPlayerEnteredRoom(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateList();
        MasterCheck();
        base.OnPlayerLeftRoom(otherPlayer);
    }
    private void UpdateList()
    {
        for (int i = 0; i < playerNames.Count; i++)
        {
            playerNames[i].SetActive(true);
        }
        Player[] players = PhotonNetwork.PlayerList;
        if(players.Length>playerNames.Count)
        {
            for(int i = playerNames.Count;i<players.Length;i++)
            {
                playerNames.Add(Instantiate(namePrefab));
                playerNames[i].transform.SetParent(content.transform, false);
            }
        }
        else if(players.Length<playerNames.Count)
        {
            for (int i = players.Length; i < playerNames.Count; i++)
            {
                playerNames[i].SetActive(false);
            }
        }
        for (int i = 0; i < players.Length; i++)
        {
            playerNames[i].GetComponent<Text>().text = players[i].NickName;
        }
    }
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Arena");
        }

    }
    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }
    private void MasterCheck()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            masterNotice.text = "You are the room owner";
        }
        else
        {
            masterNotice.text = "Waiting for room owner to start";
        }
    }
    private void PanelToggle()
    {
        lobbyPanel.SetActive(!lobbyPanel.activeSelf);
        roomPanel.SetActive(!roomPanel.activeSelf);
    }
}
