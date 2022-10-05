// See https://aka.ms/new-console-template for more information

using TwoPhase_Test1;

var connString = "Data Source=192.168.66.12,1433;Initial Catalog=testDB;User Id=SA;Password=AaBb@1234;";

using (var coordinator = new TwoPhaseCommitCoordinator())
{
    coordinator.AddCohort(
        new TwoPhaseCommitCohort(
            "UPDATE Test1 SET FirstName = @FirstName WHERE Id = @Id;",
            new { Id = 1, FirstName = "Tony" },
            connString));

    coordinator.AddCohort(
        new TwoPhaseCommitCohort(
            "UPDATE Test3 SET LastName = @LastName WHERE Id = @Id;",
            new { Id = 1, LastName = "Chen" },
            connString));

    coordinator.Finish(coordinator.Vote());
}

Console.WriteLine("Update Finish!");