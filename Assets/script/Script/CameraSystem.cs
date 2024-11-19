using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace CodeMonkey.CameraSystem {

    public class CameraSystem : MonoBehaviour {

        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private bool useEdgeScrolling = false;
        [SerializeField] private bool useDragPan = false;
        [SerializeField] private float fieldOfViewMax = 50;
        [SerializeField] private float fieldOfViewMin = 10;
        [SerializeField] private float followOffsetMin = 5f;
        [SerializeField] private float followOffsetMax = 50f;
        [SerializeField] private float followOffsetMinY = 10f;
        [SerializeField] private float followOffsetMaxY = 50f;

        private bool dragPanMoveActive;
        private Vector2 lastMousePosition;
        private float targetFieldOfView = 50;
        private Vector3 followOffset;
        private LensSettings lensSettings;


        private void Awake() {
            lensSettings = cinemachineVirtualCamera.m_Lens;
            //followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        }

        private void Update() {
            HandleCameraMovement();

            HandleCameraRotation();

            HandleCameraZoom_LowerY();
        }
        [Header("Move")]
        [SerializeField]    int minX = -26; // Giới hạn tối thiểu trục x
        [SerializeField]     int maxX = 30; // Giới hạn tối đa trục x
         [SerializeField]    int minZ = -177; // Giới hạn tối thiểu trục z
        [SerializeField]     int maxZ = -113; // Giới hạn tối đa trục z
        private void HandleCameraMovement() {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
            if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
            if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
            if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

            Vector3 moveDir = transform.up * inputDir.z + transform.right * inputDir.x;

            float moveSpeed = 50f;
            float lerpFactor = 0.7f; // Giá trị của lerpFactor có thể được điều chỉnh tùy ý

            Vector3 targetPosition = transform.position + moveDir * moveSpeed * Time.deltaTime;

           targetPosition.y = 30;
            if (targetPosition.x > maxX)  targetPosition.x= maxX;
           if (targetPosition.x < minX)  targetPosition.x= minX;
           if (targetPosition.z < minZ)  targetPosition.z= minZ;
           if (targetPosition.z > maxZ)  targetPosition.z= maxZ;
            // targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpFactor);
        }

        private void HandleCameraRotation() {
            float rotateDir = 0f;
            if (Input.GetKey(KeyCode.E)) rotateDir = +1f;
            if (Input.GetKey(KeyCode.Q)) rotateDir = -1f;

            float rotateSpeed = 100f;
            transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
        }

        private void HandleCameraZoom_FieldOfView() {
            if (Input.mouseScrollDelta.y > 0) {
                targetFieldOfView -= 5;
            }
            if (Input.mouseScrollDelta.y < 0) {
                targetFieldOfView += 5;
            }

            targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);

            float zoomSpeed = 10f;
            cinemachineVirtualCamera.m_Lens.FieldOfView =
                Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
        }

        private void HandleCameraZoom_MoveForward() {
            Vector3 zoomDir = followOffset.normalized;

            float zoomAmount = 3f;
            if (Input.mouseScrollDelta.y > 0) {
                followOffset -= zoomDir * zoomAmount;
            }
            if (Input.mouseScrollDelta.y < 0) {
                followOffset += zoomDir * zoomAmount;
            }

            if (followOffset.magnitude < followOffsetMin) {
                followOffset = zoomDir * followOffsetMin;
            }

            if (followOffset.magnitude > followOffsetMax) {
                followOffset = zoomDir * followOffsetMax;
            }

            float zoomSpeed = 10f;
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
                Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
        }

        private void HandleCameraZoom_LowerY() {
            float zoomAmount = 5f;
            
            if (Input.mouseScrollDelta.y > 0) {
                targetFieldOfView -= zoomAmount;
            }
            if (Input.mouseScrollDelta.y < 0) {
                targetFieldOfView += zoomAmount;
            }

            targetFieldOfView = Mathf.Clamp(targetFieldOfView, followOffsetMinY, followOffsetMaxY);
            float zoomSpeed = 10f;
            cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime*zoomSpeed);
        }

    }

}