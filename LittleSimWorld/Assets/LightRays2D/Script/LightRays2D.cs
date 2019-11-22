using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(MeshFilter)),RequireComponent(typeof(MeshRenderer))]
public class LightRays2D:LightRays2DAbstract{

	private MeshRenderer mr;

	//For sorting layers
	[HideInInspector]
	public int sortingLayer=0;
	private int _sortingLayer;
	[HideInInspector]
	public int orderInLayer=0;
	private int _orderInLayer;

	protected override void GetReferences(){
		mr=GetComponent<MeshRenderer>();
	}

	protected override Material GetMaterial(){
		return mr.sharedMaterial;
	}

	protected override void Update(){
		base.Update();
		if (sortingLayer!=_sortingLayer || orderInLayer!=_orderInLayer){
			mr.sortingLayerID=sortingLayer;
			mr.sortingOrder=orderInLayer;
			_sortingLayer=sortingLayer;
			_orderInLayer=orderInLayer;
		}
        if (WeatherSystem.Instance && WeatherSystem.Instance.sunnyToday)
        {
            float biggestValue = DayNightCycle.Instance.curve.keys.GetUpperBound(0);
            color1.a = DayNightCycle.Instance.intensity / biggestValue;
            color2.a = DayNightCycle.Instance.intensity / biggestValue;
        }
        else
        {
            color1.a = 0;
            color2.a = 0;
        }
        

    }
	
	protected override void ApplyMaterial(Material material){
		
	}
	
}
