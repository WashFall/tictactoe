using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate : MonoBehaviour
{
    public int value;
    public int clickCount;
    public Sprite circle, cross;
    public bool validClick;
    public SpriteRenderer rend;
    public GameObject gridShield;

    private void Start()
    {
        clickCount = 0;
        rend = GetComponentInChildren<SpriteRenderer>();
    }

    public void SpriteCheck()
    {
        if (value == 1)
            rend.sprite = cross;
        else if (value == 2)
            rend.sprite = circle;
    }

    private void OnMouseDown()
    {
        if (!gridShield.activeSelf)
        {
            foreach (var player in GameManager.INSTANCE.activeGame.Players)
            {
                if (player.Name == SaveUserData.data.Name && player.PLayerNumber == GameManager.INSTANCE.player)
                {
                    validClick = true;
                    break;
                }
                else
                {
                    validClick = false;
                }
            
            }

            if (validClick && value == 0)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit) && !GameManager.INSTANCE.gameOver)
                {
                    if (GameManager.INSTANCE.player == 1 && value == 0) 
                    {
                        rend.sprite = cross;
                        value = 1;
                        GameManager.INSTANCE.drawCheck.Add(value);
                        ServiceLocator.GetAudio().PlayOnce("click", 0.3f);
                    }
                    else if (GameManager.INSTANCE.player == 2 && value == 0)
                    {
                        rend.sprite = circle;
                        value = 2;
                        GameManager.INSTANCE.drawCheck.Add(value);
                        ServiceLocator.GetAudio().PlayOnce("click", 0.3f);
                    }

                    GameManager.INSTANCE.clickedCoord = this;
                    if (clickCount == 0)
                    {
                        if (GameManager.INSTANCE.player == 1)
                        {
                            GameManager.INSTANCE.player = 2;
                        }
                        else if (GameManager.INSTANCE.player == 2)
                        {
                            GameManager.INSTANCE.player = 1;
                        }
                    }
                    GameManager.INSTANCE.CheckAndSaveGame();
                    clickCount++;
                }
            }
        }
        
    }
}
