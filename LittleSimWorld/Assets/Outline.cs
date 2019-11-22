

using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Outline : MonoBehaviour
{
    public SpriteRenderer RendererOfOutlinedObject;
    public GameObject  OutlineGO;
    [Range(0, 30)]
    public int outlineThickness = 10;
    public Color OutlineColor = new Color(255, 244, 0);
    public bool Enabled = false;

    private void Awake()
    {
        if(!RendererOfOutlinedObject)
            RendererOfOutlinedObject = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        if (!OutlineGO) { 
            GameObject prefab = (GameObject)Resources.Load("OutlinePrefab", typeof(GameObject));
            OutlineGO = Instantiate<GameObject>(prefab, RendererOfOutlinedObject.transform.position, RendererOfOutlinedObject.transform.rotation, RendererOfOutlinedObject.transform);
            SpriteRenderer outlineSR = OutlineGO.GetComponent<SpriteRenderer>();
            outlineSR.sortingOrder = RendererOfOutlinedObject.sortingOrder;
            outlineSR.spriteSortPoint = RendererOfOutlinedObject.spriteSortPoint;
            outlineSR.sprite = RendererOfOutlinedObject.sprite;
            
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            outlineSR.GetPropertyBlock(mpb);
            mpb.SetFloat("_Outline", 1);
            mpb.SetColor("_OutlineColor", OutlineColor);
            mpb.SetFloat("_OutlineSize", outlineThickness);
            outlineSR.SetPropertyBlock(mpb);
        }

        if (!OutlineGO.GetComponent<Animation>().isPlaying)
            OutlineGO.GetComponent<Animation>().Play();
        
        DisableOutline();
    }

    public void OnEnable()
    {
        EnableOutline(); // TODO delete
    }

    public void OnDisable()
    {
        DisableOutline();
    }

    public void EnableOutline()
    {
        if(OutlineGO && !OutlineGO.activeSelf)
            OutlineGO.SetActive(true);
            
        Enabled = true;         
    }

    public void DisableOutline()
    {
        if (OutlineGO && OutlineGO.activeSelf)
            OutlineGO.SetActive(false);
            
        Enabled = false;
    }
}
   
