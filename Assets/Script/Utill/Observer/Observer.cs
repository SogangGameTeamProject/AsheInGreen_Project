using UnityEngine;

namespace AshGreen.Obsever
{
    //옵저버 패턴 구현을 위한 옵저버 클래스
    public abstract class Observer : MonoBehaviour
    {
        public abstract void Notify(Subject subject);
    }
}
