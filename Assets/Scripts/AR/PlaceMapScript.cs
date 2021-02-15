using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceMapScript : MonoBehaviour
{
    public static PlaceMapScript instance;

    #region AR Section
    public ARRaycastManager raycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public ARPlaneManager planeManager;
    #endregion

    #region GameObject
    private GameObject gamePlay;
    private GameObject canvasAR;
    private GameObject canvasGame;
    private GameObject canvasHowto;
    private GameObject canvasAdmin;
    #endregion

    private Vector3 mapUpdatePosition;
    private Quaternion mapUpdateRotation;
    private Quaternion defaultRotate;

    public bool isReset;
    [HideInInspector] public bool isSetPosition;

    private float adminCheck = 0;
    private bool admincShow;

    private bool IsPointerOverUIObject()                                                                                            // Bool for check pointer Cancel Raycast through button that position of AR object will change
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 30;
        canvasHowto = GameObject.Find("CanvasHowto");
        canvasGame = GameObject.Find("Canvas");
        canvasGame.SetActive(false);
        canvasAR = GameObject.Find("CanvasAR");
        canvasAR.SetActive(false);
        gamePlay = GameObject.Find("GamePlay");
        gamePlay.SetActive(false);
        canvasAdmin = GameObject.Find("CanvasAdmin");
        canvasAdmin.SetActive(false);
    }

    private void Start()
    {
        DisablePlane();
    }

    void Update()
    {
        PlaceObject();
        Admin();
    }

    public void DisablePlane()
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }

    void PlaceObject()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon) && !IsPointerOverUIObject())         // Touch for place Map save to List hits
            {
                Pose hitPose = hits[0].pose;                                                                                        // Set hitpose by List hits

                if (!gamePlay.activeInHierarchy)                                                                                    // Map not Show in Scene
                {
                    canvasHowto.transform.GetChild(0).gameObject.SetActive(false);
                    canvasHowto.transform.GetChild(1).gameObject.SetActive(true);
                    canvasHowto.transform.GetChild(2).gameObject.SetActive(false);
                    canvasAR.SetActive(true);
                    gamePlay.SetActive(true);                                                                                       // Show Map
                    gamePlay.transform.rotation = Quaternion.Euler(hitPose.rotation.x, hitPose.rotation.y, hitPose.rotation.z);
                    gamePlay.transform.position = hitPose.position;                                                                 // Set Map Position by hitPose Position
                }
                else                                                                                                                // Map is Already Show || Check Button Set Position 
                {
                    if (!isSetPosition)                                                                                             // Not Press Set Position Button
                    {
                        gamePlay.transform.position = hitPose.position;                                                             // Set Map Position by hitPose Position
                        gamePlay.transform.rotation = hitPose.rotation;
                        mapUpdatePosition = gamePlay.transform.position;                                                            // Save Map Postion to mapUpdatePosition
                        mapUpdateRotation = gamePlay.transform.rotation;
                    }
                    else if (isSetPosition)                                                                                         // Press Set Position Button
                    {
                        gamePlay.transform.position = mapUpdatePosition;                                                            // Set Map Position by mapUpdatePosition
                        gamePlay.transform.rotation = mapUpdateRotation;
                    }
                }
            }
        }
    }

    public void SetPosition()
    {
        DisablePlane();                                                                                                             // Disable all Plane
        planeManager.enabled = false;                                                                                               // Disable ARPlaneManager

        isSetPosition = true;
        canvasHowto.transform.GetChild(1).gameObject.SetActive(false);                                                              // Hide Howto Slide Map
        canvasGame.SetActive(true);                                                                                                 // Show Gameplay
        canvasAR.SetActive(false);                                                                                                  // Hide AR set position button

        if (!isReset)
        {
            CallOnStart.instance.LoadGameSetup();                                                                                   // Load Function for new game
        }
        else if (isReset)
        {
            isReset = false;
            gamePlay.transform.GetChild(1).gameObject.SetActive(true);                                                              // Show Enemy Group
        }
    }

    public void ResetPosition()
    {
        DisablePlane();                                                                                                             // Disable all Plane
        planeManager.enabled = true;                                                                                                // Enable ARPlaneManager

        isReset = true;                                                                                                             // isReset true for not call LoadGaneSetup again
        isSetPosition = false;
        GameManager.instance.isPause = false;                                                                                       // not Pause for set time scale to 1
        canvasHowto.transform.GetChild(0).gameObject.SetActive(true);                                                               // Show Howto Rotate Phone
        canvasGame.SetActive(false);                                                                                                // Hide Canvas of Gameplay
        gamePlay.transform.GetChild(1).gameObject.SetActive(false);                                                                 // Hide Enemy Group
        gamePlay.SetActive(false);                                                                                                  // Hide Gameplay
    }

    void Admin()
    {
        if (adminCheck >= 10)
        {
            adminCheck = 0;

            if (!canvasAdmin.activeInHierarchy)
            {
                canvasAdmin.SetActive(true);
            }
            else
            {
                canvasAdmin.SetActive(false);
            }
        }
    }

    public void ShowAdminTab()
    {
        Debug.Log(adminCheck);
        adminCheck += 1;
    }
}

