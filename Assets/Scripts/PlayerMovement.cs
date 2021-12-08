using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private Joystick playJoy;
    [SerializeField] private Joystick playRotJoy;

    private CharacterController charCont;
    private Rigidbody charBody;

    public float speed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        charCont = GetComponent<CharacterController>();
        charBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<PhotonView>().IsMine)
        {
            Vector3 stickDir = new Vector3(playJoy.Horizontal, 0.0f, playJoy.Vertical);
            stickDir = transform.rotation * stickDir;
            Vector3 rotStickDir = new Vector3(0.0f, playRotJoy.Horizontal, 0.0f);
            charBody.velocity = speed * stickDir;
            charBody.angularVelocity = new Vector3(0.0f, rotStickDir.y, 0.0f);
        }
        else
        {
            charBody.velocity = Vector3.zero;
            charBody.angularVelocity = Vector3.zero;
        }
    }

    public void Bind(GameObject camvas)
    {
        playJoy = camvas.GetComponentsInChildren<Joystick>()[0];
        playRotJoy = camvas.GetComponentsInChildren<Joystick>()[1];
        camvas.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Follow = transform;
        camvas.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().LookAt = transform;
    }
}
