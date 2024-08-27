using System.Collections;
using UnityEngine;


namespace AshGreen.Obsever
{
    //옵저버 패턴 구현을 위한 서브젝트 객체
    public abstract class Subject : MonoBehaviour
    {
        private readonly ArrayList _observers = new ArrayList();//옵저버들이 저장될 배열리스트

        //옵저버 객체 추가
        public void Attach(Observer observer)
        {
            _observers.Add(observer);
        }

        //옵저버 객체 제거
        public void Detach(Observer observer)
        {
            _observers.Remove(observer);
        }

        //옵저버들에게 알림 발생
        public void NotifyObservers()
        {
            foreach (Observer observer in _observers)
            {
                observer.Notify(this);
            }
        }
    }
}
