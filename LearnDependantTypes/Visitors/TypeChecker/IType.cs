using System.Collections.Generic;
using LearnDependantTypes.Lexing;

namespace LearnDependantTypes.Visitors.TypeChecker
{
    public interface IType
    {
        Location Location { get; }
    }
    
    public struct TUnit : IType
    {
        public Location Location { get; }

        public TUnit(Location location)
        {
            Location = location;
        }
    }

    public struct TBool : IType
    {
        public Location Location { get; }

        public TBool(Location location)
        {
            Location = location;
        }
    }
    public struct TInt : IType
    {
        public Location Location { get; }

        public TInt(Location location)
        {
            Location = location;
        }
    }

    public struct TFunction : IType
    {
        public List<IType> Args;
        public IType Return;
        public Location Location { get; }

        public TFunction(List<IType> args, IType @return, Location location)
        {
            Args = args;
            Return = @return;
            Location = location;
        }
    }
}