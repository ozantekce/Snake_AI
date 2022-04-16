using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private float size = 1f;


    [SerializeField]
    int rows, columns;

    [SerializeField]
    Transform startPositionOfPoints;

    public GameObject pointPrefab;
    private void Start()
    {
        Vector3 startPos = startPositionOfPoints.position;
        Vector3 addVector = Vector3.zero;
        for (int i = 0; i < rows; i++)
        {
            addVector.x = 0;
            for (int j = 0; j < columns; j++)
            {

                Instantiate(pointPrefab, startPos + addVector, Quaternion.identity);
                addVector.x += startPositionOfPoints.localScale.x;
            }
            addVector.y -= startPositionOfPoints.localScale.y;

        }
    }

}