using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pose : MonoBehaviour
{
    [SerializeField] GameObject sphere;
    [SerializeField] TextMeshPro label;
    [Space]
    [SerializeField] int positionsQueueBuffer = 5;

    //[SerializeField] Vector3[] positions = new Vector3;
    //[SerializeField] Vector3[] positions;
    [SerializeField] Queue<Vector3> positions = new Queue<Vector3>();



    private void Start()
    {
        //positions = new Vector3[positionsQueueBuffer];
    }

    public void SetDebugActive(bool isActive)
    {
        sphere.SetActive(isActive);
        label.gameObject.SetActive(isActive);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void UpdatePosition(Vector3 position)
    {
        //transform.position = position;
        PushNewPosition(position);
        transform.position = CalculateAveragePosition();
    }

    public void UpdateLabel(string newLabel)
    {
        label.text = newLabel;
    }

    private void PushNewPosition(Vector3 position)
    {


        positions.Enqueue(position);
        if (positions.Count > positionsQueueBuffer)
        {
            positions.Dequeue();
        }

    }

    private Vector3 CalculateAveragePosition()
    {
        Vector3 accumulatedPosition = new Vector3();
        int validInputCount = 0;

        foreach (Vector3 position in positions)
        {
            //if (position != Vector3.zero)
            if (!IsWithinRangeOfZero(position, 0.5f))
            {
                accumulatedPosition += position;
                validInputCount++;
            }
        }
        //Debug.Log($"{gameObject.name} : positions.count = {positions.Count}\nvalidInputCount = {validInputCount}");
        if (validInputCount < 1) { validInputCount = 1; }
        return accumulatedPosition / validInputCount;
    }

    private bool IsWithinRangeOfZero(Vector3 v, float range)
    {
        return v.magnitude < range;
    }
}
