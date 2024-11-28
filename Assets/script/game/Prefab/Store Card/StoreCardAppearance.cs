using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCardAppearance : MonoBehaviour
{
    [SerializeField] private Image image;
    public void SetImage(byte cardIndex)
    {
        Sprite cardSprite = Util.TransferStoreCardSprite(cardIndex);
        image.sprite = cardSprite;
    }
}
