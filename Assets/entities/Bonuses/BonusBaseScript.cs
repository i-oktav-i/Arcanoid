using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class BonusBaseScript : MonoBehaviour
{
    protected Color color = Color.yellow;
    protected Color textColor = Color.black;
    protected String text = BonusLabels.PointsBonusLabel;
    public GameObject bonusPrefab;

    protected GameObject playerObject;

    private const int pointsPerActivation = 100;
    private const float deltaY = 0.02f;

    protected virtual void BonusActivate()
    {
      prefabmanger.instance.GameData.Points += pointsPerActivation;
      prefabmanger.instance.GameData.PointsToBall += pointsPerActivation;
    }

    protected virtual void initializeFields(){}

    protected virtual void initializeBonus()
    {
        initializeFields();
        playerObject = GameObject.Find("PlayerRender");
        gameObject.GetComponent<SpriteRenderer>().color = color;
        var textComponent = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.color = textColor;
    }

    void Start()
    {
        initializeBonus();
    }


  void Update()
    {
        if (Time.timeScale > 0)
        {
            var pos = transform.position;
            pos.y = gameObject.transform.position.y - deltaY;
            transform.position = pos;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    if (other.gameObject.CompareTag("Player")) {
      BonusActivate();
      Destroy(gameObject);
    }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bonus"))
        {
            BonusActivate();
            Destroy(gameObject);
        }
    }*/
}
