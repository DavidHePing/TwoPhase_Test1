using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace TwoPhase_Test1;

public class TwoPhaseCommitCohort : IDisposable
{
    private readonly IDbConnection _dbConnection;
    private readonly string _sql;
    private readonly object _param;
    private IDbTransaction _transaction;

    public TwoPhaseCommitCohort(string sql, string connString)
        : this(sql, null, connString)
    {
    }

    public TwoPhaseCommitCohort(string sql, object param, string connString)
    {
        this._dbConnection = new SqlConnection(connString);
        this._sql = sql;
        this._param = param;
    }

    public Task<bool> ExecuteAsync()
    {
        return Task.Run(
            () =>
            {
                try
                {
                    this._dbConnection.Open();
                    this._transaction = this._dbConnection.BeginTransaction();

                    // need 'Dapper' library
                    this._dbConnection.Execute(this._sql, this._param, this._transaction);

                    return true;
                }
                catch
                {
                    // TODO: log
                }

                return false;
            });
    }

    public Task CommitAsync()
    {
        return Task.Run(
            () =>
            {
                this._transaction?.Commit();

                this.Dispose();
            });
    }

    public Task RollbackAsync()
    {
        return Task.Run(
            () =>
            {
                this._transaction?.Rollback();

                this.Dispose();
            });
    }

    public void Dispose()
    {
        this._dbConnection?.Dispose();
        this._transaction?.Dispose();
    }
}