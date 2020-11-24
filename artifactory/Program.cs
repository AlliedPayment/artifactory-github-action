using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace artifactory
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //input sha 
            //input branch 
            //input artifactory secret 

            //output version 

            var sha = Environment.GetEnvironmentVariable("INPUT_SHA");
            var branch = Environment.GetEnvironmentVariable("INPUT_BRANCH");
            var artifactoryToken = Environment.GetEnvironmentVariable("ARTIFACTORY_TOKEN");
            

            var svc = new ArtifactService(artifactoryToken);
            //sha = "b150d5b0d26f4f9314a42a9435226751b7a011fa";
           // branch = "master";

            var build = svc.FindBuilds(sha)
                    .FirstOrDefault(x => x.Branch == branch)
                ;
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
    }
}
