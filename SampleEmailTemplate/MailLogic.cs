using EmailUtility.Model;
using Microsoft.Extensions.Logging;
using RazorEngineCore;
using System;
using System.IO;
using System.Net.Mail;

namespace EmailUtility
{
    public class MailLogic
    {
        private readonly ILogger _logger;
        public MailLogic(ILogger<MailLogic> logger)
        {
            _logger = logger;
        }
        private void SendSampleTemplate(SampleModel model)
        {
            
            string TemplateName = "template";

            RazorEngine razorEngine = new();
            string viewPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TemplateName + ".cshtml");
            string content = File.ReadAllText(viewPath);
            _logger.LogInformation($"Sending mail {TemplateName} to {model.ToEmailAddress}.");
            IRazorEngineCompiledTemplate<RazorEngineTemplateBase<SampleModel>> compiledTemplate = razorEngine.Compile<RazorEngineTemplateBase<SampleModel>>(content);

            string output = compiledTemplate.Run(instance =>
            {
                instance.Model = model;
            });

            MailMessage mm = new()
            {
                Subject = model.Subject,
                IsBodyHtml = true,
                Priority = MailPriority.High
            };
            if (output != null)
            {
                mm.Body = output;
            }

            mm.To.Add(model.ToEmailAddress);
            mm.From = new MailAddress("from@domain.com");
            var smtpClient = new SmtpClient
            {
                Host = "smtpServer.domain.com",
                UseDefaultCredentials = true,

            };
            smtpClient.Send(mm);
        }

        internal void GetFiles(string filePath)
        {
            try
            {
                _logger.LogInformation("Getting files from path");
                string[] fileEntries = Directory.GetFiles(filePath, "*.*");
                foreach (string fileName in fileEntries)
                {
                    _logger.LogInformation(message: $"Processing file {fileName}");
                    ProcessFile(fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                if (ex.InnerException is not null)
                {
                    _logger.LogError(ex.Message + ex.StackTrace);
                }
            }

        }
        internal void ProcessFile(string inputFile)
        {
            try
            {
                string line;
                SampleModel model = new()
                {
                    Name = "User",
                    Subject = "Testing Email Utility",
                    ToEmailAddress = "to@domain.com",
                    RepoName = "EmailUtility"
                };
                TextReader tr = new StreamReader(inputFile);
                while ((line = tr.ReadLine()) != null)
                {
                    if (line.Length > 0)
                    {
                        //Add your code to create model if your are creating it from each line in the file present in "C:\\Model_Data" folder
                        SendSampleTemplate(model);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                if (ex.InnerException is not null)
                {
                    _logger.LogError(ex.Message + ex.StackTrace);
                }
            }
        }
    }
}
