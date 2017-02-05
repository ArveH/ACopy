using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using ACommandLineParser;
using ACopyLib.Reader;
using ACopyLib.U4Views;
using ACopyLib.Writer;
using ADatabase;
using ALogger;

namespace ACopyConsole
{
    internal class Program
    {
        private static string programName = "ACopyConsole";

        private static int Main(string[] args)
        {
            IArgumentCollection settings;
            try
            {
                settings = ArgumentCollectionFactory.CreateArgumentCollection();
                settings.AddCommandLineArguments(args);
            }
            catch (Exception ex)
            {
                IALogger logger = new ConsoleLogger();
                logger.Write(ex);
                return -1;
            }

            if (settings["ArgumentDescription"].IsSet)
            {
                PrintFullUsage(programName, settings);
                return 0;
            }

            if (!settings.VerifyArguments())
            {
                PrintUsage(args.Length==0? programName : args[0], settings);
                return -1;
            }

            List<string> tables = GetTables(args);

            var dbContext = CreateDbContext(settings);

            Exception cantConnectException;
            if (!CanGetConnected(dbContext, out cantConnectException))
            {
                return -1;
            }

            if (settings["Direction"].Value == "in")
            {
                CopyIn(dbContext, settings, tables);
            }
            else
            {
                CopyOut(dbContext, settings, tables);
            }

            return 0;
        }

        private static bool CanGetConnected(IDbContext dbContext, out Exception exception)
        {
            bool canGetConnected = true;
            exception = null;
            try
            {
                dbContext.PowerPlant.CreateDbSchema().IsTable("asysdummy");
            }
            catch (Exception ex)
            {
                canGetConnected = false;
                exception = ex;
                string errorMessage = "";
                Exception currEx = ex;
                while (currEx != null)
                {
                    errorMessage += currEx.Message + "\n";
                    currEx = currEx.InnerException;
                }
                new ConsoleLogger().Write(errorMessage);
            }

            return canGetConnected;
        }

        private static IDbContext CreateDbContext(IArgumentCollection settings)
        {
            var dbContext = settings["DBProvider"].Value.ToUpper().Substring(0, 1) == "O" ? DbContextFactory.CreateOracleContext() : DbContextFactory.CreateSqlServerContext();

            SetConnectionString(dbContext, settings);

            return dbContext;
        }

        private static void SetConnectionString(IDbContext dbContext, IArgumentCollection settings)
        {
            if (settings["ConnectionString"].IsSet)
            {
                dbContext.CreateConnectionString(ConfigurationManager.ConnectionStrings[settings["ConnectionString"].Value].ConnectionString);
            }
            else
            {
                dbContext.CreateConnectionString(settings["User"].Value, settings["Password"].Value, settings["Database"].Value, settings["Server"].Value);
            }
        }

        private static void CopyOut(IDbContext dbContext, IArgumentCollection settings, List<string> tables)
        {
            IAWriter writer = AWriterFactory.CreateInstance(dbContext, new ConsoleLogger());
            writer.Directory = settings["Folder"].Value;
            if (settings["MaxDegreeOfParallelism"].IsSet) writer.MaxDegreeOfParallelism = int.Parse(settings["MaxDegreeOfParallelism"].Value);
            if (settings["DataFileSuffix"].IsSet) writer.DataFileSuffix = settings["DataFileSuffix"].Value;
            if (settings["SchemaFileSuffix"].IsSet) writer.SchemaFileSuffix = settings["SchemaFileSuffix"].Value;
            if (settings["ConversionsFile"].IsSet) writer.ConversionsFile = settings["ConversionsFile"].Value;
            writer.UseCompression = settings["UseCompression"].IsSet;
            writer.UseU4Indexes = settings["UseU4Indexes"].IsSet;
            writer.Write(tables);
        }

        private static void CopyIn(IDbContext dbContext, IArgumentCollection settings, List<string> tables)
        {
            IALogger logger = new ConsoleLogger();
            IAReader reader = AReaderFactory.CreateInstance(dbContext, logger);
            reader.Directory = settings["Folder"].Value;
            if (settings["MaxDegreeOfParallelism"].IsSet) reader.MaxDegreeOfParallelism = Int32.Parse(settings["MaxDegreeOfParallelism"].Value);
            if (settings["BatchSize"].IsSet) reader.BatchSize = int.Parse(settings["BatchSize"].Value);
            if (settings["UseCollation"].IsSet) reader.Collation = settings["UseCollation"].Value;
            if (settings["DataFileSuffix"].IsSet) reader.DataFileSuffix = settings["DataFileSuffix"].Value;
            if (settings["SchemaFileSuffix"].IsSet) reader.SchemaFileSuffix= settings["SchemaFileSuffix"].Value;
            if (settings["ConversionsFile"].IsSet) reader.ConversionsFile = settings["ConversionsFile"].Value;
            reader.CreateClusteredIndex = settings["CreateClusteredIndex"].IsSet;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int totalTables;
            int failedTables;
            reader.Read(tables, out totalTables, out failedTables);

            int totalViews = 0;
            int failedViews = 0;
            if (settings["View"].IsSet)
            {
                CreateViews(dbContext, out totalViews, out failedViews, logger);
            }

            stopWatch.Stop();

            logger.Write("");
            logger.Write(string.Format("{0} tables created.", totalTables));
            if (failedTables > 0)
            {
                logger.Write(string.Format("Found {0} tables with errors", failedTables));
            }
            logger.Write(string.Format("{0} views created.", totalViews));
            if (failedViews > 0)
            {
                logger.Write(string.Format("Found {0} views with errors", failedViews));
            }
            logger.Write(string.Format("Total time: {0}", stopWatch.Elapsed));
        }

        private static void CreateViews(IDbContext dbContext, out int totalViews, out int failedViews, IALogger logger)
        {
            totalViews = 0;
            failedViews = 0;
            try
            {
                IU4Views views = U4ViewFactory.CreateInstance(dbContext);
                if (!views.HasViewsSource)
                {
                    logger.Write(string.Format("ERROR: Can't create views without tables {0} and {1}", views.AagTableName, views.AsysTableName));
                    return;
                }
                views.DoViews(out totalViews, out failedViews, logger);
            }
            catch (Exception ex)
            {
                logger.Write(ex);
            }
        }

        private static List<string> GetTables(string[] args)
        {
            return args.Where(arg => arg[0] != '-').ToList();
        }

        private static void PrintUsage(string program, IArgumentCollection settings)
        {
            IArgumentVisitor argVisitor = ArgumentVisitorFactory.CreateArgumentVisitor();
            settings.Accept(argVisitor);
            
            Console.WriteLine(argVisitor.GetUsage(program));
        }

        private static void PrintFullUsage(string program, IArgumentCollection settings)
        {
            IArgumentVisitor argVisitor = ArgumentVisitorFactory.CreateArgumentVisitor(true);
            settings.Accept(argVisitor);

            Console.WriteLine(argVisitor.GetUsage(program));
        }

    }
}
