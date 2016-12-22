using System.IO;

namespace ACommandLineParser.Arguments
{
    public class FolderArgument: ArgumentBase
    {
        public FolderArgument()
        {
            Name = "Folder";
            ShortName = "-f";
            base.Value = ".\\";
        }

        public override string Value
        {
            get { return base.Value; }
            set
            {
                base.Value = value;
                if (!Directory.Exists(base.Value))
                {
                    Directory.CreateDirectory(base.Value);
                }

            }
        }

        public override string Syntax => "-f<folder_name>";

        public override string Description => "The folder for the the data and schema files. If the folder doesn't exist, it will be created. If -f is not used, the files reside in the current folder.";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return true;
        }
    }
}
