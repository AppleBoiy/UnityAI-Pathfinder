using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Camera _mainCamera;
    
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private LineRenderer lineRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
        _mainCamera = Camera.main;
        
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.positionCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickToMove();
        }
        if (Vector3.Distance(transform.position, _agent.destination) < 0.1f)
        {
            markerPrefab.SetActive(false);
        } else if (_agent.hasPath)
        {
            DrawPath();
        }
    }


    private void ClickToMove()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            SetDestination(hit.point);
        }
    }

    private void SetDestination(Vector3 destination)
    {
        _agent.SetDestination(destination);
        markerPrefab.SetActive(true);
        markerPrefab.transform.position = destination;
    }

    private void DrawPath()
    {
        lineRenderer.positionCount = _agent.path.corners.Length;
        lineRenderer.SetPosition(0, transform.position);
        
        if (_agent.path.corners.Length < 2)
        {
            return;
        }
        
        for (int i = 1; i < _agent.path.corners.Length; i++)
        {
            var pathPoint = _agent.path.corners[i];
            Vector3 pointPosition = new Vector3(pathPoint.x, pathPoint.y, pathPoint.z);
            lineRenderer.SetPosition(i, pointPosition);
        }
        
    }
}
