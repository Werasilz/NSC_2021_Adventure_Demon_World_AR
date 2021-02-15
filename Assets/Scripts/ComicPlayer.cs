using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicPlayer : MonoBehaviour
{
    public Sprite[] picture;
    private Image image;
    int i = 0;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (i >= 7)
        {
            i = 0;
            image.sprite = picture[i];
            GameObject.Find("Comic").gameObject.SetActive(false);
            GameManager.instance.isReadComic = true;
        }
    }

    public void TabComic()
    {
        i += 1;
        image.sprite = picture[i];
    }

    public void SkipComic()
    {
        i = 7;
    }
}


