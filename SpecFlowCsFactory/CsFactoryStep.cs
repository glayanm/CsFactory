using NUnit.Framework;

namespace SpecFlowCsFactory;

[Binding]
public class CsFactoryStep
{
    private User _user;

    [When(@"Create default Model")]
    public void WhenCreateDefaultModel()
    {
        _user = new CsFactory.CsFactory().Create<User>();
    }

    [Then(@"Name is ""(.*)"" number is ""(.*)""")]
    public void ThenNameIsNumberIs(string name, int number)
    {
        Assert.AreEqual(_user.Name, name);
        Assert.AreEqual(_user.Number, number);
    }
}

public class User
{
    public int Number { get; set; }

    public string Name { get; set; }
}