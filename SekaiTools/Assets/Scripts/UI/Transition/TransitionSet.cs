using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Transition
{
    [CreateAssetMenu(menuName = "SekaiTools/Transition/TransitionSet")]
    public class TransitionSet : ScriptableObject
    {
        public List<Transition> transitions = new List<Transition>();

        public Transition GetValue(string name)
        {
            foreach (var transition in transitions)
            {
                if (name.Equals(transition.name))
                    return transition;
            }
            return null;
        }
    }
}