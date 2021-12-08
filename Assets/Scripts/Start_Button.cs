using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;

public class Start_Button : MonoBehaviourPunCallbacks
{
    private bool loaded = false;
    public Button startButton;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnClickStart()
    {
        if (loaded)
        {
            SceneManager.LoadScene("Menu");
        }
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        GetComponentInChildren<Text>().text = "Start";
        loaded = true;
        PhotonNetwork.NickName = "Player " + Random.Range(0, 100).ToString("000"); //*
    }
}
