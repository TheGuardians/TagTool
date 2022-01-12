using System;

namespace TagTool.Commands.Common
{
    public enum CommandError
    {
        None,
        CustomMessage,
        CustomError,
        ArgCount,
        ArgInvalid,
        OperationFailed,
        TagInvalid,
        SyntaxInvalid,
        DirectoryNotFound,
        FileNotFound,
        FileIO,
        FileType,
        CacheUnsupported,
        YesNoSyntax,
    }

    class TagToolError
    {
        private static readonly object Mutex = new object();

        public TagToolError(CommandError cmdError, string customMessage = null)
        {
            lock (Mutex)
            {
                // if we're not at the start of the line, insert a new one to avoid ugliness with Console.Write()
                if (Console.LargestWindowWidth != 0 && Console.CursorLeft > 0)
                    Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Red;

                if (cmdError != CommandError.None)
                {
                    bool showHelpMessage = true;

                    if (cmdError != CommandError.CustomMessage && cmdError != CommandError.CustomError)
                    {
                        string outputLine = "\nERROR: ";

                        switch (cmdError)
                        {
                            case CommandError.ArgCount:
                                outputLine += "Incorrect amount of arguments supplied";
                                break;
                            case CommandError.ArgInvalid:
                                outputLine += "An invalid argument was specified";
                                break;
                            case CommandError.OperationFailed:
                                outputLine += "An internal operation failed to evaluate";
                                showHelpMessage = false;
                                break;
                            case CommandError.TagInvalid:
                                outputLine += "The specified tag does not exist in the current tag cache";
                                break;
                            case CommandError.SyntaxInvalid:
                                outputLine += "Invalid syntax used";
                                break;
                            case CommandError.DirectoryNotFound:
                                outputLine += "The specified directory could not be found!";
                                break;
                            case CommandError.FileNotFound:
                                outputLine += "The specified file could not be found!";
                                break;
                            case CommandError.FileIO:
                                outputLine += "A file IO operation could not be completed";
                                showHelpMessage = false;
                                break;
                            case CommandError.FileType:
                                outputLine += "The specified file is of the incorrect type";
                                break;
                            case CommandError.CacheUnsupported:
                                outputLine += "The specified blam cache is not supported";
                                showHelpMessage = false;
                                break;
                            case CommandError.YesNoSyntax:
                                outputLine += "A response option other than \"y\" or \"n\" was given";
                                showHelpMessage = false;
                                break;
                        }

                        Console.WriteLine(outputLine);
                    }

                    bool hasCustomMessage = customMessage != null && customMessage != "";

                    if (cmdError == CommandError.CustomError && hasCustomMessage)
                    {
                        Console.WriteLine("\nERROR: " + customMessage);
                        showHelpMessage = false;
                    }

                    else if (hasCustomMessage)
                        Console.WriteLine("> " + customMessage);

                    if (showHelpMessage)
                        Console.WriteLine($"\nEnter \"Help {CommandRunner.CurrentCommandName}\" for command syntax.");
                }

                Console.ResetColor();
            }
        }
    }
}
