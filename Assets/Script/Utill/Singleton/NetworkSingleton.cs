using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

//게임 매니저 구현을 위한 제너릭 클래스 구현
public class NetworkSingleton<T> : NetworkBehaviour where T : Component
{
    private static T _instance;//게임 매니저의 인스턴스
    [SerializeField]
    private bool _isDontDestroy = false;//삭제방지 여부

    //get 프로퍼티로 인스턴스 반환
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();//
                //지정한 타입의 오브젝트가 없을 시 새로운 GameObject를 생성하고 이름을 바꾼 후 지정되지 않은 유형의 컴포넌트를 추가
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    public override void OnNetworkSpawn()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (_isDontDestroy)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}