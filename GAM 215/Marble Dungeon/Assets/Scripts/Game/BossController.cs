using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles events and patterns for the boss
/// </summary>
public class BossController : MonoBehaviour {
    #region Enums and Constants

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

    /// <summary>
    /// The number of degrees in a circle
    /// </summary>
    private const int DEGREES_IN_CIRCLE = 360;

    #endregion

    #region Reference Variables

    /// <summary>
    /// Reference to player game object
    /// </summary>
    private GameObject player = null;

    /// <summary>
    /// Reference to the game controller
    /// </summary>
    private GameController gameController = null;

    /// <summary>
    /// Reference to our renderer component
    /// </summary>
    private Renderer bossRenderer = null;

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

    /// <summary>
    /// The max health of the boss
    /// </summary>
    [SerializeField] private int maxHealth = 800;

    /// <summary>
    /// Our damage
    /// </summary>
    [SerializeField] private int damage = 1;

    /// <summary>
    /// The delay between attacks
    /// </summary>
    [SerializeField] private float attackDelay = 1.0f;

    /// <summary>
    /// Sound to play when we get damaged
    /// </summary>
    [SerializeField] private AudioClip damageSound = null;

    /// <summary>
    /// The angle between the direction of the circular shots
    /// </summary>
    [SerializeField] private float circularAngle = 10.0f;

    /// <summary>
    /// How many seconds to wait before doing the second wave attack in `ALTERNATING_CIRCULAR`
    /// </summary>
    [SerializeField] private float alternatingDelay = 1.0f;

    #endregion

    #region Helper Variables

    /// <summary>
    /// A mapping of states to a list of attacks to be used for randomizing attacks for certain states
    /// </summary>
    private Dictionary<State, List<Attack>> stateAttackMap = new Dictionary<State, List<Attack>>();

    /// <summary>
    /// The current state of the boss
    /// </summary>
    private State currentState = State.NORMAL;

    /// <summary>
    /// A reference to the attack list tied to the current state
    /// </summary>
    private List<Attack> currentAttackList;

    /// <summary>
    /// The current attack pattern of the boss
    /// </summary>
    private Attack currentAttack = Attack.CIRCULAR;

    /// <summary>
    /// The current health of the boss
    /// </summary>
    private int currentHealth = 0;

    /// <summary>
    /// Original color of the boss
    /// </summary>
    private Color originalColor;

    /// <summary>
    /// Current color of the boss
    /// </summary>
    private Color currentColor;

    /// <summary>
    /// Position to look at
    /// </summary>
    private Vector3 lookAtPosition = Vector3.zero;

    /// <summary>
    /// Position to spawn bullets
    /// </summary>
    private Vector3 spawnPosition = Vector3.zero;

    /// <summary>
    /// The time of our last attack
    /// </summary>
    private float lastAttack = 0;

    /// <summary>
    /// Number of times we want to apply a rotation
    /// </summary>
    private int numRotationSteps = 0;

    #endregion

    #region Event Handlers

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        bossRenderer = this.GetComponent<Renderer>();
        originalColor = bossRenderer.material.color;
        currentColor = originalColor;

