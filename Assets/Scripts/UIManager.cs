using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{

    // PlayerController 객체 가져오기 
    private PlayerController playerController;

    [SerializeField] private GameObject settingPanel;



    private void Awake()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }

    private void Start() {
        settingPanel.SetActive(false);
    }

    // 캐릭터 방향키 관련 

    public void onLeftButtonDown()
    {
        playerController.MoveLeft();
    }


    public void onRightButtonDown()
    {
        playerController.MoveRight();
    }
    public void OnLeftButtonUp()
    {
        playerController.StopMoveLeft();
    }

    public void OnRightButtonUp()
    {
        playerController.StopMoveRight();
    }

    // 셋팅 예 아니오 조절 
    public void onClickYesButtonDown() {
        Application.Quit();
    }


    public void onClickNoButtonDown() {
        settingPanel.SetActive(false);
    }

    public void onClickSettingButtonDown() {
        settingPanel.SetActive(true);
    }

}
