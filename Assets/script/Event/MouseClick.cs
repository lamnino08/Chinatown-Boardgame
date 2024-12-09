using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask; 
    [SerializeField] private float maxDistance = 100f; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            PerformRaycast();
        }
    }

    /// <summary>
    /// Performs a raycast from the mouse position and detects objects within the specified LayerMask.
    /// </summary>
    private void PerformRaycast()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI"); // Click to UI
        }
         
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, layerMask);

        if (hits.Length > 0)
        {
            RaycastHit lastHit = hits[0];

            // GamePopupManager.Toast($"Last hit object: {lastHit.collider.gameObject.name}");

            PieceGameObject piece = lastHit.collider.GetComponent<PieceGameObject>();


            piece.OnMouseClick();
        } else
        {
            GameMaster.gameManager.OnTableClick();
        }
    }
}
