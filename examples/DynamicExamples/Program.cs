using System.Dynamic;

dynamic d = new ExpandoObject();
d.a = new ExpandoObject();
d.a.b = new ExpandoObject();
d.a.b.c = "hello!";
d.foo = "bar";

Console.WriteLine(d.a.b.c);