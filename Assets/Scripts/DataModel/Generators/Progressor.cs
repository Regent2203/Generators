using UnityEngine;
using System.Collections.Generic;

namespace DataModel.Generators
{
    public class Progressor : MonoBehaviour
    {
        private IList<(IProgressable, float)> _objects = new List<(IProgressable, float)>(); //object (generator) and its step (progress per frame)


        public void AddProgressable(IProgressable progressable, float delay)
        {
            var step = Time.fixedDeltaTime / delay;

            _objects.Add((progressable, step));            
        }
                
        private void FixedUpdate()
        {
            ProcessGenerators();            
        }

        private void ProcessGenerators()
        {
            foreach (var gen in _objects)
                if (gen.Item1.IsActive)
                    gen.Item1.AddProgress(gen.Item2);
        }
    }
}