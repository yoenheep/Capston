using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector2 velocity;

    private float smoothTimeX;
    private float smoothTimeY;

    GameObject player;

    public bool bounds = true;
    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;

    private void Start()
    {
        player = GameManager.gameMgr.Player;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (GameManager.gameMgr.nowStage == 0) //��1
        {
            minCameraPos = new Vector3(-78, -29, -19.5f);
            maxCameraPos = new Vector3(45, 10.5f, -19.5f);
        }
        else if (GameManager.gameMgr.nowStage == 1) //��2
        {
            minCameraPos = new Vector3(169, -29, -19.5f);
            maxCameraPos = new Vector3(298, 1, -19.5f);  
        }
        else if (GameManager.gameMgr.nowStage == 2) //��3
        {
            minCameraPos = new Vector3(400, -29, -19.5f);
            maxCameraPos = new Vector3(544, 0, -19.5f);
        }
        else if (GameManager.gameMgr.nowStage == 3) //��4
        {
            minCameraPos = new Vector3(579, -29, -19.5f);
            maxCameraPos = new Vector3(753, 10, -19.5f);
        }
        else if (GameManager.gameMgr.nowStage == 4 || GameManager.gameMgr.nowStage == 5) //Ʈ��1
        {
            minCameraPos = new Vector3(80, -25, -19.5f);
            maxCameraPos = new Vector3(130, 20, -19.5f);
        }
        else if (GameManager.gameMgr.nowStage == 6) //�̴� ����
        {
            minCameraPos = new Vector3(336, -8, -19.5f);
            maxCameraPos = new Vector3(366, 0, -19.5f);
        }
        else if (GameManager.gameMgr.nowStage == 7) //����
        {
            minCameraPos = new Vector3(92, -135.5f, -19.5f);
            maxCameraPos = new Vector3(128, -130, -19.5f);
        }
        else if (GameManager.gameMgr.nowStage == 8) //��Ʈ����
        {
            minCameraPos = new Vector3(801, -25, -19.5f);
            maxCameraPos = new Vector3(940, 0, -19.5f);
        }

        if (bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
                Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z));
        }
    }


}
