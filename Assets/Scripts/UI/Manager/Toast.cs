﻿using UnityEngine;
using System.Collections.Generic;

public class Toast : MonoSingle<Toast>
{

    public UISprite blackBg;
    public UILabel contentLabel;
    public GameObject offsetObj;
    private Queue<string> queue = new Queue<string>();

    public void Show(int local)
    {
        Show(Localization.Get(local));
    }

    public void Show(string str)
    {
        queue.Enqueue(str);
        if (tween == null || !isPlaying)
        {
            Exec();
        }
    }

    TweenPosition tween;
    bool isPlaying = false;

    private void Exec()
    {
        if (queue.Count > 0)
        {
            isPlaying = true;
            if (!offsetObj.activeSelf)
                offsetObj.SetActive(true);
            contentLabel.text = queue.Peek();
            offsetObj.transform.localPosition=Vector3.zero;
            blackBg.width = GameCore.Util.Instance.GetLength(queue.Peek()) * 34 + 30;
            tween = TweenPosition.Begin(offsetObj, 0.8f, new Vector3(0, 180, 0));
            queue.Dequeue();
            EventDelegate.Set(tween.onFinished, () =>
            {
                if (queue.Count <= 0)
                {
                    offsetObj.SetActive(false);
                    isPlaying = false;
                } else
                {
                    Exec();
                }
            });
        }
    }
}

