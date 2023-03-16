using System.Collections.Generic;
using System.Windows;

namespace InterviewApp.DataModels
{
    public interface INode
    {
        double Length { get; }
        List<INode> GetNeighbours(INode from);
        Point Position { get; }
    }
}
