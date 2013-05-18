using ProjectReference;
using ProjectReferences.Models;
using ProjectReferences.Shared;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.CreateManager();
        }

        private void CreateManager()
        {

            
//            var smallRequest = new AnalysisRequest();
//            smallRequest.RootFile = @"D:\Work\Aerdata\StreamInteractive\Dev-2.7\Repositories\DataAccess\DataAccess.csproj";
//            smallRequest.OutPutType = OutPutType.HtmlDocument;
//            smallRequest.OutPutFolder = @"C:\temp\yumloutput";
//
//            RootNode smallRootNode = Manager.CreateRootNode(smallRequest);
//            Manager.Process(smallRootNode);
//
//            OutputResponse smallOutputResponse = Manager.CreateOutPut(smallRequest, smallRootNode);





            var slnRequest = new AnalysisRequest();
            slnRequest.RootFile = @"D:\Work\Aerdata\StreamInteractive\Dev-2.7\Stream2.sln";
            slnRequest.OutPutType = OutPutType.HtmlDocument;
            slnRequest.OutPutFolder = @"C:\temp\yumloutput";

            RootNode slnRootNode = Manager.CreateRootNode(slnRequest);
            Manager.Process(slnRootNode);

            OutputResponse slnOutputResponse = Manager.CreateOutPut(slnRequest, slnRootNode);


            var projRequest = new AnalysisRequest();
            projRequest.RootFile = @"D:\Work\Aerdata\StreamInteractive\Dev-2.7\Clients\Stream2.Clients.WpfSmartClient\Stream2.Clients.WpfSmartClient.csproj";
            projRequest.OutPutType = OutPutType.YumlReferenceList;
            projRequest.OutPutFolder = @"C:\temp\yumloutput";

            RootNode projRootNode = Manager.CreateRootNode(projRequest);
            Manager.Process(projRootNode);

            OutputResponse projOutputResponse = Manager.CreateOutPut(projRequest, projRootNode);
        }
    }
}
