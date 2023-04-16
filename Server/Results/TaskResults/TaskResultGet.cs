﻿using Server.Database.Models;
using Server.Results.GroupResults;
using Server.Results.ComputerResults;
using Newtonsoft.Json;
using static Mysqlx.Expect.Open.Types;
using System.Text.Json.Serialization;

namespace Server.Results.TaskResults
{
    public class TaskResultGet
    {
        public int GroupId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GroupResultGet? Group { get; set; }
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TaskResultComputerGet? Computer { get; set; }
        public TaskResultGet(Tasks task, MyContext context)
        {
            this.GroupId = task.IdGroup;
            Group group = context.Groups!.Find(task.IdGroup)!;
            if (group!.Name.StartsWith("pc_"))
            {
                Computer = new TaskResultComputerGet(group, context);
                return;
            }
            Group = new GroupResultGet(group, context);
        }


    }
}
