using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoSessionLeave : MonoBehaviour
{
    [SerializeField]
    private Button leaveSessionBtn;

    private void OnEnable()
    {
        Debug.Log("세션종료");
        StartCoroutine(LeaveSeession());
    }
    IEnumerator LeaveSeession()
    {
        yield return new WaitForSeconds(0.5f);
        leaveSessionBtn.onClick.Invoke();
    }
}
