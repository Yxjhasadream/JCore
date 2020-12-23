using System;

namespace GitTool
{
    public class GitTolException : Exception
    {
        public GitTolException()
        {

        }

        public GitTolException(string message) : base(message)
        {

        }

        public GitTolException(string message, Exception innException) : base(message, innException)
        {

        }
    }
}
