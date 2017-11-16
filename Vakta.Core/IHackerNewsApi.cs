using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Refit;

namespace Vakta.Core
{
    public interface IHackerNewsApi
    {
        [Get("/search?tags=front_page")]
        Task<SearchResult> GetFrontPageResults();
    }

    public class Title
    {
        public string Value { get; set; }
        public string MatchLevel { get; set; }
        public IList<object> MatchedWords { get; set; }
    }

    public class Url
    {
        public string Value { get; set; }
        public string MatchLevel { get; set; }
        public IList<object> MatchedWords { get; set; }
    }

    public class Author
    {
        public string Value { get; set; }
        public string MatchLevel { get; set; }
        public IList<object> MatchedWords { get; set; }
    }

    public class HighlightResult
    {
        public Title Title { get; set; }
        public Url Url { get; set; }
        public Author Author { get; set; }
    }

    public class Hit
    {   
        public string Title { get; set; }
        public string Url { get; set; }
        public string Author { get; set; }
        public int Points { get; set; }
        public object StoryText { get; set; }
        public object CommentText { get; set; }
        public int NumComments { get; set; }
        public object StoryId { get; set; }
        public object StoryTitle { get; set; }
        public object StoryUrl { get; set; }
        public object ParentId { get; set; }
        public string ObjectID { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("created_at_i")]
        public int CreatedAtI { get; set; }
        
        [JsonProperty("_highlightResult")]
        public HighlightResult HighlightResult { get; set; }

        [JsonProperty("_tags")]
        public IList<string> Tags { get; set; }
        
        public override string ToString()
        {
            return Title;
        }
    }

    public class SearchResult
    {
        public IList<Hit> Hits { get; set; }
        public int NbHits { get; set; }
        public int Page { get; set; }
        public int NbPages { get; set; }
        public int HitsPerPage { get; set; }
        public int ProcessingTimeMS { get; set; }
        public bool ExhaustiveNbHits { get; set; }
        public string Query { get; set; }
        public string Params { get; set; }
    }

}