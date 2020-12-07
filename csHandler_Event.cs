using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 이벤트 필드(csEvent를 상속받은 Evnet들)를 선언하고 특정 객체가 자신의 메소드를
/// 필드에 등록하여 다른 객체가 필드를 호출함으로 인해 필드에 등록된 해당 객체의 메소드를 
/// 호출할 수 있게 하는 클래스.
/// 필드 선언은 이 클래스를 상속받은 클래스에서 이뤄진다
/// </summary>
public class csHandler_Event : Singleton<csHandler_Event>
{
	#region Registration Field

	protected Dictionary<string, csEvent> m_Dictionary_MethodName_Event = new Dictionary<string, csEvent>();

	private Dictionary<object, List<csEvent>> m_Dictionary_Target_EventList = new Dictionary<object, List<csEvent>>();

	// 이벤트 모음
	protected List<csEvent> m_List_Event = new List<csEvent>();

	#endregion

	protected override void Awake()
	{
		base.Awake();

		if (Instance != this)
		{
			return;
		}

		DontDestroyOnLoad(gameObject);

		InstantiateField();
	}

	/// <summary>
	/// 선언된 이벤트 필드의 인스턴스화 및 외부 메소드를 등록하기 위한 준비
	/// </summary>
	protected void InstantiateField()
	{
		FieldInfo[] FieldInfos = GetType().GetFields((BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static));

		// 이벤트 필드 처리
		for (int i = 0; i < FieldInfos.Length; i++)
		{
			object Obj;

			// 필드의 해당 타입과 이름으로 인스턴스 생성
			Obj = Activator.CreateInstance(FieldInfos[i].FieldType, FieldInfos[i].Name);

			// 생성한 인스턴스를 필드에 값으로 설정
			FieldInfos[i].SetValue(this, Obj);

			// 모든 이벤트 필드는 csEvent 클래스를 상속 받음. 상속받는 공통 클래스로 캐스팅하여 동일한 처리
			csEvent EventObj = (csEvent)Obj;

			// 접두어가 선언되지 않았다는 것은 외부 메소드의 기능 없이 단독으로 기능을 수행하는 클래스이므로 건너뜀 예) csValue
			if (EventObj.DicPrefixes == null)
			{
				continue;
			}

			if (!m_List_Event.Contains(EventObj))
			{
				m_List_Event.Add(EventObj);
			}

			// 이벤트 필드의 접두어를 추출, 필드 이름과 묶어서 Key로 삽입한다. (예 : OnStart_(접두어) + NormalAttack(필드 이름) = OnStart_NormalAttack)
			// 이 Key는 외부 메소드의 이름이 되며 외부 메소드 식별에 사용된다.
			List<string> List_MethodName = new List<string>();
			foreach (string prefixes in EventObj.DicPrefixes.Keys)
			{
				List_MethodName.Add(prefixes + FieldInfos[i].Name);

				m_Dictionary_MethodName_Event.Add(prefixes + FieldInfos[i].Name, (csEvent)Obj);
			}
		}
	}

