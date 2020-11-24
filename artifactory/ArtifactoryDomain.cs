using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace artifactory.Artifactory.Domain
{
    public class AQLQueryResults<T>
    {
        public List<T> results { get; set; }
        public Range range { get; set; }
    }

    public class ArtifactFileInfoResult
    {
        public string repo { get; set; }
        public string path { get; set; }
        public DateTime created { get; set; }
        public string createdby { get; set; }
        public DateTime lastModified { get; set; }
        public string modifiedby { get; set; }
        public Uri DownloadUri { get; set; }
        public string mimeType { get; set; }
        public string size { get; set; }
        public Uri Uri { get; set; }
    }


    public class ArtifactResult : ArtifactAQLQueryResult
    {
        public List<Artifact> Artifacts { get; set; }

        public partial class Artifact
        {
            [JsonProperty("modules")]
            public List<Module> Modules { get; set; }
        }

        public partial class Module
        {
            [JsonProperty("builds")]
            public List<Build> Builds { get; set; }
        }

        public partial class Build
        {
            [JsonProperty("build.properties")]
            public List<BuildProperty> BuildProperties { get; set; }
        }

        public partial class BuildProperty
        {
            [JsonProperty("build.property.key")]
            public string BuildPropertyKey { get; set; }

            [JsonProperty("build.property.value")]
            public string BuildPropertyValue { get; set; }
        }

    }
    public class ArtifactAQLQueryResult
    {
        public string repo { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public long size { get; set; }
        public DateTime created { get; set; }
        public string created_by { get; set; }
        public DateTime modified { get; set; }
        public string modified_by { get; set; }
        public DateTime updated { get; set; }
    }

    public class Range
    {
        public int start_pos { get; set; }
        public int end_pos { get; set; }
        public int total { get; set; }
    }

    public class ArtifactoryBuildInfo
    {
        public DateTimeOffset? BuildStarted { get; set; }
        public DateTimeOffset? BuildEnded { get; set; }
        public string BuildName { get; set; }
        public string ArtifactoryBuildNumber { get; set; }
        public Uri TeamCityBuildUrl { get; set; }

        public string Owner { get; set; }

        public string Repository { get; set; }

        public string Sha { get; set; }

        public string Version { get; set; }
        public string Branch { get; set; }
        public string BuildNumber { get; set; }
        public string BuildConfigurationName { get; set; }
        public string Status { get; set; }
    }
}
