本套件用途主要是在 Unit Test 與 End To End 測試, 可以快速地產生測試資料.
是根據 JAVA 的 JFactory 做的 C# 版，還在開發途中 ( JFactory在文底參考資料)

這個套件目前還在開發中...

### 建立物件

建立物件所有簡單型別都會給值, 數字則是同類型的物件逐筆累加.

```C#
// #1 Create Object

// Create A User Data on Default

var user = CsFactory.CsFactory.Create<User>();
// user : Name: Name#1, Status: Enable, Age: 1
Console.WriteLine($"{nameof(user)} :: {user}");

// if we create second user than ... 
var user2 = CsFactory.CsFactory.Create<User>();
// user 2 : Name: Name#2, Status: Enable, Age: 2
Console.WriteLine($"{nameof(user2)} :: {user2}");

// And now we given name is roy in third user
var user3 = CsFactory.CsFactory.Create<User>(p => p.Name = "Roy");
// user3 :: Name: Roy, Status: Enable, Age: 3
Console.WriteLine($"{nameof(user3)} :: {user3}");

```

---

### 取用已建立物件

也可以直接取用前面建好的物件

```C#
// And then you can query user when the user exist.
var queryUser = CsFactory.CsFactory.Query<User>(p => p.Name == "Roy");
// queryUser :: Name: Roy, Status: Enable, Age: 3
Console.WriteLine($"{nameof(queryUser)} :: {queryUser}");

// if not exist ... 
var queryUser2 = CsFactory.CsFactory.Query<User>(p => p.Name == "Joey");
// you will get new User 
// queryUser2 :: Name: , Status: Enable, Age: 0
Console.WriteLine($"{nameof(queryUser2)} :: {queryUser2}");
```
---

### 物件關聯

當物件中有關聯的時候, 可以用下列方式建立.

```C#
// #2 Master Detail Object
var order1 = CsFactory.CsFactory.Create<Order>(p => p.Amount = 10000);
// order1 :: OrderId: OrderId#1, Amount: 10000, CreateTime: 2024/3/13 上午 12:01:00,
// user: Name: Name#1, Status: Enable, Age: 1
Console.WriteLine($"{nameof(order1)} :: {order1}");

// if you will setting user Name you can.. 
var order2 = CsFactory.CsFactory.Create<Order>(p =>
{
    p.user = CsFactory.CsFactory.Create<User>(p => p.Name = "Tim");
    p.OrderId = "PA123456789";
});
// order2 :: OrderId: PA123456789, Amount: 2, CreateTime: 2024/3/13 上午 12:02:00,
// user: Name: Tim, Status: Enable, Age: 4
Console.WriteLine($"{nameof(order2)} :: {order2}");
```

參考：https://github.com/leeonky/jfactory