        // If stateAttacks doesn't contain all the states, populate it with the default attack
        System.Array stateList = System.Enum.GetValues(typeof(State));
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
            foreach (Attack attack in System.Enum.GetValues(typeof(Attack)))
            {
                // AND the mask with the attack value to see if it is part of this state
                if ((attackMask & (int) attack) != 0)
                {
                    attackList.Add(attack);
                }
            }
        }

        currentHealth = maxHealth;
        spawnPosition = this.transform.position;
        spawnPosition.y = player.transform.position.y;
        numRotationSteps = (int)(DEGREES_IN_CIRCLE / circularAngle);
    }
	
	/// <summary>
    /// Decide what to do every frame
    /// </summary>
	private void Update()
    {
        // Set the state of the boss
        ApplyState();

        // Figure out what happens per state
        HandleState();
	}

    /// <summary>
    /// Handle what happens when we get shot
    /// </summary>
    /// <param name="damage">The amount of damage we might take</param>
    private void OnShot(int damage)
    {
        // Try to hurt the enemy and change color accordingly
        ChangeHealth(-damage);
        ChangeColor();

        // Play damage sound
        AudioSource.PlayClipAtPoint(damageSound, this.transform.position);
    }

    /// <summary>
    /// Handle what happens when we die
    /// </summary>
    private void OnDestroy()
    {
        // Tell game controller we died
        gameController.SendMessage("OnBossDeath");
    }

    #endregion

    #region Helper Functions

    /// <summary>
    /// Manages Boss state
    /// </summary>
    private void ApplyState()
    {
        // TODO: Make variables to save calculation for state health values
        if (currentHealth <= maxHealth / 10 && false)
        {
            currentState = State.CRITICAL;
        }
        else if (currentHealth <= maxHealth / 2 && false)
        {
            currentState = State.ANGRY;
        }
        else
        {
            currentState = State.NORMAL;
        }
    }

    /// <summary>
    /// Manages what happens per state
    /// </summary>
    private void HandleState()
    {
        // Get the attack list and select a random attack
        currentAttackList = stateAttackMap[currentState];
        currentAttack = currentAttackList[Random.Range(0, currentAttackList.Count)];
        float now = Time.time;
        if (now >= lastAttack + attackDelay)
        {
            lastAttack = now;
            PerformAttack();
        }
    }

    /// <summary>
    /// Try to perform the current attack
    /// </summary>
    private void PerformAttack()
    {
        // Always look at player before attacking and make sure we always look at the same y position, so we don't rotate weirdly
        lookAtPosition = player.transform.position;
        lookAtPosition.y = this.transform.position.y;
        this.transform.LookAt(lookAtPosition);

        if (currentAttack == Attack.CIRCULAR)
        {
            CircularAttack(this.transform.rotation.eulerAngles, numRotationSteps, circularAngle);
        }
        else if (currentAttack == Attack.ALTERNATING_CIRCULAR)
        {
            StartCoroutine(AlternatingCircularAttack(this.transform.rotation.eulerAngles, numRotationSteps / 2, circularAngle));
        }
    }

    /// <summary>
    /// An attack that spawns bullets in a circle around us
    /// </summary>
    private void CircularAttack(Vector3 initialRotation, int numRotationSteps, float circularAngle, int startIndex = 0, int increment = 1)
    {
        Vector3 currentRotation = initialRotation;

        // Start spawning attacks in a circle
        for (int i = startIndex; i < numRotationSteps; i++)
        {
            currentRotation.y = initialRotation.y + circularAngle * i;
            gameController.SpawnProjectile(spawnPosition, Quaternion.Euler(currentRotation), this.tag, player.tag, damage, false);
        }
    }

    /// <summary>
    /// Similar to circular attack, but does two waves of attacks
    /// </summary>
    private IEnumerator AlternatingCircularAttack(Vector3 initialRotation, int numRotationSteps, float circularAngle)
    {
        float newCircularAngle = circularAngle * 2;

        // Extend lastAttack to be after the second wave starts
        lastAttack += alternatingDelay;

        // Start first wave of attacks
        CircularAttack(initialRotation, numRotationSteps, newCircularAngle, 0, 2);

        yield return new WaitForSeconds(alternatingDelay);

        // Start second wave of attacks
        CircularAttack(initialRotation, numRotationSteps + 1, newCircularAngle, 1, 2);
    }

    /// <summary>
    /// Attempts to apply a health change to the boss
    /// </summary>
    /// <param name="value">The amount to add to our health</param>
    private void ChangeHealth(int value)
    {
        currentHealth += value;

        // Destroy boss if current health is <= 0
        if (currentHealth <= 0)
        {
            this.transform.DetachChildren();
            GameObject.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Change the color of the boss
    /// </summary>
    private void ChangeColor()
    {
        // Gradually increase red color for boss (assuming boss color is black)
        currentColor.r = 1 - ((float)currentHealth / maxHealth);
        // TODO: reduce blue and green color if boss color happens to not be black
        bossRenderer.material.color = currentColor;
    }

    #endregion
}
