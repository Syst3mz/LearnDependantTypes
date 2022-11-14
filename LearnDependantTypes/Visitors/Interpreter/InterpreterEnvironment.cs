using System;
using System.Collections.Generic;

namespace LearnDependantTypes.Visitors.Interpreter
{
    public class InterpreterEnvironment<T>
    {
        private InterpreterEnvironment<T> _contained;
        private Dictionary<string, T> _map = new Dictionary<string, T>();

        public InterpreterEnvironment()
        {
            _contained = null;
        }

        public InterpreterEnvironment(InterpreterEnvironment<T> contained)
        {
            _contained = contained;
        }

        public void Bind(string x, T to)
        {
            if (_map.ContainsKey(x))
            {
                throw new AlreadyBoundError($"{x} is already bound to {_map[x]}");
            }
            else
            {
                _map.Add(x, to);
            }
        }
        
        public void Set(string x, T to)
        {
            if (_map.ContainsKey(x))
            {
                _map[x] = to;
            }
            else
            {
                if (_contained != null)
                {
                    _contained.Set(x, to);
                }
                else
                {
                    throw new LookupError($"{x} is not found in current context");
                }
            }
        }
        
        public T Get(string x)
        {
            if (_map.ContainsKey(x))
            {
                return _map[x];
            }
            else
            {
                if (_contained != null)
                {
                    return _contained.Get(x);
                }
                else
                {
                    throw new LookupError($"{x} is not found in current context");
                }
            }
        }
    }

    public class AlreadyBoundError : Exception
    {
        public AlreadyBoundError(string message) : base(message)
        {
        }
    }

    public class LookupError : Exception
    {
        public LookupError(string message) : base(message)
        {
        }
    }
}