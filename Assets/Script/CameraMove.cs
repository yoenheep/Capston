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

        if (GameManager.gameMgr.nowStage == 0) //¸Ê1
        {
            minCameraPos = new Vector3(-7.1f, -29, -19.5f);
            maxCameraPos = new Vector3(108, 0, -19.5f);
        }
        else if (GameManager.gameMgr.nowStage == 1) //¸Ê2
        {
            minCameraPos = new Vector3(-7.5f, -29, -19.5f);
            maxCameraPos = new Vector3(118, -15, -19.5f);  
        }
        else if (GameManager.gameMgr.nowStage == 2) //¸Ê3
        {
            minCameraPos = new Vector3(-32, -29, -19.5f);
            maxCameraPos = new Vector3(112, -15, -19.5f);
        }
        else if (GameManager.gameMgr.nowStage == 3) //¸Ê4
        {
            minCameraPos = new Vector3(-32, -29, -19.5f);
            maxCameraPos = new Vector3(143, 10, -19.5f);
        }
        else if (GameManager.gameMgr.nowStage == 5) //Æ®·¦
        {
            minCameraPos = new Vector3(-12, -24, -19.5f);
            maxCameraPos = new Vector3(46, 33, -19.5f);
        }
        else if (GameManager.gameMgr.nowStage == 4) //¹Ì´Ï º¸½º
        {
            minCameraPos = new Vector3(-7.5f, -21, -19.5f);
            maxCameraPos = new Vector3(23, -18, -19.5f);
        }
        else if (GameManager.gameMgr.nowStage == 6) //¶ó½ºÆ®º¸½º
        {
            minCameraPos = new Vector3(-31, -24, -19.5f);
            maxCameraPos = new Vector3(113, -13, -19.5f);
        }

        if (bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
                Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z));
        }
    }


}
