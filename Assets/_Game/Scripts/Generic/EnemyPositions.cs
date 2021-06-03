#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EnemyPositions : MonoBehaviour
{
    public static List<GameObject> instantiatedPathNodes = new List<GameObject>();
    public static GameObject NodeToSpawn;

    [MenuItem("AI/AddPointToCurrentAI %9", false, 1)]

    //[MenuItem("AI/AddPointToCurrentAI %9", false, 1)]
    public static void AddNewEnemiePoint()
    {
        SceneView scene = SceneView.lastActiveSceneView;
        EnemyAI enemieData = Selection.activeGameObject.GetComponent<EnemyAI>();
        Event e = Event.current;
        if (enemieData != null)
        {
            Vector3 mousePos = e.mousePosition;
            print(mousePos);
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;
            //enemieData.points.Add(scene.camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10)));
            Ray ray = scene.camera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (enemieData.points.Count == 0)
            {
                enemieData.points.Add(enemieData.transform.position);
                Debug.LogWarning("The list must contain 1 item atleast, added 1 point");
            }
            // origin, direction, max distance
            if (Physics.Raycast(ray, out hit, 100f))
            {
                enemieData.points.Add(hit.point);
            } else print("doesn't work");
        }
    }

    [MenuItem("AI/ShowAIPath %8", false, 1)]
    public static void ShowAIPath()
    {
        EnemyAI[] paths = GameObject.FindObjectsOfType<EnemyAI>();
        foreach(EnemyAI path in paths)
        {
            foreach (Vector3 point in path.points)
            {
                instantiatedPathNodes.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
                instantiatedPathNodes[instantiatedPathNodes.Count-1].transform.position = point;
                instantiatedPathNodes[instantiatedPathNodes.Count-1].tag = "Paths"; 
            }
        }
    }

    [MenuItem("AI/DeletePaths %7", false, 1)]
    public static void RemoveAIPaths()
    {
        foreach(GameObject obj in instantiatedPathNodes)
        {
            DestroyImmediate(GameObject.FindWithTag("Paths"));
        }
    }
}
#endif