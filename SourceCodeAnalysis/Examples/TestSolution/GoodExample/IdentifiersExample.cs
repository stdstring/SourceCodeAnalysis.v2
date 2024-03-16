namespace GoodExample
{
    public class IdentifiersExample
    {
        public class SomeSimpleClassA
        {
        }

        public class SomeSimpleClassB
        {
        }

        public class SomeClass<TParam1, TParam2>
        {
        }

        public interface ISomeInterface<TParam>
        {
        }

        public class SomeInterfaceImpl<TParam> : ISomeInterface<TParam>
        {
        }

        public void DeclareSomeIdentifiers()
        {
            SomeSimpleClassA someObjA = new SomeSimpleClassA();
            SomeSimpleClassB someObjB = new SomeSimpleClassB();
            SomeClass<SomeSimpleClassA, Int32> someObj = new SomeClass<SomeSimpleClassA, Int32>();
            SomeInterfaceImpl<SomeSimpleClassB> someImplObj = new SomeInterfaceImpl<SomeSimpleClassB>();
            Action<String, SomeSimpleClassA> someAction = (s, obj) => { };
        }
    }
}
