using UnityEngine;

/// <summary>
/// 이벤트 필드를 선언하고 다른 객체들이 접근하여 이벤트를 호출할 수 있게 해주는 클래스.
/// </summary>
public class csHandler_Event_Sample : csHandler_Event
{
	#region 상태 관리
	// 아래 필드를 통해서 상태를 시작 및 종료를 호출할 수 있다.
	// 예) 상태 시작 : csHandler_Event_Sample.Pause.Start();
	// 예) 상태 종료 : csHandler_Event_Sample.Pause.Stop();
	// 특정 상태가 현재 활성화 상태인지 체크할 수 있고, 예) if(csHandler_Event_Sample.Pause.Active)
	// 만약 필드에 외부 메소드가 등록돼있다면 시작 및 종료를 호출할 때 해당 메소드도 호출된다.

	public static csState Pause;
	public static csState GameOver;

	#endregion

	#region 명령
	// 아래 필드를 통해서 필드에 등록된 외부 메소드를 호출할 수 있다.
	// csSender는 매개변수가 없는 메소드를 등록 및 호출. 예) csHandler_Event_Sample.EnterNewZone.Send();
	// csSender<T>, csSender<T1, T2> 는 각각 매개변수가 1개, 2개인 메소드를 등록 및 호출. 예) csHandler_Event_Sample.RemoveFormation.Send(해당 타입의 인자);
	// csReturner<T1, T2>는 반환값이 존재하는 메소드를 등록 및 호출. 메소드에 매개변수가 존재할 경우,
	// T1이 매개변수, T2가 반환 타입이다. 호출할 때는 인자값을 넘기며 호출. 예) csHandler_Event_Sample.GetFormation.Return(해당 타입의 인자); 
	// 매개변수가 없을 경우 반환 타입만 지정하여 등록하고 인자 없이 호출. 예) csHandler_Event_Sample.필드이름.Return(); 

	public static csSender EnterNewZone;
	public static csSender<Vector2> RemoveFormation;
	public static csReturner<Vector2, csFormation> GetFormation;

	#endregion

	#region 값 저장 및 읽기
	// 단순히 값을 저장 및 읽기 위한 필드
	// 저장 예) csHandler_Event_Sample.Position_Current.Set(해당 타입의 값);
	// 읽기 예) csHandler_Event_Sample.Position_Current.Get();

	public static csValue<Vector2> Position_Current;

	#endregion
}
