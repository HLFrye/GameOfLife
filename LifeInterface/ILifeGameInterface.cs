using System;

namespace LifeInterface
{
    public interface ILifeGameInterface
    {
        bool Get(int x, int y);
        void Add(int x, int y);
        void Remove(int x, int y);
        bool Update();
        int CellCount { get; }
    }
}
