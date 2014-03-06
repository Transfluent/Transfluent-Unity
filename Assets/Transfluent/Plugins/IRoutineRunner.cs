using UnityEngine;
using System.Collections;

public interface IRoutineRunner
{
	void runRoutine(IEnumerator routineToRun);
}

public class RoutineRuner : IRoutineRunner
{
	private RunnerMonobehaviour runner;
	public RoutineRuner()
	{
		runner = GameObject.FindObjectOfType<RunnerMonobehaviour>();
		if(runner == null)
		{
			GameObject go = new GameObject("serviceRunner");
			runner = go.AddComponent<RunnerMonobehaviour>();
		}
	}
	public void runRoutine(IEnumerator routineToRun)
	{
		runner.StartCoroutine(routineToRun);
	}

	
}