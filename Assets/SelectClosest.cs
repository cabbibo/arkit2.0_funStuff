using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectClosest : MonoBehaviour {

  public LockToImage baseObject;
  public LockToImage selector;

public Light light;
public AudioPlayer audioPlayer;
  public InfoSphere[] spheres;
  public int closest;
  public int oClosest;
  public float closestD;
  private int currentID;

  public TextMesh text;

  public LineRenderer lr;



	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
    if( baseObject.added == true  && selector.added == true ){
    oClosest = closest;
    float closestD = 100000;

    for( int i =0 ; i<spheres.Length; i ++ ){
      float d = ( new Vector2(selector.transform.position.x,selector.transform.position.z) - new Vector2(spheres[i].transform.position.x,spheres[i].transform.position.z) ).magnitude;
      if( d  < closestD ){
        closestD = d;
        closest = i;
      }

    }

    lr.SetPosition( 0 , selector.transform.position );
    lr.SetPosition( 1 , new Vector3(selector.transform.position.x, spheres[closest].transform.position.y ,selector.transform.position.z));
    lr.SetPosition( 2,spheres[closest].transform.position);
    lr.startColor = light.color;
    lr.endColor = light.color;
    text.color = light.color;

  light.transform.position = selector.transform.position + Vector3.up * spheres[closest].transform.position.y;
   

    if( closest  != oClosest ){
      OnNewSphere(closest);
    }


  }

	}

  void OnNewSphere(int id){
    currentID = id;

    audioPlayer.Play(spheres[id].clip);
    text.transform.position = selector.transform.position+ selector.transform.TransformDirection(Vector3.forward) * .02f + Vector3.up * (spheres[id].transform.position.y- selector.transform.position.y);
    text.text = spheres[id].title;
    light.color = spheres[id].color;
    

  }
}
