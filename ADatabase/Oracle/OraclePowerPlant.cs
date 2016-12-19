namespace ADatabase.Oracle
{
    public class OraclePowerPlant: PowerPlant
    {
        public override IDbSchema CreateDbSchema()
        {
            return new OracleSchema(DbContext);
        }

        public override ICommands CreateCommands()
        {
            return new OracleCommands(DbContext);
        }

        public override IColumnFactory CreateColumnFactory()
        {
            return new OracleColumnFactory();
        }

        public override IFastCopy CreateFastCopy()
        {
            return new OracleFastCopy(DbContext);
        }

        public override IDataCursor CreateDataCursor()
        {
            return new OracleDataCursor(DbContext);
        }
    }
}
