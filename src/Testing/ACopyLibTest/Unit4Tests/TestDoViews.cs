using System.Collections.Generic;
using ACopyLib.U4Views;
using ACopyTestHelper;
using ADatabase;
using FluentAssertions;

namespace ACopyLibTest.Unit4Tests
{
    public class TestDoViews: TestBase
    {
        private const string Asysview = "testasysview";
        private const string Aagview = "testaagview";
        private const string Testview = "vitestview";

        public virtual void Setup()
        {
            base.Setup("testview");
            DbSchema.DropView(Testview);
            DbSchema.DropTable(Asysview);
            DbSchema.DropTable(Aagview);
            TestTableCreator.CreateUnit4TestableWithAllTypes(DbContext, TestTable);
            CreateViewTables();
        }

        public override void Cleanup()
        {
            base.Cleanup();
            DbSchema.DropView(Testview);
            DbSchema.DropTable(Asysview);
            DbSchema.DropTable(Aagview);
        }

        private void CreateViewTables()
        {
            DbSchema.DropTable(Asysview);
            DbSchema.DropTable(Aagview);
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnTypeName.Int, "priority", 0, 15, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Varchar, "query", 4000, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Char, "status", 1, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Char, "table_name", 25, false, "' '", "Danish_Norwegian_CI_AS")
            };
            DbSchema.CreateTable(new TableDefinition(Aagview, columns, ""));
            columns.Add(columnFactory.CreateInstance(ColumnTypeName.Char, "db_name", 10, false, "' '", "Danish_Norwegian_CI_AS"));
            DbSchema.CreateTable(new TableDefinition(Asysview, columns, ""));
        }

        private void InsertIntoAsysview(string query)
        {
            string insertStmt = string.Format("insert into {0} (table_name, status, db_name, query) values ('{1}', 'N', ' ', '{2}')", Asysview, Testview, query);
            Commands.ExecuteNonQuery(insertStmt);
        }

        private void InsertIntoAsysview(string viewName, string testVal)
        {
            string insertStmt = string.Format("insert into {0} (table_name, status, query) values ('{1}', 'N', 'select ''{2}'' as col0 from {3}')", Asysview, viewName, testVal, TestTable);
            Commands.ExecuteNonQuery(insertStmt);
        }

        private void InsertIntoAagview()
        {
            string insertStmt = string.Format("insert into {0} (table_name, status, query) values ('{1}', 'N', 'select ''a'' as col0 from {2}')", Aagview, Testview, TestTable);
            Commands.ExecuteNonQuery(insertStmt);
        }

        private void InsertIntoAagview(string query)
        {
            string insertStmt = string.Format("insert into {0} (table_name, status, query) values ('{1}', 'N', '{2}')", Aagview, Testview, query);
            Commands.ExecuteNonQuery(insertStmt);
        }

        private void DoViews()
        {
            IU4Views views = U4ViewFactory.CreateInstance(DbContext);
            views.AagTableName = Aagview;
            views.AsysTableName = Asysview;
            int totalViews;
            int failedViews;
            views.DoViews(out totalViews, out failedViews);
        }

        // TestMethod
        protected void TestDoViews_When_SimpleViewInAsysview_Then_IsViewTrue()
        {
            InsertIntoAsysview(Testview, "s");
            DoViews();
            DbSchema.IsView(Testview).Should().BeTrue("because view was created");
        }

        // TestMethod
        protected void TestDoViews_When_ViewInAsysviewAndAagview_Then_AagviewIsUsed()
        {
            InsertIntoAagview();
            InsertIntoAsysview(Testview, "s");
            DoViews();
            string val = (string)Commands.ExecuteScalar(string.Format("select col0 from {0}", Testview));
            val.Should().Be("a", "because when the same view exists in asysview and aagview, then aagview is used");
        }

        // TestMethod
        protected void TestDoViews_When_ViewContainNativeFunction_Then_ViewCreated(string body)
        {
            InsertIntoAagview(body);
            DoViews();
            DbSchema.IsView(Testview).Should().BeTrue("because view with database specific function was created");
        }

        // TestMethod
        protected void TestDoViews_When_ViewContainAgrFunction_Then_ViewCreated()
        {
            InsertIntoAsysview(string.Format("select guid2str(guid_col) as new_col1 from {0}", TestTable));
            DoViews();
            DbSchema.IsView(Testview).Should().BeTrue("because view with database specific function was created");
        }

        // TestMethod
        protected void TestDoViews_When_ViewContainingQuotedName_Then_ViewCreated()
        {
            InsertIntoAsysview(string.Format("select int_col as \"mycol\" from {0}", TestTable));
            DoViews();
            DbSchema.IsView(Testview).Should().BeTrue("because view with quoted name was created");
        }

        // TestMethod
        protected void TestDoViews_When_ViewContainingEmptyString()
        {
            InsertIntoAsysview(string.Format("select int_col from {0} where varchar_col <> ''''", TestTable));
            DoViews();
            DbSchema.IsView(Testview).Should().BeTrue("because view with empty string was created");

            string sql = string.Format("select int_col from {0}", Testview);
            var res = Commands.ExecuteScalar(sql);
            res.Should().NotBeNull("because there are rows in the test table");
        }
    }
}
