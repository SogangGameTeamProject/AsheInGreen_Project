using AshGreen.Character;
using System.Collections;
using Unity.Netcode;
using Unity.Services.Multiplayer;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Animator m_menuAnimator;

    [SerializeField]
    private CharacterConfig[] m_characterDatas;

    [SerializeField]
    private AudioClip m_confirmClip;

    private bool m_pressAnyKeyActive = true;
    private const string k_enterMenuTriggerAnim = "enter_menu";

    [SerializeField]
    private SceneName nextScene = SceneName.CharacterSelection;

    private IEnumerator Start()
    {
        ClearAllCharacterData();
        // Wait for the network Scene Manager to start
        yield return new WaitUntil(() => NetworkManager.Singleton.SceneManager != null);
        // detecting the events
        LoadingSceneManager.Instance.Init();
    }

    public void OnClickHost(ISession session)
    {
        //AudioManager.Instance.PlaySoundEffect(m_confirmClip);
        Debug.Log(session.Code);
        PlayerPrefs.SetString("SessionCode", session.Code);
        LoadingSceneManager.Instance.LoadScene(nextScene);
    }

    public void OnClickJoin(ISession session)
    {
        PlayerPrefs.SetString("SessionCode", session.Code);
        AudioManager.Instance.PlaySoundEffect(m_confirmClip);
        StartCoroutine(Join());
    }

    public void OnClickQuit()
    {
        AudioManager.Instance.PlaySoundEffect(m_confirmClip);
        Application.Quit();
    }

    private void ClearAllCharacterData()
    {
        // Clean the all the data of the characters so we can start with a clean slate
        foreach (CharacterConfig data in m_characterDatas)
        {
            data.EmptyData();
        }
    }

    // We use a coroutine because the server is the one who makes the load
    // we need to make a fade first before calling the start client
    private IEnumerator Join()
    {
        LoadingFadeEffect.Instance.FadeAll();

        yield return new WaitUntil(() => LoadingFadeEffect.s_canLoad);

        //NetworkManager.Singleton.StartClient();
    }
}
