using System.Collections.Generic;

namespace CmdR.Validation
{
    public class ValidationResultModel
    {
        public ValidationResultModel()
        {
            Title = "Validation Of Command Failed";
            Errors = new Dictionary<string, List<string>>();
        }
        public string Title { get; }
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}