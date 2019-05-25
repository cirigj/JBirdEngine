using UnityEngine;
using System.Collections;
using JBirdLib;

/// <summary>
/// I wanted a custom editor so this is literally just a SO container for a custom class.
/// </summary>
[CreateAssetMenu]
public class JBirdNameGenerator : ScriptableObject {

	public Markov.NameGenerator nameGenerator;

}