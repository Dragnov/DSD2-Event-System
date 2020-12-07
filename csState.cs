using System;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// 각 객체 및 게임 전체의 특정 상태를 관리하는 외부 메소드 호출을 위한 이벤트 클래스.
/// 외부 메소드를 꼭 등록할 필요는 없다. 예를들어 현재 특정 상태가 활성화 상태인지 체크하는 용도로도 사용.
/// 외부 메소드를 등록할 때 메소드 이름에는 m_DicPrefixes에 포함 된 접두어가 필요
/// </summary>
public class csState : csEvent
{
    public delegate void Callback(Action _callback = null);

    // 외부 메소드가 등록 될 delegate 변수. 상태의 시작 및 종료를 알린다
    public Callback StartCallback;
    public Callback StopCallback;

	protected bool m_bIsActive;   // 상태의 현재 활성화 상태
    public bool Active          // 활성화 여부를 얻어오거나 값을 설정함
    {
        get
        {
            return m_bIsActive;
        }
        private set
        {
            if ((value == true) && !m_bIsActive)
            {
				m_bIsActive = true;
            }
            else if ((value == false) && m_bIsActive)
            {
				m_bIsActive = false;
            }
        }
    }

    public csState(string _name) : base(_name)
    {
		InitFields();
    }

	/// <summary>
	/// 타입 및 접두어 초기화
	/// </summary>
	protected override void InitFields()
    {
		m_Fields = new FieldInfo[]
		{
			Type.GetField("StartCallback"),
			Type.GetField("StopCallback")
        };

		StoreFieldNames();

		m_DelegateTypes = new Type[] 
        {
            typeof(Callback),
            typeof(Callback)
		};

		m_DicPrefixes = new Dictionary<string, int>()
        {
            { EventPrefixType.OnStart_.ToString(), 0 },
            { EventPrefixType.OnStop_.ToString(), 1 }
        };
    }

	/// <summary>
	/// 이벤트 필드에 외부 메소드 등록
	/// </summary>
	/// <param name="_target"></param>
	/// <param name="_method"></param>
	/// <param name="_variant"></param>
    public override void Register(object _target, string _method, int _variant)
    {
		AddExternalMethodToField(_target, m_Fields[_variant], _method, m_DelegateTypes[_variant]);		
	}

	/// <summary>
	/// 상태 시작 메소드 호출. 만약 인수로 콜백을 전달받으면 콜백을 포함한 델리게이트 호출
	/// </summary>
	/// <param name="_callback"></param>
	public virtual void Start(Action _callback = null)
	{
		// 만약 이미 활성화 상태라면
		if (Active)
        {
            return;
        }

		// 상태 시작이므로 값 설정
		Active = true;

		// 등록된 외부 메소드 호출
		if(StartCallback != null)
		{
			if (_callback != null)
			{
				StartCallback(_callback);
			}
			else
			{
				StartCallback();
			}
		}
	}

	/// <summary>
	/// 상태 종료 메소드 호출. 만약 인수로 콜백을 전달받으면 콜백을 포함한 델리게이트 호출
	/// </summary>
	/// <param name="_callback"></param>
	public virtual void Stop(Action _callback = null)
    {
        // 만약 이미 종료 상태라면
        if (!Active)
        {
			return;
        }

		// 상태 종료이므로 값 설정
		Active = false;

		// 등록된 외부 메소드 호출
		if (StopCallback != null)
		{			
			if (_callback != null)
			{
				StopCallback(_callback);
			}
			else
			{
				StopCallback();
			}
		}		
    }
}

