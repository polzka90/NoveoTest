using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Api.Db;
using Api.Queue;
using Api.Request;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("Workflow")]
    [ApiController]
    public class WorkflowController : ControllerBase
    {
        public PostgreSqlContext context { get; set; }
        private DesignTimeDbContextFactory factory;
        private const string MYQUEUE = "Fila";

        public WorkflowController()
        {
            factory = new DesignTimeDbContextFactory();
            context = factory.CreateDbContext(new string[0]);
        }

        [HttpGet]
        public IEnumerable<WorkflowDTO> Get()
        {
            return context.WorkFlow.ToList();
        }

        [HttpPost]
        public WorkflowDTO Post(WorkflowRequest req)
        {

            context.WorkFlow.Add(new WorkflowDTO() { 
            Data = req.Data,
            Steps = req.Steps,
            UUID = req.UUID,
            Status = StatusWF.Inserted
            });
            context.SaveChanges();
            MessageHandler.Send(MYQUEUE, req.UUID.ToString());
            return context.WorkFlow.Find(req.UUID);
        }

        [HttpPatch, Route("{UUID}")]
        public string Patch(Guid UUID)
        {
            WorkflowDTO workflowSelected = context.WorkFlow.Find(UUID);
            workflowSelected.Status = StatusWF.Consumed;
            context.WorkFlow.Update(workflowSelected);
            context.SaveChanges();

            return "Successo";
        }

        [HttpGet, Route("Consume")]
        public HttpResponseMessage Consume(int id)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            string data = MessageHandler.Receive(MYQUEUE);

            WorkflowDTO workflowSelected = context.WorkFlow.Find(Guid.Parse(data));

            writer.Write(workflowSelected.Data);
            writer.Flush();
            stream.Position = 0;

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Export.csv" };
            return result;
        }
    }
}