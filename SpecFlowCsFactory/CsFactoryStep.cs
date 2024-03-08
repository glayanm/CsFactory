using Newtonsoft.Json;
using NUnit.Framework;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecFlowCsFactory;

[Binding]
public class CsFactoryStep
{
    private readonly SpecFlowOutputHelper _outputHelper;
    private User _user;

    public CsFactoryStep(SpecFlowOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }


    [When(@"Query user name is ""(.*)""")]
    public void WhenQueryUserNameIs(string name)
    {
        _user = CsFactory.CsFactory.Query<User>(p => p.Name == name);
    }

    [Then(@"Name is ""(.*)"" number is ""(.*)""")]
    public void ThenNameIsNumberIs(string name, int number)
    {
        _outputHelper.WriteLine(JsonConvert.SerializeObject(_user));
        Assert.AreEqual(_user.Name, name);
    }
}

public class User
{
    public int Number { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public decimal Balance { get; set; }
}