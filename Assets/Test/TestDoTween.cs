using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TestDoTween : MonoBehaviour 
{

    //public Vector3[] path0;
    //public Vector3[] path1;

    public Ease ease = Ease.Linear;
    public Transform point_1;
    public Transform point_2;


    private Vector3[] path = new Vector3[4]; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            path[0] = transform.localPosition;
            path[1] = point_1.transform.localPosition;
            path[2] = point_2.transform.localPosition;
            path[3] = Vector3.zero;
            transform.DOLocalPath(path, 2f, PathType.CatmullRom, PathMode.Sidescroller2D, 10).SetEase(Ease.Linear);
        }
        else if(Input.GetMouseButtonDown(1))
        {
            transform.localPosition = new Vector3(-540f, -270f); 
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (transform.localPosition.x < 0)
        //    {
        //        transform.DOLocalPath(path0, 3);
        //    }
        //    else
        //    {
        //        transform.DOLocalPath(path1, 3);
        //    }
        //}
	}
}
