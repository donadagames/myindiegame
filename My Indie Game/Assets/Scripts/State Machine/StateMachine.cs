using System;
using System.Collections.Generic;

public class StateMachine
{
    public IState currentState;
    private Dictionary<Type, List<Transition>> transitions =
        new Dictionary<Type, List<Transition>>();
    private List<Transition> currentTransitions =
        new List<Transition>();
    private List<Transition> anyTransitions =
       new List<Transition>();
    private static List<Transition> EmptyTransitions =
        new List<Transition>(0);

    public bool shouldChange = false;

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);
        currentState?.Tick();
    }

    public class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            Condition = condition;
            To = to;
        }
    }
    public void SetState(IState state)
    {
        if (state == currentState) return;
        currentState?.OnExit();
        currentState = state;

        transitions.TryGetValue(currentState.GetType(), out
            currentTransitions);
        if (currentTransitions == null)
            currentTransitions = EmptyTransitions;
        currentState.OnEnter();
    }

    public void AddTransition(IState from, IState to,
        Func<bool> predicate)
    {
        if (transitions.TryGetValue(from.GetType(),
            out var _transitions) == false)
        {
            _transitions = new List<Transition>();
            transitions[from.GetType()] = _transitions;
        }
        _transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state,
        Func<bool> predicate)
    {
        anyTransitions.Add(new Transition(state, predicate));
    }

    private Transition GetTransition()
    {
        foreach (var transition in anyTransitions)
        {
            if (transition.Condition())
                return transition;
        }

        foreach (var transition in currentTransitions)
        {
            if (transition.Condition())
                return transition;
        }
        return null;
    }
}
