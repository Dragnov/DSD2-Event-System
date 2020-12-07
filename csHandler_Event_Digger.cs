using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 프로젝트에서 접근을 용이하게 하기 위해 전역적으로 사용 가능한 이벤트 모음
/// </summary>
public class csHandler_Event_Digger : csHandler_Event
{
	#region 상태 관리
	// 아래 필드를 통해서 상태를 시작 및 종료를 호출할 수 있다.
	// 예) csHandler_Event_Digger.Play.Start();
	// 특정 상태가 현재 활성화 상태인지 체크할 수 있고,
	// 만약 필드에 외부 메소드가 등록돼있다면 시작 및 종료를 호출할 때 해당 메소드도 호출된다.

	public static csState Play;				
	public static csState Pause;			
	public static csState Downward;			
	public static csState Fever;			
	public static csState Fever_Background;
	public static csState Death;
	public static csState GameOver;
	public static csState Booster;

	#endregion

	#region 명령
	// 아래 필드를 통해서 필드에 등록된 외부 메소드를 호출할 수 있다.
	// csSender는 매개변수가 없는 메소드를 등록 및 호출. 예) csHandler_Event_Digger.EnterNewZone.Send();
	// csSender<T>, csSender<T1, T2> 는 각각 매개변수가 1개, 2개인 메소드를 등록 및 호출. 예) csHandler_Event_Digger.RemoveFormation.Send(해당 타입의 인자);
	// csReturner<T1, T2>는 반환값이 존재하는 메소드를 등록 및 호출. 메소드에 매개변수가 존재할 경우,
	// T1이 매개변수, T2가 반환 타입이다. 호출할 때는 인자값을 넘기며 호출. 예) csHandler_Event_Digger.GetFormation.Return(해당 타입의 인자); 
	// 매개변수가 없을 경우 반환 타입만 지정하여 등록하고 인자 없이 호출. 예) csHandler_Event_Digger.필드이름.Return(); 

	public static csReturner<Vector2, csFormation> GetFormation;
	public static csSender<Vector2> RemoveFormation;
	public static csSender<float> Set_HUD_Fever;
	public static csSender<float> Set_Volume_SFX;
	public static csSender<Action> Downward_Action_Shake;
	public static csSender EnterNewZone;
	public static csSender InitializeFever;
	public static csSender SucceedCombo;
	public static csSender Stop_Chase_Monster;
	public static csSender<DeathType, Action> Death_Action;
	public static csSender<Action> Concealment_Monster;
	public static csSender<Action> Continue_Action_Earthquake;
	public static csSender<Action> Continue_Action_Revival;
	public static csSender Continue_Action_Fall_Block;

	public static csSender BatchingTest;

	#endregion

	#region 값 저장 및 읽기
	// 단순히 값을 저장 및 읽기 위한 필드
	// 저장 예) csHandler_Event_Digger.Buff.Set(해당 타입의 값);
	// 읽기 예) csHandler_Event_Digger.Buff.Get();

	public static csValue<Dictionary<int, csBuff>> Buff;
	public static csValue<DirectionType> UpwardDirection;
	public static csValue<float> Distance_Digger_Monster;
	public static csValue<DeathType> DeathType;
	public static csValue<Vector2> Position_Current_Digger;

	#endregion
}
