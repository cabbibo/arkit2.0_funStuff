using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectSpheres : MonoBehaviour {

  public LockToImage start;
  public LockToImage end;

  private LineRenderer lr;

  public AudioClip clip;
  public AudioPlayer player;
  public AudioListenerTexture audioTexture;


  // Use this for initialization
  void Start () {
    
    lr = GetComponent<LineRenderer>();
  }
  
  // Update is called once per frame
  void Update () {

    if( start.added == true  && end.added == true ){

    Vector3 dif = start.CenterObject.transform.position - end.CenterObject.transform.position;

    float angleDif = start.CenterObject.transform.eulerAngles.y - end.CenterObject.transform.eulerAngles.y;
     angleDif /= 360;
     angleDif *= 3.14f * 2;

     angleDif = Mathf.Abs(angleDif);
    print( angleDif );



    // Audio Stuff
    for( int i = 0; i< lr.positionCount; i++){
      float v = (float)i / (float)lr.positionCount;

      Vector3 p = start.CenterObject.transform.position - dif * v;


      float aVal = Mathf.Clamp( audioTexture.samples[ (int)(i * 1)  + (int)angleDif * 100 ] * 1024,0 , .1f);
      p.y += Mathf.Sin( v * 3.14159f ) * (.1f  * angleDif + .2f*aVal);// +  Mathf.Clamp( audioTexture.samples[ (int)(i * 2)  + 0 ] * 1024,0 , .1f);
      lr.SetPosition(i,p);

    }

    float l = dif.magnitude;


    float r1 = Random.Range(0,.999f);

    if( r1 > dif.magnitude * dif.magnitude * angleDif ){

      float r2 = Random.Range( 0, .999f);
      if( r2 < .02* angleDif / dif.magnitude ){
        player.Play( clip , (.2f* angleDif / dif.magnitude ) +  r2 , angleDif);
      }

    }

  }

    
  }
}
