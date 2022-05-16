using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaveFPS
{
    public class PlayerCam : MonoBehaviour
    {
        public float sensX;
        public float sensY;

        public Transform orientation;
        public Transform camHolder;

        float xRotation;
        float yRotation;


        public Rigidbody teleporter;
        public float teleporterVelocity = 10;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            // get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // rotate cam and orientation
            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            
            //quick turn
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(Spin(180, 0.15f));
            }
            
            //teleport throw
            if (teleporter && Input.GetMouseButtonDown(0))
            {
                teleporter.MovePosition(transform.position + (transform.forward * 1.5f));
                teleporter.velocity = transform.forward * teleporterVelocity;
            }
        }

        IEnumerator Spin(float rotation, float time)
        {
            float t = 0;
            while (t < time)
            {
                yRotation += Time.deltaTime * rotation / time;
                t += Time.deltaTime;
                yield return null;
            }
        }

        public void DoFov(float endValue)
        {
            //GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
        }

        public void DoTilt(float zTilt)
        {
            //transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
        }
    }
}