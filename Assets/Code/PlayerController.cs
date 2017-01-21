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

public class PlayerController : BitStrap.Singleton<PlayerController>
{
    [Header("Config")]
    public GameObject BtnFinishHotDog;
    public Transform HotDogSpawn;
    public GameObject HotDogPrefab;

    [Space(5f)]
    public IngredientObject IngrCucumber;
    public IngredientObject IngrTomato;
    public IngredientObject IngrCheese;
    public IngredientObject IngrOnion;

    [Space(5f)]
    public SqueezeBottle BottleKetchup;
    public SqueezeBottle BottleMustard;
    public SqueezeBottle BottleChocolate;


    private ISelectable m_SelectedObject;
    private Order m_SelectedOrder;
    private GameObject m_HotDogObject;

    private void Start()
    {
        BtnFinishHotDog.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.GetComponent<MonoBehaviour>() is ISelectable)
            {
                if (m_SelectedObject != null)
                    m_SelectedObject.Unselect();

                m_SelectedObject = hit.transform.GetComponent<MonoBehaviour>() as ISelectable;
                m_SelectedObject.Select();
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


    public bool OnOrderChosen(Order ChosenOrder)
    {
        if (m_SelectedOrder == null)
        {
            m_SelectedOrder = ChosenOrder;
            BtnFinishHotDog.SetActive(true);

            m_HotDogObject = Instantiate<GameObject>(HotDogPrefab, HotDogSpawn);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnFinishHotDogClicked()
    {
        // TODO validate the m_SelectedOrder with our hot dog

        m_SelectedOrder.GuestIsAngry();

        // Back to initial state
        ResetAll();
        Destroy(m_HotDogObject);
        m_SelectedOrder = null;
        BtnFinishHotDog.SetActive(false);
    }

    private void ResetAll()
    {
        IngrCheese.Reset();
        IngrCucumber.Reset();
        IngrOnion.Reset();
        IngrTomato.Reset();

        BottleChocolate.Reset();
        BottleKetchup.Reset();
        BottleMustard.Reset();
    }
}
