using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    [SerializeField] public float MouseSensitivity = 40f;
    public float xRotation = 0f;
    public float yRotation = 0f;

    public float lookDownClamp = -70f;
    public float lookUpClamp = 70f;

    public float gunOffsetRotation = 8f;

	public Transform headCamera;
	public Transform gunHolder;
    public Transform bodyTransform;
	//public GunController gunController;

	public bool xInverted = false;
	public bool yInverted = false;

	// Use this for initialization
	void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("p")) {
			Application.Quit();
		}

		float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        bool lookingDown = Mathf.Abs(xRotation) != xRotation;
        float offsetInverter = lookingDown ? -1 : 1;
        float gunOffset = lookingDown ? lookDownClamp : lookUpClamp;
        gunOffset = xRotation / gunOffset;
        gunOffset *= gunOffsetRotation;
        gunOffset *= offsetInverter;

		yRotation -= mouseX;

		bodyTransform.localRotation = Quaternion.Euler(0f, -yRotation, 0f);
		headCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        gunHolder.localRotation = Quaternion.Euler(gunOffset, 0f, 0f);
    }

	public void WhenScoping()
    {
		MouseSensitivity -= 30;
    }

    public void WhenNotScoping()
    {
		MouseSensitivity += 30;
    }
}