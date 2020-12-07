using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 반환값이 있는 외부 메소드 호출을 위한 이벤트 클래스.
/// 외부 메소드의 이름에는 m_DicPrefixes에 포함 된 접두어가 필요
/// </summary>
/// <typeparam name="T"></typeparam>
public class csReturner<T> : csEvent
{
	public delegate V Returner<V>();

	// 외부 메소드가 등록 될 delegate 변수. 
	// 이 변수를 호출하여 외부 메소드를 호출함
	public Returner<T> Return;

	public csReturner(string _name) : base(_name)
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
			GetType().GetField("Return")
		};

		StoreFieldNames();

		m_DelegateTypes = new Type[]
		{
			typeof(Returner<>)
		};

		m_DicPrefixes = new Dictionary<string, int>()
		{
			{ EventPrefixType.OnReturn_.ToString(), 0 }
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
/// 매개 변수 두 개를 사용하여 어떤 델리게이트를 등록하고 
/// 호출하느냐에 따라 한 개의 인수를 전달하여 반환값을 얻을 수 있다.
/// 외부 메소드의 이름에는 m_DicPrefixes에 포함 된 접두어가 필요
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public class csReturner<T1, T2> : csEvent
{
	public delegate V2 Returner<V1, V2>(V1 _value);
	
	public Returner<T1, T2> Return;

	public csReturner(string _name) : base(_name)
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
			GetType().GetField("Return")
		};

		StoreFieldNames();

		m_DelegateTypes = new Type[]
		{
			typeof(csReturner<,>.Returner<,>)
		};

		m_DicPrefixes = new Dictionary<string, int>()
		{
			{ EventPrefixType.OnReturn_.ToString(), 0 }
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
