using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGuideLine : MonoBehaviour
{
    [SerializeField] MeshRenderer gridGuideline;
    [SerializeField] Color guideColor;
    [SerializeField, Range(0, 1)] float transparency;
    Material gridGuideMat;
    // Start is called before the first frame update
    void Start()
    {
        gridGuideMat = gridGuideline.material;
        guideColor = gridGuideMat.color;
        gridGuideline.enabled = false;
    }


    public void UpdateTrans(float transparency)
    {
        gridGuideline.enabled = true;
        gridGuideMat.color = new Color(guideColor.r, guideColor.g, guideColor.b,transparency);
        
    }
    public void DontShow()
    {
        gridGuideline.enabled = false;
    }
    public Vector3 GetCenter()
    {
        return gridGuideline.bounds.center;
    }
}
