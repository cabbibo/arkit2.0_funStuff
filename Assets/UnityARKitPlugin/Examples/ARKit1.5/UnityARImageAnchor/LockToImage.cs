using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class LockToImage : MonoBehaviour {


  [SerializeField]
  private ARReferenceImage referenceImage;

  public bool added;
  public Vector3 startingPosition;
 // public Vector3 verticalOffset;
  public Vector3 targetPosition;
  public Quaternion targetRotation;
  public GameObject CenterObject;
  public bool tweenRotation;

  // Use this for initialization
  void Start () {
    UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
    UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
    UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
    startingPosition = transform.position;

    targetPosition = transform.position;
    targetRotation = transform.rotation;

  }

  void AddImageAnchor(ARImageAnchor arImageAnchor)
  {
    Debug.Log ("image anchor added");
    if (arImageAnchor.referenceImageName == referenceImage.imageName) {
      added = true;
      targetPosition = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
      targetRotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
    }
  }

  void UpdateImageAnchor(ARImageAnchor arImageAnchor)
  {
    Debug.Log ("image anchor updated");
    if (arImageAnchor.referenceImageName == referenceImage.imageName) {
      targetPosition = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
      targetRotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
    }

  }

  void RemoveImageAnchor(ARImageAnchor arImageAnchor)
  {
    Debug.Log ("image anchor removed");
    added = false;
    transform.position = startingPosition;

  }

  void OnDestroy()
  {
    UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
    UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
    UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;
  }

  // Update is called once per frame
  void FixedUpdate () {

    if( added == true ){
      transform.position = targetPosition;//Vector3.Lerp( transform.position , targetPosition , .1f );
      if( tweenRotation ){ transform.rotation = targetRotation;};//Quaternion.Slerp( transform.rotation , targetRotation , .1f );
    }
  }
}