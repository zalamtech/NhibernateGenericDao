using System;
using Castle.Windsor;
using Com.Googlecode.Genericdao.Console.Entities;
using Com.Googlecode.Genericdao.Console.Repositories;
using Com.Googlecode.Genericdao.Search;

namespace Com.Googlecode.Genericdao.Console
{
    public class Program
    {
        private static IWindsorContainer _container;

        static void Main()
        {
            //_container = new WindsorContainer();
            //_container.Install(FromAssembly.This());

            //TestSearchPerson();
            //TestBasicTypes();
            TestParams();
            TestParams(1);
            TestParams(1,2,3,4);
            System.Console.ReadKey();
        }

        private static void TestParams(params int[] ints)
        {
            if (ints == null)
            {
                System.Console.WriteLine("Params is null");
                return;
            }

            if (ints.Length == 0)
            {
                System.Console.WriteLine("Params are empty");
                return;
            }

            foreach (var i in ints)
            {
                System.Console.WriteLine("Param : " + i);
            }
        }

        private static void TestBasicTypes()
        {
            const double d = 12.2;
            const float f = 13.1f;
            const long l = 123L;
            const int i = 123;
            const string s = "abc";

            System.Console.WriteLine(d.GetType().IsClass ? "double is Class" : "double is not Class");

            System.Console.WriteLine(f.GetType().IsClass ? "float is Class" : "float is not Class");

            System.Console.WriteLine(l.GetType().IsClass ? "long is Class" : "long is not Class");

            System.Console.WriteLine(i.GetType().IsClass ? "int is Class" : "int is not Class");

            System.Console.WriteLine(s.GetType().IsClass ? "string is Class" : "string is not Class");

            System.Console.WriteLine("double code = " + Type.GetTypeCode(typeof(double)) + ", Double code = " + TypeCode.Double);
        }

        private static void TestSearchPerson()
        {
            var personDao = _container.Resolve<IPersonDao>();

            var search = new Search.Search();
            search.SetPage(0);
            search.SetMaxResults(10);
            //var nameFilter = Filter.Equal("City.Name", "Lille");

            search.SetSorts(new[] {new Sort(false, "City.Name", true)});
            var notesFilter = Filter.Equal("Notes", "sdfsdfqsef");
            //var filter = Filter.Or(nameFilter, notesFilter);

            //search.SetFilters(new[] { nameFilter });

            var personList = personDao.Search<Person, int>(search);

            foreach (var person in personList)
            {
                System.Console.WriteLine("Person " + person.Name + ", " + person.City.Name);
            }
        }
    }
}
