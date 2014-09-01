#region copyright
// ------------------------------------------------------------------------
// <copyright file="Program.cs" company="Zalamtech SARL">
//	Copyright 2014.
// </copyright>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// <author>Abdoul DIALLO</author>
// <date>2014-9-1 18:17</date>
// ------------------------------------------------------------------------
#endregion
using Castle.Windsor;
using Castle.Windsor.Installer;
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
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());

            TestSearchPerson();
            
            System.Console.ReadKey();
        }

        private static void TestSearchPerson()
        {
            var personDao = _container.Resolve<IPersonDao>();

            var search = new Search.Search();
            search.SetPage(0);
            search.SetMaxResults(10);
            
            var nameFilter = Filter.Ilike("City.Name", "Par%");
            var notesFilter = Filter.Ilike("Notes", "");
            var filter = Filter.Or(nameFilter, notesFilter);

            search.SetFilters(new[] { filter });
            search.SetSorts(new[] { new Sort(false, "City.Name", true) });

            var personList = personDao.Search<Person, int>(search);

            foreach (var person in personList)
            {
                System.Console.WriteLine("Person " + person.Name + ", " + person.City.Name);
            }
        }
    }
}
