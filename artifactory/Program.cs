using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace artifactory
{
    class Program
    {
        public class BranchInfo
        {
            public class CommitInfo
            {
                public string Sha { get; set; }
            }
            public CommitInfo Commit { get; set; }
            public string Name { get; set; }
        }

        static void Main(string[] args)
        {
            
            //input sha 
            //input branch 
            //input artifactory secret 

            //output version 

            var sha = Environment.GetEnvironmentVariable("INPUT_SHA");
            var branch = Environment.GetEnvironmentVariable("INPUT_BRANCH");
            var branchJson = Environment.GetEnvironmentVariable("INPUT_BRANCHES");
            var artifactoryToken = Environment.GetEnvironmentVariable("ARTIFACTORY_TOKEN");

            var svc = new ArtifactService(artifactoryToken);
            //sha = "b150d5b0d26f4f9314a42a9435226751b7a011fa";

            var branches = new List<string>() { branch };
            if (string.IsNullOrEmpty(branch))
            {
                branches = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BranchInfo>>(branchJson)
                    .Select(x => x.Name).ToList();
                ;
            }

            var build = svc.FindBuilds(sha)
                    .FirstOrDefault(x => branches
                        .Contains(x.Branch, StringComparer.CurrentCultureIgnoreCase));
            
            if (build == null)
            {
                Console.WriteLine("CANNOT FIND BUILD");
                Environment.ExitCode = 2;
            }
            else
            {
                Console.WriteLine("VERSION: " + build.Version);
                Console.WriteLine("::set-output name=build-version::{0}", build.Version);
            }

            
        }

        private static int GetPriority(BranchInfo branch, string sha)
        {
            if (sha.Equals(branch.Commit.Sha, StringComparison.CurrentCultureIgnoreCase))
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}
