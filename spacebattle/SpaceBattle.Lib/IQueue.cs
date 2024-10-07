using SpaceBattle.Lib;

namespace Spacebattle.Lib;

public interface IQueue
{
    void Enqueue(ICommand cmd);
    ICommand Dequeue();
}
