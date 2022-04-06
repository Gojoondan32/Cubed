using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    #region Singleton
    public static GameState instance;

    private void Awake()
    {
        instance = this;
        currentState = State.Menu;
    }
    #endregion
    public enum State { Menu, Game, Help, Quit}

    private State currentState;

    public State CurrentState { get { return currentState; } set { currentState = value; } }



}
