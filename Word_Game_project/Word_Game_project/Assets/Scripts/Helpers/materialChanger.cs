using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialChanger : MonoBehaviour
{
    private MaterialPropertyBlock propertyBlock;
    [SerializeField] private Renderer myRenderer;
    public Color color;

    private void Start()
    {
        changeColor(color);
    }

    public void changeColor(Color col)
    {
        propertyBlock = new MaterialPropertyBlock();

        myRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetColor("_Color", col);

        myRenderer.SetPropertyBlock(propertyBlock);
    }

}
