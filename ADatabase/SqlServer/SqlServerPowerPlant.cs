namespace ADatabase.SqlServer
{
    public class SqlServerPowerPlant : PowerPlant
    {
        public override IDbSchema CreateDbSchema()
        {
            return new SqlServerSchema(DbContext);
        }

        public override ICommands CreateCommands()
        {
            return new SqlServerCommands(DbContext);
        }

        public override IColumnFactory CreateColumnFactory()
        {
            return new SqlServerColumnFactory();
        }

        public override IFastCopy CreateFastCopy()
        {
            return new SqlServerFastCopy(DbContext);
        }

        public override IDataCursor CreateDataCursor()
        {
            return new SqlServerDataCursor(DbContext);
        }
    }
}