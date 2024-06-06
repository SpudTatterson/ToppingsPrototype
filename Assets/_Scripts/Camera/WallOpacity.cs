using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class WallOpacity : MonoBehaviour
{
    private string wallTag = "Wall";
    private RaycastHit hit;
    public List<Material> materials = new List<Material>();

    private void Update()
    {
        // SetTransparent(GetWallMaterial(), .25f);
    } 

    private Material GetWallMaterial()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, Mathf.Infinity);

        if(hit.collider.CompareTag(wallTag) && hit.collider.TryGetComponent(out Renderer renderer))
        {
            return renderer.material;
        }

        return null;
    }

    private void SetTransparent(Material material, float opacity)
    {
        if (material == null) return;
        if (materials.Contains(material)) return;

        material.shader = Shader.Find("Universal Render Pipeline/Lit");

        material.SetFloat("_Surface", 1);
        material.SetFloat("_Blend", 1);
        materials.Add(material);

        Color color = material.color;
        color.a = opacity;
        material.color = color;
    }

    private void SetOpaque(Material material)
    {
        if (material == null) return;

        material.SetFloat("_Surface", 0);
        material.SetFloat("_Blend", 0);
        materials.Remove(material);

        Color color = material.color;
        color.a = 1.0f;
        material.color = color;
    }
}
