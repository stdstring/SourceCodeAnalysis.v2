namespace BadExample
{
    public class IdentifiersExample
    {
        public class SоmeSimpleClassA
        {
        }

        public class SomeSimpleClassB
        {
        }

        public class SomeClass<TPаrаm1, TParam2>
        {
        }

        public interface ISomеInterface<TParam>
        {
        }

        public class SomeInterfaceImpl<TParam> : ISomеInterface<TParam>
        {
        }

        public void DeclareSomeIdentifiers()
        {
            SоmeSimpleClassA someObjA = new SоmeSimpleClassA();
            SomeSimpleClassB somеObjB = new SomeSimpleClassB();
            SomeClass<SоmeSimpleClassA, Int32> someObj = new SomeClass<SоmeSimpleClassA, Int32>();
            SomeInterfaceImpl<SomeSimpleClassB> someImplОbj = new SomeInterfaceImpl<SomeSimpleClassB>();
            Action<String, SоmeSimpleClassA> локальноеДействие = (парам1, парам2) => { };
        }
    }
}
