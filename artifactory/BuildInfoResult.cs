using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace artifactory 
{
    public partial class BuildInfoResult
    {
        [JsonProperty("results")] public List<Result> Results { get; set; }

        [JsonProperty("range")] public Range Range { get; set; }
    }

    public partial class Range
    {
        [JsonProperty("start_pos")] public long StartPos { get; set; }

        [JsonProperty("end_pos")] public long EndPos { get; set; }

        [JsonProperty("total")] public long Total { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("build.created")] public DateTimeOffset BuildCreated { get; set; }

        [JsonProperty("build.created_by")] public string BuildCreatedBy { get; set; }

        [JsonProperty("build.name")] public string BuildName { get; set; }

        [JsonProperty("build.number")] public string BuildNumber { get; set; }

        [JsonProperty("build.properties")] public List<BuildProperty> BuildProperties { get; set; }

        [JsonProperty("build.started")] public DateTimeOffset BuildStarted { get; set; }

        [JsonProperty("build.url")] public Uri BuildUrl { get; set; }
    }

    public partial class BuildProperty
    {
        [JsonProperty("build.property.key")] public string BuildPropertyKey { get; set; }

        [JsonProperty("build.property.value")] public string BuildPropertyValue { get; set; }
    }
}