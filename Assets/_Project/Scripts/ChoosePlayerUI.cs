using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlayerUI : MonoBehaviour
{
    [SerializeField] private Button _knightBtn;
    [SerializeField] private Button _wizardBtn;

    void Awake()
    {
        _knightBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ChoosePlayer(PlayerType.Knight);
            GameManager.Instance.PlayGame();
            this.gameObject.SetActive(false);
        });
        _wizardBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ChoosePlayer(PlayerType.Wizard);
            GameManager.Instance.PlayGame();
            this.gameObject.SetActive(false);
        });
    }
}
