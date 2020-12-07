using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이벤트 클래스의 최상위 클래스.
/// 상속받는 이벤트들의 공통적인 메소드 및 필드 정의
/// </summary>
public abstract class csEvent {

    #region Field

    protected FieldInfo[] m_Fields;             // delegate 타입으로 선언된 필드의 필드 정보 모음

    protected Type[] m_DelegateTypes;           // delegate 타입으로 선언된 필드의 타입 정보 모음
	
	protected Dictionary<string, int> m_DicPrefixes;    // 이벤트에 등록될 외부 메소드를 구분하기 위한 접두어 모음
    public Dictionary<string, int> DicPrefixes
    {
        get
        {
            return m_DicPrefixes;
        }
    }

    public string[] FieldNames;					// 이벤트 필드 이름 모음

    protected Type m_Type = null;               // 클래스의 타입
    public Type Type
    {
        get
        {
            if(m_Type == null)
            {
                m_Type = GetType();
            }

            return m_Type;
        }
    }

    protected string m_EventName = null;        // 이벤트 식별에 사용됨
    public string EventName
    {
        get
        {
            return m_EventName;
        }
    }

    #endregion

    public csEvent(string _name = "")
    {
        m_EventName = _name;
    }

    #region Abstract Method

    /// <summary>
    /// 필드 초기화
    /// </summary>
    protected abstract void InitFields();

    /// <summary>
    /// 이벤트에 외부 메소드 등록
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="_method"></param>
    /// <param name="_variant"></param>
    public abstract void Register(object _target, string _method, int _variant);

    #endregion

    /// <summary>
    /// 이벤트 필드에 등록된 메소드 해제
    /// </summary>
    /// <param name="_target"></param>
    public virtual void Unregister(object _target)
    {
        RemoveExternalMethodFromField(_target);
    }

    /// <summary>
    /// 이벤트 필드에 등록된 메소드 전체 해제
    /// </summary>
    public virtual void Unregister()
    {
        RemoveAllExternalMethodFromField();
    }  

    /// <summary>
    /// 외부 메소드가 등록될 델리게이트 변수 이름 저장
    /// </summary>
	protected void StoreFieldNames()
	{
		FieldNames = new string[m_Fields.Length];

		for (int i = 0; i < m_Fields.Length; i++)
		{
			FieldNames[i] = m_Fields[i].Name;
		}
	}

    /// <summary>
    /// 클래스 인수 개수에 따른 Generic Type 생성
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    protected Type MakeGenericType(Type _type)
    {
		Type MakeType = null;
		Type[] GenericArguments = Type.GetGenericArguments();
        Debug.LogError(GenericArguments.Length);

        for(int i = 0; i < GenericArguments.Length; i++)
        {
            Debug.Log(GenericArguments[i].Name);
        }

		switch(GenericArguments.Length)
		{
			case 1:

				MakeType = _type.MakeGenericType(new Type[] { GenericArguments[0], GenericArguments[0] });
				 
				break;

			case 2:

				MakeType = _type.MakeGenericType(new Type[] { GenericArguments[0], GenericArguments[1], GenericArguments[0], GenericArguments[1] });

				break;

			case 3:

				MakeType = _type.MakeGenericType(new Type[] { GenericArguments[0], GenericArguments[1], GenericArguments[2], GenericArguments[0], GenericArguments[1], GenericArguments[2] });

				break;
		}

		return MakeType;
    }

    /// <summary>
    /// 필드에 외부 메소드 등록
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="_field"></param>
    /// <param name="_method"></param>
    /// <param name="_type"></param>
    protected void RegisterExternalMethodToField(object _target, FieldInfo _field, string _method, Type _type)
    {        
        // 메소드가 포함된 클래스, 메소드 이름, 등록하려는 클래스 타입에 맞춰서 생성한 타입으로 델리게이트 생성  
        Delegate Delegate = Delegate.CreateDelegate(_type, _target, _method);
      
        if (Delegate == null)
        {
            return;
        }

        // 필드(해당 이벤트 클래스의 델리게이트 변수)에 델리게이트 등록
        _field.SetValue(this, Delegate);
    }

    /// <summary>
    /// 필드에 복수의 외부 메소드 등록
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="_field"></param>
    /// <param name="_method"></param>
    /// <param name="_type"></param>
    protected void AddExternalMethodToField(object _target, FieldInfo _field, string _method, Type _type)
	{
        // 복수의 메소드 등록의 경우 델리게이트를 생성 후
        Delegate Delegate = Delegate.CreateDelegate(_type, _target, _method);
        // Combine으로 연결함
        Delegate Assignment = Delegate.Combine((Delegate)_field.GetValue(this), Delegate);
		
		if(Assignment == null)
		{
			return;
		}

		_field.SetValue(this, Assignment);
	}

    /// <summary>
    /// 이벤트 필드에 자신의 메소드를 등록한 객체에 해당되는 메소드 제거
    /// </summary>
    /// <param name="_target"></param>
    private void RemoveExternalMethodFromField(object _target)
    {
        for(int i = 0; i < m_Fields.Length; i++)
        {
            Delegate DelegateConnectedToField = (Delegate)m_Fields[i].GetValue(this);

            if (DelegateConnectedToField == null)
            {
                continue;
            }

            Delegate[] Delegates = DelegateConnectedToField.GetInvocationList();

            for (int j = 0; j < Delegates.Length; j++)
            {
                if (Delegates[j].Target == _target)
                {
                    DelegateConnectedToField = Delegate.Remove(DelegateConnectedToField, Delegates[j]);
                }
            }
 
            m_Fields[i].SetValue(this, DelegateConnectedToField);
        }         
    }

    /// <summary>
    /// 이벤트 필드에 등록된 메소드 전체 제거
    /// </summary>
	protected void RemoveAllExternalMethodFromField()
	{
		for (int i = 0; i < m_Fields.Length; i++)
		{
			FieldInfo Field = m_Fields[i];

			Delegate DelegateConnectedToField = (Delegate)Field.GetValue(this);

			if (DelegateConnectedToField == null)
			{
				continue;
			}

			DelegateConnectedToField = Delegate.RemoveAll(DelegateConnectedToField, DelegateConnectedToField);

			Field.SetValue(this, DelegateConnectedToField);
		}
	}
}

