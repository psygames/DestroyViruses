﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public float interval = 1/30f;

    private Image mImage;
    private int mIndex = 0;
    private float mTime = 0;

    private void Awake()
    {
        mImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        mIndex = 0;
        mTime = interval;
        SetSprite();
    }

    private void SetSprite()
    {
        if (sprites.Length <= 0)
            return;
        mImage.sprite = sprites[mIndex];
        if (mImage.type == Image.Type.Simple)
        {
            mImage.SetNativeSize();
        }
    }

    void Update()
    {
        if (sprites.Length <= 1)
            return;

        mTime -= Time.deltaTime;

        if (mTime <= 0)
        {
            mTime = interval;
            mIndex = (mIndex + 1) % sprites.Length;
            SetSprite();
        }
    }
}
