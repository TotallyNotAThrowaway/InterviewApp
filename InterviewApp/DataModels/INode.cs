using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewApp.DataModels
{
    public interface INode
    {
        double Length { get; }
        List<INode> GetNeighbours(INode from);
        Point Position { get; }
    }
}
