using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/**
 * Stack-based Finite State Machine.
 * Push and pop states to the FSM.
 * 
 * States should push other states onto the stack 
 * and pop themselves off.
 */
using System;


public class FSM
{

    private Stack<FSMState> stateStack = new Stack<FSMState>(); //Stack is collection of objects, using LIFO

    public delegate void FSMState(FSM fsm, GameObject gameObject);


    public void Update(GameObject gameObject)
    {
        if (stateStack.Peek() != null)  //peek looks at top of stack without removing
            stateStack.Peek().Invoke(this, gameObject);
    }

    public void pushState(FSMState state) //Pushes new state to top of the Stack
    {
        stateStack.Push(state);
    }

    public void popState()  //removes state from top of stack
    {
        stateStack.Pop();
    }
}
