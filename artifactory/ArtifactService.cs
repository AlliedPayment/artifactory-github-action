using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using artifactory.Artifactory.Domain;
using Newtonsoft.Json;


namespace artifactory 
{
    public class ArtifactService
    {

        public string artifactory_token;
        public ArtifactService(string artifactory_token)
        {
            this.artifactory_token = artifactory_token;
        }

       
        public List<ArtifactoryBuildInfo> FindBuilds(string sha)
        {

            var results = new List<ArtifactoryBuildInfo>();
            Console.WriteLine(sha);

//            var aql = @"
//{
//  ""module.build.@buildInfo.env.build.vcs.number"": {
//    ""$eq"": ""{0}""
//  }
//}
//";

            var aql = @"
{
  ""$or"": [
 {""module.build.@buildInfo.env.build.vcs.number"": {""$eq"": ""{0}""  }},
 {""module.build.@buildInfo.env.GITHUB_SHA"": {""$eq"": ""{0}""  }}
]
}
";

            var include =
                @".include(""artifact.module.build.@buildInfo.env.build.vcs.number"",""artifact.module.build.@buildInfo.env.version.assembly"",""artifact.module.build.@buildInfo.env.vcsroot.branch"",""artifact.module.build.@buildInfo.env.teamcity.build.branch"",""artifact.module.build.@buildInfo.env.teamcity.build.id"")";
            //include = @".include(""*"",""property.*"",""module.artifacts.*"")";
            include = @".include(""*"",""property.*"")";
            aql = Newtonsoft.Json.Linq.JObject.Parse(aql).ToString(Formatting.Indented);
            //  var head = p.Head.Ref;
            //  aql = aql.Replace("{0}", head);
            aql = aql.Replace("{0}", sha);
            var query = string.Format("builds.find({0})", aql) + include;

            var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            var byteArray = Encoding.ASCII.GetBytes(artifactory_token);

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            var r = client.PostAsync("https://alliedpayment.jfrog.io/alliedpayment/api/search/aql",
                new StringContent(query)).Result;
            var content = r.Content.ReadAsStringAsync().Result;
            var searchResults = Newtonsoft.Json.JsonConvert
                .DeserializeObject<BuildInfoResult>(content);
            foreach (var bi in searchResults.Results)
            {
                var buildInfo = new ArtifactoryBuildInfo();
                buildInfo.BuildName = bi.BuildName;
                buildInfo.ArtifactoryBuildNumber = bi.BuildNumber;
                buildInfo.TeamCityBuildUrl = bi.BuildUrl;
                var dict = bi.BuildProperties.ToDictionary(z => z.BuildPropertyKey, z => z.BuildPropertyValue);
                buildInfo.Version = dict.GetValue("buildInfo.env.version.assembly") ??
                                    dict.GetValue("buildInfo.env.VERSION_NUMBER");

                var branch = dict.GetValue("buildInfo.env.teamcity.build.branch") ??
                                  dict.GetValue("buildInfo.env.vcsroot.branch") ??
                                  dict.GetValue("buildInfo.env.GITHUB_REF")
                                  ;
                branch = System.Text.RegularExpressions.Regex.Replace(branch, @"\/?refs\/heads\/(.*)", "$1");
                if (branch == "refs/heads/master")
                {
                    branch = "master";
                }

                Console.WriteLine("VERSION=" + buildInfo.Version);
                Console.WriteLine("BRANCH=" + branch);

                buildInfo.Branch = branch;
                buildInfo.BuildNumber = dict.GetValue("buildInfo.env.BUILD_NUMBER") ??
                                        dict.GetValue("buildInfo.env.GITHUB_RUN_NUMBER");

                buildInfo.BuildConfigurationName = dict.GetValue("buildInfo.env.teamcity.buildConfName") ??
                                                   dict.GetValue("buildInfo.env.JFROG_CLI_BUILD_NAME");


                Console.WriteLine("BuildNumber=" + buildInfo.BuildNumber);
                Console.WriteLine("BuildConfigurationName =" + buildInfo.BuildConfigurationName);
                                                   ;
                buildInfo.Sha = sha;
                Console.WriteLine("SHA=" + buildInfo.Sha);

                results.Add(buildInfo);

                //GITHUB_REF 
                //GITHUB_RUN_ID 
                //GITHUB_RUN_NUMBER
                //GITHUB_WORKFLOW 
                //JFROG_CLI_BUILD_URL
                //JFROG_CLI_BUILD_NUMBER 
                //JFROG_CLI_BUILD_NAME 
            }


            //buildInfo.env.version.assembly
            //buildInfo.env.teamcity.build.branch
            //build.started
            //build.url
            //build.created 
            //build.name 
            //build.number 
            //buildInfo.env.BUILD_NUMBER
            //buildInfo.env.teamcity.buildConfName

            //createdon
            //completed on 
            //I'd like to get the time the build took

            return results;
        }

    }

    public static class DictionaryExtensions
    {
        public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
        {
            TV value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}
