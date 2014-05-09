using System.Collections;
using UnityEngine;

public interface IRoutineRunner
{
	void runRoutine(IEnumerator routineToRun);
}

public class RoutineRunner : IRoutineRunner
{
	private readonly RunnerMonobehaviour runner;

	public RoutineRunner()
	{
		runner = Object.FindObjectOfType<RunnerMonobehaviour>();
		if(runner == null)
		{
			var go = new GameObject("serviceRunner");
			go.hideFlags = HideFlags.HideAndDontSave;
			runner = go.AddComponent<RunnerMonobehaviour>();
		}
	}

	public void runRoutine(IEnumerator routineToRun)
	{
		runner.StartCoroutine(routineToRun);
	}
}