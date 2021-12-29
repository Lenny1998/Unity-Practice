using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistableObject
{
    private int shapeId = int.MinValue;

    public int ShapeId
    {
        get
        {
            return shapeId;
        }
        set
        {
            if (shapeId == int.MinValue && value != int.MinValue)
            {
                shapeId = value;
            }
            else
            {
                Debug.LogError("不允许修改shapeId.");
            }
        }
    }

    public int MaterialId { get; private set; }

    private Color color;

    private MeshRenderer meshRenderer;

    private static int colorPropertyId = Shader.PropertyToID("_Color");
    private static MaterialPropertyBlock sharedPropertyBlock;

    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }
    public void SetColor(Color color)
    {
        this.color = color;
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        meshRenderer.SetPropertyBlock(sharedPropertyBlock);
    }

    public void SetMaterial(Material material, int materialId)
    {
        meshRenderer.material = material;
        MaterialId = materialId;
    }


    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(color);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.Version >0 ? reader.ReadColor() : Color.white);
    }
}
