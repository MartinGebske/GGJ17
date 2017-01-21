using UnityEngine;

public interface ISelectable
{
    void Select();
    void Unselect();
}

public interface IValidatable
{
    float GetScore(); // 100.0 is perfect, 0.0 is bad.
    void SetValidation(int Val); // set the validation for this hot dog. 0 is inactive
    bool IsValidationActive();
}

public class PlayerController : MonoBehaviour
{
    private ISelectable m_SelectedObject;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<MonoBehaviour>() is ISelectable)
                {
                    if (m_SelectedObject != null)
                        m_SelectedObject.Unselect();

                    m_SelectedObject = hit.transform.GetComponent<MonoBehaviour>() as ISelectable;
                    m_SelectedObject.Select();
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (m_SelectedObject != null)
            {
                m_SelectedObject.Unselect();
                m_SelectedObject = null;
            }
        }
    }
}
