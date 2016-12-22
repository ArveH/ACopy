namespace ADatabase
{
    public interface ICommands
    {
        int ExecuteNonQuery(string sql);
        object ExecuteScalar(string sql);
    }
}