using System;
using UnityEngine;

public class IngredientObject : MonoBehaviour, ISelectable
{
    public enum IngredientType
    {
        Tomato,
        Cucumber
    }

    [Header("Config")]
    public IngredientType TypeOfIngredient;
    public GameObject IngredientSlicePrefab;
    public float UpOffset = 2.0f;

    private bool m_isSelected = false;
    public bool IsSelected { get { return m_isSelected; } set { m_isSelected = value; } }

    private GameObject m_CurrentSlice;

    private void Update()
    {
        // handle movement here when it is selected
        if (IsSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1 << 10)) // only on the "table" layer
            {
                m_CurrentSlice.transform.position = hit.point + Vector3.up * UpOffset;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(m_CurrentSlice.transform.position, Vector3.down, out hit))
                {
                    // Put down ingredient slice
                    Instantiate<GameObject>(
                        IngredientSlicePrefab, 
                        hit.point, 
                        Quaternion.Euler(UnityEngine.Random.Range(70.0f,90.0f), 270.0f, 0.0f));
                }
            }
        }
    }

    public void Select()
    {
        m_CurrentSlice = Instantiate<GameObject>(IngredientSlicePrefab);
        Collider col = m_CurrentSlice.GetComponentInChildren<Collider>();
        if (col != null)
            col.enabled = false;

        IsSelected = true;
    }

    public void Unselect()
    {
        IsSelected = false;

        Destroy(m_CurrentSlice);
    }
}
