using NcodedXMobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzbget_silk.Service
{

    public class NZBGetService
    {
        private readonly Model.NZBGetServer server;
        private string Url(string function) => server.GetURL(function);
        private string Url() => server.GetURL();

        public NZBGetService(Model.NZBGetServer server)
        {
            this.server = server ?? throw new ArgumentException("NZBGetService will not work without server.", "server");
        }

        public async Task<Model.JsonRPCResult<string>> Version()
        {
            try
            {
                return await JsonRequest.Get<Model.JsonRPCResult<string>>(Url("version"));
            }
            catch
            {
                return null;
            }
        }
        public async Task<Model.JsonRPCResult<Model.JsonRPCStatus>> Status()
        {
            try
            {
                return await JsonRequest.Get<Model.JsonRPCResult<Model.JsonRPCStatus>>(Url("status"));
            }
            catch
            {
                return null;
            }
        }
        public async Task<Model.JsonRPCResult<int>> Append(string fileName, byte[] fileContent)
        {
            string fileContent64 = Convert.ToBase64String(fileContent);

            try
            {
                return await JsonRequest.Post<Model.JsonRPCResult<int>>(Url(),
                    new Model.JsonRPCCall("append", fileName, fileContent64, "", 0, false, false, "", 0, "SCORE", new Dictionary<string, string>() { { "*unpack:", "yes" } }));
            }
            catch
            {
                return null;
            }
        }
        public async Task<Model.JsonRPCResult<List<Model.JsonRPCGroup>>> ListGroups()
        {
            try
            {
                return await JsonRequest.Post<Model.JsonRPCResult<List<Model.JsonRPCGroup>>>(Url(),
                    new Model.JsonRPCCall("listgroups", 0));
            }
            catch
            {
                return null;
            }
        }
        public async Task<Model.JsonRPCResult<bool>> PauseDownload()
        {
            try
            {
                return await JsonRequest.Post<Model.JsonRPCResult<bool>>(Url(),
                    new Model.JsonRPCCall("pausedownload"));
            }
            catch
            {
                return null;
            }
        }
        public async Task<Model.JsonRPCResult<bool>> ResumeDownload()
        {
            try
            {
                return await JsonRequest.Post<Model.JsonRPCResult<bool>>(Url(),
                    new Model.JsonRPCCall("resumedownload"));
            }
            catch
            {
                return null;
            }
        }
        public const string EDIT_Q_GROUP_DELETE = "GroupFinalDelete";
        public const string EDIT_Q_GROUP_MOVETOP = "GroupMoveTop";
        public async Task<Model.JsonRPCResult<bool>> EditQueue(string command, string param, params int[] ids)
        {
            try
            {
                return await JsonRequest.Post<Model.JsonRPCResult<bool>>(Url(),
                    new Model.JsonRPCCall("editqueue", command, param, ids));
            }
            catch
            {
                return null;
            }
        }


        

    }
}
