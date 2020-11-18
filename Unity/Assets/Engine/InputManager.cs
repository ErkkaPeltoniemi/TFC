using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject selectedGameObject;
    public GameState gameState { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && selectedGameObject != null)
        {
            BroadcastMessage("StartRecordingPlan", selectedGameObject);
            gameState = GameState.Recording;
        }
        if (gameState == GameState.FreeLook)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameState = GameState.Playing;
                SendMessage("StartPlayingPlans", false);
            }
            HandleMouseClickFreeLook();
        }
    }

    private void HandleMouseClickFreeLook()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.up, 10);

            Vector3 result = Vector3.zero;
            float dist;
            if (plane.Raycast(cursorRay, out dist))
            {
                result = cursorRay.GetPoint(dist);
            }

            RaycastHit hitInfo;

            if (Physics.Linecast(Camera.main.transform.position, result, out hitInfo, Physics.AllLayers))
            {
                var gameObjectClicked = hitInfo.collider != null ? hitInfo.collider.gameObject : null;

                if (gameObjectClicked != null && gameObjectClicked.tag == "Player")
                {
                    OnPlayerCharacterClicked(hitInfo.collider.gameObject);
                }
            }
        }       
    }

    public void OnPlayerCharacterClicked(GameObject gameObject)
    {
        selectedGameObject = gameObject;
        if (gameState != GameState.Recording)
        {
            BroadcastMessage("ToFollowMode", gameObject);
        }
    }

    public void OnRecordingFinished()
    {
        gameState = GameState.FreeLook;
        BroadcastMessage("ToFreeLookMode");
    }

    public void OnRecordingStarted()
    {
        gameState = GameState.Recording;
    }

    public void OnPlayingFinished()
    {
        gameState = GameState.FreeLook;
        BroadcastMessage("ToFreeLookMode");
    }
}

public enum GameState
{
    FreeLook, Recording, Playing
}
