using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이벤트 필드에 등록할 메소드 선언 예
/// </summary>
public class ExampleEventRegister : MonoBehaviour
{
    private void Start()
    {
        // csHandler_Event_Sample : csHandler_Event를 상속받은 클래스
        // Register함수를 통해서 자신을 인자로 넘겨주면 OnSend_, OnReturn_, OnStart_, OnStop_ 
        // 등의 접두어가 포함된 메소드만 추출하여 해당하는 이벤트 필드에 등록한다.
        csHandler_Event_Sample.Instance.Register(this);
    }

    /// <summary>
    /// 접두어가 포함되지 않은 메소드는 등록 대상에서 제외
    /// </summary>
    private void Pause()
    {

    }

    private void OnStart_Pause(Action _callback = null)
    {
        /*
         * 해당 기능 실행
         */

        // 콜백이 존재할 경우 호출
        _callback?.Invoke();
    }

    private void OnStop_Pause(Action _callback = null)
    {   
        /*
         * 해당 기능 실행
         */

        // 콜백이 존재할 경우 호출
        _callback?.Invoke();
    }

    private void OnSend_MoveUp()
    {
        /*
         * 해당 기능 실행
         */
    }

    private void OnSend_RemoveBlock(Vector2 _value)
    {
        /*
         * 해당 기능 실행
         */
    }

    private void OnReturn_GetBlock(Vector2 _value)
    {
        /*
         * 해당 기능 실행
         */
    }
}
