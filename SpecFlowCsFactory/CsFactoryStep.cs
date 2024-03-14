using CsFactory;
using Newtonsoft.Json;
using NUnit.Framework;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecFlowCsFactory;

[Binding]
public class CsFactoryStep
{
    private readonly SpecFlowOutputHelper _outputHelper;
    private User _actual;
    private User _expected;
    private Order _order;
    private User _user;

    public CsFactoryStep(SpecFlowOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        _expected = new User();
        _actual = new User();
        CsFactory.CsFactory.Clear();
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

    [When(@"Query order id is ""(.*)""")]
    public void WhenQueryOrderIdIs(int id)
    {
        _order = CsFactory.CsFactory.Query<Order>(p => p.Id == id);
    }

    [Then(@"User name is ""(.*)""")]
    public void ThenUserNameIs(string userName)
    {
        _outputHelper.WriteLine(JsonConvert.SerializeObject(_order));
        Assert.AreEqual(_order.user.Name, userName);
    }

    [Given(@"Create user name is ""(.*)"" number is ""(.*)"" to be actual")]
    public void GivenCreateUserNameIsNumberIsToBeActual(string name, int number)
    {
        _actual = CsFactory.CsFactory.Create<User>(p =>
        {
            p.Name = name;
            p.Number = number;
        });
    }

    [Given(@"Fork user name is ""(.*)""  be fork")]
    public void GivenForkUserNameIsBeFork(string name)
    {
        _expected = CsFactory.CsFactory.Fork<User>(p => p.Name == name);
    }

    [When(@"actual number is ""(.*)""")]
    public void WhenActualNumberIs(int number)
    {
        _actual.Number = number;
    }

    [Then(@"expected user number is ""(.*)""")]
    public void ThenExpectedUserNumberIs(int expectedNumber)
    {
        _outputHelper.WriteLine(JsonConvert.SerializeObject(_expected));
        Assert.AreEqual(expectedNumber, _expected.Number);
    }

    [Given(@"Fork user name is ""(.*)""  be expected")]
    public void GivenForkUserNameIsBeExpected(string roy)
    {
    }

    [Given(@"Fork user name is ""(.*)""  be expected and set Password is ""(.*)""")]
    public void GivenForkUserNameIsBeExpectedAndSetPasswordIs(string name, string password)
    {
        _expected = CsFactory.CsFactory.Query<User>(p => p.Name == name)
            .ToForkExpected<User>(p => p.Password = password);
    }

    [Then(@"expected user password is ""(.*)""")]
    public void ThenExpectedUserPasswordIs(string password)
    {
        Assert.AreEqual(password, _expected.Password);
    }

    [Given(@"Create User name is ""(.*)""")]
    public void GivenCreateUserNameIs(string name)
    {
        CsFactory.CsFactory.Create<User>(p => p.Name = name);
    }

    [Given(@"Create Order id is ""(.*)""")]
    public void GivenCreateOrderIdIs(int orderId)
    {
        CsFactory.CsFactory.Create<Order>(p => p.Id = orderId);
    }
}

public class User
{
    public int Number { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public decimal Balance { get; set; }
}

public class Order
{
    public int Id { get; set; }

    public User user { get; set; }
}