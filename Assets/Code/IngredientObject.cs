using System;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : MonoBehaviour, ISelectable, IValidatable
{
    public enum IngredientType
    {
        Tomato,
        Cucumber,
        Cheese,
        Onion,
        Banana
    }

    [Header("Config")]
    public IngredientType TypeOfIngredient;
    public GameObject IngredientSlicePrefab;
    public float UpOffset = 2.0f;
    public Vector3 RotOffset = new Vector3();

    private bool m_isSelected = false;
    public bool IsSelected { get { return m_isSelected; } set { m_isSelected = value; } }

    private GameObject m_CurrentSlice;

    /* Validation */
    public int ValidationCount = 2;

    private List<GameObject> ValidationPlacedSlices = new List<GameObject>();

    private void Update()
    {
        // handle movement here when it is selected
        if (IsSelected && Time.timeScale > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1 << 10)) // only on the "table" layer
            {
                m_CurrentSlice.transform.position = hit.point + Vector3.up * UpOffset;
            }

            if (Physics.Raycast(ray, out hit) && hit.transform.GetComponent<MonoBehaviour>() is ISelectable)
            {
                // don't put down when we're selecting another ingredient right now
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(m_CurrentSlice.transform.position, Vector3.down, out hit))
                {
                    // Put down ingredient slice
                    ValidationPlacedSlices.Add(Instantiate<GameObject>(
                        IngredientSlicePrefab, 
                        hit.point, 
                        Quaternion.Euler(
                            RotOffset.x + UnityEngine.Random.Range(-15.0f, 15.0f), 
                            RotOffset.y, 
                            RotOffset.z + UnityEngine.Random.Range(-15.0f, 15.0f))));

                    AudioManager.Instance.PlayIngredient();
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

        AudioManager.Instance.PlayPickupIngredient();
    }

    public void Unselect()
    {
        IsSelected = false;

        Destroy(m_CurrentSlice);
    }

    public void Reset()
    {
        foreach(GameObject slice in ValidationPlacedSlices)
        {
            Destroy(slice);
        }
        ValidationPlacedSlices = new List<GameObject>();
    }

    public float GetScore()
    {
        int diff = Mathf.Abs(ValidationCount - ValidationPlacedSlices.Count);

        if (diff == 0)
            return 20f * ValidationCount;
        else if (diff >= 1 && diff <= 2 && ValidationPlacedSlices.Count > 0)
            return 10f * ValidationCount;
        else if (diff >= 3 && diff <= 4 && ValidationPlacedSlices.Count > 0)
            return 5f * ValidationCount;
        else
            return 0.0f;
    }

    public void SetValidation(int Val)
    {
        ValidationCount = Val;
    }

    public bool IsValidationActive()
    {
        return ValidationCount > 0;
    }
}
