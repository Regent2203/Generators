using System;

namespace DataModel.Generators
{
    public interface IGeneratorsService
    {
        void AddGenerator();
        void RemoveGenerator();
        float GetGenerator();

        //event Action GeneratorsChanged;
    }
}