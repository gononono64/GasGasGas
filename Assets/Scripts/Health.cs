using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class Health : NetworkBehaviour
{

    
    [SyncVar] [SerializeField] private int _currentHealth = 100;
    [SyncVar] [SerializeField] private int _maxHealth = 100;
    

    public int CurrentHealth { get { return _currentHealth; } private set { _currentHealth = value; } }//This is just a wrapper for _currentHealth
    public int MaxHealth { get { return _maxHealth; }  set { _maxHealth = value; } } // same dealy
    public bool IsAlive { get { return _isAlive; } set { _isAlive = value; } } // same dealy


    [SyncVar(hook = nameof(IsAliveHook))] [SerializeField] private bool _isAlive = true;
    public event Action OnDeath;
    public event Action OnRevive;

    private void Start()
    {
        Reset();
    }
    void IsAliveHook(bool _old, bool _new)
    {       
        if (_new)
            OnRevive?.Invoke();
        else
            OnDeath?.Invoke();
    }

   
    public void Heal(int _amount) // shared synced
    {
        if (_amount <= 0)
            return;

        CurrentHealth += _amount;

        if (CurrentHealth <= MaxHealth)        
            return;
        
        CurrentHealth = MaxHealth;
    }

    [Command]
    public void CmdHeal(int _amount)
    {
        Heal(_amount);
    }

    public void Damage(int _amount)
    {
        if (_amount <= 0)
            return;

        CurrentHealth -= _amount;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            IsAlive = false;            
        }
    }

    [Command]
    public void CmdDamage(int _amount)
    {
        Damage(_amount);        

    }

    public void Reset()
    {
        CurrentHealth = MaxHealth;
        IsAlive = true;        
    }

    [Command]
    public void CmdReset()
    {
        Reset();        
    }

    public void OnButtonClick_Debug()
    {       
        CmdDamage(10);
    }

    #region Start & Stop Callbacks
    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() 
    {
        CurrentHealth = _currentHealth;
        MaxHealth = _maxHealth;
    }   
    #endregion

    
}
