using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDistanceBetween : MonoBehaviour {

  public MakeCliff start;
  public MakeCliff end;
  public TextMesh text;
  public Transform sText;
  public Transform eText;
  private LineRenderer lr;
  public Transform textRotate;
	// Use this for initialization
	void Start () {

    lr = GetComponent<LineRenderer>();
		
	}
	
	// Update is called once per frame
	void Update () {

    Vector3 dif = start.transform.position - end.transform.position;
    dif -= Vector3.up *dif.y;
    float l = dif.magnitude;
    dif = dif.normalized;

    Vector3 c = Vector3.up;//Vector3.Cross( dif , Vector3.up );
    float up = 1.3f;
    float up2 = 1.4f;


    float maxHeight = Mathf.Max( start.fHeight,end.fHeight);
    sText.position = start.transform.position + c * start.fHeight * up;
    eText.position = end.transform.position + c * end.fHeight * up;
    textRotate.transform.position = start.transform.position  - dif * l * .5f + c* maxHeight * up2;
    text.text = ""+(Mathf.Floor(l * 1000)/1000); 
    lr.SetPosition(0,start.transform.position + c * start.fHeight * up);
    lr.SetPosition(1,start.transform.position + c * maxHeight * up2);
    lr.SetPosition(2,end.transform.position + c * maxHeight * up2);
    lr.SetPosition(3,end.transform.position + c * end.fHeight * up);
    textRotate.LookAt(start.transform.position + c * maxHeight * up2);
    //textRotate.Rotate( Vector3.up , 0.2f);
		
	}
}
