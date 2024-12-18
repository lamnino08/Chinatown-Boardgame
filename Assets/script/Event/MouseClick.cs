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
            OnMouseDownLeft();
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            OnMouseDownRight();
        }
    }

    /// <summary>
    /// Performs a raycast from the mouse position and detects objects within the specified LayerMask.
    /// </summary>
    private void OnMouseDownLeft()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Clicked on UI element");
            return; // Không thực hiện logic Raycast nếu nhấn vào UI
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, layerMask);

        if (hits.Length > 0)
        {
            RaycastHit lastHit = hits[0];

            // GamePopupManager.Toast($"Last hit object: {lastHit.collider.gameObject.name}");

            PieceGameObject piece = lastHit.collider.GetComponent<PieceGameObject>();
            if (piece != null)
            {
                piece.OnMouseClick();
            } else
            {
                GamePopupManager.Toast("Miss component");
            }
        } else
        {
            GameMaster.gameManager.OnTableClick();
        }
    }

    private void OnMouseDownRight()
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
