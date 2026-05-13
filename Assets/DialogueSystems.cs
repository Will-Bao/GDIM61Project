using UnityEngine;
using TMPro;

public class DialogueSystems : MonoBehaviour
{
    [SerializeField] private TMP_Text _dialogue;
    [SerializeField] private GameObject _dialoguebox;
    public void Say()   
    {
        if (_dialogue != null)
        {
            _dialogue.text = "";
            _dialoguebox.SetActive(true);
        }
        else
        {
            _dialoguebox.SetActive(false);
            _dialogue.text = "Test";
        }
    }
}
