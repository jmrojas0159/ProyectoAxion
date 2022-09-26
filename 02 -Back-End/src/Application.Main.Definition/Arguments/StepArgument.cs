using System.Collections.Generic;

namespace Application.Main.Definition.Arguments
{
    public class StepArgument : IStepArgument
    {
        public Execution Execution { get; set; }
        public string Username { get; set; }
        public IEnumerable<IStep> Steps { get; set; }
    }
}