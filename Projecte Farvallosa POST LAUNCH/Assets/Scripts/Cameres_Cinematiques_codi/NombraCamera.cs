using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NombraCamera : MonoBehaviour
{
    public string nom;
    public Camera camera;

    public NombraCamera(string in_nom, Camera in_camera)
    {
        nom = in_nom;
        camera = in_camera;
    }
}