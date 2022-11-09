using System.Collections.Generic;

namespace ArsuLeo.CS.Utils.Service.App
{
    public static class ApplicationArgumentsUtil
    {
        public static readonly string[] DefaultPrepends = new string[] { "--", "/" };

        public static bool ArgsHasArgPrepends(List<string> args, string arg, string[] prepends)
        {
            return GetArgIndexPrepends(args, arg, prepends) >= 0;
        }
        public static bool ArgsHasArgPrepends(List<string> args, string arg)
        {
            return ArgsHasArgPrepends(args, arg, DefaultPrepends);
        }
        public static bool ArgsHasArg(List<string> args, string arg)
        {
            return GetArgIndex(args, "--" + arg) >= 0 || GetArgIndex(args, "/" + arg) >= 0;
        }

        public static int GetArgIndexPrepends(List<string> args, string arg)
        {
            return GetArgIndexPrepends(args, arg, DefaultPrepends);
        }
        public static int GetArgIndexPrepends(List<string> args, string arg, string[] prepends)
        {

            for (int i = 0; i < prepends.Length; i++)
            {
                int index = GetArgIndex(args, prepends[i] + arg);
                if (index >= 0)
                {
                    return index;
                }
            }
            return -1;
        }

        public static int GetArgIndex(List<string> args, string arg)
        {
            return args.IndexOf(arg);
        }
    }
}
