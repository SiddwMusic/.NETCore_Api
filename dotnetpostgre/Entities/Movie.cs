using System;
using System.Collections.Generic;
using System.Linq;
using Neo4j.Driver.V1;

namespace dotnetpostgre.Entities
{
    public class Movivy
    {
        public const string MovieKey = "movie";
        public const string TitleKey = "title";
        public const string TaglineKey = "tagline";
        public const string ReleasedKey = "released";
        public const string ActorsKey = "actors";

    //    public Movie(IRecord record)
    //    {
    //        var movie = record.GetOrDefault(MovieKey, (INode)null);
    //        if (movie != null)
    //        {
    //            Title = movie.GetOrDefault<string>(TitleKey, null);
    //            Tagline = movie.GetOrDefault<string>(TaglineKey, null);
    //            Released = movie.GetOrDefault<int?>(ReleasedKey, null);
    //        }

    //        var actors = record.GetOrDefault(ActorsKey, (List<object>)null);
    //        if (actors != null)
    //        {
    //            Actors = actors.Select(actor => new PersonModel((INode)actor)).OrderBy(p => p.Name);
    //        }
    //    }

    //    public string Title { get; }

    //    public string Tagline { get; }

    //    public int? Released { get; }

    //    public IEnumerable<PersonModel> Actors { get; }
    //}

    //public class PersonModel
    //{
    //    public const string NameKey = "name";

    //    public PersonModel(INode node)
    //    {
    //        if (node == null)
    //        {
    //            throw new ArgumentNullException(nameof(node));
    //        }

    //        Name = node.GetOrDefault<string>(NameKey, null);
    //    }

    //    public string Name { get; }
    }
    public class CastResult
    {
        public string name { get; set; }
        public string job { get; set; }
        public IEnumerable<string> role { get; set; }
    }

    public class MovieResult
    {
        public string title { get; set; }
        public IEnumerable<CastResult> cast { get; set; }
    }
    public class NodeResult
    {
        public string title { get; set; }
        public string label { get; set; }
    }

    public class Movie
    {
        public string title { get; set; }
        public int released { get; set; }
        public string tagline { get; set; }
    }

    public class Person
    {
        public string name { get; set; }
        public int born { get; set; }
    }

}
