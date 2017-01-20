using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SqueezeBottle : MonoBehaviour, ISelectable
{
    [Header("Config")]
    public SqueezeLine SqueezeLinePrefab;
    public float PointsMinDistance = 0.1f;
    public float UpOffset = 2.0f;

    private SqueezeLine CurrentSqueezeLine;
    private List<Vector3> CurrentPoints = new List<Vector3>();

    private bool m_isSelected = false;
    public bool IsSelected { get { return m_isSelected; } set { m_isSelected = value; } }

    private ParticleSystem m_ParticleSystem;
    private Vector3 m_StartPos;
    private bool m_PreventSpill = false;

    private void Start()
    {
        m_ParticleSystem = GetComponentInChildren<ParticleSystem>();
        m_StartPos = transform.position;
    }

    private void Update()
    {
        // handle movement here when it is selected
        if (IsSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1 << 10)) // only on the "table" layer
            {
                transform.position = hit.point + Vector3.up * UpOffset;
            }


            if (Input.GetMouseButtonDown(0) && !m_PreventSpill)
            {
                // create the SqueezeLine object
                CurrentSqueezeLine = Instantiate<SqueezeLine>(SqueezeLinePrefab);

                // start particle effect
                m_ParticleSystem.Play();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // end particle effect
                m_ParticleSystem.Stop();

                m_PreventSpill = false;
            }
            else if (Input.GetMouseButton(0) && !m_PreventSpill)
            {
                // raycast from bottle to bottom 
                if (Physics.Raycast(transform.position, Vector3.down, out hit))
                {
                    // check if current raycast target is further away from any currentPoints than minDistance
                    if (IsPointFurtherAway(hit.point))
                    {
                        // add it to CurrentPoints and CurrentSqueezeLine.AddNewPoint
                        CurrentPoints.Add(hit.point);
                        StartCoroutine(AddNewPoint(CurrentSqueezeLine, hit.point));
                        //CurrentSqueezeLine.AddNewPoint(hit.point);
                    }
                }
            }
        }        
    }

    IEnumerator AddNewPoint(SqueezeLine toLine, Vector3 point)
    {
        yield return new WaitForSeconds(0.2f);

        toLine.AddNewPoint(point);
    }

    private bool IsPointFurtherAway(Vector3 Point)
    {
        foreach(Vector3 p in CurrentPoints)
        {
            if (Vector3.Distance(p, Point) < PointsMinDistance)
                return false;
        }

        return true;
    }

    public void Reset()
    {
        CurrentPoints = new List<Vector3>();
        CurrentSqueezeLine = null;
    }

    public void Select()
    {
        // waits until mousebuttonUp for actually squeezing out stuff
        m_PreventSpill = true;

        IsSelected = true;
    }

    public void Unselect()
    {
        IsSelected = false;
        transform.position = m_StartPos;
        m_PreventSpill = false;
    }
}