using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCardAppearance : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    public void SetImage(byte cardIndex)
    {
        Material material = Util.TransferStoreCardSprite(cardIndex);
        _renderer.material = material;
    }
}