	/// <summary>
	/// 특정 객체가 자신의 메소드를 특정 이벤트 필드에 등록하기 위한 메소드.
	/// 이벤트 필드의 식별은 메소드 이름으로 이루어진다
	/// </summary>
	/// <param name="_target"></param>
	public void Register(object _target)
	{
		// 매개변수로 전달받은 클래스의 메소드 추출. List_MethodInfo에 저장한다.
		List<MethodInfo> List_MethodInfo = GetMethods(_target.GetType());

		// InstantiateField 메소드에서 저장한 m_Dictionary_MethodName_Event의 csEvent 클래스를 추출하여 저장하기 위한 용도
		csEvent Event;

		// 추출한 메소드를 이벤트 필드 내의 델리게이트 변수에 등록
		for (int i = 0; i < List_MethodInfo.Count; i++)
		{
			// Key에 해당하는 메소드 이름이면 Value를 추출한 후 계속 진행. 없으면 건너뜀
			if (!m_Dictionary_MethodName_Event.TryGetValue(List_MethodInfo[i].Name, out Event))
			{
				continue;
			}

			// 이벤트 필드에서 접두어를 선언할 때 아래 예처럼 접두어 개수가 복수일 때를 감안하여 식별을 위해 int형 값을 쌍으로 등록한다
			// Prefixes = new Dictionary<string, int>() { { EventPrefixType.OnStart_.ToString(), 0 }, { EventPrefixType.OnStop_.ToString(), 1 } };          

			// out int Index는 그 쌍으로 등록된 값을 추출하여 저장하기 위한 용도

			// 접두어는 접두어 끝에 '_'가 붙어있다 (예 : OnStart_) 
			// 메소드 이름에서 '_'를 기준으로 접두어만 빼내어 그 접두어에 해당하는 값을 추출(예 : OnStart_NormalAttack(메소드 이름) => OnStart_)
			Event.DicPrefixes.TryGetValue(List_MethodInfo[i].Name.Substring(0, List_MethodInfo[i].Name.IndexOf('_') + 1), out int Index);

			// 필드에 메소드 등록(정확히는 필드 내의 델리게이트 변수에 등록)
			// _target : 외부 메소드가 선언된 클래스, m_List_MethodInfo[i].Name : 외부 메소드 이름, Index : 접두어 식별값
			Event.Register(_target, List_MethodInfo[i].Name, Index);
			
			AddEventConnectedToTarget(Event, _target);
		}
	}

	public void Unregister(object _target)
	{
		RemoveEventConnectedToTarget(_target);
	}

	/// <summary>
	/// 이벤트 필드에 등록할 특정 객체의 메소드 얻어오기
	/// </summary>
	/// <param name="_type"></param>
	/// <returns></returns>
	protected List<MethodInfo> GetMethods(Type _type)
	{
		List<MethodInfo> List_MethodInfo = new List<MethodInfo>();

		// 메소드 얻어옴
		MethodInfo[] ArrayMethods = _type.GetMethods((BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

		// 얻어온 메소드에서 식별 작업을 통해 해당하는 메소드일 경우 저장
		for (int i = 0; i < ArrayMethods.Length; i++)
		{
			// 접두어는 접두어 끝에 '_'가 붙어있다 (예 : OnStart_) 
			// 메소드 이름에서 '_'를 기준으로 접두어 추출
			string GetPrefix = ArrayMethods[i].Name.Substring(0, ArrayMethods[i].Name.IndexOf('_') + 1);

			// 예외처리. 얻어온 모든 메소드가 해당하는 접두어를 가지고 있다는 보장이 없기 때문
			try
			{
				// 추출한 접두어(GetPrefix)가 열거형 SupportedPrefixes에 해당하는 접두어인지 확인
				Enum.Parse(typeof(EventPrefixType), GetPrefix);
			}
			catch
			{
				// 접두어가 열거형 SupportedPrefixes에 해당하지 않으면 진행하지 않고 건너뛴다
				continue;
			}

			// 여기까지 진행 되면 이벤트에 등록될 메소드라는 의미이므로 저장
			List_MethodInfo.Add(ArrayMethods[i]);
		}

		return List_MethodInfo;
	}

	/// <summary>
	/// 자신의 메소드를 등록하려는 특정 객체와 그 객체의 메소드가 등록된 이벤트 저장
	/// </summary>
	/// <param name="_event"></param>
	/// <param name="_target"></param>
	private void AddEventConnectedToTarget(csEvent _event, object _target)
	{
		string EventName = _event.EventName;

		Component Target = (Component)_target;

		if (m_Dictionary_Target_EventList.ContainsKey(_target))
        {
			if(!m_Dictionary_Target_EventList[_target].Contains(_event))
            {
				m_Dictionary_Target_EventList[_target].Add(_event);			
			}							
		}
		else
        {
			List<csEvent> List_Event = new List<csEvent>
			{
				_event
			};

			m_Dictionary_Target_EventList.Add(_target, List_Event);			
		}
	}

	private void RemoveEventConnectedToTarget(object _target)
	{
		if (!m_Dictionary_Target_EventList.ContainsKey(_target))
        {
			return;
        }

		csEvent[] Array_Event = m_Dictionary_Target_EventList[_target].ToArray();

		for(int i = 0; i < Array_Event.Length; i++)
        {
			Array_Event[i].Unregister(_target);
        }

		m_Dictionary_Target_EventList.Remove(_target);	
	}
}

