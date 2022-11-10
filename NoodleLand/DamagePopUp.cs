using System;
using System.Collections;
using System.Collections.Generic;
using NoodleLand.Entities;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(TextMeshPro))]
public class DamagePopUp : Entity
{
    private TextMeshPro _textMeshPro;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }


    void Animate()
    {
        float randomX = transform.position.x + Random.Range(-2, 2);
        float randomY = transform.position.y + Random.Range(-2, 2);
        
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 1f);
        LeanTween.move(gameObject, new Vector2(randomX, randomY), 1f).setOnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    public void Set(int amount)
    {
        _textMeshPro.text = amount.ToString();
        Animate();

    }
}
