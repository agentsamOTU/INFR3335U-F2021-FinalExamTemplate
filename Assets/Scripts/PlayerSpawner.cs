using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject cameraPrefab;

    public float minX, maxX;
    public float minZ, maxZ;

    void OnEnable()
    {
        Vector3 randomPosition = Vector3.zero;//new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
        GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        Transform roboTransform = newPlayer.GetComponentsInChildren<Transform>()[1];
        roboTransform.position = randomPosition;
        if (newPlayer.GetComponent<PhotonView>().IsMine)
        {
            newPlayer.GetComponentInChildren<PlayerMovement>().Bind(Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity));
        }
    }
}
