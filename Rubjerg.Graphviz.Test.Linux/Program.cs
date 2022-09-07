using SE.Geospatial.Functions.Dynamic.Workflows.GraphHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubjerg.Graphviz.Test.Linux
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var basicOperations = new CGraphBasicOperations();
            foreach (var i in Enumerable.Range(0, 15400))
            {
                basicOperations.TestReadDotFile();
            }

            basicOperations.TestReadDotFile();
            basicOperations.TestCopyAttributes();
            basicOperations.TestNodeMerge();
            basicOperations.TestDeletions();
            basicOperations.TestEdgeContraction();

            var edgeCases = new CGraphEdgeCases();
            foreach (var i in Enumerable.Range(0, 1))
            {
                edgeCases.TestAttributeDefaults();
                edgeCases.TestAttributeIntroduction();
                edgeCases.TestCopyToNewRoot();
                edgeCases.TestCopyUnintroducedAttributes();
                edgeCases.TestCreateNestedStructures();
                edgeCases.TestEdgeEquals();
                edgeCases.TestEdgesInSubgraphs();
                edgeCases.TestMarshaling();
                edgeCases.TestNodeAndGraphWithSameName();
                edgeCases.TestNonStrictGraph();
                edgeCases.TestRecursiveSubgraphDeletion();
                edgeCases.TestRootOfRoot();
                edgeCases.TestSelfLoopEnumeration();
                Console.WriteLine(i);
            }


            var integrationTests = new CGraphIntegrationTests();
            integrationTests.TestAddNode(500, 10);
            integrationTests.TestBFS(100, 10);
            integrationTests.TestDeleteNode(500, 10);
            integrationTests.TestGetNode(100, 10000);
            integrationTests.TestTopologicalEqualsClone(10, 5);
            integrationTests.TestTopologicalEqualsCloneWithSubgraphs(10, 5);
            integrationTests.TestTopologicalEqualsIdentity(10, 5);



            var stressTests = new CGraphStressTests();
            stressTests.TestAddNode(500, 10);
            stressTests.TestBFS(100, 10);
            stressTests.TestDeleteNode(500, 10);
            stressTests.TestGetNode(100, 10000);
            stressTests.TestTopologicalEqualsClone(10, 5);
            stressTests.TestTopologicalEqualsCloneWithSubgraphs(10, 5);
            stressTests.TestTopologicalEqualsIdentity(10, 5);

            var graphCreator = new GraphCreator();
            var graph = graphCreator.FromDotString(@"
                digraph{  
                  bgcolor=""white""; 
                  pad = 1;
                            rankdir = TB;
                            newrank = true;
                            splines = spline;
                            ordering =in;
                            edge[arrowsize = 1, color = ""#7E6BC9""];
                            node[shape = box, color = ""#7E6BC9""];

                # expected parameters
                # {
                # parameters: {
                # designId: ""00000000-0000-0000-0000-000000000000""
                #     //assignmentType: ""Selecting Alternative"",
                #     // If assignedTo is missing then it will use the CallerUserId
                #     //assignTo: """"
                #
                #   }
                # }
                            entry[
                              label = ""Start copy"",
                              width = 3,
                              shape = invhouse,
                              height = 0.9,
                              se_type1 = OnDemandTrigger
                            ];

                            copyDesignInformation[
                              shape = record,
                              label = ""{Get Design To Copy|Create New Design Resource|{Copy DNX|Copy DSR|Copy Print Template}|Await Copies}"",
                              se_executionOrder = Parallel,
                              se_awaitType = All,

                              se_type1 = Api,
                              se_method1 = Get,
                              se_executionAwaitGroup1 = 1,
                              se_service1 = DesignerCoordination,
                              se_modifyRequest1 = ""(request, ctx, log) => {
          
                                request.RequestUri = new Uri($\""api/v1/design/{ctx.State.parameters.designId}?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);
                    }"",
                    se_resultVariable1 = DesignToCopyResource,
    
                    se_type2 = Api,
                    se_method2 = POST,
                    se_executionAwaitGroup2 = 2,
                    se_service2  = DesignerCoordination,
                    se_modifyRequest2 = ""(request, ctx, log) => {
                      request.RequestUri = new Uri($\""api/v1/design?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);
       
                      var body = new
                      {
                          id = Guid.NewGuid(),
                          name = $\""{ctx.State.DesignToCopyResource.name} - Copy\"",
                        workRequestId = ctx.State.DesignToCopyResource.workRequestId,
                          designLinearUnitWkid = ctx.State.DesignToCopyResource.designLinearUnitWkid,
                          gisState = ctx.State.DesignToCopyResource.gisState,
                          skipNetworkExtract = true,
                          customProperties = ctx.State.DesignToCopyResource.customProperties,
                          referenceDesignIds = ctx.State.DesignToCopyResource.referenceDesignIds,
                          workflowSessionId = ctx.State.DesignToCopyResource.workflowSessionId
                      };
                        request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, \""application/json\"");
                    }"",    
                    se_resultVariable2 = NewDesignResource,
    
                    se_type3 = Api,
                    se_method3 = POST,
                    se_executionAwaitGroup3 = 3,
                    se_service3  = DesignerCoordination,
                    se_modifyRequest3 = ""(request, ctx, log) => {
                      request.RequestUri = new Uri($\""api/v1/assignment?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);
    
                      var body = new
                      {
                          id = Guid.NewGuid(),
                          //assignedTo = ctx.CallerUserId,
                          designId = ctx.State.NewDesignResource.id,
                          assignmentType = ctx.State.parameters.assignmentType,
                          workflowSessionId = ctx.State.DesignToCopyResource.workflowSessionId
                      };
                    request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, \""application/json\"");
                    }
                "",
                    se_resultVariable3 = NewAssignmentResource,
    
    
                    #
                    #se_type4 = Api,
                    #se_method4 = POST,
                    #se_executionAwaitGroup4 = 4,
                    #se_service4  = KvStorage,
                    #se_modifyRequest4 = ""(request, ctx, log) => {
                    #  request.RequestUri = new Uri($\""api/v1/kv/SE.Geospatial.Designer/Settings/TestFolder/CopiedKeyWithAttachment3?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);
                    #
                    #  var body = new { copied = true };
                    #  request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, \""application/json\"");
                    #
                    #}"",   
    
                    se_type5 = ApiCopyStream,
                    se_await5 = false,
                    se_executionAwaitGroup5 = 5,
                    se_resultVariable5 = DNXTask,
                    se_fromMethod5 = GET,
                    se_fromService5 = DesignerStorage,
                    se_fromModifyRequest5 = ""(request, ctx, log) => {
                      request.RequestUri = new Uri($\""api/v1/design/{ctx.State.DesignToCopyResource.id}?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);
                      request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(\""application/vnd.schneider-electric.dnx\""));
                    }"",
                    se_fromProcessResponse5 = ""async (response, ctx, log) => {
                      ctx.State.ShouldCopyDnx = true;
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ctx.State.ShouldCopyDnx = false;
                    }
                    else
                    {
                        throw new Exception($\""Get DNX file failed with status code - {response.StatusCode}\"");
                        }
                }
                    }"",
                    se_toMethod5 = POST,
                    se_toService5 = DesignerStorage,
                    se_toModifyRequest5 = ""(request, ctx, log) => {
                      if (ctx.State.ShouldCopyDnx)
                {
                    request.RequestUri = new Uri($\""api/v1/design/{ctx.State.NewDesignResource.id}?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);

                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(\""application/vnd.schneider-electric.dnx\""));
                      }
                    }"",


                    se_type6 = ApiCopyStream,
                    se_await6 = false,
                    se_executionAwaitGroup6 = 5,
                    se_resultVariable6 = DSRTask,
                    se_fromMethod6 = GET,
                    se_fromService6 = DesignerStorage,
                    se_fromModifyRequest6 = ""(request, ctx, log) => {
                      request.RequestUri = new Uri($\""api/v1/design/{ctx.State.DesignToCopyResource.id}?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);
                      request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(\""application/vnd.schneider-electric.dsr\""));
                    }"",
                    se_fromProcessResponse6 = ""async (response, ctx, log) => {
                      ctx.State.ShouldCopyDsr = true;
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ctx.State.ShouldCopyDsr = false;
                    }
                    else
                    {
                        throw new Exception($\""Get DSR file failed with status code - {response.StatusCode}\"");
                        }
                }
                    }"",
                    se_toMethod6 = POST,
                    se_toService6 = DesignerStorage,
                    se_toModifyRequest6 = ""(request, ctx, log) => {
                      if (ctx.State.ShouldCopyDsr)
                {
                    request.RequestUri = new Uri($\""api/v1/design/{ctx.State.NewDesignResource.id}?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);

                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(\""application/vnd.schneider-electric.dsr\""));
                      }
                    }""


                    se_type7 = ApiCopyStream,
                    se_await7 = false,
                    se_executionAwaitGroup7 = 5,
                    se_resultVariable7 = PrintTemplateTask,
                    se_fromMethod7 = GET,
                    se_fromService7 = DesignerStorage,
                    se_fromModifyRequest7 = ""(request, ctx, log) => {
                        request.RequestUri = new Uri($\""api/v1/printTemplate/{ctx.State.DesignToCopyResource.id}?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);
                    }"",
                    se_fromProcessResponse7 = ""async (response, ctx, log) => {
                      ctx.State.ShouldCopyPrintTemplate = true;
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ctx.State.ShouldCopyPrintTemplate = false;
                    }
                    else
                    {
                        throw new Exception($\""Get PrintTemplate file failed with status code - {response.StatusCode}\"");
                        }
                }
                    }"",
                    se_toMethod7 = POST,
                    se_toService7 = DesignerStorage,
                    se_toModifyRequest7 = ""(request, ctx, log) => {
                      if (ctx.State.ShouldCopyPrintTemplate)
                {
                    request.RequestUri = new Uri($\""api/v1/printTemplate/{ctx.State.NewDesignResource.id}?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);
                      }
                    }""


                    se_type8 = Await,
                    se_executionAwaitGroup8 = 6,
                    se_task8 = DNXTask,
    
    
                    se_type9 = Await,
                    se_executionAwaitGroup9 = 6,
                    se_task9 = DSRTask,
    
    
                    se_type10 = Await,
                    se_executionAwaitGroup10 = 6,
                    se_task10 = PrintTemplateTask

                    #se_type11 = Api,
                    #se_method11 = POST,
                    #se_executionAwaitGroup11 = 7,
                    #se_service11  = DesignerCoordination,
                    #se_modifyRequest11 = ""(request, ctx, log) => {
                    #  request.RequestUri = new Uri($\""api/v1/assignment/{ctx.State.NewAssignmentResource.id}/assign?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);
                    #
                    #  
                    #  string userId = string.Empty;
                    #  try
                    #  {
                    #    if(!string.IsNullOrWhiteSpace(ctx.State.parameters.assignedTo.ToString()))
                    #    {
                    #      userId = ctx.State.parameters.assignedTo.ToString();
                    #    }
                    #  }
                    #  catch
                    #  { 
                    #    userId = ctx.CallerUserId;
                    #  }
                    #     
                    #  var body = new { 
                    #    assignedTo = userId
                    #  };
                    #  request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, \""application/json\"");
                    #}"",
                    #se_resultVariable11 = NewAssignmentResource,
                    #
                    #
                    #se_type12 = Api,
                    #se_method12 = GET,
                    #se_executionAwaitGroup12 = 8,
                    #se_service12  = Access,
                    #se_modifyRequest12 = ""(request, ctx, log) => {
                    #  request.RequestUri = new Uri($\""api/v1/users?userId={ctx.State.NewAssignmentResource.assignedTo}&tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);
                    #}"",
                    #se_resultVariable12 = NewAssignmentUserResource,
                    #
                    #
                    #se_type13 = Api,
                    #se_method13 = POST,
                    #se_executionAwaitGroup13 = 9,
                    #se_service13 = Communication,
                    #se_modifyRequest13 = ""(request, ctx, log) => {
                    #  request.RequestUri = new Uri($\""api/v1/email?tenantId={ctx.TenantId}\"", UriKind.RelativeOrAbsolute);
                    #  
                    #  if(string.IsNullOrWhiteSpace(ctx.State.NewAssignmentUserResource.email.ToString())
                    #    //|| ctx.State.parameters.author == ctx.State.SupervisorAssignmentResource.assignedTo
                    #  )
                    #  {
                    #    // skip in the event that the user does not have an email address or
                    #    // if the author is the same as the assignedTo parameter - no need to email yourself
                    #    request.RequestUri = null;
                    #    return;
                    #  }
                    #  else{
                    #    var body = new {
                    #      to = $\""{ctx.State.NewAssignmentUserResource.email}\"",
                    #      subject = $\""New DXI work has been assigned to you\"",
                    #      body = $\""A Selecting Alternative job has been assigned to you.\r\n\r\nOpen here https://arcfm-westus-01-dev.arcfmsolution.com/functions/api/LaunchCustomUriScheme/arcfmdxi?handlerName=Workflow&assignmentId={ctx.State.NewAssignmentResource.id}\"",
                    #      htmlBody = $\""\""
                    #    };
                    #    request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, \""application/json\"");
                    #  }
                    #
                    #}""
                  ]
  
                  exit[
                    shape = invhouse,
                    label = ""Exit"",
                    color = ""#22FF44"",
                    style = filled;
                  ]
  
 
                  entry->copyDesignInformation[
                    #constraint=false
                  ]
                  copyDesignInformation->exit;
  
  
    
                }

            ");


            var entryNode = graph.GetNode("entry");
        }

    }

    public interface IGraphCreator
    {
        public IRootGraph FromDotString(string parametersGraph);
    }

    public interface IRootGraph
    {
        INode GetNode(string entry);
    }

    public interface INode
    {
        IDictionary<string, string> GetAttributes();
        string GetName();
        IEnumerable<IEdge> EdgesOut();
    }

    public interface IEdge
    {
        IDictionary<string, string> GetAttributes();
        string GetName();
        INode Head();
    }
}
