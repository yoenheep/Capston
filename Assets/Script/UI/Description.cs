using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Description : MonoBehaviour
{
    public GameObject Descrip;

    private void Update()
    {
        Ray();
    }

    void Ray()
    {
        Vector3 rayPosition = transform.position + Vector3.down * 1f;

        Debug.DrawRay(rayPosition, Vector3.left * 3f, new Color(0, 1, 0));
        Debug.DrawRay(rayPosition, Vector3.right * 3f, new Color(0, 1, 0));
        RaycastHit2D leftRayHit = Physics2D.Raycast(rayPosition, Vector3.left, 3f, LayerMask.GetMask("Player"));
        RaycastHit2D rightRayHit = Physics2D.Raycast(rayPosition, Vector3.right, 3f, LayerMask.GetMask("Player"));

        if (leftRayHit.collider != null || rightRayHit.collider != null)
        {
            GameUI.UIData.ItemZTxtObj.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Z))
            {
                Descrip.SetActive(true);
            }
        }
        else
        {
            GameUI.UIData.ItemZTxtObj.SetActive(false);
        }
    }
}
