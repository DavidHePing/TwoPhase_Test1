namespace TwoPhase_Test1;

public class TwoPhaseCommitCoordinator : IDisposable
{
    private readonly List<TwoPhaseCommitCohort> cohorts = new List<TwoPhaseCommitCohort>();

    public void AddCohort(TwoPhaseCommitCohort cohort)
    {
        this.cohorts.Add(cohort);
    }

    public bool Vote()
    {
        var tasks = this.cohorts.Select(x => x.ExecuteAsync()).ToList();

        Task.WaitAll(tasks.ToArray<Task>());

        return tasks.All(x => x.Result);
    }

    public void Finish(bool voteResult)
    {
        var tasks = voteResult
            ? this.cohorts.Select(x => x.CommitAsync()).ToArray()
            : this.cohorts.Select(x => x.RollbackAsync()).ToArray();

        Task.WaitAll(tasks);
    }

    public void Dispose()
    {
        this.cohorts.ForEach(x => x.Dispose());
    }
}