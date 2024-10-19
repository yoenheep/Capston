using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField] int maxPage;
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform RoundPageRect;

    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;

    public GameObject preBtn;
    public GameObject nextBtn;

    private void Awake()
    {
        currentPage = 1;
        targetPos = RoundPageRect.localPosition;
    }

    private void Update()
    {
        if (currentPage == 1) 
        {
            preBtn.SetActive(false);
        }
        else if ( currentPage == maxPage)
        {
            nextBtn.SetActive(false);
        }
        else
        {
            nextBtn.SetActive(true);
            preBtn.SetActive(true);
        }
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    public void MovePage()
    {
        RoundPageRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }
}
