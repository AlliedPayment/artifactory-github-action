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
            //var sha = "58ddf91ac961cf0dd6e06a672c89e3efdd00a247";
            //var branch = "bug/billgo500s";

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
                Console.WriteLine("::set-output name=aws-build-version::{0}", build.Version);
            }

            
        }
    }
}
