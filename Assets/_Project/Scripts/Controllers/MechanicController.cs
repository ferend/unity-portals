using UnityEngine;

public class MechanicController : MonoBehaviour
{
    public Mechanic[] states;
    private int _index;

    private void Awake()
    {
        _index = 0;
    }

    public Mechanic GetActiveMechanics()
    {
        return states[_index];
    }
    private void Update()
    {
        GetActiveMechanics().KeyboardControls();
        
        if (Input.GetMouseButtonDown(0))
            GetActiveMechanics().OnDown();
        else if (Input.GetMouseButton(0))
            GetActiveMechanics().OnDrag();
        else if (Input.GetMouseButtonUp(0))
            GetActiveMechanics().OnUp();
    }
}
