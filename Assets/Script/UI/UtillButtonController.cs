using UnityEngine;

public class UtillButtonController : MonoBehaviour
{
    public void OpenPopup(GameObject popupObj)
    {
        popupObj.SetActive(true);
    }

    public void ClosePopup(GameObject popupObj)
    {
        popupObj.SetActive(false);
    }
    
    public void ExitGame()
    {
        // 게임 종료
        Application.Quit();

        // 에디터에서 실행 중일 때 플레이 모드 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
