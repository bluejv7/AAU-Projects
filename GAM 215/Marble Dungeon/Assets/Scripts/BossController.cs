using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Handles events and patterns for the boss
/// </summary>
public class BossController : MonoBehaviour {
    #region Enums

    /// <summary>
    /// The different states the boss can be in
    /// </summary>
    public enum State
    {
        NORMAL, ANGRY, CRITICAL
    }

    /// <summary>
    /// The different attacks the boss can do - using powers of two so I can use bitwise operations to combine attacks per state
    /// (Trying to circumvent limitations in the unity editor)
    /// </summary>
    public enum Attack
    {
        CIRCULAR = 1, ALTERNATING_CIRCULAR = 2, SPIRAL = 4, TARGET = 8, SPAWN_ENEMIES = 16
    }

    #endregion

    #region Reference Variables

    /// <summary>
    /// Reference to player game object
    /// </summary>
    private GameObject player = null;

    #endregion

    #region Boss Configs

    /// <summary>
    /// Types of attacks each state can do
    /// </summary>
    [SerializeField] private List<int> stateAttacks = new List<int>();

    /// <summary>
    /// Default attack to use if state isn't configured
    /// </summary>
    [SerializeField] private Attack defaultAttack = Attack.CIRCULAR;

    #endregion

    #region Helper Variables

    /// <summary>
    /// A mapping of states to a list of attacks to be used for randomizing attacks for certain states
    /// </summary>
    private Dictionary<State, List<Attack>> stateAttackMap = new Dictionary<State, List<Attack>>();

    #endregion

    #region Event Handlers

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        // If stateAttacks doesn't contain all the states, populate it with the default attack
        Array stateList = Enum.GetValues(typeof(State));
        for (int i = stateAttacks.Count; i < stateList.Length; i++)
        {
            stateAttacks.Add((int) defaultAttack);
        }

        // Initialize `stateAttackMap` with the attacks we want
        foreach (State state in stateList)
        {
            int attackMask = stateAttacks[(int) state];
            List<Attack> attackList = new List<Attack>();
            stateAttackMap.Add(state, attackList);
            foreach (Attack attack in Enum.GetValues(typeof(Attack)))
            {
                // AND the mask with the attack value to see if it is part of this state
                if ((attackMask & (int) attack) != 0)
                {
                    attackList.Add(attack);
                }
            }
        }
	}
	
	/// <summary>
    /// Decide what to do every frame
    /// </summary>
	private void Update()
    {
	    
	}

    #endregion
}
