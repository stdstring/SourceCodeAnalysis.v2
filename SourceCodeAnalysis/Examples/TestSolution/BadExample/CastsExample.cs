using SomeBaseLibrary;

namespace BadExample
{
    public class SomeDerivedClass : SomeBaseClass
    {
    }

    public class CastsExample
    {
        public void DoSomeCasts()
        {
            Int32 iSource = 13;
            String sSource = "IDDQD";
            SomeBaseClass someObjectSource = new SomeDerivedClass();
            Object objDest1 = (Object) iSource;
            Object objDest2 = (Object) sSource;
            Object objDest3 = (Object) someObjectSource;
            Int32 iDest1 = (Int32) objDest1;
            Int32 iDest2 = (Int32) iSource;
            String sDest1 = (String) objDest2;
            String sDest2 = (String) sSource;
            SomeDerivedClass someObjectDest1 = (SomeDerivedClass) someObjectSource;
            SomeBaseClass someObjectDest2 = (SomeBaseClass) objDest3;
            SomeBaseClass someObjectDest3 = (SomeBaseClass) someObjectSource;
            SomeBaseClass someObjectDest4 = (SomeBaseClass) someObjectDest1;
        }
    }
}
