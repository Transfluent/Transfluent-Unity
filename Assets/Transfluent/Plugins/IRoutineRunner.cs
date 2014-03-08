using UnityEngine;
using System.Collections;

public interface IRoutineRunner
{
	void runRoutine(IEnumerator routineToRun);
}

public class RoutineRunner : IRoutineRunner
{
	private RunnerMonobehaviour runner;
	public RoutineRunner()
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