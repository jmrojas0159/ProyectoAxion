using System.Collections.Generic;

namespace Application.Main.Definition.Arguments
{
    public interface IStepArgument
    {
        Execution Execution { get; set; }
        string Username { get; set; }
        IEnumerable<IStep> Steps { get; set; }
    }
}