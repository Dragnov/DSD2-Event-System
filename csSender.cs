using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 매개변수가 없는 외부 메소드 호출을 위한 이벤트 클래스.
/// 외부 메소드의 이름에는 m_DicPrefixes에 포함 된 접두어가 필요
/// </summary>
public class csSender : csEvent
{
    public delegate void Sender();

	// 외부 메소드가 등록 될 delegate 변수. 
	// 이 변수를 호출하여 외부 메소드를 호출함
	public Sender Send;  

    public csSender(string _name) : base(_name)
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
            GetType().GetField("Send")
        };

		StoreFieldNames();

		m_DelegateTypes = new Type[] 
        {
            typeof(Sender)
        };

		m_DicPrefixes = new Dictionary<string, int>()
        {
            { EventPrefixType.OnSend_.ToString(), 0 }
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
        if (_method == null)
        {
            return;
        }

        Send += (Sender)Delegate.CreateDelegate(m_DelegateTypes[_variant], _target, _method);
    }
}

/// <summary>
/// 매개변수가 포함 된 외부 메소드 호출을 위한 이벤트 클래스
/// 외부 메소드의 이름에는 m_DicPrefixes에 포함 된 접두어가 필요
/// </summary>
/// <typeparam name="T"></typeparam>
public class csSender<T> : csSender
{
    public delegate void Sender<V>(V _value);

	// 외부 메소드가 등록 될 delegate 변수.
	// 이 변수를 호출하여 외부 메소드를 호출함.
	// 한 개의 매개변수 전달 가능
	public new Sender<T> Send;

	public csSender(string _name) : base(_name) { }

	/// <summary>
	/// 타입 및 접두어 초기화
	/// </summary>
	protected override void InitFields()
    {
        m_Fields = new FieldInfo[] 
        {
            GetType().GetField("Send")
        };

		StoreFieldNames();

		m_DelegateTypes = new Type[]
		{
			typeof(csSender<>.Sender<>)
        };

		m_DicPrefixes = new Dictionary<string, int>()
        {
            { EventPrefixType.OnSend_.ToString(), 0 }
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
        if (_method == null)
        {
            return;
        }

		AddExternalMethodToField(_target, m_Fields[_variant], _method, MakeGenericType(m_DelegateTypes[_variant]));
    }
}

/// <summary>
/// 매개변수가 두 개 포함 된 외부 메소드 호출을 위한 이벤트 클래스
/// 외부 메소드의 이름에는 m_DicPrefixes에 포함 된 접두어가 필요
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public class csSender<T1, T2> : csSender
{
	public delegate void Sender<V1, V2>(V1 _value1, V2 _value2);

	// 외부 메소드가 등록 될 delegate 변수. 
	// 이 변수를 호출하여 외부 메소드를 호출함
	public new Sender<T1, T2> Send;  

	public csSender(string _name) : base(_name) { }

	/// <summary>
	/// 타입 및 접두어 초기화
	/// </summary>
	protected override void InitFields()
	{
		m_Fields = new FieldInfo[]
		{
			GetType().GetField("Send")
		};

		StoreFieldNames();

		m_DelegateTypes = new Type[]
		{
			typeof(csSender<,>.Sender<,>)
		};

		m_DicPrefixes = new Dictionary<string, int>()
		{
			{ EventPrefixType.OnSend_.ToString(), 0 }
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
		if (_method == null)
		{
			return;
		}

		AddExternalMethodToField(_target, m_Fields[_variant], _method, MakeGenericType(m_DelegateTypes[_variant]));		
	}
}

/// <summary>
/// 매개변수가 세 개 포함 된 외부 메소드 호출을 위한 이벤트 클래스
/// 외부 메소드의 이름에는 m_DicPrefixes에 포함 된 접두어가 필요
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
public class csSender<T1, T2, T3> : csSender
{
	public delegate void Sender<V1, V2, V3>(V1 _value1, V2 _value2, V3 _value3);

	public new Sender<T1, T2, T3> Send;  

	public csSender(string _name) : base(_name)
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
			GetType().GetField("Send")
		};

		StoreFieldNames();

		m_DelegateTypes = new Type[]
		{
			typeof(csSender<,,>.Sender<,,>)
		};

		m_DicPrefixes = new Dictionary<string, int>()
		{
			{ EventPrefixType.OnSend_.ToString(), 0 }
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
		if (_method == null)
		{
			return;
		}

		AddExternalMethodToField(_target, m_Fields[_variant], _method, MakeGenericType(m_DelegateTypes[_variant]));
	}
}
