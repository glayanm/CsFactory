namespace SpecFlowCsFactory.Hooks;

[Binding]
public class Hooks
{
    [BeforeFeature]
    public static void Before()
    {
        CsFactory.CsFactory.Create<User>(p => p.Name = "Yuan");
        CsFactory.CsFactory.Create<User>(p => p.Name = "Ame");
        CsFactory.CsFactory.Create<User>(p =>
        {
            p.Name = "Dika";
            p.Number = 10;
        });

        CsFactory.CsFactory.Create<Order>(p => p.Id = 123456);
    }
}