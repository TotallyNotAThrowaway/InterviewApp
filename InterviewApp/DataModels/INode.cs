using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewApp.DataModels
{
    internal interface INode
    {
        double Length { get; }
        List<INode> GetNeighbours(INode from);
    }
}
