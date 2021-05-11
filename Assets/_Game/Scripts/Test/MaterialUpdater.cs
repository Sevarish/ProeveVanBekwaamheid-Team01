using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialUpdater : MonoBehaviour
{
    public Material materialToUpdate;
    public Transform playerToTrack;

    void Update()
    {
        materialToUpdate.SetVector("_Position", new Vector4(playerToTrack.position.x, playerToTrack.position.y, playerToTrack.position.z, 0));
    }
}
