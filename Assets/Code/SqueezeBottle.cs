using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SqueezeBottle : MonoBehaviour, ISelectable, IValidatable
{
    [Header("Config")]
    public SqueezeLine SqueezeLinePrefab;
    public SqueezeLine SqueezeLineValidationPrefab;

    public float PointsMinDistance = 0.1f;
    public float UpOffset = 2.0f;
    public float SqueezeDelaySeconds = 0.15f;
    public Vector3 ValidationOffset = new Vector3(-5.0f, 2.0f, 0.0f);
    public bool UsesSine = true;

    private SqueezeLine CurrentSqueezeLine;
    private List<Vector3> CurrentPoints = new List<Vector3>();

    private bool m_isSelected = false;
    public bool IsSelected { get { return m_isSelected; } set { m_isSelected = value; } }

    private ParticleSystem m_ParticleSystem;
    private Vector3 m_StartPos;
    private bool m_PreventSpill = false;
    private bool m_WasMissingTable = false;

    /* For Validation */
    public float ValidationX = 1.0f; // kleiner streckt die kurve in die Länge | größer macht mehr intervalle

    public float ValidationY = 1.0f; // kleiner macht kleinere Kurven | größer größere Kurven
    // -> 0 ist ein strich, 1 ist kurvig

    private SqueezeLine valSqueezeLine;

    private void Start()
    {
        m_ParticleSystem = GetComponentInChildren<ParticleSystem>();
        m_StartPos = transform.position;
    }

    private void Update()
    {
        // DEBUGGING
        if (Input.GetKeyDown(KeyCode.R))
            Reset();
        if (Input.GetKeyDown(KeyCode.T))
            ShowValidationWave();
        if (Input.GetKeyDown(KeyCode.S))
            Debug.Log(name + " Score: " + GetScore());

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
                ray = new Ray(transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit, 1000.0f, ~(1 << 10)))
                {
                    if (m_WasMissingTable)
                    {
                        CurrentSqueezeLine = Instantiate<SqueezeLine>(SqueezeLinePrefab);
                        m_WasMissingTable = false;
                    }

                    // check if current raycast target is further away from any currentPoints than minDistance
                    if (IsPointFurtherAway(hit.point))
                    {
                        // add it to CurrentPoints and CurrentSqueezeLine.AddNewPoint
                        CurrentPoints.Add(hit.point);
                        StartCoroutine(AddNewPoint(CurrentSqueezeLine, hit.point));
                        //CurrentSqueezeLine.AddNewPoint(hit.point);
                    }
                }
                else
                {
                    m_WasMissingTable = true;
                }
            }
        }        
    }

    IEnumerator AddNewPoint(SqueezeLine toLine, Vector3 point)
    {
        yield return new WaitForSeconds(SqueezeDelaySeconds);

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
        m_WasMissingTable = false;

        SqueezeLine[] allLines = FindObjectsOfType<SqueezeLine>();
        foreach (SqueezeLine line in allLines)
        {
            Destroy(line.gameObject);
        }
    }

    public void Select()
    {
        // waits until mousebuttonUp for actually squeezing out stuff
        m_PreventSpill = true;

        IsSelected = true;

        transform.Rotate(Vector3.forward, 180.0f);
        GetComponent<Collider>().enabled = false;
    }

    public void Unselect()
    {
        IsSelected = false;
        transform.position = m_StartPos;
        m_PreventSpill = false;
        m_WasMissingTable = false;
        m_ParticleSystem.Stop();

        transform.Rotate(Vector3.forward, -180.0f);
        GetComponent<Collider>().enabled = true;
    }


    private void ShowValidationWave()
    {
        valSqueezeLine = Instantiate<SqueezeLine>(SqueezeLineValidationPrefab);

        for (float i = 0.0f; i < 10.0f; i+=0.1f)
        {
            valSqueezeLine.AddNewPoint(
                new Vector3(i, 0.0f, 
                (UsesSine ? Mathf.Sin(i * ValidationX) : Mathf.Cos(i * ValidationX)) * ValidationY) + ValidationOffset);
        }
    }

    public float GetScore()
    {
        return Mathf.Clamp(100.0f - valSqueezeLine.GetTotalDeviation(CurrentPoints), 0.0f, 100);
    }

    public void SetValidation(int Val)
    {
        switch(Val)
        {
            case 0:
                ValidationX = 1.0f;
                ValidationY = 1.0f; 
                break;
            case 1:
                ValidationX = 4.0f;
                ValidationY = 1.0f;
                break;
            case 2:
                ValidationX = 1.0f;
                ValidationY = 0.5f;
                break;
            case 3:
                ValidationX = 1.5f;
                ValidationY = 1.0f;
                break;
            default:
            case 4:
                ValidationX = 2.0f;
                ValidationY = 0.5f;
                break;
        }

        ShowValidationWave();
    }
}