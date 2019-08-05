using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DestroyViruses
{
    public class test : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Image>().SetSprite("SpriteAtlas/common", "fx2_f1");
        }
    }
}