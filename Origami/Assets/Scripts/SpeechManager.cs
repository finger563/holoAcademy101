using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    bool placing = false;
    bool placingSD = false;
    bool placingPT = false;

    public GameObject SD = null;
    public GameObject PT = null;

    // Use this for initialization
    void Start()
    {
        if (SD == null)
        {
            SD = this.transform.Find("MX2+").gameObject;
        }
        if (PT == null)
        {
            PT = this.transform.Find("PT").gameObject;
        }
        keywords.Add("Reset world", () =>
        {
            // Call the OnReset method on every descendant object.
            this.BroadcastMessage("OnReset");
        });

        keywords.Add("Drop Sphere", () =>
        {
            var focusObject = GazeGestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnDrop");
            }
        });

        keywords.Add("Place Scene", () =>
        {
            OnPlaceScene();
        });

        keywords.Add("Increase Size", () =>
        {
            OnIncreaseSize();
        });

        keywords.Add("Decrease Size", () =>
        {
            OnDecreaseSize();
        });

        keywords.Add("Place Smart Drive", () =>
        {
            OnPlaceSmartDrive();
        });

        keywords.Add("Place Push Tracker", () =>
        {
            OnPlacePushTracker();
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnPlaceScene()
    {
        // On each Select gesture, toggle whether the user is in placing mode.
        placing = !placing;

        // If the user is in placing mode, display the spatial mapping mesh.
        if (placing)
        {
            SpatialMapping.Instance.DrawVisualMeshes = true;
        }
        // If the user is not in placing mode, hide the spatial mapping mesh.
        else
        {
            SpatialMapping.Instance.DrawVisualMeshes = false;
        }
    }

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnPlaceSmartDrive()
    {
        // On each Select gesture, toggle whether the user is in placing mode.
        placingSD = !placingSD;

        // If the user is in placing mode, display the spatial mapping mesh.
        if (placingSD)
        {
            SpatialMapping.Instance.DrawVisualMeshes = true;
        }
        // If the user is not in placing mode, hide the spatial mapping mesh.
        else
        {
            SpatialMapping.Instance.DrawVisualMeshes = false;
        }
    }

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnPlacePushTracker()
    {
        // On each Select gesture, toggle whether the user is in placing mode.
        placingPT = !placingPT;

        // If the user is in placing mode, display the spatial mapping mesh.
        if (placingPT)
        {
            SpatialMapping.Instance.DrawVisualMeshes = true;
        }
        // If the user is not in placing mode, hide the spatial mapping mesh.
        else
        {
            SpatialMapping.Instance.DrawVisualMeshes = false;
        }
    }

    void OnIncreaseSize()
    {
        iTween.ScaleBy(SD, Vector3.one * 2.0f, 1.0f);
        iTween.ScaleBy(PT, Vector3.one * 2.0f, 1.0f);
    }

    void OnDecreaseSize()
    {
        iTween.ScaleBy(SD, Vector3.one * 0.5f, 1.0f);
        iTween.ScaleBy(PT, Vector3.one * 0.5f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // If the user is in placing mode,
        // update the placement to match the user's gaze.

        if (placing)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, SpatialMapping.PhysicsRaycastMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                this.transform.position = hitInfo.point;

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.rotation = toQuat;
            }
        }
        else if (placingSD)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, SpatialMapping.PhysicsRaycastMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                SD.transform.position = hitInfo.point;

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                SD.transform.rotation = toQuat;
            }
        }
        else if (placingPT)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, SpatialMapping.PhysicsRaycastMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                PT.transform.position = hitInfo.point;

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                PT.transform.rotation = toQuat;
            }
        }
    }
}