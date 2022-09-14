using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameListExpand : MonoBehaviour
{
    public GameObject button;
    public Vector2 newSize;
    private RectTransform listSize;
    public bool myGames;

    private void OnEnable()
    {
        SaveManager.Instance.gameList = this;
        if(myGames)
            SaveManager.Instance.ListGames();
    }

    private void OnDestroy()
    {
        SaveManager.Instance.ListUnsubscribe("/games/");
    }

    void Start()
    {
        float buttonSize = button.GetComponent<RectTransform>().rect.height;
        listSize = GetComponent<RectTransform>();
        newSize = new Vector2(0.0f, buttonSize + 25f);
        if(!myGames)
            SaveManager.Instance.ListSubscribe("/games/");
    }

    void Update()
    {
        listSize.sizeDelta = newSize * transform.childCount;
    }
}
