using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR.iOS;


// From the examples from unity AR plugin!
public class ARAddPrefabToScene1 : MonoBehaviour {



  public GameObject prefabObject;

  // Distance in Meters
  public float distanceFromCamera = .3f;
  private HashSet<string> m_Clones;


  private float m_TimeUntilRemove = 5.0f;

  void  Awake() {
    UnityARSessionNativeInterface.ARUserAnchorAddedEvent += ExampleAddAnchor;
    UnityARSessionNativeInterface.ARUserAnchorRemovedEvent += AnchorRemoved;
    m_Clones = new HashSet<string>();
  }
  
  public void ExampleAddAnchor(ARUserAnchor anchor)
  {
    if (m_Clones.Contains(anchor.identifier))
    {
            Console.WriteLine("Our anchor was added!");
    }
  }

  public void AnchorRemoved(ARUserAnchor anchor)
  {
    if (m_Clones.Contains(anchor.identifier))
    {
            m_Clones.Remove(anchor.identifier);
            Console.WriteLine("AnchorRemovedExample: " + anchor.identifier);
    }
  }

  // Update is called once per frame
  void Update () {


    if( Input.GetMouseButtonDown(0) == true || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)){


      GameObject clone = Instantiate(prefabObject, Camera.main.transform.position + (this.distanceFromCamera * Camera.main.transform.forward), Quaternion.identity);
      UnityARUserAnchorComponent component = clone.GetComponent<UnityARUserAnchorComponent>();
      m_Clones.Add(component.AnchorId);
      
    }

  }
}